using Microsoft.AspNetCore.Mvc;
using GamePlayService.Services;
using GamePlayService.Dtos.Game;
namespace GamePlayService.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class GameController(GameService gameService) : ControllerBase
	{
		private readonly GameService _gameService = gameService;

		[HttpPost]
		public async Task<IActionResult> CreateGame()
		{
			var newGame = await _gameService.CreateGameAsync();
			return CreatedAtAction(nameof(GetGame), new { id = newGame.Id }, newGame);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetGame(Guid id)
		{
			var game = await _gameService.GetGameByIdAsync(id);
			if (game == null)
			{
				return NotFound();
			}
			return Ok(game);
		}

		[HttpPost("{id}/moves")]
		public async Task<IActionResult> MakeMove(Guid gameId, [FromBody] MoveDto moveDto)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			try
			{
				// Виклик сервісу для виконання ходу
				var result = await _gameService.MakeMoveAsync(gameId, moveDto);

				if (!result.IsSuccess)
				{
					return BadRequest(new { Message = result.ErrorMessage });
				}

				return Ok(new
				{
					GameId = gameId,
					Move = result.Move,
					BoardStateFEN = result.BoardStateFEN,
					CurrentTurn = result.CurrentTurn,
					Message = "Move made successfully!"
				});
			}
			catch (Exception ex)
			{
				// Логування помилки
				//_logger.LogError(ex, "Error making move for GameId: {GameId}", id);
				return StatusCode(500, new { Message = "An unexpected error occurred." });
			}
		}


		// 4. Отримання списку активних ігор
		[HttpGet]
		public IActionResult GetAllGames()
		{
			// TODO: Додати логіку отримання списку ігор
			return Ok(new { Games = new[] { "Game1", "Game2" } });
		}
	}
}
