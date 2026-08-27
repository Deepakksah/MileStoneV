using System;

namespace VideoOS.Management.Server;

public class GeminiWebApplicationHost : GeminiApplicationHost
{
	private GeminiHostManagementService _hostManagementService;

	private GeminiRequestContext _systemRequestContext;

	public GeminiWebApplicationHost(GeminiServiceHostProperties properties)
		: base(properties, null)
	{
		_systemRequestContext = new GeminiRequestContext(this, RequestContextType.SystemContext);
		_hostManagementService = new GeminiHostManagementService();
		((IGeminiServerService)_hostManagementService).StartService((IGeminiRequestContext)_systemRequestContext);
		_hostManagementService.StartRootHost(this, _systemRequestContext);
	}

	public GeminiRequestContext CreateSystemContext()
	{
		CheckIfDisposed();
		return _hostManagementService.BeginRequest(_systemRequestContext, InstanceId, RequestContextType.SystemContext, null, HostRequestType.Default, null);
	}

	public GeminiRequestContext CreateContext(RequestContextType requestContextType, HostRequestType hostRequestType, params object[] additionalParameters)
	{
		CheckIfDisposed();
		return _hostManagementService.BeginRequest(_systemRequestContext, InstanceId, requestContextType, null, hostRequestType, additionalParameters);
	}

	private void CheckIfDisposed()
	{
		if (base.IsDisposed)
		{
			throw new ObjectDisposedException("GeminiApplicationHost");
		}
	}
}
