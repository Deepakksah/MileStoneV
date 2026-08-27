using System;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security.Jwt;
using Owin;
using VideoOS.Common.Integration.WebApi;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.WebApi;

public sealed class Startup : IDisposable
{
	private static HttpConfiguration _config;

	private bool disposedValue;

	public static void Configuration(IAppBuilder appBuilder, IServerConfig config, WebApiStatus webApiStatus)
	{
		if (appBuilder == null)
		{
			throw new ArgumentNullException("appBuilder");
		}
		if (config == null)
		{
			throw new ArgumentNullException("config");
		}
		_config = new HttpConfiguration();
		_config.MapHttpAttributeRoutes();
		_config.Filters.Add(new ServerExceptionFilter());
		_config.Filters.Add(new ValidateModelAttribute());
		_config.Filters.Add(new AuthorizeAttribute());
		_config.Services.Replace(typeof(IHttpControllerActivator), new ControllerActivator());
		_config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
		_config.Services.Replace(typeof(IExceptionLogger), new ServerUnhandledExceptionLogger(Telemetry.GetILogger("WebApi")));
		string address = config.WebApiConfig.AuthorizationServerUri.ToString();
		appBuilder.Use<ExceptionLoggerMiddleware>(new object[2]
		{
			Telemetry.GetILogger("WebApi"),
			webApiStatus
		});
		TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
		{
			ValidateAudience = true,
			ValidAudience = "management"
		};
		JwtBearerAuthenticationOptions options = new JwtBearerAuthenticationOptions
		{
			TokenValidationParameters = tokenValidationParameters,
			TokenHandler = new JwtSecurityTokenHandlerWithConfigurationManager(address, config.WebApiConfig.SecuredServer, delayConfigurationLoad: false)
		};
		appBuilder.UseJwtBearerAuthentication(options);
		appBuilder.Use(typeof(ScopeValidationMiddleware), Telemetry.GetILogger("WebApi"), new ScopeValidationOptions
		{
			RequiredScope = "management"
		});
		appBuilder.UseWebApi(_config);
	}

	private void Dispose(bool disposing)
	{
		if (!disposedValue)
		{
			if (disposing && _config != null)
			{
				_config.Dispose();
				_config = null;
			}
			disposedValue = true;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
