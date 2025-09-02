using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    public class TestControllers : BaseController
    {
        public TestControllers(IServiceManager serviceManager) : base(serviceManager)
        {
        }
        [HttpGet()]
        public IActionResult Testo()
        {
            return Ok("good");
        }
    }

}
