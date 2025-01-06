using GamePlayService.Models;

namespace GamePlayService.Services;
public class MoveResult
{
	public bool IsSuccess { get; set; }
	public string? ErrorMessage { get; set; }
	public Move? Move { get; set; }
	public string? BoardStateFEN { get; set; }
	public string? CurrentTurn { get; set; }
}