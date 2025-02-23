using GamePlayService.Dtos.Game;
using Microsoft.AspNetCore.SignalR;

namespace GamePlayService.Services;
public class GameHub(ActiveGamesService activeGamesService) : Hub
{
	private readonly ActiveGamesService _activeGamesService = activeGamesService;

	public async Task<string> CreateGame(string player1, string player2)
	{
		var gameId = await _activeGamesService.CreateGameSessionAsync(player1, player2);

		return gameId.ToString();
	}
	public async Task JoinGame(string gameId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
		var gameSession = await _activeGamesService.GetGameSessionAsync(gameId);
		if (gameSession != null)
		{
			await Clients.Caller.SendAsync("ReceiveGameState", gameSession);
		}
	}
	public async Task MakeMove(string gameId, MoveDto moveDto)
	{
		await _activeGamesService.MakeMoveAsync(gameId, moveDto);

		var updatedGameSession = await _activeGamesService.GetGameSessionAsync(gameId);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", updatedGameSession);
	}
	public async Task FinishGame(string gameId, string result)
	{
		var updatedGameSession = await _activeGamesService.GetGameSessionAsync(gameId);
		await _activeGamesService.RemoveGameSessionAsync(gameId, result);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", updatedGameSession);
	}
	public async Task LeaveGame(string gameId)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
	}
}
