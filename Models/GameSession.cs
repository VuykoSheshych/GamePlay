namespace GamePlayService.Models;
public class GameSession
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string PlayerWhite { get; set; } = string.Empty;
	public string PlayerBlack { get; set; } = string.Empty;
	public List<Move>? Moves { get; set; }
	public string CurrentFen { get; set; } = string.Empty;
	public bool IsFinished { get; set; } = false;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
