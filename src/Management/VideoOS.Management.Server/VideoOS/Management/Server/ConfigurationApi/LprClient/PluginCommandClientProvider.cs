using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.LPR.Common.WCF.Client;
using VideoOS.LPR.Common.WCF.DataContracts;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi.LprClient;

public class PluginCommandClientProvider : IPluginCommandClientProvider
{
	private static readonly Guid EventServerServiceType = new Guid("08AB8F23-9AEF-4298-9CAA-F39259FE7CDA");

	private PluginCommandClientWrapper _pluginCommandClient;

	private readonly object _clientLock = new object();

	public PluginCommandClientWrapper Client
	{
		get
		{
			lock (_clientLock)
			{
				if (_pluginCommandClient == null)
				{
					_pluginCommandClient = GetClient();
				}
				return _pluginCommandClient;
			}
		}
	}

	public void Close()
	{
		lock (_clientLock)
		{
			if (_pluginCommandClient != null)
			{
				_pluginCommandClient.Close();
				_pluginCommandClient = null;
			}
		}
	}

	private static PluginCommandClientWrapper GetClient()
	{
		Configuration.ServiceURIInfo? serviceURIInfo = null;
		try
		{
			serviceURIInfo = FindService(Configuration.Instance.GetRegisteredServiceUriInfo(EventServerServiceType, EnvironmentManager.Instance.CurrentSite.ServerId));
		}
		catch (CommunicationMIPException innerException)
		{
			throw new ServiceFaultException(ErrorCode.ImageServerCommunicationError, "GetRegisteredServiceUriInfo", innerException);
		}
		if (!serviceURIInfo.HasValue)
		{
			throw new ServiceFaultException(ErrorCode.EventServerNotRegistered, "GetRegisteredServiceUriInfo", null);
		}
		return new PluginCommandClientWrapper(EnvironmentManager.Instance.CurrentSite.ServerId, new Uri(serviceURIInfo.Value.UriArray[0]));
	}

	private static Configuration.ServiceURIInfo? FindService(IEnumerable<Configuration.ServiceURIInfo> services)
	{
		foreach (Configuration.ServiceURIInfo service in services)
		{
			if (service.UriArray.Any())
			{
				return service;
			}
		}
		return null;
	}
}
