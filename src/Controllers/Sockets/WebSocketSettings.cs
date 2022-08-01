using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace HttpDriver.Controllers.Sockets
{
    public class WebSocketSettings
    {
        #region Properties

        [FromQuery]
        [Range(0, 360)]
        public int FramesPerSecond { get; init; } = 30;

        #endregion
    }
}