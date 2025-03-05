using GamePlayService.Dtos.Game;
using GamePlayService.Models;
using Microsoft.AspNetCore.SignalR;

namespace GamePlayService.Services;
public class GameHub(GameSessionService gamesSessionService, GameSearchService gameSearchService) : Hub
{
	private readonly GameSessionService _gameSessionService = gamesSessionService;
	private readonly GameSearchService _gameSearchService = gameSearchService;

	public async Task<string> CreateGame(Dictionary<string, string> players)
	{
		var gameId = await _gameSessionService.CreateGameSessionAsync(players);

		return gameId.ToString();
	}
	public async Task JoinGame(string gameId, List<string> connectionIds)
	{
		if (connectionIds == null || connectionIds.Count == 0)
		{
			return;
		}

		foreach (var connectionId in connectionIds)
		{
			await Groups.AddToGroupAsync(connectionId, gameId);
			await Clients.Group(gameId).SendAsync("GameFound", gameId);
			await Clients.Group(gameId).SendAsync("ReceiveGameState", await _gameSessionService.GetGameSessionAsync(gameId));
		}
	}
	public async Task StartGameSearch(string playerId)
	{
		await _gameSearchService.AddPlayerToSearchQueue(playerId, Context.ConnectionId);

		var playersWithConnectionIds = await _gameSearchService.FindPlayersForGame();
		if (playersWithConnectionIds != null)
		{
			var gameId = await CreateGame(playersWithConnectionIds);

			await JoinGame(gameId, [playersWithConnectionIds.Values.First(), playersWithConnectionIds.Values.Last()]);
		}
	}
	public async Task TerminateGameSearch(string player)
	{
		await _gameSearchService.RemovePlayerFromSearchQueue(player);
	}
	public async Task MakeMove(string gameId, MoveDto moveDto)
	{
		var moveResult = await _gameSessionService.TryMakeMoveAsync(gameId, moveDto);
		await Clients.Group(gameId).SendAsync("ReceiveMove", moveResult);

		if (moveResult is "You cannot make moves with your opponent's pieces!" or
							"Invalid move for this type of piece!" or
							"The final square is already occupied by an allied piece!" or
							"You cannot move into check!" or
							"Invalid castling move!")
		{
			return;
		}

		var updatedGameSession = await _gameSessionService.GetGameSessionAsync(gameId);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", updatedGameSession);

		if (moveResult.EndsWith('#'))
		{
			var activeColor = new BoardState(updatedGameSession!.CurrentFen).ActiveColor;
			string winner = activeColor == "w" ? "Black" : "White";
			await FinishGame(gameId, winner);
		}
		else if (moveResult.Contains("½-½"))
		{
			await FinishGame(gameId, "Draw");
		}
	}
	public async Task FinishGame(string gameId, string result)
	{
		var gameSession = await _gameSessionService.GetGameSessionAsync(gameId);
		if (gameSession != null)
		{
			string winner;
			if (result == "Black") winner = gameSession.PlayerBlack;
			else if (result == "White") winner = gameSession.PlayerWhite;
			else winner = "Draw";

			await Clients.Group(gameId).SendAsync("GameFinished", winner);
			await Groups.RemoveFromGroupAsync(gameSession.PlayerBlackConnectionId, gameId);
			await Groups.RemoveFromGroupAsync(gameSession.PlayerWhiteConnectionId, gameId);

		}
		await _gameSessionService.RemoveGameSessionAsync(gameId, result);
	}
}
