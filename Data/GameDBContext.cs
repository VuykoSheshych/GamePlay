using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using GamePlayService.Models;

namespace GamePlayService.Data
{
	public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
	{
		public DbSet<Game> Games { get; set; }
		public DbSet<Move> Moves { get; set; }
		public async Task SeedDataAsync()
		{
			if (!Games.Any())
			{
				var gamesData = await File.ReadAllTextAsync("games.json");
				var games = JsonConvert.DeserializeObject<List<Game>>(gamesData);
				if (games != null)
				{
					await Games.AddRangeAsync(games);
				}
			}

			if (!Moves.Any())
			{
				var movesData = await File.ReadAllTextAsync("moves.json");
				var moves = JsonConvert.DeserializeObject<List<Move>>(movesData);
				if (moves != null)
				{
					await Moves.AddRangeAsync(moves);
				}
			}

			await SaveChangesAsync();
		}
	}
}