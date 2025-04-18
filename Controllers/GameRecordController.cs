using Microsoft.AspNetCore.Mvc;
using GamePlayService.Services;

namespace GamePlayService.Controllers;

/// <include file='.docs/xmldocs/Controllers.xml' path='doc/class/member[@name="GameRecordController"]/*' />
[ApiController]
[Route("api/games")]
public class GameRecordController(IGameRecordService gameRecordService) : ControllerBase
{
	private readonly IGameRecordService _gameRecordService = gameRecordService;

	/// <include file='.docs/xmldocs/Controllers.xml' path='doc/method/member[@name="GameRecordController.GetGameRecordById"]/*' />
	[HttpGet("{gameId}")]
	public async Task<IActionResult> GetGameRecordById(Guid gameId)
	{
		var game = await _gameRecordService.GetGameRecordByIdAsync(gameId);
		if (game == null)
		{
			return NotFound();
		}
		return Ok(game);
	}

	/// <include file='.docs/xmldocs/Controllers.xml' path='doc/method/member[@name="GameRecordController.GetAllGameRecords"]/*' />
	[HttpGet]
	public async Task<IActionResult> GetAllGameRecords()
	{
		var games = await _gameRecordService.GetAllGameRecordsAsync();
		if (games == null)
		{
			return NotFound();
		}
		return Ok(games);
	}
}
