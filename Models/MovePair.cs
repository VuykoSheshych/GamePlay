namespace GamePlayService.Models;
public class MovePair
{
	public Guid Id { get; set; }
	public int MoveNumber { get; set; }
	public string? WhiteMoveFrom { get; set; }
	public string? WhiteMoveTo { get; set; }
	public string? WhiteSanNotation { get; set; }
	public string? BlackMoveFrom { get; set; }
	public string? BlackMoveTo { get; set; }
	public string? BlackSanNotation { get; set; }
	public string? FenBefore { get; set; }
	public string? FenAfter { get; set; }
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}