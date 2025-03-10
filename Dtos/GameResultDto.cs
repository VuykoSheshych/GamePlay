namespace GamePlayService.Dtos;

public record GameResultDto
{
	public required string GameId { get; set; }
	public required string PlayerLooserName { get; set; }
	public required int PlayerLooserElo { get; set; }
	public required string PlayerWinnerName { get; set; }
	public required int PlayerWinnerElo { get; set; }
}