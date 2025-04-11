namespace GamePlay.Services;

/// <include file='.docs/xmldocs/Services.xml' path='doc/class/member[@name="IGameSearchService"]/*' />
public interface IGameSearchService
{
	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSearchService.AddPlayerToSearchQueue"]/*' />
	Task AddPlayerToSearchQueue(string playerName, string playerConnectionId);

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSearchService.FindPlayersForGame"]/*' />
	Task<List<(string name, string id)>?> FindPlayersForGame();

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSearchService.GetPlayerConnectionId"]/*' />
	Task<string?> GetPlayerConnectionId(string playerName);

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSearchService.RemovePlayerFromSearchQueue"]/*' />
	Task RemovePlayerFromSearchQueue(string playerName);
}