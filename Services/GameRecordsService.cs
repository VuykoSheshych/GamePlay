using GamePlayService.Data;
using GamePlayService.Models;
using Microsoft.EntityFrameworkCore;

namespace GamePlayService.Services
{
	public class GameRecordsService(GameDbContext context)
	{
		private readonly GameDbContext _context = context;
		public async Task<GameRecord?> GetGameRecordByIdAsync(Guid id)
		{
			return await _context.GameRecords.FindAsync(id);
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
