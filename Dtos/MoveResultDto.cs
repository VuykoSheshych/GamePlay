namespace GamePlay.Dtos;

/// <include file='.docs/xmldocs/DTOs.xml' path='doc/class/member[@name="MoveResultDto"]/*' />
public record MoveResultDto(bool IsSuccess, string Message)
{
	/// <include file='.docs/xmldocs/DTOs.xml' path='doc/method/member[@name="MoveResultDto.Success"]/*' />
	public static MoveResultDto Success(string message) => new(true, message);

	/// <include file='.docs/xmldocs/DTOs.xml' path='doc/method/member[@name="MoveResultDto.Error"]/*' />
	public static MoveResultDto Error(string message) => new(false, message);
}