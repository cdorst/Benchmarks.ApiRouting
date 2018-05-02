using Microsoft.AspNetCore.Mvc;

namespace Default.Controllers
{
    [Route("api/1")]
    public class ValuesController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id) => Ok($"Id: {id}");
    }
}
