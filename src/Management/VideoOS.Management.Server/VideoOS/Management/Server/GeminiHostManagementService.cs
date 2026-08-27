using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public sealed class GeminiHostManagementService : IGeminiServerService
{
	private GeminiHostManagementKernel<GeminiApplicationHost, GeminiRequestContext> _kernel;

	private GeminiApplicationHost _applicationHost;

	private const string Area = "GeminiHostManagementService";

	private const string Layer = "Business";

	public void StartRootHost(GeminiWebApplicationHost geminiWebApplicationHost, GeminiRequestContext _systemRequestContext)
	{
		GeminiTracingService.TraceEnterEtw(0, "GeminiHostManagementService", "Business", "StartRootHost");
		List<GeminiServiceHostProperties> hostProperties = new List<GeminiServiceHostProperties>();
		_kernel.Start(hostProperties, 0L);
	}

	public GeminiRequestContext BeginRequest(IGeminiRequestContext requestContext, Guid instanceId, RequestContextType contextType, IdentityDescriptor userContext, HostRequestType type, params object[] additionalParameters)
	{
		GeminiValidation.CheckForNull(requestContext, "requestContext");
		GeminiRequestContext geminiRequestContext = _kernel.CreateRequest(instanceId, -1L, contextType, type, additionalParameters);
		if (geminiRequestContext != null)
		{
			try
			{
				if (userContext != null)
				{
					geminiRequestContext.UserContext = userContext;
					return geminiRequestContext;
				}
				if (type != HostRequestType.AspNet && type != HostRequestType.Wcf)
				{
					geminiRequestContext.UserContext = requestContext.UserContext;
				}
				return geminiRequestContext;
			}
			catch (Exception ex)
			{
				geminiRequestContext?.Dispose();
				requestContext.RequestTracer.TraceException(0, "GeminiHostManagementService", "Business", ex);
				throw;
			}
		}
		return null;
	}

	private GeminiApplicationHost LoadHost(GeminiServiceHostProperties properties, GeminiApplicationHost parentHost)
	{
		return new GeminiApplicationHost(properties, parentHost);
	}

	void IGeminiServerService.StartService(IGeminiRequestContext requestContext)
	{
		GeminiTracingService.TraceEnterEtw(0, "GeminiHostManagementService", "Business", "StartService");
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		_applicationHost = ((GeminiRequestContext)requestContext).ApplicationHost;
		_kernel = new GeminiHostManagementKernel<GeminiApplicationHost, GeminiRequestContext>(_applicationHost, LoadHost);
	}

	void IGeminiServerService.StopService(IGeminiRequestContext requestContext)
	{
	}
}
