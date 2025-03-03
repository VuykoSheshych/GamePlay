namespace GamePlayService.Models.Pieces;
public class Pawn : ChessPiece
{
	public override List<string> GetPossibleMoves(string position, BoardState boardState)
	{
		List<string> moves = [];
		var (row, col) = BoardState.ConvertToBoardIndex(position);

		int direction = (Color == "w") ? -1 : 1;
		int startRow = (Color == "w") ? 6 : 1;
		int promotionRow = (Color == "w") ? 0 : 7;

		// Один крок вперед (тільки якщо клітинка пуста)
		if (IsValidCell(row + direction, col) && boardState.Board[row + direction, col] == '\0')
		{
			AddMove(moves, row + direction, col, promotionRow);
		}

		Console.WriteLine($"Checking two-step move for {position}. StartRow: {startRow}, row: {row}, dir: {direction}");
		Console.WriteLine($"Cell {row + direction},{col} = {boardState.Board[row + direction, col]}");
		Console.WriteLine($"Cell {row + 2 * direction},{col} = {boardState.Board[row + 2 * direction, col]}");

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

	private bool IsValidCell(int row, int col)
	{
		return row >= 0 && row < 8 && col >= 0 && col < 8;
	}

	private void AddMove(List<string> moves, int row, int col, int promotionRow)
	{
		string move = $"{(char)(col + 'a')}{8 - row}";
		if (row == promotionRow)
		{
			// Додаємо можливі перетворення
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

	private bool IsOpponentPiece(char piece)
	{
		if (piece == '\0') return false; // Порожня клітинка — не противник

		bool isWhitePiece = char.IsUpper(piece);
		bool isBlackPiece = char.IsLower(piece);

		return (Color == "w" && isBlackPiece) || (Color == "b" && isWhitePiece);
	}
}