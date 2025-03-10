using StackExchange.Redis;

namespace GamePlayService.Services;

public class GameSearchService(IConnectionMultiplexer redis)
{
	private readonly IDatabase _db = redis.GetDatabase();
	public async Task AddPlayerToSearchQueue(string playerName, string connectionId)
	{
		await _db.HashSetAsync("search:players", playerName, connectionId);
	}
	public async Task<List<(string name, string id)>?> FindPlayersForGame()
	{
		var playersWithConnectionIds = new List<(string name, string id)>();
		var selectedPlayers = new HashSet<string>();

		var allPlayers = await _db.HashKeysAsync("search:players");

		foreach (var player in allPlayers)
		{
			string playerName = player.ToString();

			if (!selectedPlayers.Contains(playerName))
			{
				var connectionId = await GetPlayerConnectionId(playerName);
				if (!string.IsNullOrEmpty(connectionId))
				{
					playersWithConnectionIds.Add((playerName, connectionId));
					selectedPlayers.Add(playerName);

					if (playersWithConnectionIds.Count == 2)
					{
						await RemovePlayerFromSearchQueue(playersWithConnectionIds.First().name);
						await RemovePlayerFromSearchQueue(playersWithConnectionIds.Last().name);

						return playersWithConnectionIds;
					}
				}
			}
		}

		return null;
	}
	public async Task<string?> GetPlayerConnectionId(string playerName)
	{
		return await _db.HashGetAsync("search:players", playerName);
	}
	public async Task RemovePlayerFromSearchQueue(string playerName)
	{
		await _db.HashDeleteAsync("search:players", playerName);
	}
}