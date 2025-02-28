using System.Text.Json;
using GamePlayService.Dtos.Game;
using GamePlayService.Models;
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
	public bool IsValidMove(BoardState boardState, MoveDto moveDto)
	{
		// Логіка перевірки ходу:
		// 1. Перевірити, чи гравець робить хід своєю фігурою.
		// 2. Перевірити, чи хід відповідає правилам для даної фігури.
		// 3. Перевірити, чи хід не суперечить поточному стану дошки.
		// 4. Обробка спеціальних випадків (шах, мат, рокіровка тощо).
		return true; // або false, якщо хід недійсний.
	}
	public async Task MakeMoveAsync(string gameId, MoveDto moveDto)
	{
		var game = await GetGameSessionAsync(gameId);
		if (game == null) return;

		var actualBoardState = new BoardState(game.CurrentFen);

		if (!IsValidMove(actualBoardState, moveDto)) return;

		game.Moves.Add(new Move()
		{
			MoveNumber = actualBoardState.FullmoveNumber,
			PlayerColor = actualBoardState.ActiveColor,
			From = moveDto.From,
			To = moveDto.To,
			Promotion = moveDto.Promotion,
			SanNotation = CreateMoveSanNotation(actualBoardState, moveDto),
			FenBefore = actualBoardState.FEN
		});

		actualBoardState.ApplyMove(moveDto);
		game.CurrentFen = actualBoardState.FEN;

		await SaveGameSessionAsync(game);
	}
	public string CreateMoveSanNotation(BoardState boardState, MoveDto moveDto)
	{
		return $"{moveDto.From}-{moveDto.To}";
	}
	public async Task SaveGameRecordAsync(GameSession gameSession)
	{
		await _gameRecordService.AddGameRecordAsync(new GameRecord()
		{
			PlayerWhite = gameSession.PlayerWhite,
			PlayerBlack = gameSession.PlayerBlack,
			Moves = gameSession.Moves,
			Result = gameSession.Result
		});
	}
}