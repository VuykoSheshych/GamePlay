namespace GamePlayService.Models;

public class GameRecord
{
	public Guid Id { get; set; } = new Guid();
	public string PlayerWhite { get; set; } = string.Empty;
	public string PlayerBlack { get; set; } = string.Empty;
	public virtual List<Move>? Moves { get; set; }
	public string Result { get; set; } = string.Empty;
	public DateTime Date { get; set; } = DateTime.UtcNow;
}