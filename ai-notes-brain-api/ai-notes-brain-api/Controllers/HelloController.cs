using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ai_notes_brain_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelloController : ControllerBase
    {
        [HttpGet(Name = "Hello")]
        public ActionResult<string> SayHello()
        {
            return Ok("Hello World! This is your API talking.");
        }
    }
}
