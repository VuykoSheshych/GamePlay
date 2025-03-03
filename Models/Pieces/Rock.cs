namespace GamePlayService.Models.Pieces;
public class Rook : ChessPiece
{
	public override List<string> GetPossibleMoves(string position, BoardState boardState)
	{
		List<string> moves = [];

		int row = position[1] - '1';
		int col = position[0] - 'a';

		for (int i = 0; i < 8; i++)
		{
			if (i != row) moves.Add($"{(char)(col + 'a')}{i + 1}");
			if (i != col) moves.Add($"{(char)(i + 'a')}{row + 1}");
		}

		return moves;
	}
}
