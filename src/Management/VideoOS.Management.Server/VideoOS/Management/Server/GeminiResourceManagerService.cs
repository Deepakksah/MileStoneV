using System;

namespace VideoOS.Management.Server;

public sealed class GeminiResourceManagerService : IGeminiServerService
{
	public TComponent CreateComponent<TComponent>(IGeminiRequestContext requestContext) where TComponent : GeminiResourceComponent
	{
		TComponent val = Activator.CreateInstance<TComponent>();
		val.Initialize(requestContext);
		return val;
	}

	void IGeminiServerService.StartService(IGeminiRequestContext requestContext)
	{
	}

	void IGeminiServerService.StopService(IGeminiRequestContext requestContext)
	{
	}
}
