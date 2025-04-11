using GamePlay.Models;

namespace GamePlay.Services;

/// <include file='.docs/xmldocs/Services.xml' path='doc/class/member[@name="IGameRecordService"]/*' />
public interface IGameRecordService
{
	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameRecordService.GetGameRecordByIdAsync"]/*' />
	Task<GameRecord?> GetGameRecordByIdAsync(Guid gameId);

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameRecordService.GetAllGameRecordsAsync"]/*' />
	Task<List<GameRecord>?> GetAllGameRecordsAsync();

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameRecordService.AddGameRecordAsync"]/*' />
	Task AddGameRecordAsync(GameRecord gameRecord);
}