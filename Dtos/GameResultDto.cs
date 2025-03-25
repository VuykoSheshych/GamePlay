namespace GamePlayService.Dtos;

/// <include file='.docs/xmldocs/DTOs.xml' path='doc/class/member[@name="ChatMessageDto"]/*' />
public record GameResultDto(string GameId, string PlayerLooserName, int PlayerLooserElo, string PlayerWinnerName, int PlayerWinnerElo) { }