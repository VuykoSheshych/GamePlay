using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using GamePlayService.Models;

namespace GamePlayService.Data
{
	public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
	{
		public DbSet<GameRecord> GameRecords { get; set; }
		public DbSet<MovePair> Moves { get; set; }
		public async Task SeedDataAsync()
		{
			if (!GameRecords.Any())
			{
				var gamesData = await File.ReadAllTextAsync("GameRecords.json");
				var games = JsonConvert.DeserializeObject<GameRecord>(gamesData);
				if (games != null)
				{
					await GameRecords.AddRangeAsync(games);
				}
			}

			await SaveChangesAsync();
		}
	}
}