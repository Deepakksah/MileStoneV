using System;

namespace VideoOS.Server;

public interface IWebApiConfig
{
	int Port { get; }

	TimeSpan ServiceDiscoveryTimeout { get; }

	TimeSpan ServiceDiscoveryCheckInterval { get; }

	TimeSpan SleepBetweenRecorderReconnection { get; }

	TimeSpan SleepBetweenGetCompleteConfigurationReconnection { get; }

	Uri AuthorizationServerUri { get; }

	bool SecuredServer { get; }

	string Scheme { get; }
}
