using System.Web;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Forwarder;

namespace HttpDriver.Controllers
{
    [Controller]
    [Route("reverse-proxy")]
    public class ReverseProxyController : Controller
    {
        private readonly HttpMessageInvoker _httpClient;
        private readonly IHttpForwarder _httpForwarder;
        private readonly ForwarderRequestConfig _requestOptions;

        #region Constructors

        public ReverseProxyController(IHttpForwarder httpForwarder)
        {
            _httpClient = new HttpMessageInvoker(new SocketsHttpHandler());
            _httpForwarder = httpForwarder;
            _requestOptions = new ForwarderRequestConfig();
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("{encodedUrl}/{**catchAll}")]
        public async Task GetAsync(string encodedUrl)
        {
            var uri = new Uri(HttpUtility.UrlDecode(encodedUrl));

            if (uri.Scheme is "http" or "https")
            {
                var destinationPrefix = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
                await _httpForwarder.SendAsync(HttpContext, destinationPrefix, _httpClient, _requestOptions, (context, request) =>
                {
                    request.RequestUri = new Uri(uri, context.Request.RouteValues.Values.LastOrDefault()?.ToString());
                    request.Headers.Host = null;
                    return default;
                });
            }
            else
            {
                Response.StatusCode = 404;
            }
        }

        #endregion
    }
}