using System.Text.RegularExpressions;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Yarp.ReverseProxy.Forwarder;

[Controller]
public class ProxyController : Controller {
  private readonly HttpMessageInvoker httpClient;
  private readonly IHttpForwarder httpForwarder;
  private readonly ForwarderRequestConfig requestOptions;

  public ProxyController(IHttpForwarder httpForwarder) {
    this.httpClient = new HttpMessageInvoker(new SocketsHttpHandler());
    this.httpForwarder = httpForwarder;
    this.requestOptions = new ForwarderRequestConfig();
  }

  [HttpGet]
  [Route("{encodedUrl}/{**catchAll}")]
  public async Task GetAsync(string encodedUrl) {
    var uri = new Uri(HttpUtility.UrlDecode(encodedUrl));
    if (uri.Scheme == "http" || uri.Scheme == "https") {
      var destinationPrefix = $"{uri.Scheme}://{uri.Host}:{uri.Port}";
      await httpForwarder.SendAsync(HttpContext, destinationPrefix, httpClient, requestOptions, (context, request) => {
        request.RequestUri = new Uri(uri, context.Request.RouteValues.Values.LastOrDefault()?.ToString());
        request.Headers.Host = null;
        return default;
      });
    } else {
      Response.StatusCode = 404;
    }
  }
}
