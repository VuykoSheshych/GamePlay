using StackExchange.Redis;

namespace GamePlayService.Services;

public class GameSearchService(IConnectionMultiplexer redis)
{
	private readonly IDatabase _db = redis.GetDatabase();
	private readonly static List<string> players = [];

	public async Task AddPlayerToSearchQueue(string playerId, string connectionId)
	{
		await _db.HashSetAsync("search:players", playerId, connectionId);
	}

	public async Task<Dictionary<string, string>?> FindPlayersForGame()
	{
		var playersWithConnectionIds = new Dictionary<string, string>();
		var selectedPlayers = new HashSet<string>();

		var allPlayers = await _db.HashKeysAsync("search:players");

		foreach (var player in allPlayers)
		{
			string playerId = player.ToString();

			if (!selectedPlayers.Contains(playerId))
			{
				var connectionId = await GetPlayerConnectionId(playerId);
				if (!string.IsNullOrEmpty(connectionId))
				{
					playersWithConnectionIds.Add(playerId, connectionId);
					selectedPlayers.Add(playerId);

					if (playersWithConnectionIds.Count == 2)
					{
						await RemovePlayerFromSearchQueue(playersWithConnectionIds.Keys.First());
						await RemovePlayerFromSearchQueue(playersWithConnectionIds.Keys.Last());

						return playersWithConnectionIds;
					}
				}
			}
		}

		return null;
	}
	public async Task<string?> GetPlayerConnectionId(string playerId)
	{
		var connectionId = await _db.HashGetAsync("search:players", playerId);
		return connectionId;
	}
	public async Task RemovePlayerFromSearchQueue(string player)
	{
		await _db.HashDeleteAsync("search:players", player);
	}
}