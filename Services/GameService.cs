using GamePlayService.Data;
using GamePlayService.Dtos.Game;
using GamePlayService.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePlayService.Services
{
	public class GameService(GameDbContext context)
	{
		private readonly GameDbContext _context = context;

		public async Task<GameRecord> CreateGameAsync()
		{
			var newGame = new GameRecord();
			_context.GameRecords.Add(newGame);
			await _context.SaveChangesAsync();
			return newGame;
		}
		public async Task<GameRecord?> GetGameByIdAsync(Guid id)
		{
			return await _context.GameRecords.FindAsync(id);
		}
		public async Task<List<GameRecord>?> GetAllGamesAsync()
		{
			return await _context.GameRecords.ToListAsync();
		}
		// public async Task<MoveResult> MakeMoveAsync(Guid gameId, MoveDto moveDto)
		// {
		// 	var game = await _context.Games
		// 		.Include(g => g.Moves)
		// 		.FirstOrDefaultAsync(g => g.Id == gameId);

		// 	if (game == null)
		// 	{
		// 		return new MoveResult
		// 		{
		// 			IsSuccess = false,
		// 			ErrorMessage = "Game not found."
		// 		};
		// 	}

		// 	if (!IsValidMove(game, moveDto))
		// 	{
		// 		return new MoveResult
		// 		{
		// 			IsSuccess = false,
		// 			ErrorMessage = "Invalid move."
		// 		};
		// 	}

		// 	var move = new Move
		// 	{
		// 		From = moveDto.From,
		// 		To = moveDto.To,
		// 		Promotion = moveDto.Promotion,
		// 		Timestamp = DateTime.UtcNow
		// 	};

		// 	game.Moves.Add(move);
		// 	game.BoardStateFEN = UpdateBoardState(new BoardState(game.BoardStateFEN), move);
		// 	game.CurrentTurn = game.CurrentTurn == "White" ? "Black" : "White";

		// 	await _context.SaveChangesAsync();

		// 	return new MoveResult
		// 	{
		// 		IsSuccess = true,
		// 		Move = move,
		// 		BoardStateFEN = game.BoardStateFEN,
		// 		CurrentTurn = game.CurrentTurn
		// 	};
		// }

		// public string UpdateBoardState(BoardState boardState, Move move)
		// {
		// 	var newBoard = (string[,])boardState.GetBoardFromFEN().Clone();

		// 	var toPosition = boardState.ConvertToBoardIndex(move.To);
		// 	var fromPosition = boardState.ConvertToBoardIndex(move.From);

		// 	newBoard[toPosition.Rank, toPosition.File] = newBoard[fromPosition.Rank, fromPosition.File];
		// 	newBoard[fromPosition.Rank, fromPosition.File] = "";

		// 	if (!string.IsNullOrEmpty(move.Promotion))
		// 	{
		// 		newBoard[toPosition.Rank, toPosition.File] = move.Promotion;
		// 	}

		// 	boardState.BoardToFEN(newBoard);

		// 	return boardState.FEN;
		// }

		// public bool IsValidMove(Game game, MoveDto moveDto)
		// {
		// 	// Логіка перевірки ходу:
		// 	// 1. Перевірити, чи гравець робить хід своєю фігурою.
		// 	// 2. Перевірити, чи хід відповідає правилам для даної фігури.
		// 	// 3. Перевірити, чи хід не суперечить поточному стану дошки.
		// 	// 4. Обробка спеціальних випадків (шах, мат, рокіровка тощо).
		// 	return true; // або false, якщо хід недійсний.
		// }

		/* public bool IsKingInCheck(string kingPosition)
		{
			foreach (var piece in pieces)
			{
				var possibleMoves = piece.GetPossibleMoves(piece.Position, board);
				if (possibleMoves.Contains(kingPosition))
				{
					return true;
				}
			}
			return false;
		} */
	}
}
