using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

/// <summary>
/// Todo: 2.21 Create Base Controller
/// </summary>
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
//[Authorize(Policy = "CanRead")]
public class ApiController: ControllerBase
{
    
}