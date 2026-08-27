using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class GeminiHostManagementKernel<THost, TRequest> where THost : ApplicationHost<GeminiRequestContext> where TRequest : RequestContextBase
{
	public delegate THost LoadHost(GeminiServiceHostProperties properties, THost parentHost);

	private THost _rootHost;

	private LoadHost _loadHost;

	private ConcurrentDictionary<Guid, Lazy<THost>> _serviceHosts;

	public GeminiHostManagementKernel(THost rootHost, LoadHost loadHost)
	{
		_rootHost = rootHost;
		_loadHost = loadHost;
		_serviceHosts = new ConcurrentDictionary<Guid, Lazy<THost>>();
	}

	public TRequest CreateRequest(Guid hostId, long requestId, RequestContextType requestType, HostRequestType type, params object[] additionalParameters)
	{
		return _serviceHosts.GetOrAdd(hostId, (Guid x) => new Lazy<THost>(() => _loadHost(new GeminiServiceHostProperties(), null))).Value.CreateRequest(requestType, type, additionalParameters) as TRequest;
	}

	public void Start(List<GeminiServiceHostProperties> hostProperties, long requestId)
	{
		using TRequest val = CreateRequest(_rootHost.InstanceId, requestId, RequestContextType.SystemContext, HostRequestType.Default);
		_rootHost.Start(val as GeminiRequestContext);
	}
}
