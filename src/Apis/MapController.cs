using System.Globalization;
using System.Text.RegularExpressions;
using HttpDriver.Apis.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace HttpDriver.Apis
{
    [ApiController]
    [Route("api")]
    public class MapController : Controller
    {
        private static readonly Regex Me = new(
            @"^([0-9A-F]+)-([0-9A-F]+)\s+(r|-)(w|-)(x|-)(p|s)\s+([0-9A-F]+)\s+([0-9A-F]+):([0-9A-F]+)\s+([0-9]+)\s+(.*)$",
            RegexOptions.IgnoreCase);

        #region Methods

        [HttpGet]
        [Route("proc/{pid}/maps")]
        public async Task<IActionResult> GetAsync(int pid)
        {
            return Json(await System.IO.File.ReadAllLinesAsync($"/proc/{pid}/maps").ContinueWith(t => t.Result
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Me.Match(x))
                .Where(x => x.Success)
                .Select(Map)));
        }

        #endregion

        #region Statics

        private static MapEntry Map(Match match)
        {
            return new MapEntry
            {
                Start = ulong.Parse(match.Groups[1].Value, NumberStyles.HexNumber),
                End = ulong.Parse(match.Groups[2].Value, NumberStyles.HexNumber),
                Perms = Permissions(match),
                Offset = ulong.Parse(match.Groups[7].Value, NumberStyles.HexNumber),
                DevMajor = ushort.Parse(match.Groups[8].Value, NumberStyles.HexNumber),
                DevMinor = ushort.Parse(match.Groups[9].Value, NumberStyles.HexNumber),
                Inode = ulong.Parse(match.Groups[10].Value),
                Pathname = match.Groups[11].Value
            };
        }

        private static MapEntryPermissions Permissions(Match match)
        {
            var result = MapEntryPermissions.None;
            if (match.Groups[3].Value == "r") result |= MapEntryPermissions.Read;
            if (match.Groups[4].Value == "w") result |= MapEntryPermissions.Write;
            if (match.Groups[5].Value == "x") result |= MapEntryPermissions.Execute;
            if (match.Groups[6].Value == "s") result |= MapEntryPermissions.Shared;
            return result;
        }

        #endregion
    }
}