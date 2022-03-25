using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;

[ApiController]
[Route("api/v1.0")]
[OpenApiTags("proc")]
public class MemoryController : Controller {
  private readonly static Regex me = new Regex(@"^([0-9A-F]+):([0-9A-F]+)$", RegexOptions.IgnoreCase);
  private readonly MemoryService memoryService;

  public MemoryController(MemoryService memoryService) {
    this.memoryService = memoryService;
  }

  [HttpGet]
  [ProducesResponseType(typeof(FileContentResult), 200)]
  [Route("proc/{pid}/mem/{request}")]
  public async Task GetAsync(int pid, string request) {
    Response.Headers.Add("Content-Type", "application/octet-stream");
    var maps = request.Split(',');
    for (var i = 0; i < maps.Length; i++) {
      var match = me.Match(maps[i]);
      if (match.Success) {
        var address = Parse(match.Groups[1].Value);
        var buffer = new byte[Parse(match.Groups[2].Value)];
        if (memoryService.Read(pid, address, buffer)) {
          await Response.Body.WriteAsync(BitConverter.GetBytes((ushort) i), 0, 2);
          await Response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
      }
    }
  }

  [HttpPut]
  [OpenApiBodyParameter("application/octet-stream")]
  [ProducesResponseType(typeof(void), 204)]
  [ProducesResponseType(typeof(void), 404)]
  [Route("proc/{pid}/mem/{address}")]
  public async Task<IActionResult> PutAsync(int pid, string address) {
    var buffer = new MemoryStream();
    await Request.Body.CopyToAsync(buffer);
    return memoryService.Write(pid, Parse(address), buffer.ToArray()) ? NoContent() : NotFound();
  }

  private ulong Parse(string? value) {
    if (string.IsNullOrEmpty(value)) return 0;
    return Convert.ToUInt64(value, 16);
  }
}
