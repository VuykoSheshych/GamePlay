using ChessShared.Enums;

namespace GamePlay.Models.Pieces;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="Knight"]/*' />
public class Knight(PlayerColor color, string position) : ChessPiece(color, position)
{
	/// <inheritdoc/>
	public override List<string> GetPossibleMoves(BoardState boardState)
	{
		List<string> moves = [];
		var (row, col) = ConvertToBoardIndex(Position);

		// Можливі ходи для коня
		var knightMoves = new (int, int)[]
		{
			(-2, -1), (-2, 1),  // Два кроки вгору, один вліво/вправо
            (2, -1), (2, 1),    // Два кроки вниз, один вліво/вправо
            (-1, -2), (-1, 2),  // Один крок вгору, два вліво/вправо
            (1, -2), (1, 2)     // Один крок вниз, два вліво/вправо
        };

		foreach (var (dRow, dCol) in knightMoves)
		{
			int newRow = row + dRow;
			int newCol = col + dCol;

			// Перевіряємо, чи клітинка знаходиться в межах дошки
			if (IsValidCell(newRow, newCol))
			{
				char target = boardState.Board[newRow, newCol];

				// Якщо клітинка порожня або містить фігуру супротивника — це допустимий хід
				if (target == '\0' || IsOpponentPiece(target))
				{
					moves.Add($"{(char)(newCol + 'a')}{8 - newRow}");
				}
			}
		}

		return moves;
	}
}