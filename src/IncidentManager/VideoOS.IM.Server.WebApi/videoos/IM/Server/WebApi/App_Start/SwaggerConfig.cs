using System.Web.Http;
using Swashbuckle.Application;

namespace VideoOS.IM.Server.WebApi.App_Start;

public class SwaggerConfig
{
	public static void Register()
	{
		GlobalConfiguration.Configuration.EnableSwagger(delegate(SwaggerDocsConfig c)
		{
			c.SingleApiVersion("v1", "IncidentManager WebAPI");
		}).EnableSwaggerUi(delegate(SwaggerUiConfig c)
		{
			c.EnableApiKeySupport("Authorization", "header");
		});
	}
}
