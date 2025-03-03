using System.Text.Json;
using GamePlayService.Dtos.Game;
using GamePlayService.Models;
using GamePlayService.Models.Pieces;
using StackExchange.Redis;

namespace GamePlayService.Services;

public class GameSessionService(IConnectionMultiplexer redis, GameRecordService gameService)
{
	private readonly GameRecordService _gameRecordService = gameService;
	private readonly IDatabase _db = redis.GetDatabase();
	private readonly TimeSpan _expiration = TimeSpan.FromHours(1);
	public async Task<GameSession?> GetGameSessionAsync(string gameId)
	{
		var json = await _db.StringGetAsync($"game:{gameId}");
		return json.IsNullOrEmpty ? null : JsonSerializer.Deserialize<GameSession>(json!);
	}
	public async Task<Guid> CreateGameSessionAsync(Dictionary<string, string> players)
	{
		GameSession newGame = new()
		{
			PlayerWhite = players.Keys.First(),
			PlayerWhiteConnectionId = players.Values.First(),
			PlayerBlack = players.Keys.Last(),
			PlayerBlackConnectionId = players.Values.Last()
		};

		var json = JsonSerializer.Serialize(newGame);
		await _db.StringSetAsync($"game:{newGame.Id}", json, _expiration);
		return newGame.Id;
	}
	public async Task RemoveGameSessionAsync(string gameId, string result)
	{
		var game = await GetGameSessionAsync(gameId);

		if (game != null)
		{
			game.Result = result;
			await SaveGameRecordAsync(game);
		}
		await _db.KeyDeleteAsync($"game:{gameId}");
	}
	public async Task SaveGameSessionAsync(GameSession gameSession)
	{
		var json = JsonSerializer.Serialize(gameSession);
		await _db.StringSetAsync($"game:{gameSession.Id}", json, _expiration);
	}
	public async Task<string> TryMakeMoveAsync(string gameId, MoveDto moveDto)
	{
		var game = await GetGameSessionAsync(gameId);
		if (game == null) return "Game not found!";

		var actualBoardState = new BoardState(game.CurrentFen);
		var moveResult = ChessValidator.GetMoveValidationResult(actualBoardState, moveDto);

		if (moveResult == "You cannot make moves with your opponent's pieces!" ||
			moveResult == "Invalid move for this type of piece!" ||
			moveResult == "The final square is already occupied by an allied piece!") return moveResult;

		game.Moves.Add(new Move()
		{
			MoveNumber = actualBoardState.FullmoveNumber,
			PlayerColor = actualBoardState.ActiveColor,
			From = moveDto.From,
			To = moveDto.To,
			Promotion = moveDto.Promotion,
			SanNotation = moveResult,
			FenBefore = actualBoardState.FEN
		});

		actualBoardState.ApplyMove(moveDto);
		game.CurrentFen = actualBoardState.FEN;

		await SaveGameSessionAsync(game);
		return moveResult;
	}
	public async Task SaveGameRecordAsync(GameSession gameSession)
	{
		await _gameRecordService.AddGameRecordAsync(new GameRecord()
		{
			Id = gameSession.Id,
			PlayerWhite = gameSession.PlayerWhite,
			PlayerBlack = gameSession.PlayerBlack,
			Moves = gameSession.Moves,
			Result = gameSession.Result
		});
	}
}