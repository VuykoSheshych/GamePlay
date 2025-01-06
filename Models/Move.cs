namespace GamePlayService.Models;
public class Move
{
	public Guid MoveId { get; set; } // Унікальний ідентифікатор ходу
	public Guid GameId { get; set; } // Гра, до якої належить хід
	public string Player { get; set; } // "White" або "Black"
	public string From { get; set; } // Початкова клітинка (наприклад, "e2")
	public string To { get; set; } // Кінцева клітинка (наприклад, "e4")
	public string Piece { get; set; } // Фігура, що рухається (наприклад, "P" для пішака)
	public string? CapturedPiece { get; set; } // Якщо був захоплений суперник (опціонально)
	public string? Promotion { get; set; } // Перетворення фігури (опціонально)
	public DateTime Timestamp { get; set; } // Час, коли зроблено хід
}