using GamePlay.Data;
using GamePlay.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePlay.Services;

/// <inheritdoc cref="IGameRecordService" />
public class GameRecordService(GameDbContext context) : IGameRecordService
{
	/// <inheritdoc />
	public async Task<GameRecord?> GetGameRecordByIdAsync(Guid gameId)
	{
		var gameRecord = await context.GameRecords.FindAsync(gameId);
		if (gameRecord != null)
		{
			gameRecord.Moves = [.. gameRecord.Moves
					.OrderBy(m => m.MoveNumber)
					.ThenBy(m => m.PlayerColor == "w" ? 0 : 1)];
		}
		return gameRecord;
	}

	/// <inheritdoc/>
	public async Task<List<GameRecord>?> GetAllGameRecordsAsync()
	{
		return await context.GameRecords.ToListAsync();
	}

	/// <inheritdoc/>
	public async Task AddGameRecordAsync(GameRecord gameRecord)
	{
		await context.AddAsync(gameRecord);
		await context.SaveChangesAsync();
	}
}
