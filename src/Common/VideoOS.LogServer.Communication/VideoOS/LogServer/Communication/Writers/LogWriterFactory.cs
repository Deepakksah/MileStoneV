using System;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.CommunicationFactory;
using VideoOS.LogServer.Communication.LogWriterServiceReference;
using VideoOS.LogServer.Communication.MIP;
using VideoOS.Platform;

namespace VideoOS.LogServer.Communication.Writers;

public static class LogWriterFactory
{
	public static ILogWriter Create(ILogger logger)
	{
		if (!EnvironmentWrapper.IsInitialized)
		{
			throw new MIPEnvironmentNotInitializedException("Cannot create ILogWriter instance without MIP environment being initialized.");
		}
		return Create(logger, new EnvironmentWrapper());
	}

	public static ILogWriter Create(ILogger logger, ServerId managementServerId)
	{
		if (!EnvironmentWrapper.IsInitialized)
		{
			throw new MIPEnvironmentNotInitializedException("Cannot create ILogWriter instance without MIP environment being initialized.");
		}
		return Create(logger, new EnvironmentWrapper(managementServerId));
	}

	private static ILogWriter Create(ILogger logger, IEnvironmentWrapper environmentWrapper)
	{
		ClientWrapper<LogWriterServiceClient> clientWrapper = new ClientWrapper<LogWriterServiceClient>(new LogWriterServiceClientFactory(environmentWrapper, logger), logger);
		ClientWrapper<LogWriterTokenServiceClient> tokenClientWrapper = new ClientWrapper<LogWriterTokenServiceClient>(new LogWriterTokenServiceClientFactory(environmentWrapper, logger), logger);
		return new LogWriter(clientWrapper, tokenClientWrapper, environmentWrapper, logger);
	}

	[Obsolete("Creating LogWriter directly from logServerUri is deprecated. LogWriter should only be created with a valid MIP environment present and initialized.")]
	public static ILogWriter Create(ILogger logger, Uri logServerUri)
	{
		EnvironmentWrapper environmentWrapper = (EnvironmentWrapper.IsInitialized ? new EnvironmentWrapper() : null);
		ClientWrapper<LogWriterServiceClient> clientWrapper = new ClientWrapper<LogWriterServiceClient>(new LogWriterServiceClientFactory(environmentWrapper, logger, logServerUri), logger);
		ClientWrapper<LogWriterTokenServiceClient> tokenClientWrapper = new ClientWrapper<LogWriterTokenServiceClient>(new LogWriterTokenServiceClientFactory(environmentWrapper, logger, logServerUri), logger);
		return new LogWriter(clientWrapper, tokenClientWrapper, environmentWrapper, logger);
	}
}
