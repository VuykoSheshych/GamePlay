namespace GamePlayService.Models.Pieces;
public class Queen(string color, string position) : ChessPiece(color, position)
{
	public override List<string> GetPossibleMoves(BoardState boardState)
	{
		List<string> moves = [];

		moves.AddRange(new Rook(Color, Position).GetPossibleMoves(boardState));
		moves.AddRange(new Bishop(Color, Position).GetPossibleMoves(boardState));

		return moves;
	}
}