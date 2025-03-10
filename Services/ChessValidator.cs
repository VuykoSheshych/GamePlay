using GamePlayService.Dtos;
using GamePlayService.Models;
using GamePlayService.Models.Pieces;

namespace GamePlayService.Services;
public static class ChessValidator
{
	public static MoveResultDto GetMoveValidationResult(BoardState boardState, MoveDto moveDto)
	{
		var (fromRow, fromCol) = ChessPiece.ConvertToBoardIndex(moveDto.From);

		char piece = boardState.Board[fromRow, fromCol];

		if (!IsPlayerMakeMoveWithTheirPiece(boardState.ActiveColor, piece))
			return MoveResultDto.Error("You cannot make moves with your opponent's pieces!");

		if (!IsMoveCompliesWithRulesForGivenPiece(piece, boardState, moveDto))
			return MoveResultDto.Error("Invalid move for this type of piece!");

		if (!IsTargetSquareOccupiedByAlliedPiece(piece, boardState, moveDto))
			return MoveResultDto.Error("The final square is already occupied by an allied piece!");

		var simulatedBoard = new BoardState(boardState.FEN);
		simulatedBoard.ApplyMove(moveDto);

		if (IsKingInCheck(simulatedBoard, boardState.ActiveColor))
			return MoveResultDto.Error("You cannot move into check!");

		if (char.ToLower(piece) == 'k' && IsCastlingMove(moveDto))
		{
			if (!IsCastlingValid(boardState, moveDto))
				return MoveResultDto.Error("Invalid castling move!");
		}

		return MoveResultDto.Success(CreateMoveSanNotation(boardState, moveDto));
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

		if (chessPiece == null) return false;

		var possibleMoves = chessPiece.GetPossibleMoves(boardState);

		if (!possibleMoves.Contains(moveDto.To)) return false;

		return true;
	}
	private static bool IsTargetSquareOccupiedByAlliedPiece(char piece, BoardState boardState, MoveDto moveDto)
	{
		var (toRow, toCol) = ChessPiece.ConvertToBoardIndex(moveDto.To);

		if (boardState.Board[toRow, toCol] != '\0' && (char.IsUpper(boardState.Board[toRow, toCol]) == char.IsUpper(piece)))
		{
			return false;
		}
		return true;
	}
	private static string CreateMoveSanNotation(BoardState boardState, MoveDto moveDto)
	{
		var (fromRow, fromCol) = ChessPiece.ConvertToBoardIndex(moveDto.From);
		var (toRow, toCol) = ChessPiece.ConvertToBoardIndex(moveDto.To);

		char piece = boardState.Board[fromRow, fromCol];
		char targetPiece = boardState.Board[toRow, toCol];

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
		if (char.ToLower(piece) == 'p' && (toRow == 0 || toRow == 7))
		{
			moveNotation += "=Q";
			//moveNotation += $"={moveDto.Promotion}"; // В майбутньому буде отримуватись фігура перетворення окремо
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
	private static bool IsPathClear(BoardState boardState, MoveDto moveDto, int direction)
	{
		var (fromRow, fromCol) = ChessPiece.ConvertToBoardIndex(moveDto.From);
		var (toRow, toCol) = ChessPiece.ConvertToBoardIndex(moveDto.To);

		int step = direction > 0 ? 1 : -1;
		for (int col = fromCol + step; col != toCol; col += step)
		{
			if (boardState.Board[fromRow, col] != '\0')
				return false;
		}

		return true;
	}
	private static bool IsCastlingValid(BoardState boardState, MoveDto moveDto)
	{
		string kingColor = boardState.ActiveColor;
		bool isKingSide = moveDto.To == "g1" || moveDto.To == "g8";
		bool isQueenSide = moveDto.To == "c1" || moveDto.To == "c8";

		// 🔹 1. Перевіряємо, чи є право на рокіровку у FEN-нотації
		string requiredRight = kingColor == "w"
			? (isKingSide ? "K" : "Q")
			: (isKingSide ? "k" : "q");

		if (!boardState.CastlingRights.Contains(requiredRight))
			return false; // Рокіровка недоступна

		// 🔹 2. Перевіряємо, чи немає фігур між королем і турою
		if (isKingSide && !IsPathClear(boardState, moveDto, 1)) return false;
		if (isQueenSide && !IsPathClear(boardState, moveDto, -1)) return false;

		// 🔹 3. Перевіряємо, чи король не під шахом
		if (IsKingInCheck(boardState, kingColor)) return false;

		// 🔹 4. Перевіряємо, чи король не проходить через атаковані клітинки
		string[] squaresToCheck = isKingSide
			? ["e1", "f1", "g1"]
			: ["e1", "d1", "c1"];

		if (kingColor == "b")
			squaresToCheck = isKingSide
				? ["e8", "f8", "g8"]
				: ["e8", "d8", "c8"];

		foreach (var square in squaresToCheck)
		{
			var tempBoard = new BoardState(boardState.FEN);
			tempBoard.ApplyMove(new MoveDto { From = moveDto.From, To = square });
			if (IsKingInCheck(tempBoard, kingColor))
				return false;
		}

		return true;
	}
	private static bool IsCastlingMove(MoveDto moveDto)
	{
		return moveDto.From == "e1" && (moveDto.To == "g1" || moveDto.To == "c1") ||  // Біла рокіровка
			   moveDto.From == "e8" && (moveDto.To == "g8" || moveDto.To == "c8");    // Чорна рокіровка
	}
	private static bool IsKingInCheck(BoardState boardState, string kingColor)
	{
		var kingPosition = FindKing(boardState, kingColor);
		if (kingPosition == null) return false;

		int kingRow = kingPosition.Value.row;
		int kingCol = kingPosition.Value.col;

		// Перевіряємо, чи атакують короля будь-які ворожі фігури
		foreach (var piecePosition in boardState.GetAllPieces(kingColor == "w" ? "b" : "w"))
		{
			var (row, col) = piecePosition;
			char pieceChar = boardState.Board[row, col];
			ChessPiece? piece = CreateChessPiece(pieceChar, kingColor == "w" ? "b" : "w", $"{(char)(col + 'a')}{8 - row}");

			if (piece == null) continue;

			var possibleMoves = piece.GetPossibleMoves(boardState);
			if (possibleMoves.Contains($"{(char)(kingCol + 'a')}{8 - kingRow}"))
			{
				return true;
			}
		}
		return false;
	}
	private static bool IsCheckmate(BoardState boardState, string kingColor)
	{
		if (!IsKingInCheck(boardState, kingColor)) return false;

		foreach (var piecePosition in boardState.GetAllPieces(kingColor))
		{
			var (row, col) = piecePosition;
			char pieceChar = boardState.Board[row, col];
			ChessPiece? piece = CreateChessPiece(pieceChar, kingColor, $"{(char)(col + 'a')}{8 - row}");

			if (piece == null) continue;

			var possibleMoves = piece.GetPossibleMoves(boardState);

			foreach (var move in possibleMoves)
			{
				var simulatedBoard = new BoardState(boardState.FEN);
				simulatedBoard.ApplyMove(new MoveDto { From = $"{(char)(col + 'a')}{8 - row}", To = move });

				if (!IsKingInCheck(simulatedBoard, kingColor))
				{
					return false; // Якщо хоча б один хід рятує короля — це не мат
				}
			}
		}

		return true; // Якщо жоден хід не рятує короля, це мат
	}
	private static (int row, int col)? FindKing(BoardState boardState, string kingColor)
	{
		char kingSymbol = kingColor == "w" ? 'K' : 'k';

		for (int row = 0; row < 8; row++)
		{
			for (int col = 0; col < 8; col++)
			{
				if (boardState.Board[row, col] == kingSymbol)
					return (row, col);
			}
		}
		return null;
	}
	private static ChessPiece? CreateChessPiece(char pieceType, string color, string position)
	{
		return char.ToLower(pieceType) switch
		{
			'p' => new Pawn(color, position),
			'r' => new Rook(color, position),
			'n' => new Knight(color, position),
			'b' => new Bishop(color, position),
			'q' => new Queen(color, position),
			'k' => new King(color, position),
			_ => null,
		};
	}
}