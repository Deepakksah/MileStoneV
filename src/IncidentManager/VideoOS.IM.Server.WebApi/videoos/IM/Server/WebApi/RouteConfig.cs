using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Swashbuckle.Application;

namespace VideoOS.IM.Server.WebApi;

public class RouteConfig
{
	public static void RegisterRoutes(RouteCollection routes)
	{
		routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
		routes.MapHttpRoute("swagger_root", "", null, null, (HttpMessageHandler)(object)new RedirectHandler((HttpRequestMessage message) => message.RequestUri.ToString(), "swagger"));
		routes.MapRoute("Default", "{controller}/{action}/{id}", new
		{
			controller = "Home",
			action = "Index",
			id = UrlParameter.Optional
		});
	}
}
