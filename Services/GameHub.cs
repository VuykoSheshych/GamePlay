using System.Text.Json;
using ChessShared.Dtos;
using ChessShared.Enums;
using GamePlay.Models;
using Microsoft.AspNetCore.SignalR;

namespace GamePlay.Services;

/// <include file='.docs/xmldocs/Services.xml' path='doc/class/member[@name="GameHub"]/*' />
public class GameHub(IGameSessionService gamesSessionService, IGameSearchService gameSearchService) : Hub
{
	private readonly IGameSessionService _gameSessionService = gamesSessionService;
	private readonly IGameSearchService _gameSearchService = gameSearchService;

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="GameHub.CreateGame"]/*' />
	public async Task<string> CreateGame(List<(string name, string id)> players)
	{
		var gameId = await _gameSessionService.CreateGameSessionAsync(players);

		return gameId.ToString();
	}

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="GameHub.JoinGame"]/*' />
	public async Task JoinGame(string gameId, List<(string name, string id)> players)
	{
		if (players == null || players.Count != 2) return;

		foreach (var (name, id) in players)
		{
			await Groups.AddToGroupAsync(id, gameId);
			await _gameSearchService.RemovePlayerFromSearchQueue(name);
		}

		await Clients.Group(gameId).SendAsync("GameFound", gameId);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", JsonSerializer.Serialize(await _gameSessionService.GetGameSessionAsync(gameId)));
	}

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="GameHub.StartGameSearch"]/*' />
	public async Task StartGameSearch(string playerName)
	{
		await _gameSearchService.AddPlayerToSearchQueue(playerName, Context.ConnectionId);

		var playersWithConnectionIds = await _gameSearchService.FindPlayersForGame();
		if (playersWithConnectionIds != null)
		{
			var gameId = await CreateGame(playersWithConnectionIds);

			await JoinGame(gameId, playersWithConnectionIds);
		}
	}

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="GameHub.StopGameSearch"]/*' />
	public async Task StopGameSearch(string playerName)
	{
		await _gameSearchService.RemovePlayerFromSearchQueue(playerName);
	}

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="GameHub.MakeMove"]/*' />
	public async Task MakeMove(string gameId, MoveDto moveDto)
	{
		var moveResult = await _gameSessionService.TryMakeMoveAsync(gameId, moveDto);
		await Clients.Group(gameId).SendAsync("ReceiveMove", moveResult);

		if (!moveResult.IsSuccess) return;

		var updatedGameSession = await _gameSessionService.GetGameSessionAsync(gameId);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", JsonSerializer.Serialize(updatedGameSession));

		if (moveResult.Message.EndsWith('#'))
		{
			var activeColor = new BoardState(updatedGameSession!.CurrentFen).ActiveColor;
			string looser = activeColor == PlayerColor.White ? updatedGameSession.WhitePlayer.Name : updatedGameSession.BlackPlayer.Name;
			await FinishGame(gameId, looser);
		}
		else if (moveResult.Message.Contains("½-½"))
		{
			await FinishGame(gameId, "½-½");
		}
	}

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="GameHub.SendMessage"]/*' />
	public async Task SendMessage(string gameId, ChatMessageDto chatMessage)
	{
		var game = await _gameSessionService.GetGameSessionAsync(gameId);

		game?.Messages.Add(chatMessage);

		await Clients.Group(gameId).SendAsync("ReceiveGameState", game);
	}

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="GameHub.FinishGame"]/*' />
	public async Task FinishGame(string gameId, string looser)
	{
		GameResult result = GameResult.Draw;
		var gameSession = await _gameSessionService.GetGameSessionAsync(gameId);
		if (gameSession != null)
		{
			if (looser == gameSession.BlackPlayer.Name) result = GameResult.WhiteWin;
			else if (looser == gameSession.WhitePlayer.Name) result = GameResult.BlackWin;
			else result = GameResult.Draw;

			await Clients.Group(gameId).SendAsync("GameFinished", looser);
			await Groups.RemoveFromGroupAsync(gameSession.BlackPlayer.ConnectionId, gameId);
			await Groups.RemoveFromGroupAsync(gameSession.WhitePlayer.ConnectionId, gameId);

		}
		await _gameSessionService.RemoveGameSessionAsync(gameId, result);
	}
}