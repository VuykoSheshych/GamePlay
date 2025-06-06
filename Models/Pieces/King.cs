using ChessShared.Enums;

namespace GamePlay.Models.Pieces;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="King"]/*' />
public class King(PlayerColor color, string position) : ChessPiece(color, position)
{
	/// <inheritdoc/>
	public override List<string> GetPossibleMoves(BoardState boardState)
	{
		List<string> moves = [];
		var (row, col) = ConvertToBoardIndex(Position);

		// Можливі напрямки руху для короля: вгору, вниз, вліво, вправо та по діагоналях
		var directions = new (int, int)[]
		{
			(-1, 0),  // Вгору
			(1, 0),   // Вниз
			(0, -1),  // Вліво
			(0, 1),   // Вправо
			(-1, -1), // Ліворуч вниз (діагональ)
			(-1, 1),  // Праворуч вниз (діагональ)
			(1, -1),  // Ліворуч вгору (діагональ)
			(1, 1)    // Праворуч вгору (діагональ)
		};

		foreach (var (dRow, dCol) in directions)
		{
			int currentRow = row + dRow;
			int currentCol = col + dCol;

			// Перевіряємо, чи клітинка знаходиться в межах дошки
			if (IsValidCell(currentRow, currentCol))
			{
				char target = boardState.Board[currentRow, currentCol];

				// Якщо клітинка порожня або містить фігуру супротивника, додаємо хід
				if (target == '\0' || IsOpponentPiece(target))
				{
					moves.Add($"{(char)(currentCol + 'a')}{8 - currentRow}");
				}
			}
		}

		// 🔹 Додаємо можливі ходи для рокіровки
		AddCastlingMoves(boardState, moves);

		return moves;
	}
	private void AddCastlingMoves(BoardState boardState, List<string> moves)
	{
		// Перевіряємо, чи король ще має право на рокіровку
		if ((Color == PlayerColor.White && Position == "e1") || (Color == PlayerColor.Black && Position == "e8"))
		{
			bool canCastleKingside = Color == PlayerColor.White ? boardState.CastlingRights.Contains('K') : boardState.CastlingRights.Contains('k');
			bool canCastleQueenside = Color == PlayerColor.White ? boardState.CastlingRights.Contains('Q') : boardState.CastlingRights.Contains('q');

			if (canCastleKingside && IsKingsideCastlingPossible(boardState))
			{
				moves.Add(Color == PlayerColor.White ? "g1" : "g8"); // Коротка рокіровка
			}
			if (canCastleQueenside && IsQueensideCastlingPossible(boardState))
			{
				moves.Add(Color == PlayerColor.White ? "c1" : "c8"); // Довга рокіровка
			}
		}
	}
	private bool IsKingsideCastlingPossible(BoardState boardState)
	{
		int rank = (Color == PlayerColor.White) ? 7 : 0;

		// Перевіряємо, чи клітинки між королем і турою порожні
		return boardState.Board[rank, 5] == '\0' && boardState.Board[rank, 6] == '\0';
	}
	private bool IsQueensideCastlingPossible(BoardState boardState)
	{
		int rank = (Color == PlayerColor.White) ? 7 : 0;

		// Перевіряємо, чи клітинки між королем і турою порожні
		return boardState.Board[rank, 1] == '\0' && boardState.Board[rank, 2] == '\0' && boardState.Board[rank, 3] == '\0';
	}
}