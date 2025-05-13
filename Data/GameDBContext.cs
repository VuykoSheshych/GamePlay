using Microsoft.EntityFrameworkCore;
using ChessShared.Models;

namespace GamePlay.Data
{
	/// <include file='.docs/xmldocs/DbContext.xml' path='doc/class/member[@name="GameDbContext"]/*' />
	public class GameDbContext(DbContextOptions<GameDbContext> options) : DbContext(options)
	{
		/// <include file='.docs/xmldocs/DbContext.xml' path='doc/method/member[@name="GameDbContext.GameRecords"]/*' />
		public DbSet<GameRecord> GameRecords { get; set; }

		/// <include file='.docs/xmldocs/DbContext.xml' path='doc/method/member[@name="GameDbContext.Moves"]/*' />
		public DbSet<Move> Moves { get; set; }

		/// <inheritdoc/>
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<GameRecord>()
				.Property(g => g.Result)
				.HasConversion<string>();

			modelBuilder.Entity<Move>()
				.Property(m => m.PlayerColor)
				.HasConversion<string>();

			modelBuilder.Entity<Move>()
				.Property(m => m.Promotion)
				.HasConversion<string>();
		}
	}
}