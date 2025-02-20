namespace GamePlayService.Models.Pieces;
public class Pawn : ChessPiece
{
	public override List<string> GetPossibleMoves(string position, string[,] board)
	{
		List<string> moves = [];
		int row = position[1] - '1';
		int col = position[0] - 'a';

		int direction = (Color == "white") ? 1 : -1;

		// Один крок вперед
		if (row + direction >= 0 && row + direction < 8)
		{
			moves.Add($"{(char)(col + 'a')}{row + direction + 1}");
		}

		// Два кроки вперед (з початкової позиції)
		if ((Color == "white" && row == 1) || (Color == "black" && row == 6))
		{
			if (row + 2 * direction >= 0 && row + 2 * direction < 8)
			{
				moves.Add($"{(char)(col + 'a')}{row + 2 * direction + 1}");
			}
		}
		return moves;
	}
}