using System.Text;

namespace GamePlayService.Models;
public class BoardState
{
	public char[,] Board { get; private set; } = new char[8, 8]; // Матричне представлення дошки
	public string ActiveColor { get; private set; } = "w"; // "w" або "b"
	public string CastlingRights { get; private set; } = "KQkq"; // Дозволені рокіровки
	public string EnPassant { get; private set; } = "-"; // Поле для ен-пассан (або "-")
	public int HalfmoveClock { get; private set; } = 0; // Лічильник півходів
	public int FullmoveNumber { get; private set; } = 1; // Номер повного ходу

	public string FEN => GenerateFEN(); // Генерація FEN при запиті

	public BoardState()
	{
		LoadFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
	}

	public BoardState(string fen)
	{
		LoadFromFEN(fen);
	}

	private void LoadFromFEN(string fen)
	{
		var parts = fen.Split(' ');

		// Завантаження дошки
		var rows = parts[0].Split('/');
		for (int rank = 0; rank < 8; rank++)
		{
			int file = 0;
			foreach (var symbol in rows[rank])
			{
				if (char.IsDigit(symbol))
				{
					file += symbol - '0';
				}
				else
				{
					Board[rank, file++] = symbol;
				}
			}
		}

		// Завантаження інших параметрів
		ActiveColor = parts[1];
		CastlingRights = parts[2];
		EnPassant = parts[3];
		HalfmoveClock = int.Parse(parts[4]);
		FullmoveNumber = int.Parse(parts[5]);
	}

	private string GenerateFEN()
	{
		StringBuilder fenBuilder = new();

		// Генерація позиції
		for (int rank = 0; rank < 8; rank++)
		{
			int emptyCount = 0;
			for (int file = 0; file < 8; file++)
			{
				char piece = Board[rank, file];
				if (piece == '\0')
				{
					emptyCount++;
				}
				else
				{
					if (emptyCount > 0)
					{
						fenBuilder.Append(emptyCount);
						emptyCount = 0;
					}
					fenBuilder.Append(piece);
				}
			}
			if (emptyCount > 0)
			{
				fenBuilder.Append(emptyCount);
			}
			if (rank < 7)
			{
				fenBuilder.Append('/');
			}
		}

		// Додавання решти параметрів
		fenBuilder.Append($" {ActiveColor} {CastlingRights} {EnPassant} {HalfmoveClock} {FullmoveNumber}");

		return fenBuilder.ToString();
	}

	public static (int Rank, int File) ConvertToBoardIndex(string position)
	{
		return (8 - (position[1] - '0'), position[0] - 'a');
	}
}