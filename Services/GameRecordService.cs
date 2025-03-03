using GamePlayService.Data;
using GamePlayService.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePlayService.Services
{
	public class GameRecordService(GameDbContext context)
	{
		private readonly GameDbContext _context = context;
		public async Task<GameRecord?> GetGameRecordByIdAsync(Guid id)
		{
			var gameRecord = await _context.GameRecords.FindAsync(id);
			if (gameRecord != null)
			{
				gameRecord.Moves = [.. gameRecord.Moves
					.OrderBy(m => m.MoveNumber)
					.ThenBy(m => m.PlayerColor == "w" ? 0 : 1)];
			}
			return gameRecord;
		}
		public async Task<List<GameRecord>?> GetAllGameRecordsAsync()
		{
			return await _context.GameRecords.ToListAsync();
		}
		public async Task AddGameRecordAsync(GameRecord gameRecord)
		{
			await _context.AddAsync(gameRecord);
			await _context.SaveChangesAsync();
		}
	}
}
