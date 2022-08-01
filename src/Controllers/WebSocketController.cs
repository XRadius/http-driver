using HttpDriver.Controllers.Sockets;
using HttpDriver.Services;
using Microsoft.AspNetCore.Mvc;

namespace HttpDriver.Controllers
{
    [Controller]
    [Route("ws")]
    public class WebSocketController : Controller
    {
        #region Methods

        [HttpGet]
        [Route("direct/{pid}")]
        public async Task<IActionResult> DirectAsync(int pid, WebSocketSettings settings)
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest) return StatusCode(400);
            using var service = new DirectMemoryService(pid);
            using var socket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            WebSocketChannel.Create(service, settings, socket).Run();
            return Ok();
        }

        #endregion
    }
}