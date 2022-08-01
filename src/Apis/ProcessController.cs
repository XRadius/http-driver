using HttpDriver.Apis.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace HttpDriver.Apis
{
    [ApiController]
    [Route("api")]
    public class ProcessController : Controller
    {
        #region Methods

        [HttpGet]
        [Route("proc")]
        public async Task<IActionResult> GetAsync()
        {
            return Json(await Task.WhenAll(Directory.EnumerateDirectories("/proc")
                .Select(Path.GetFileName)
                .Where(x => int.TryParse(x, out _))
                .Select(x => ReadAsync(int.Parse(x!)))));
        }

        #endregion

        #region Statics

        private static async Task<ProcessEntry> ReadAsync(int pid)
        {
            var cmdlineTask = System.IO.File.ReadAllTextAsync($"/proc/{pid}/cmdline");
            var statusTask = System.IO.File.ReadAllLinesAsync($"/proc/{pid}/status");
            var cmdline = await cmdlineTask.ContinueWith(t => t.Result.Split('\0', StringSplitOptions.RemoveEmptyEntries));
            var status = await statusTask.ContinueWith(t => t.Result.Select(y => y.Split(":\t", 2)).ToList());

            if (cmdline.Length != 0)
            {
                var command = cmdline[0];
                var args = cmdline.Skip(1);
                return new ProcessEntry { Pid = pid, Command = command, Args = args };
            }
            else
            {
                var name = status.FirstOrDefault(x => x[0] == "Name");
                var command = name != null ? $"[{name[1]}]" : string.Empty;
                return new ProcessEntry { Pid = pid, Command = command };
            }
        }

        #endregion
    }
}