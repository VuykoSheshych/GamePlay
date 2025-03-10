using GamePlayService.Dtos;

namespace GamePlayService.Models;
public class GameSession
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public required PlayerDto WhitePlayer { get; set; }
	public required PlayerDto BlackPlayer { get; set; }
	public List<Move> Moves { get; set; } = [];
	public string CurrentFen { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
	public string Result { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
