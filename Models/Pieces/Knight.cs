namespace GamePlayService.Models.Pieces;
public class Knight : ChessPiece
{
	public override List<string> GetPossibleMoves(string position, BoardState boardState)
	{
		List<string> moves = [];
		int row = position[1] - '1';
		int col = position[0] - 'a';

		int[] dRow = { -2, -2, -1, -1, 1, 1, 2, 2 };
		int[] dCol = { -1, 1, -2, 2, -2, 2, -1, 1 };

		for (int i = 0; i < 8; i++)
		{
			int newRow = row + dRow[i];
			int newCol = col + dCol[i];
			if (newRow >= 0 && newRow < 8 && newCol >= 0 && newCol < 8)
			{
				moves.Add($"{(char)(newCol + 'a')}{newRow + 1}");
			}
		}
		return moves;
	}
}