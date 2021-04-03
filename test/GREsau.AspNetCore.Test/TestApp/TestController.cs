using Microsoft.AspNetCore.Mvc;

namespace GREsau.AspNetCore.Test.TestApp
{
    [Route("/")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public ActionResult<Model> Post(Model model) => model;
    }
}
