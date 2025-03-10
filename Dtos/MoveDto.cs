namespace GamePlayService.Dtos;

public record MoveDto
{
	public required string From { get; set; }
	public required string To { get; set; }
	public string? Promotion { get; set; }
}
