namespace GamePlayService.Models.Pieces;
public class Queen : ChessPiece
{
	public override List<string> GetPossibleMoves(string position, BoardState boardState)
	{
		List<string> moves = [];
		moves.AddRange(new Rook().GetPossibleMoves(position, boardState));
		moves.AddRange(new Bishop().GetPossibleMoves(position, boardState));
		return moves;
	}
}