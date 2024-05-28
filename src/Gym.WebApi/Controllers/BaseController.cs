using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gym.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class BaseController : ControllerBase
{ }