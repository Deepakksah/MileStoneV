using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.CommunicationFactory;
using VideoOS.LogServer.Communication.LogReaderServiceReference;
using VideoOS.LogServer.Communication.MIP;
using VideoOS.Platform;

namespace VideoOS.LogServer.Communication.Readers;

public static class LogReaderFactory
{
	public static ILogReader Create(ILogger logger)
	{
		if (!EnvironmentWrapper.IsInitialized)
		{
			throw new MIPEnvironmentNotInitializedException("Cannot create ILogReader instance without MIP environment being initialized.");
		}
		return Create(logger, new EnvironmentWrapper());
	}

	public static ILogReader Create(ILogger logger, ServerId managementServerId)
	{
		if (!EnvironmentWrapper.IsInitialized)
		{
			throw new MIPEnvironmentNotInitializedException("Cannot create ILogReader instance without MIP environment being initialized.");
		}
		return Create(logger, new EnvironmentWrapper(managementServerId));
	}

	private static ILogReader Create(ILogger logger, IEnvironmentWrapper environmentWrapper)
	{
		ClientWrapper<LogReaderServiceClient> clientWrapper = new ClientWrapper<LogReaderServiceClient>(new LogReaderServiceClientFactory(environmentWrapper, logger), logger);
		ClientWrapper<LogReaderTokenServiceClient> tokenClientWrapper = new ClientWrapper<LogReaderTokenServiceClient>(new LogReaderTokenServiceClientFactory(environmentWrapper, logger), logger);
		return new LogReader(clientWrapper, tokenClientWrapper, environmentWrapper);
	}
}
