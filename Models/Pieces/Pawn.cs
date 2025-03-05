namespace GamePlayService.Models.Pieces;
public class Pawn(string color, string position) : ChessPiece(color, position)
{
	public override List<string> GetPossibleMoves(BoardState boardState)
	{
		List<string> moves = [];
		var (row, col) = ConvertToBoardIndex(Position);

		int direction = (Color == "w") ? -1 : 1;
		int startRow = (Color == "w") ? 6 : 1;
		int promotionRow = (Color == "w") ? 0 : 7;

		// Один крок вперед (тільки якщо клітинка пуста)
		if (IsValidCell(row + direction, col) && boardState.Board[row + direction, col] == '\0')
		{
			AddMove(moves, row + direction, col, promotionRow);
		}

		// Два кроки вперед (з початкової позиції, якщо обидві клітинки вільні)
		if (row == startRow && boardState.Board[row + direction, col] == '\0' && boardState.Board[row + 2 * direction, col] == '\0')
		{
			AddMove(moves, row + 2 * direction, col, promotionRow);
		}

		// Взяття по діагоналі (ліворуч і праворуч)
		foreach (int side in new[] { -1, 1 })
		{
			if (IsValidCell(row + direction, col + side))
			{
				char target = boardState.Board[row + direction, col + side];
				if (target != '\0' && IsOpponentPiece(target))
				{
					AddMove(moves, row + direction, col + side, promotionRow);
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
	private static void AddMove(List<string> moves, int row, int col, int promotionRow)
	{
		string move = $"{(char)(col + 'a')}{8 - row}";
		if (row == promotionRow)
		{
			moves.Add($"{move}=Q");
			moves.Add($"{move}=R");
			moves.Add($"{move}=B");
			moves.Add($"{move}=N");
		}
		else
		{
			moves.Add(move);
		}
	}
}