using BytStore.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.PresentationLayer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController:ControllerBase
    {
        private readonly IServiceManager serviceManager;

        public BaseController(IServiceManager serviceManager)
        {
            this.serviceManager = serviceManager;
        }
    }
}
