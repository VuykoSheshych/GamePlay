using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using GamePlayService.Services;
using GamePlayService.Models;
using GamePlayService.Dtos.Game;
namespace GamePlayService.Controllers;

[Authorize]
[ApiController]
[Route("api/secure")]
public class SecureController() : ControllerBase
{

}