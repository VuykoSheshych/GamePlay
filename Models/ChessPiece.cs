namespace GamePlayService.Models;
public abstract class ChessPiece
{
	public string? Color { get; set; }
	public string? Position { get; set; }
	public abstract List<string> GetPossibleMoves(string position, string[,] board);
}