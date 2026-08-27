using System;
using System.Net;
using System.ServiceModel;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.LogConfigurationServiceReference;
using VideoOS.LogServer.Communication.MIP;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public class LogConfigurationTokenServiceClientFactory : LogServiceClientFactoryBase<LogConfigurationTokenServiceClient>
{
	private readonly IEnvironmentWrapper _environmentWrapper;

	private readonly Uri _logServerUri;

	public LogConfigurationTokenServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger)
		: base(logger)
	{
		_environmentWrapper = environmentWrapper ?? throw new ArgumentNullException("environmentWrapper");
	}

	public LogConfigurationTokenServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger, Uri logServerUri)
		: base(logger)
	{
		if (logServerUri == null)
		{
			throw new ArgumentNullException("logServerUri");
		}
		_logServerUri = logServerUri;
		_environmentWrapper = environmentWrapper;
	}

	public override LogConfigurationTokenServiceClient Create()
	{
		if (_environmentWrapper != null)
		{
			EndpointAddress serverAddress = LookupServerAddress(_environmentWrapper, "LogConfigurationToken", isToken: true);
			return CreateClient(CredentialCache.DefaultNetworkCredentials, serverAddress);
		}
		if (_logServerUri != null)
		{
			EndpointAddress serverAddress2 = LookupServerAddress(_logServerUri, "LogConfigurationToken", isToken: true);
			return CreateClient(CredentialCache.DefaultNetworkCredentials, serverAddress2);
		}
		throw new InvalidOperationException("Couldn't create the instance of LogRetentionServiceClient without MIP environment being initialized or Uri being provided.");
	}

	private static LogConfigurationTokenServiceClient CreateClient(NetworkCredential networkCredential, EndpointAddress serverAddress)
	{
		LogConfigurationTokenServiceClient logConfigurationTokenServiceClient = new LogConfigurationTokenServiceClient(LogServiceClientFactoryBase<LogConfigurationTokenServiceClient>.GetTokenBinding(), serverAddress);
		try
		{
			LogServiceClientFactoryBase<LogConfigurationTokenServiceClient>.UpdateEndPoint(logConfigurationTokenServiceClient.Endpoint);
			logConfigurationTokenServiceClient.ClientCredentials.Windows.ClientCredential = networkCredential;
			return logConfigurationTokenServiceClient;
		}
		catch (Exception)
		{
			logConfigurationTokenServiceClient.Close();
			throw;
		}
	}
}
