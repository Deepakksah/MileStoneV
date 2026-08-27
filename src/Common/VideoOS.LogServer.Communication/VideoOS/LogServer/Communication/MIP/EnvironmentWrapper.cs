using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.LogServer.Communication.MIP;

public class EnvironmentWrapper : IEnvironmentWrapper
{
	public static bool IsInitialized => Configuration.Instance != null;

	public ServerId ServerId { get; }

	public LoginSettings LoginSettings => LoginSettingsCache.GetLoginSettings(ServerId);

	public EnvironmentWrapper()
		: this(null)
	{
	}

	public EnvironmentWrapper(ServerId managementServerId)
	{
		if (Configuration.Instance == null)
		{
			throw new MIPEnvironmentNotInitializedException();
		}
		ServerId = managementServerId ?? Configuration.Instance.ServerFQID.ServerId;
	}

	public IEnumerable<Configuration.ServiceURIInfo> GetRegisteredServiceUriInfo(Guid serviceType)
	{
		IEnumerable<Configuration.ServiceURIInfo> registeredServiceUriInfo = ServiceUtil.GetRegisteredServiceUriInfo(serviceType, ServerId);
		return registeredServiceUriInfo ?? Enumerable.Empty<Configuration.ServiceURIInfo>();
	}
}
