using System.Text.Json;
using GamePlayService.Dtos;
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
	public async Task<Guid> CreateGameSessionAsync(List<(string name, string id)> players)
	{
		GameSession newGame = new()
		{
			WhitePlayer = new()
			{
				Name = players.First().name,
				ConnectionId = players.First().id
			},
			BlackPlayer = new()
			{
				Name = players.Last().name,
				ConnectionId = players.Last().id
			}
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
	public async Task SaveGameRecordAsync(GameSession gameSession)
	{
		await _gameRecordService.AddGameRecordAsync(new GameRecord()
		{
			Id = gameSession.Id,
			PlayerWhite = gameSession.WhitePlayer.Name,
			PlayerBlack = gameSession.BlackPlayer.Name,
			Moves = gameSession.Moves,
			Result = gameSession.Result
		});
	}
}