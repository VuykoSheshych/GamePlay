namespace GamePlayService.Models.Pieces;
public class Rook(string color, string position) : ChessPiece(color, position)
{
	public override List<string> GetPossibleMoves(BoardState boardState)
	{
		List<string> moves = [];
		var (row, col) = ConvertToBoardIndex(Position);

		// Можливі напрямки руху для тури (горизонтальні та вертикальні)
		var directions = new (int, int)[]
		{
			(-1, 0),  // Вгору
            (1, 0),   // Вниз
            (0, -1),  // Вліво
            (0, 1)    // Вправо
        };

		foreach (var (dRow, dCol) in directions)
		{
			int currentRow = row;
			int currentCol = col;

			// Рухаємося в кожному з напрямків поки не зустрінемо межу дошки або фігуру
			while (true)
			{
				currentRow += dRow;
				currentCol += dCol;

				// Перевіряємо, чи клітинка знаходиться в межах дошки
				if (!IsValidCell(currentRow, currentCol)) break;

				char target = boardState.Board[currentRow, currentCol];

				// Якщо клітинка порожня, додаємо хід
				if (target == '\0')
				{
					moves.Add($"{(char)(currentCol + 'a')}{8 - currentRow}");
				}
				// Якщо клітинка містить фігуру супротивника, додаємо хід
				else if (IsOpponentPiece(target))
				{
					moves.Add($"{(char)(currentCol + 'a')}{8 - currentRow}");
					break; // Не можна йти далі, зустріли фігуру
				}
				// Якщо клітинка містить свою фігуру, зупиняємо рух
				else
				{
					break;
				}
			}
		}

		return moves;
	}
}