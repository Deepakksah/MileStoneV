using System.Collections.Generic;

namespace VideoOS.EventServer.Communication.Configuration;

public class EventRetentionUpdate
{
	private static EventRetentionUpdate _instance = new EventRetentionUpdate();

	private List<ProviderType> _registeredProviders = new List<ProviderType>();

	private object _lockObject = new object();

	private UpdateEventsExpirationParams _requestParams = new UpdateEventsExpirationParams();

	public static EventRetentionUpdate Instance => _instance;

	private EventRetentionUpdate()
	{
	}

	public void RegisterProvider(ProviderType provider)
	{
		lock (_lockObject)
		{
			if (_registeredProviders.Count == 0)
			{
				_requestParams.DaysToKeepEventServerEvents = -1;
				_requestParams.DaysToKeepAccessControlEvents = -1;
				_requestParams.DaysToKeepLPREvents = -1;
			}
			if (!_registeredProviders.Contains(provider))
			{
				_registeredProviders.Add(provider);
			}
		}
	}

	public void SetRetentionValue(ProviderType provider, int retentionValue, ConfigurationClient client)
	{
		lock (_lockObject)
		{
			_registeredProviders.Remove(provider);
			switch (provider)
			{
			case ProviderType.EventServer:
				_requestParams.DaysToKeepEventServerEvents = retentionValue;
				break;
			case ProviderType.AccessControl:
				_requestParams.DaysToKeepAccessControlEvents = retentionValue;
				break;
			case ProviderType.LPR:
				_requestParams.DaysToKeepLPREvents = retentionValue;
				break;
			}
			if (_registeredProviders.Count == 0 && client != null && (_requestParams.DaysToKeepEventServerEvents != -1 || _requestParams.DaysToKeepAccessControlEvents != -1 || _requestParams.DaysToKeepLPREvents != -1))
			{
				client.UpdateEventsExpiration(_requestParams);
			}
		}
	}
}
