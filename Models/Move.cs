namespace GamePlayService.Models;
public class Move
{
	public Guid Id { get; set; } = new Guid(); // Первинний ключ
	public int MoveNumber { get; set; } // Порядковий номер ходу
	public PlayerColor Player { get; set; } // Білий чи Чорний
	public string From { get; set; } = string.Empty; // Звідки ходить фігура
	public string To { get; set; } = string.Empty; // Куди ходить фігура
	public string? SanNotation { get; set; } // SAN-нотація ("Nf3", "e8=Q")
	public string? FenBefore { get; set; } // FEN до ходу
	public string? FenAfter { get; set; } // FEN після ходу
	public DateTime Timestamp { get; set; } = DateTime.UtcNow; // Час ходу
}
public enum PlayerColor
{
	White,
	Black
}