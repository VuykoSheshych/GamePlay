using System.ComponentModel.DataAnnotations;

namespace GamePlayService.Dtos.Game;
public class MoveDto
{
	public required string Player { get; set; } // "White" або "Black"
	public required string From { get; set; } // Початкова клітинка (наприклад, "e2")
	public required string To { get; set; } // Кінцева клітинка (наприклад, "e4")
	public string? Promotion { get; set; } // Перетворення фігури (опціонально)
}
