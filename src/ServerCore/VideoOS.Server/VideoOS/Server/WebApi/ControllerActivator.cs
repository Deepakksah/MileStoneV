using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.WebApi;

public sealed class ControllerActivator : IHttpControllerActivator
{
	public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
	{
		if (controllerType == typeof(DriverController))
		{
			return new DriverController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(EventsCommandsController))
		{
			return new EventsCommandsController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(ServerCommandsController))
		{
			return new ServerCommandsController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(RuleCommandsController))
		{
			return new RuleCommandsController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(DatabaseCommandsController))
		{
			return new DatabaseCommandsController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(TaskController))
		{
			return new TaskController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(RecorderController))
		{
			return new RecorderController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(RecorderStatusCommandsController))
		{
			return new RecorderStatusCommandsController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(HardwareCommandsController))
		{
			return new HardwareCommandsController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(ResponseController))
		{
			return new ResponseController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(SecurityTokenController))
		{
			return new SecurityTokenController(Server.Instance.RecorderCommManager);
		}
		if (controllerType == typeof(ConfigurationController))
		{
			return new ConfigurationController(Server.Instance.RecorderCommManager, ServerConfig.Instance.WebApiConfig);
		}
		if (controllerType == typeof(RecorderRegistrationController))
		{
			return new RecorderRegistrationController(Server.Instance.RecorderCommManager, ServerConfig.Instance.WebApiConfig.ServiceDiscoveryTimeout, Server.Instance.DiscoveryEngine, ServerConfig.Instance.WebApiConfig, Server.Instance.ServerTokenCache, Telemetry.GetILogger("RecorderCommManager"));
		}
		return null;
	}
}
