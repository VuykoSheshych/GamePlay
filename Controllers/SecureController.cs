using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace GamePlayService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SecureController : ControllerBase
{
	[HttpGet("history")]
	public IActionResult GetHistory()
	{
		return Ok("Данные доступны, потому что пользователь аутентифицирован");
	}
}