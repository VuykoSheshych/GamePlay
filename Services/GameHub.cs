using GamePlayService.Dtos;
using GamePlayService.Models;
using Microsoft.AspNetCore.SignalR;

namespace GamePlayService.Services;
public class GameHub(GameSessionService gamesSessionService, GameSearchService gameSearchService) : Hub
{
	private readonly GameSessionService _gameSessionService = gamesSessionService;
	private readonly GameSearchService _gameSearchService = gameSearchService;
	public async Task<string> CreateGame(List<(string name, string id)> players)
	{
		var gameId = await _gameSessionService.CreateGameSessionAsync(players);

		return gameId.ToString();
	}
	public async Task JoinGame(string gameId, List<string> connectionIds)
	{
		if (connectionIds == null || connectionIds.Count == 0) return;

		foreach (var connectionId in connectionIds)
		{
			await Groups.AddToGroupAsync(connectionId, gameId);
		}

		await Clients.Group(gameId).SendAsync("GameFound", gameId);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", await _gameSessionService.GetGameSessionAsync(gameId));
	}
	public async Task StartGameSearch(string playerName)
	{
		await _gameSearchService.AddPlayerToSearchQueue(playerName, Context.ConnectionId);

		var playersWithConnectionIds = await _gameSearchService.FindPlayersForGame();
		if (playersWithConnectionIds != null)
		{
			var gameId = await CreateGame(playersWithConnectionIds);

			await JoinGame(gameId, [playersWithConnectionIds.First().id, playersWithConnectionIds.Last().id]);
		}
	}
	public async Task StopGameSearch(string playerName)
	{
		await _gameSearchService.RemovePlayerFromSearchQueue(playerName);
	}
	public async Task MakeMove(string gameId, MoveDto moveDto)
	{
		var moveResult = await _gameSessionService.TryMakeMoveAsync(gameId, moveDto);
		await Clients.Group(gameId).SendAsync("ReceiveMove", moveResult);

		if (!moveResult.IsSuccess) return;

		var updatedGameSession = await _gameSessionService.GetGameSessionAsync(gameId);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", updatedGameSession);

		if (moveResult.Message.EndsWith('#'))
		{
			var activeColor = new BoardState(updatedGameSession!.CurrentFen).ActiveColor;
			string winner = activeColor == "w" ? "Black" : "White";
			await FinishGame(gameId, winner);
		}
		else if (moveResult.Message.Contains("½-½"))
		{
			await FinishGame(gameId, "½-½");
		}
	}
	public async Task FinishGame(string gameId, string looser)
	{
		string result = "½-½";
		var gameSession = await _gameSessionService.GetGameSessionAsync(gameId);
		if (gameSession != null)
		{
			if (looser == gameSession.BlackPlayer.Name) result = "1 0";
			else if (looser == gameSession.WhitePlayer.Name) result = "0 1";
			else result = "½-½";

			await Clients.Group(gameId).SendAsync("GameFinished", looser);
			await Groups.RemoveFromGroupAsync(gameSession.BlackPlayer.ConnectionId, gameId);
			await Groups.RemoveFromGroupAsync(gameSession.WhitePlayer.ConnectionId, gameId);

		}
		await _gameSessionService.RemoveGameSessionAsync(gameId, result);
	}
}