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
    for (ushort i = 0; i < maps.Length; i++) {
      var match = me.Match(maps[i]);
      if (match.Success) {
        var address = Parse(match.Groups[1].Value);
        var buffer = new byte[Parse(match.Groups[2].Value)];
        if (memoryService.Read(pid, address, buffer)) {
          await Response.Body.WriteAsync(BitConverter.GetBytes(i));
          await Response.Body.WriteAsync(buffer, 0, buffer.Length);
        }
      }
    }
  }

  [HttpPut]
  [OpenApiBodyParameter("application/octet-stream")]
  [ProducesResponseType(typeof(void), 204)]
  [ProducesResponseType(typeof(void), 404)]
  [Route("proc/{pid}/mem/{request}")]
  public async Task PutAsync(int pid, string request) {
    Response.Headers.Add("Content-Type", "application/octet-stream");
    var maps = request.Split(',');
    for (ushort i = 0; i < maps.Length; i++) {
      var match = me.Match(maps[i]);
      if (match.Success) {
        var address = Parse(match.Groups[1].Value);
        var buffer = new byte[Parse(match.Groups[2].Value)];
        await Request.Body.ReadAsync(buffer, 0, buffer.Length);
        if (memoryService.Write(pid, address, buffer)) {
          await Response.Body.WriteAsync(BitConverter.GetBytes(i));
        }
      }
    }
  }

  private ulong Parse(string? value) {
    if (string.IsNullOrEmpty(value)) return 0;
    return Convert.ToUInt64(value, 16);
  }
}
