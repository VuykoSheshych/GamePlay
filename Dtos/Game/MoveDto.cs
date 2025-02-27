namespace GamePlayService.Dtos.Game;
public class MoveDto
{
	public required string From { get; set; }
	public required string To { get; set; }
	public string? Promotion { get; set; }
}
