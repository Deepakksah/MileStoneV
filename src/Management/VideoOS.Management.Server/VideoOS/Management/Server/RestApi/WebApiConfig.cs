using System;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace VideoOS.Management.Server.RestApi;

public static class WebApiConfig
{
	public static void Register(HttpConfiguration config)
	{
		if (config == null)
		{
			throw new ArgumentNullException("config");
		}
		config.MapHttpAttributeRoutes();
		config.Filters.Add(new RestApiAuthorizationFilter());
		config.Filters.Add(new AuthorizeAttribute());
		MediaTypeHeaderValue item = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault((MediaTypeHeaderValue t) => t.MediaType == "application/xml");
		config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(item);
		config.Formatters.Add(new JsonMediaTypeFormatter());
		config.Services.Replace(typeof(IExceptionHandler), new RestApiExceptionHandler());
	}
}
