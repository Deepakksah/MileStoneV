using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.GisMapCommunication.Exceptions;
using VideoOS.Platform;

namespace VideoOS.GisMapCommunication;

public abstract class ClientManagerBase<T> : IClientManagerBase<T> where T : IClientBase
{
	private Dictionary<ServerId, Dictionary<TimeSpan, T>> _cachedClients = new Dictionary<ServerId, Dictionary<TimeSpan, T>>();

	private object _lock = new object();

	private readonly TimeSpan DEFAULT_REQUEST_TIMEOUT = TimeSpan.FromMinutes(1.0);

	protected abstract T CreateClient(ServerId serverId, TimeSpan requestTimeout, Uri uri);

	public T GetClient(ServerId serverId)
	{
		return GetClient(serverId, DEFAULT_REQUEST_TIMEOUT);
	}

	public T GetClient(ServerId serverId, TimeSpan requestTimeout)
	{
		lock (_lock)
		{
			if (!_cachedClients.TryGetValue(serverId, out var value))
			{
				value = new Dictionary<TimeSpan, T>();
				_cachedClients.Add(serverId, value);
			}
			if (!value.TryGetValue(requestTimeout, out var value2))
			{
				List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, serverId);
				if (registeredServiceUriInfo == null || registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray == null || registeredServiceUriInfo[0].UriArray.Count == 0)
				{
					throw new ServiceNotRegisteredException();
				}
				value2 = CreateClient(serverId, requestTimeout, new Uri(registeredServiceUriInfo[0].UriArray[0]));
				value.Add(requestTimeout, value2);
			}
			return value2;
		}
	}

	public void Close()
	{
		lock (_lock)
		{
			_cachedClients.SelectMany((KeyValuePair<ServerId, Dictionary<TimeSpan, T>> x) => x.Value).ToList().ForEach(delegate(KeyValuePair<TimeSpan, T> x)
			{
				x.Value.Dispose();
			});
			_cachedClients.Clear();
		}
	}
}
