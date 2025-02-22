using GamePlayService.Dtos.Game;
using Microsoft.AspNetCore.SignalR;

namespace GamePlayService.Services;
public class GameHub(ActiveGamesService activeGameService) : Hub
{
	private readonly ActiveGamesService _activeGameService = activeGameService;
	public async Task JoinGame(string gameId)
	{
		await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
		var gameSession = await _activeGameService.GetGameSessionAsync(gameId);
		if (gameSession != null)
		{
			await Clients.Caller.SendAsync("ReceiveGameState", gameSession);
		}
	}
	public async Task MakeMove(string gameId, MoveDto moveDto)
	{
		await _activeGameService.MakeMoveAsync(gameId, moveDto);
		await Clients.Group(gameId).SendAsync("ReceiveGameState", moveDto);
	}
	public async Task LeaveGame(string gameId)
	{
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameId);
	}
}
