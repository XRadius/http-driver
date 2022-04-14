using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api")]
public class ProcessController : Controller {
  [HttpGet]
  [Route("proc")]
  public async Task<IActionResult> GetAsync() {
    return Json(await Task.WhenAll(Directory.EnumerateDirectories("/proc")
      .Select(Path.GetFileName)
      .Where(x => int.TryParse(x, out _))
      .Select(x => ReadAsync(int.Parse(x!)))));
  }

  private async Task<ProcessEntry> ReadAsync(int pid) {
    var cmdlineTask = System.IO.File.ReadAllTextAsync($"/proc/{pid}/cmdline");
    var statusTask = System.IO.File.ReadAllLinesAsync($"/proc/{pid}/status");
    var cmdline = await cmdlineTask.ContinueWith(x => x.Result.Split('\0', StringSplitOptions.RemoveEmptyEntries));
    var status = await statusTask.ContinueWith(x => x.Result.Select(y => y.Split(":\t", 2)).ToList());
    if (cmdline.Length != 0) {
      var command = cmdline[0];
      var args = cmdline.Skip(1);
      return new ProcessEntry {Pid = pid, Command = command, Args = args};
    } else {
      var name = status.FirstOrDefault(x => x[0] == "Name");
      var command = name != null ? $"[{name[1]}]" : string.Empty;
      return new ProcessEntry {Pid = pid, Command = command};
    }
  }
}
