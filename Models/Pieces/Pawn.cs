using ChessShared.Enums;

namespace GamePlay.Models.Pieces;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="Pawn"]/*' />
public class Pawn(PlayerColor color, string position) : ChessPiece(color, position)
{
	/// <inheritdoc/>
	public override List<string> GetPossibleMoves(BoardState boardState)
	{
		List<string> moves = [];
		var (row, col) = ConvertToBoardIndex(Position);

		int direction = (Color == PlayerColor.White) ? -1 : 1;
		int startRow = (Color == PlayerColor.White) ? 6 : 1;

		// Один крок вперед (тільки якщо клітинка пуста)
		if (IsValidCell(row + direction, col) && boardState.Board[row + direction, col] == '\0')
		{
			AddMove(moves, row + direction, col);
		}

		// Два кроки вперед (з початкової позиції, якщо обидві клітинки вільні)
		if (row == startRow && boardState.Board[row + direction, col] == '\0' && boardState.Board[row + 2 * direction, col] == '\0')
		{
			AddMove(moves, row + 2 * direction, col);
		}

		// Взяття по діагоналі (ліворуч і праворуч)
		foreach (int side in new[] { -1, 1 })
		{
			if (IsValidCell(row + direction, col + side))
			{
				char target = boardState.Board[row + direction, col + side];
				if (target != '\0' && IsOpponentPiece(target))
				{
					AddMove(moves, row + direction, col + side);
				}
			}
		}

		// Взяття "на проході"
		if (!string.IsNullOrEmpty(boardState.EnPassant) && boardState.EnPassant.Length == 2)
		{
			int enPassantRow = boardState.EnPassant[1] - '1';
			int enPassantCol = boardState.EnPassant[0] - 'a';

			if (row == startRow + 3 * direction && Math.Abs(col - enPassantCol) == 1)
			{
				moves.Add(boardState.EnPassant);
			}
		}

		return moves;
	}
	private static void AddMove(List<string> moves, int row, int col)
	{
		moves.Add($"{(char)(col + 'a')}{8 - row}");
	}
}