using System.Text.Json;
using GamePlay.Dtos;
using GamePlay.Models;
using StackExchange.Redis;

namespace GamePlay.Services;

/// <inheritdoc cref="IGameSessionService" />
public class GameSessionService(IConnectionMultiplexer redis, IGameRecordService gameService) : IGameSessionService
{
	private readonly IGameRecordService _gameRecordService = gameService;
	private readonly IDatabase _db = redis.GetDatabase();
	private readonly TimeSpan _expiration = TimeSpan.FromHours(1);

	/// <inheritdoc/>
	public async Task<GameSession?> GetGameSessionAsync(string gameId)
	{
		var json = await _db.StringGetAsync($"game:{gameId}");
		return json.IsNullOrEmpty ? null : JsonSerializer.Deserialize<GameSession>(json!);
	}

	/// <inheritdoc/>
	public async Task<Guid> CreateGameSessionAsync(List<(string name, string id)> players)
	{
		// За замовчуванням у кожного гравця буде 20 хвилин на ходи
		var gameDuration = TimeSpan.FromMinutes(20);
		// By default, each player will have 20 minutes to make moves

		GameSession newGame = new()
		{
			WhitePlayer = new(players.First().name, players.First().id, gameDuration),
			BlackPlayer = new(players.Last().name, players.Last().id, gameDuration)
		};

		var json = JsonSerializer.Serialize(newGame);
		await _db.StringSetAsync($"game:{newGame.Id}", json, _expiration);
		return newGame.Id;
	}

	/// <inheritdoc/>
	public async Task RemoveGameSessionAsync(string gameId, string result)
	{
		var game = await GetGameSessionAsync(gameId);

		if (game != null)
		{
			await SaveGameRecordAsync(game, result);
		}
		await _db.KeyDeleteAsync($"game:{gameId}");
	}

	/// <inheritdoc/>
	public async Task SaveGameSessionAsync(GameSession gameSession)
	{
		var json = JsonSerializer.Serialize(gameSession);
		await _db.StringSetAsync($"game:{gameSession.Id}", json, _expiration);
	}

	/// <inheritdoc/>
	public async Task<MoveResultDto> TryMakeMoveAsync(string gameId, MoveDto moveDto)
	{
		var game = await GetGameSessionAsync(gameId);
		if (game == null) return MoveResultDto.Error("Game not found!");

		var actualBoardState = new BoardState(game.CurrentFen);
		var moveResultDto = ChessValidator.GetMoveValidationResult(actualBoardState, moveDto);

		if (!moveResultDto.IsSuccess) return moveResultDto;

		game.Moves.Add(new Move()
		{
			MoveNumber = actualBoardState.FullmoveNumber,
			PlayerColor = actualBoardState.ActiveColor,
			From = moveDto.From,
			To = moveDto.To,
			Promotion = moveDto.Promotion,
			SanNotation = moveResultDto.Message,
			FenBefore = actualBoardState.FEN
		});

		actualBoardState.ApplyMove(moveDto);
		game.CurrentFen = actualBoardState.FEN;

		await SaveGameSessionAsync(game);
		return moveResultDto;
	}

	/// <inheritdoc/>
	public async Task SaveGameRecordAsync(GameSession gameSession, string result)
	{
		await _gameRecordService.AddGameRecordAsync(new GameRecord()
		{
			Id = gameSession.Id,
			PlayerWhite = gameSession.WhitePlayer.Name,
			PlayerBlack = gameSession.BlackPlayer.Name,
			Moves = gameSession.Moves,
			Result = result,
			Started = gameSession.CreatedAt
		});
	}
}