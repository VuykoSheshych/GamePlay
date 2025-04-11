namespace GamePlay.Dtos;

/// <include file='.docs/xmldocs/DTOs.xml' path='doc/class/member[@name="PlayerInGameDto"]/*' />
public record PlayerInGameDto(string Name, string ConnectionId, TimeSpan TimeReserve) { }