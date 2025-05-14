using System.Security.Cryptography;
using System.Text;
using ChessShared.Dtos;
using ChessShared.Enums;

namespace GamePlay.Models;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="BoardState"]/*' />
public class BoardState
{
	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.Board"]/*' />
	public char[,] Board { get; private set; } = new char[8, 8];

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.ActiveColor"]/*' />
	public PlayerColor ActiveColor { get; private set; } = PlayerColor.White;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.CastlingRights"]/*' />
	public string CastlingRights { get; private set; } = "KQkq";

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.EnPassant"]/*' />
	public string EnPassant { get; private set; } = "-";

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.HalfmoveClock"]/*' />
	public int HalfmoveClock { get; private set; } = 0;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.FullmoveNumber"]/*' />
	public int FullmoveNumber { get; private set; } = 1;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.FEN"]/*' />
	public string FEN => GenerateFEN();

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.#ctor"]/*' />
	public BoardState()
	{
		LoadFromFEN("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
	}

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.#ctor(string)"]/*' />
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

		ActiveColor = parts[1] == "w" ? PlayerColor.White : PlayerColor.Black;
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

		var colorForFEN = ActiveColor == PlayerColor.White ? "w" : "b";

		fenBuilder.Append($" {colorForFEN} {CastlingRights} {EnPassant} {HalfmoveClock} {FullmoveNumber}");

		return fenBuilder.ToString();
	}

	private static char ConvertPromotionPieceTypeToString(PromotionPieceType? piece)
	{
		return piece switch
		{
			PromotionPieceType.Queen => 'Q',
			PromotionPieceType.Rook => 'R',
			PromotionPieceType.Bishop => 'B',
			PromotionPieceType.Knight => 'N',
			_ => throw new ArgumentException("Invalid piece type")
		};
	}

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.ApplyMove"]/*' />
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
		if (char.ToLower(piece) == 'p' && (toRank == 0 || toRank == 7))
		{
			char promotedPiece = ConvertPromotionPieceTypeToString(PromotionPieceType.Queen); // За замовчуванням перетворюємо в ферзя

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

		if (EnPassant == move.To)
		{
			if (piece == 'P')
			{
				// Якщо білий пішак з'їв чорного пішака
				Board[toRank + 1, toFile] = '\0'; // Видаляємо чорного пішака
			}
			else if (piece == 'p')
			{
				// Якщо чорний пішак з'їв білого пішака
				Board[toRank - 1, toFile] = '\0'; // Видаляємо білого пішака
			}
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

		ActiveColor = ActiveColor == PlayerColor.White ? PlayerColor.Black : PlayerColor.White;

		HalfmoveClock = (piece == 'p' || Board[toRank, toFile] != '\0') ? 0 : HalfmoveClock + 1;
		if (ActiveColor == PlayerColor.White) FullmoveNumber++;
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

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="BoardState.GetAllPieces"]/*' />
	public List<(int row, int col)> GetAllPieces(PlayerColor color)
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

				if ((color == PlayerColor.White && isWhitePiece) || (color == PlayerColor.Black && isBlackPiece))
				{
					pieces.Add((row, col));
				}
			}
		}

		return pieces;
	}
}