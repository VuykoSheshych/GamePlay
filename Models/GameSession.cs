using GamePlayService.Dtos;

namespace GamePlayService.Models;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="GameSession"]/*' />
public class GameSession
{
	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameSession.Id"]/*' />
	public Guid Id { get; set; } = Guid.NewGuid();

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameSession.WhitePlayer"]/*' />
	public required PlayerInGameDto WhitePlayer { get; set; }

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameSession.WhitePlayer"]/*' />
	public required PlayerInGameDto BlackPlayer { get; set; }

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameSession.Moves"]/*' />
	public List<Move> Moves { get; set; } = [];

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameSession.Messages"]/*' />
	public List<ChatMessageDto> Messages { get; set; } = [];

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameSession.CurrentFen"]/*' />
	public string CurrentFen { get; set; } = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameSession.CreatedAt"]/*' />
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
