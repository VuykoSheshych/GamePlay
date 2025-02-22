namespace GamePlayService.Models;
public class GameSession
{
	public Guid Id { get; set; } = new Guid();
	public string PlayerWhite { get; set; } = string.Empty;
	public string PlayerBlack { get; set; } = string.Empty;
	public List<Move> Moves { get; set; } = [];
	public string CurrentFen { get; set; } = string.Empty;
	public string Result { get; set; } = "active";
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
