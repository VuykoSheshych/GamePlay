namespace GamePlay.Dtos;

/// <include file='.docs/xmldocs/DTOs.xml' path='doc/class/member[@name="MoveDto"]/*' />
public record MoveDto(string From, string To, string? Promotion = null) { }