using Microsoft.AspNetCore.Mvc;

namespace RestaurantApi.Presentation.WebApi.Controllers
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {

    }
}
