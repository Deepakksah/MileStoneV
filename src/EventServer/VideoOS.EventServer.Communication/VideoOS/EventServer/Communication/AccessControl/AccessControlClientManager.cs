using System;
using System.Collections.Generic;
using System.Configuration;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.AccessControl;

public class AccessControlClientManager
{
	private TimeSpan _defaultRequestTimeout = TimeSpan.FromMinutes(1.0);

	private Dictionary<ServerId, Dictionary<TimeSpan, AccessControlClient>> _clients = new Dictionary<ServerId, Dictionary<TimeSpan, AccessControlClient>>();

	private object _lockObject = new object();

	public AccessControlClient GetClient(ServerId serverId)
	{
		TimeSpan timeOutFromAppConfig = GetTimeOutFromAppConfig("AccessControlClientManagerTimeoutInMinutes");
		return GetClient(serverId, timeOutFromAppConfig);
	}

	public AccessControlClient GetClient(ServerId serverId, TimeSpan requestTimeout)
	{
		lock (_lockObject)
		{
			if (!_clients.TryGetValue(serverId, out var value))
			{
				value = new Dictionary<TimeSpan, AccessControlClient>();
				_clients.Add(serverId, value);
			}
			if (!value.TryGetValue(requestTimeout, out var value2))
			{
				value2 = CreateAccessControlClient(serverId, requestTimeout);
				if (value2 == null)
				{
					throw new ServiceNotRegisteredException();
				}
				value.Add(requestTimeout, value2);
			}
			return value2;
		}
	}

	public void Close()
	{
		lock (_lockObject)
		{
			foreach (Dictionary<TimeSpan, AccessControlClient> value in _clients.Values)
			{
				foreach (AccessControlClient value2 in value.Values)
				{
					value2.Dispose();
				}
			}
			_clients.Clear();
		}
	}

	private AccessControlClient CreateAccessControlClient(ServerId serverId, TimeSpan requestTimeout)
	{
		VideoOS.Platform.Configuration instance = VideoOS.Platform.Configuration.Instance;
		try
		{
			List<VideoOS.Platform.Configuration.ServiceURIInfo> registeredServiceUriInfo = instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, serverId);
			if (registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray.Count == 0)
			{
				throw new ServiceNotRegisteredException();
			}
			Version eventServerVersion = ServerReceiver.GetEventServerVersion2(registeredServiceUriInfo[0]);
			return CreateAccessControlClient(serverId, eventServerVersion, new Uri(registeredServiceUriInfo[0].UriArray[0]), requestTimeout);
		}
		catch (CommunicationMIPException innerException)
		{
			throw new ServiceNotRegisteredException("Error retrieving registered services", innerException);
		}
	}

	protected virtual AccessControlClient CreateAccessControlClient(ServerId serverId, Version eventServerVersion, Uri uri, TimeSpan requestTimeout)
	{
		return AccessControlClientFactory.Create(serverId, eventServerVersion, uri, requestTimeout);
	}

	public TimeSpan GetTimeOutFromAppConfig(string parameterName)
	{
		int result = 0;
		string text = ConfigurationManager.AppSettings.Get(parameterName);
		if (!string.IsNullOrEmpty(text) && int.TryParse(text, out result) && result > 0)
		{
			_defaultRequestTimeout = TimeSpan.FromMinutes(result);
		}
		return _defaultRequestTimeout;
	}
}
