using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class VersionController : Controller {
  [HttpGet]
  [Route("version")]
  public IActionResult GetAsync() {
    return Json(3);
  }
}
