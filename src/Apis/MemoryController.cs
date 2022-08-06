using System.Diagnostics;
using System.Text.RegularExpressions;
using HttpDriver.Services;
using Microsoft.AspNetCore.Mvc;

namespace HttpDriver.Apis
{
    [ApiController]
    [Route("api")]
    public class MemoryController : Controller
    {
        private static readonly Regex Me = new(
            @"^([0-9A-F]+):([0-9A-F]+)$",
            RegexOptions.IgnoreCase);

        #region Methods

        [HttpGet]
        [Route("proc/{pid}/mem/{request}")]
        public async Task GetAsync(int pid, string request)
        {
            Response.Headers.Add("Content-Type", "application/octet-stream");
            using var memoryService = new DirectMemoryService(pid);
            var maps = request.Split(',');
            for (ushort i = 0; i < maps.Length; i++)
            {
                var match = Me.Match(maps[i]);
                if (!match.Success) continue;
                var address = Parse(match.Groups[1].Value);
                var buffer = new byte[Parse(match.Groups[2].Value)];
                if (!memoryService.Read(address, buffer) && Debugger.IsAttached) Console.WriteLine($"Failed {address:x}:{buffer.Length:x}");
                await Response.Body.WriteAsync(BitConverter.GetBytes(i));
                await Response.Body.WriteAsync(buffer);
            }
        }

        [HttpPut]
        [Route("proc/{pid}/mem/{request}")]
        public async Task PutAsync(int pid, string request)
        {
            Response.Headers.Add("Content-Type", "application/octet-stream");
            using var memoryService = new DirectMemoryService(pid);
            var maps = request.Split(',');
            for (ushort i = 0; i < maps.Length; i++)
            {
                var match = Me.Match(maps[i]);
                if (!match.Success) continue;
                var address = Parse(match.Groups[1].Value);
                var buffer = new byte[Parse(match.Groups[2].Value)];
                await Request.Body.ReadAsync(buffer);
                if (!memoryService.Write(address, buffer)) continue;
                await Response.Body.WriteAsync(BitConverter.GetBytes(i));
            }
        }

        #endregion

        #region Statics

        private static ulong Parse(string? value)
        {
            return string.IsNullOrEmpty(value) ? 0 : Convert.ToUInt64(value, 16);
        }

        #endregion
    }
}