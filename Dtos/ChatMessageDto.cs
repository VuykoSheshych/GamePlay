namespace GamePlayService.Dtos;
public record ChatMessageDto
{
	public required string Author { get; set; }
	public required string Text { get; set; }
}
