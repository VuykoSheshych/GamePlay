namespace GamePlay.Models;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="Move"]/*' />
public class Move
{
	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.Id"]/*' />
	public Guid Id { get; set; } = Guid.NewGuid();

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.MoveNumber"]/*' />
	public int MoveNumber { get; set; }

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.PlayerColor"]/*' />
	public string PlayerColor { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.From"]/*' />
	public string From { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.To"]/*' />
	public string To { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.Promotion"]/*' />
	public string? Promotion { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.SanNotation"]/*' />
	public string SanNotation { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.FenBefore"]/*' />
	public string FenBefore { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="Move.Timestamp"]/*' />
	public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}