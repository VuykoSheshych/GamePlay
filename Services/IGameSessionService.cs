using ChessShared.Models;
using ChessShared.Dtos;
using ChessShared.Enums;

namespace GamePlay.Services;

/// <include file='.docs/xmldocs/Services.xml' path='doc/class/member[@name="IGameSessionService"]/*' />
public interface IGameSessionService
{
	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSessionService.GetGameSessionAsync"]/*' />
	Task<GameSession?> GetGameSessionAsync(string gameId);

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSessionService.CreateGameSessionAsync"]/*' />
	Task<Guid> CreateGameSessionAsync(List<(string name, string id)> players);

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSessionService.RemoveGameSessionAsync"]/*' />
	Task RemoveGameSessionAsync(string gameId, GameResult result);

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSessionService.SaveGameSessionAsync"]/*' />
	Task SaveGameSessionAsync(GameSession gameSession);

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSessionService.TryMakeMoveAsync"]/*' />
	Task<MoveResultDto> TryMakeMoveAsync(string gameId, MoveDto moveDto);

	/// <include file='.docs/xmldocs/Services.xml' path='doc/method/member[@name="IGameSessionService.SaveGameRecordAsync"]/*' />
	Task SaveGameRecordAsync(GameSession gameSession, GameResult result);
}