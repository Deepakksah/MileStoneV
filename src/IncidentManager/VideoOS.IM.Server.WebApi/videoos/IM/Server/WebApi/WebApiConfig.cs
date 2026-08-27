using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Newtonsoft.Json;
using VideoOS.IM.Server.WebApi.Filters;
using VideoOS.IM.Server.WebApi.Services;

namespace VideoOS.IM.Server.WebApi;

public static class WebApiConfig
{
	public static void Register(HttpConfiguration config)
	{
		config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
		config.Formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.Objects;
		config.Filters.Add(new AuthenticationFilter());
		config.Filters.Add(new LicensingFilter());
		config.Services.Add(typeof(IExceptionLogger), new UnhandledExceptionLogger());
		config.MapHttpAttributeRoutes();
		config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new
		{
			id = RouteParameter.Optional
		});
	}
}
