using System.Text;
using GamePlayService.Dtos;

namespace GamePlayService.Models;
public class BoardState
{
	public char[,] Board { get; private set; } = new char[8, 8];
	public string ActiveColor { get; private set; } = "w";
	public string CastlingRights { get; private set; } = "KQkq";
	public string EnPassant { get; private set; } = "-";
	public int HalfmoveClock { get; private set; } = 0;
	public int FullmoveNumber { get; private set; } = 1;
	public string FEN => GenerateFEN();
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

		ActiveColor = parts[1];
		CastlingRights = parts[2];
		EnPassant = parts[3];
		HalfmoveClock = int.Parse(parts[4]);
		FullmoveNumber = int.Parse(parts[5]);
	}
	private string GenerateFEN()
	{
		StringBuilder fenBuilder = new();

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

		fenBuilder.Append($" {ActiveColor} {CastlingRights} {EnPassant} {HalfmoveClock} {FullmoveNumber}");

		return fenBuilder.ToString();
	}
	public void ApplyMove(MoveDto move)
	{
		var (fromRank, fromFile) = ChessPiece.ConvertToBoardIndex(move.From);
		var (toRank, toFile) = ChessPiece.ConvertToBoardIndex(move.To);

		char piece = Board[fromRank, fromFile];

		if (char.ToLower(piece) == 'k' && Math.Abs(fromFile - toFile) == 2)
		{
			HandleCastling(fromRank, toFile);
		}

		// Обробка перетворення пішака
		if (char.ToLower(piece) == 'p' && (toRank == 0 || toRank == 7) && !string.IsNullOrEmpty(move.Promotion))
		{
			char promotedPiece = move.Promotion[0];

			if (char.IsLower(piece))
				promotedPiece = char.ToLower(promotedPiece);
			else
				promotedPiece = char.ToUpper(promotedPiece);

			Board[toRank, toFile] = promotedPiece;
		}
		else
		{
			Board[toRank, toFile] = piece;
		}

		Board[fromRank, fromFile] = '\0';

		EnPassant = "-";
		if (char.ToLower(piece) == 'p' && Math.Abs(fromRank - toRank) == 2)
		{
			int enPassantRank = (fromRank + toRank) / 2;
			EnPassant = $"{move.From[0]}{8 - enPassantRank}";
		}

		if (piece == 'K') CastlingRights = CastlingRights.Replace("K", "").Replace("Q", "");
		if (piece == 'k') CastlingRights = CastlingRights.Replace("k", "").Replace("q", "");
		if (piece == 'R' && move.From == "h1") CastlingRights = CastlingRights.Replace("K", "");
		if (piece == 'R' && move.From == "a1") CastlingRights = CastlingRights.Replace("Q", "");
		if (piece == 'r' && move.From == "h8") CastlingRights = CastlingRights.Replace("k", "");
		if (piece == 'r' && move.From == "a8") CastlingRights = CastlingRights.Replace("q", "");

		ActiveColor = ActiveColor == "w" ? "b" : "w";

		HalfmoveClock = (piece == 'p' || Board[toRank, toFile] != '\0') ? 0 : HalfmoveClock + 1;
		if (ActiveColor == "w") FullmoveNumber++;
	}
	private void HandleCastling(int rank, int toFile)
	{
		if (toFile == 6) // Коротка рокіровка (король йде e1 → g1 або e8 → g8)
		{
			Board[rank, 5] = Board[rank, 7]; // Тура рухається h1 → f1 або h8 → f8
			Board[rank, 7] = '\0'; // Видаляємо туру зі старого місця
		}
		else if (toFile == 2) // Довга рокіровка (король йде e1 → c1 або e8 → c8)
		{
			Board[rank, 3] = Board[rank, 0]; // Тура рухається a1 → d1 або a8 → d8
			Board[rank, 0] = '\0'; // Видаляємо туру зі старого місця
		}
	}
	public List<(int row, int col)> GetAllPieces(string color)
	{
		List<(int row, int col)> pieces = [];

		for (int row = 0; row < 8; row++)
		{
			for (int col = 0; col < 8; col++)
			{
				char piece = Board[row, col];

				if (piece == '\0') continue;

				bool isWhitePiece = char.IsUpper(piece);
				bool isBlackPiece = char.IsLower(piece);

				if ((color == "w" && isWhitePiece) || (color == "b" && isBlackPiece))
				{
					pieces.Add((row, col));
				}
			}
		}

		return pieces;
	}
}