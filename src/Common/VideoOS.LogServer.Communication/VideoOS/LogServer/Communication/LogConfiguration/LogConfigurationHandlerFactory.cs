using System;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.CommunicationFactory;
using VideoOS.LogServer.Communication.LogConfigurationServiceReference;
using VideoOS.LogServer.Communication.MIP;
using VideoOS.Platform;

namespace VideoOS.LogServer.Communication.LogConfiguration;

public static class LogConfigurationHandlerFactory
{
	public static ILogConfigurationHandler Create(ILogger logger)
	{
		if (!EnvironmentWrapper.IsInitialized)
		{
			throw new MIPEnvironmentNotInitializedException("Cannot create ILogConfigurationHandler instance without MIP environment being initialized.");
		}
		return Create(logger, new EnvironmentWrapper());
	}

	public static ILogConfigurationHandler Create(ILogger logger, ServerId managementServerId)
	{
		if (!EnvironmentWrapper.IsInitialized)
		{
			throw new MIPEnvironmentNotInitializedException("Cannot create ILogConfigurationHandler instance without MIP environment being initialized.");
		}
		return Create(logger, new EnvironmentWrapper(managementServerId));
	}

	private static ILogConfigurationHandler Create(ILogger logger, IEnvironmentWrapper environmentWrapper)
	{
		ClientWrapper<LogConfigurationServiceClient> clientWrapper = new ClientWrapper<LogConfigurationServiceClient>(new LogConfigurationServiceClientFactory(environmentWrapper, logger), logger);
		ClientWrapper<LogConfigurationTokenServiceClient> tokenClientWrapper = new ClientWrapper<LogConfigurationTokenServiceClient>(new LogConfigurationTokenServiceClientFactory(environmentWrapper, logger), logger);
		return new LogConfigurationHandler(clientWrapper, tokenClientWrapper, environmentWrapper);
	}

	[Obsolete("Creating LogConfigurationHandler directly from logServerUri is deprecated. LogConfigurationHandler should only be created with a valid MIP environment present and initialized.")]
	public static ILogConfigurationHandler Create(ILogger logger, Uri logServerUri)
	{
		EnvironmentWrapper environmentWrapper = (EnvironmentWrapper.IsInitialized ? new EnvironmentWrapper() : null);
		ClientWrapper<LogConfigurationServiceClient> clientWrapper = new ClientWrapper<LogConfigurationServiceClient>(new LogConfigurationServiceClientFactory(environmentWrapper, logger, logServerUri), logger);
		ClientWrapper<LogConfigurationTokenServiceClient> tokenClientWrapper = new ClientWrapper<LogConfigurationTokenServiceClient>(new LogConfigurationTokenServiceClientFactory(environmentWrapper, logger, logServerUri), logger);
		return new LogConfigurationHandler(clientWrapper, tokenClientWrapper, environmentWrapper);
	}
}
