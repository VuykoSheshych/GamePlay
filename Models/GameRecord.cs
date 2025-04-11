namespace GamePlay.Models;

/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/class/member[@name="GameRecord"]/*' />
public class GameRecord
{
	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameRecord.Id"]/*' />
	public Guid Id { get; set; } = Guid.NewGuid();

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameRecord.PlayerWhite"]/*' />
	public string PlayerWhite { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameRecord.PlayerBlack"]/*' />
	public string PlayerBlack { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameRecord.Moves"]/*' />
	public virtual List<Move> Moves { get; set; } = [];

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameRecord.Result"]/*' />
	public string Result { get; set; } = string.Empty;

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameRecord.Started"]/*' />
	public DateTime Started { get; set; }

	/// <include file='.docs/xmldocs/DomainModels.xml' path='doc/method/member[@name="GameRecord.Finished"]/*' />
	public DateTime Finished { get; set; } = DateTime.UtcNow;
}