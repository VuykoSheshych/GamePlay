namespace GamePlayService.Models.Pieces;
public class Queen : ChessPiece
{
	public override List<string> GetPossibleMoves(string position, string[,] board)
	{
		List<string> moves = [];
		moves.AddRange(new Rook().GetPossibleMoves(position, board));
		moves.AddRange(new Bishop().GetPossibleMoves(position, board));
		return moves;
	}
}