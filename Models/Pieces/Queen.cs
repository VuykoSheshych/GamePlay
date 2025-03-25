namespace GamePlayService.Models.Pieces;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="Queen"]/*' />
public class Queen(string color, string position) : ChessPiece(color, position)
{
	/// <inheritdoc/>
	public override List<string> GetPossibleMoves(BoardState boardState)
	{
		List<string> moves = [];

		moves.AddRange(new Rook(Color, Position).GetPossibleMoves(boardState));
		moves.AddRange(new Bishop(Color, Position).GetPossibleMoves(boardState));

		return moves;
	}
}