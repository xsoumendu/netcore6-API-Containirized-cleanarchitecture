using Microsoft.AspNetCore.Mvc;

namespace ContainerApp.API.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AliveController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            return "API is alive!";
        }
    }
}