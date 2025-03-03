using GamePlayService.Dtos.Game;
using GamePlayService.Models;
using GamePlayService.Models.Pieces;

namespace GamePlayService.Services;

public static class ChessValidator
{
	public static string GetMoveValidationResult(BoardState boardState, MoveDto moveDto)
	{
		var (fromRank, fromFile) = BoardState.ConvertToBoardIndex(moveDto.From);

		char piece = boardState.Board[fromRank, fromFile];

		Console.WriteLine($"Checking move from {moveDto.From} to {moveDto.To}");
		Console.WriteLine($"Piece at {moveDto.From}: {piece} (expected color: {boardState.ActiveColor})");

		if (!IsPlayerMakeMoveWithTheirPiece(boardState.ActiveColor, piece))
			return "You cannot make moves with your opponent's pieces!";

		if (!IsMoveCompliesWithRulesForGivenPiece(piece, boardState, moveDto))
			return "Invalid move for this type of piece!";

		if (!IsTargetSquareOccupiedByAlliedPiece(piece, boardState, moveDto))
			return "The final square is already occupied by an allied piece!";

		// 4. Обробка спеціальних випадків (шах, мат, рокіровка тощо).

		return CreateMoveSanNotation(boardState, moveDto);
	}
	private static bool IsPlayerMakeMoveWithTheirPiece(string activeColor, char piece)
	{
		if ((activeColor == "w" && char.IsLower(piece)) || (activeColor == "b" && char.IsUpper(piece)))
		{
			return false;
		}
		return true;
	}
	private static bool IsMoveCompliesWithRulesForGivenPiece(char piece, BoardState boardState, MoveDto moveDto)
	{
		ChessPiece? chessPiece = CreateChessPiece(piece, boardState.ActiveColor, moveDto.From);
		if (chessPiece == null)
		{
			return false;
		}
		PrintBoard(boardState.Board);
		var possibleMoves = chessPiece.GetPossibleMoves(moveDto.From, boardState);

		if (!possibleMoves.Contains(moveDto.To))
		{
			return false;
		}
		return true;
	}
	private static void PrintBoard(char[,] board)
	{
		for (int r = 0; r < 8; r++)
		{
			for (int c = 0; c < 8; c++)
			{
				char piece = board[r, c];
				Console.Write(piece == '\0' ? '.' : piece);
				Console.Write(' ');
			}
			Console.WriteLine();
		}
	}
	private static bool IsTargetSquareOccupiedByAlliedPiece(char piece, BoardState boardState, MoveDto moveDto)
	{
		var (toRank, toFile) = BoardState.ConvertToBoardIndex(moveDto.To);

		if (boardState.Board[toRank, toFile] != '\0' && (char.IsUpper(boardState.Board[toRank, toFile]) == char.IsUpper(piece)))
		{
			return false;
		}
		return true;
	}
	private static string CreateMoveSanNotation(BoardState boardState, MoveDto moveDto)
	{
		var (fromRank, fromFile) = BoardState.ConvertToBoardIndex(moveDto.From);
		var (toRank, toFile) = BoardState.ConvertToBoardIndex(moveDto.To);

		char piece = boardState.Board[fromRank, fromFile];
		char targetPiece = boardState.Board[toRank, toFile];

		string pieceNotation = GetPieceNotation(piece);
		string moveNotation = moveDto.To;

		// Додаємо "x" якщо це взяття
		bool isCapture = targetPiece != '\0';
		if (isCapture)
		{
			if (char.ToLower(piece) == 'p') // Якщо це пішак, додаємо файл (букву колонки)
				moveNotation = $"{moveDto.From[0]}x{moveDto.To}";
			else
				moveNotation = $"{pieceNotation}x{moveDto.To}";
		}
		else if (pieceNotation != "")
		{
			moveNotation = $"{pieceNotation}{moveDto.To}";
		}

		// Перевірка на рокіровку
		if (char.ToLower(piece) == 'k')
		{
			if (moveDto.From == "e1" && moveDto.To == "g1") return "O-O"; // Коротка рокіровка (білий)
			if (moveDto.From == "e1" && moveDto.To == "c1") return "O-O-O"; // Довга рокіровка (білий)
			if (moveDto.From == "e8" && moveDto.To == "g8") return "O-O"; // Коротка рокіровка (чорний)
			if (moveDto.From == "e8" && moveDto.To == "c8") return "O-O-O"; // Довга рокіровка (чорний)
		}

		// Додаємо "=", якщо це перетворення пішака
		if (char.ToLower(piece) == 'p' && (toRank == 0 || toRank == 7))
		{
			moveNotation += "=Q"; // Зазвичай ферзь, але можна додати вибір фігури
		}

		// Перевірка шаху або мату
		var simulatedBoard = new BoardState(boardState.FEN);
		simulatedBoard.ApplyMove(moveDto);
		if (IsKingInCheck(simulatedBoard, boardState.ActiveColor == "w" ? "b" : "w"))
		{
			if (IsCheckmate(simulatedBoard, boardState.ActiveColor == "w" ? "b" : "w"))
			{
				moveNotation += "#";
			}
			else
			{
				moveNotation += "+";
			}
		}

		return moveNotation;
	}
	private static string GetPieceNotation(char piece)
	{
		return piece switch
		{
			'K' or 'k' => "K",
			'Q' or 'q' => "Q",
			'R' or 'r' => "R",
			'B' or 'b' => "B",
			'N' or 'n' => "N",
			_ => ""
		};
	}
	private static bool IsKingInCheck(BoardState boardState, string kingColor)
	{
		// Тут має бути логіка, яка перевіряє, чи король під атакою
		return false;
	}

	// Метод для перевірки, чи це мат
	private static bool IsCheckmate(BoardState boardState, string kingColor)
	{
		// Тут має бути логіка, яка перевіряє, чи у короля немає ходів для виходу з шаху
		return false;
	}
	private static ChessPiece? CreateChessPiece(char pieceType, string color, string position)
	{
		return char.ToLower(pieceType) switch
		{
			'p' => new Pawn { Color = color, Position = position },
			'r' => new Rook { Color = color, Position = position },
			'n' => new Knight { Color = color, Position = position },
			'b' => new Bishop { Color = color, Position = position },
			'q' => new Queen { Color = color, Position = position },
			'k' => new King { Color = color, Position = position },
			_ => null,
		};
	}
}