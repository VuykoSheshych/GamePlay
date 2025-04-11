namespace GamePlay.Models.Pieces;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="Bishop"]/*' />
public class Bishop(string color, string position) : ChessPiece(color, position)
{
	/// <inheritdoc/>
	public override List<string> GetPossibleMoves(BoardState boardState)
	{
		List<string> moves = [];
		var (row, col) = ConvertToBoardIndex(Position);

		// Можливі напрямки руху для слона (діагоналі)
		var directions = new (int, int)[]
		{
			(-1, -1),  // Вгору-вліво
            (-1, 1),   // Вгору-вправо
            (1, -1),   // Вниз-вліво
            (1, 1)     // Вниз-вправо
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