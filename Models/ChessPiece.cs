namespace GamePlayService.Models;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="ChessPiece"]/*' />
public abstract class ChessPiece(string color, string position)
{
	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="ChessPiece.Color"]/*' />
	public string Color { get; set; } = color;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="ChessPiece.Position"]/*' />
	public string Position { get; set; } = position;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="ChessPiece.GetPossibleMoves"]/*' />
	public abstract List<string> GetPossibleMoves(BoardState boardState);

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="ChessPiece.ConvertToBoardIndex"]/*' />
	public static (int row, int col) ConvertToBoardIndex(string position)
	{
		int row = 8 - (position[1] - '0');
		int col = position[0] - 'a';
		return (row, col);
	}

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="ChessPiece.IsValidCell"]/*' />
	protected bool IsValidCell(int row, int col)
	{
		return row >= 0 && row < 8 && col >= 0 && col < 8;
	}

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="ChessPiece.IsOpponentPiece"]/*' />
	protected bool IsOpponentPiece(char piece)
	{
		if (piece == '\0') return false;

		bool isWhitePiece = char.IsUpper(piece);
		bool isBlackPiece = char.IsLower(piece);

		return (Color == "w" && isBlackPiece) || (Color == "b" && isWhitePiece);
	}
}