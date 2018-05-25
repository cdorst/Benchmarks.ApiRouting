using Microsoft.AspNetCore.Mvc;
using static Entities.Constants;

namespace Default.Controllers
{
    [Route("api/1")]
    public class ValuesController : ControllerBase
    {
        [HttpGet("{id}")]
        public IActionResult Get(int id)
            => new ReadOnlyMemoryActionResult(EntityConstant.ToReadonlyMemory());
    }
}
