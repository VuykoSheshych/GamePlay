using Microsoft.AspNetCore.Mvc;
using GamePlayService.Services;
namespace GamePlayService.Controllers
{
	[ApiController]
	[Route("api/games")]
	public class GameController(GameRecordsService gameService) : ControllerBase
	{
		private readonly GameRecordsService _gameService = gameService;

		[HttpGet("{id}")]
		public async Task<IActionResult> GetGame(Guid id)
		{
			var game = await _gameService.GetGameRecordByIdAsync(id);
			if (game == null)
			{
				return NotFound();
			}
			return Ok(game);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllGames()
		{
			var games = await _gameService.GetAllGameRecordsAsync();
			if (games == null)
			{
				return NotFound();
			}
			return Ok(games);
		}
	}
}
