namespace GamePlayService.Models;
public abstract class ChessPiece(string color, string position)
{
	public string Color = color;
	public string Position = position;
	public abstract List<string> GetPossibleMoves(BoardState boardState);
	public static (int row, int col) ConvertToBoardIndex(string position)
	{
		int row = 8 - (position[1] - '0');
		int col = position[0] - 'a';
		return (row, col);
	}
	protected bool IsValidCell(int row, int col)
	{
		return row >= 0 && row < 8 && col >= 0 && col < 8;
	}
	protected bool IsOpponentPiece(char piece)
	{
		if (piece == '\0') return false;

		bool isWhitePiece = char.IsUpper(piece);
		bool isBlackPiece = char.IsLower(piece);

		return (Color == "w" && isBlackPiece) || (Color == "b" && isWhitePiece);
	}
}