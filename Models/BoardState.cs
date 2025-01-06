using System.Text;

namespace GamePlayService.Models;

public class BoardState
{
	public string FEN { get; set; }

	public BoardState()
	{
		FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
	}

	public BoardState(string fen)
	{
		FEN = fen;
	}

	public void BoardToFEN(string[,] board)
	{
		StringBuilder fenBuilder = new();

		for (int i = 0; i < 8; i++)
		{
			int emptyCount = 0;
			for (int j = 0; j < 8; j++)
			{
				var cell = board[i, j];
				if (string.IsNullOrEmpty(cell))
				{
					emptyCount++;
				}
				else
				{
					if (emptyCount > 0)
					{
						fenBuilder.Append(emptyCount.ToString());
						emptyCount = 0;
					}
					fenBuilder.Append(cell);
				}
			}
			if (emptyCount > 0)
			{
				fenBuilder.Append(emptyCount.ToString());
			}
			if (i < 7)
			{
				fenBuilder.Append("/");
			}
		}

		// Додаткові дані в FEN (потрібно буде дописати відповідно до стану гри)
		FEN = fenBuilder.ToString() + " w KQkq - 0 1";
	}

	public string[,] GetBoardFromFEN()
	{
		string[,] board = new string[8, 8];

		var rows = FEN.Split(' ')[0].Split('/');
		for (int i = 0; i < 8; i++)
		{
			int col = 0;
			foreach (var c in rows[i])
			{
				if (char.IsDigit(c))
				{
					col += int.Parse(c.ToString());
				}
				else
				{
					board[i, col] = c.ToString();
					col++;
				}
			}
		}
		return board;
	}
	public (int Rank, int File) ConvertToBoardIndex(string position)
	{
		int rank = 8 - int.Parse(position[1].ToString()); // Рядок (інвертуємо, бо шахівниця йде знизу вгору)
		int file = position[0] - 'a'; // Стовпець (a = 0, b = 1, ..., h = 7)
		return (rank, file);
	}
}
