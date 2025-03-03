using System.Text;
using GamePlayService.Dtos.Game;

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
		var (fromRank, fromFile) = ConvertToBoardIndex(move.From);
		var (toRank, toFile) = ConvertToBoardIndex(move.To);

		char piece = Board[fromRank, fromFile];

		Board[toRank, toFile] = piece;
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
	public static (int Rank, int File) ConvertToBoardIndex(string position)
	{
		int rank = 8 - (position[1] - '0');
		int file = position[0] - 'a';

		return (rank, file);
	}
}