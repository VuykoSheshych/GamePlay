namespace GamePlayService.Models;
public class Game
{
	public Guid Id { get; set; } = Guid.NewGuid(); // Унікальний ідентифікатор гри
	public string BoardStateFEN { get; set; } // Поточний стан шахової дошки
	public string CurrentTurn { get; set; } = "White"; // "White" або "Black" (чий хід зараз)
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // Час створення гри
	public bool IsFinished { get; set; } = false;
	public List<Move> Moves { get; set; } // Історія ходів
}