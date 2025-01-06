namespace GamePlayService.Models.Pieces;
public class Bishop : ChessPiece
{
	public override List<string> GetPossibleMoves(string position, string[,] board)
	{
		List<string> moves = [];

		int row = position[1] - '1';
		int col = position[0] - 'a';

		for (int i = 1; i < 8; i++)
		{
			if (row + i < 8 && col + i < 8) moves.Add($"{(char)(col + i + 'a')}{row + i + 1}");
			if (row - i >= 0 && col - i >= 0) moves.Add($"{(char)(col - i + 'a')}{row - i + 1}");
			if (row + i < 8 && col - i >= 0) moves.Add($"{(char)(col - i + 'a')}{row + i + 1}");
			if (row - i >= 0 && col + i < 8) moves.Add($"{(char)(col + i + 'a')}{row - i + 1}");
		}

		return moves;
	}
}
