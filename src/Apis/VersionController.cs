using Microsoft.AspNetCore.Mvc;

namespace HttpDriver.Apis
{
    [ApiController]
    [Route("api")]
    public class VersionController : Controller
    {
        #region Methods

        [HttpGet]
        [Route("version")]
        public IActionResult GetAsync()
        {
            return Json(4);
        }

        #endregion
    }
}