namespace GamePlayService.Dtos;

public record PlayerDto
{
	public required string Name { get; set; }
	public required string ConnectionId { get; set; }
	public TimeSpan TimeReserve { get; set; }
}