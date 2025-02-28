using GamePlayService.Dtos.Game;
using Microsoft.AspNetCore.SignalR;

namespace GamePlayService.Services;
public class GameHub(GameSessionService gamesSessionService, GameSearchService gameSearchService) : Hub
{
	private readonly GameSessionService _gamesSessionService = gamesSessionService;
	private readonly GameSearchService _gameSearchService = gameSearchService;

	public async Task<string> CreateGame(Dictionary<string, string> players)
	{
		var gameId = await _gamesSessionService.CreateGameSessionAsync(player1, player2);

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
			await Clients.Group(gameId).SendAsync("ReceiveGameState", await _gamesSessionService.GetGameSessionAsync(gameId));
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
		await _gamesSessionService.MakeMoveAsync(gameId, moveDto);

		var updatedGameSession = await _gamesSessionService.GetGameSessionAsync(gameId);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", updatedGameSession);
	}
	public async Task FinishGame(string gameId, string result)
	{
		var updatedGameSession = await _gamesSessionService.GetGameSessionAsync(gameId);
		await _gamesSessionService.RemoveGameSessionAsync(gameId, result);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", updatedGameSession);
	}
	public async Task LeaveGame(string gameId)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
	}
}
