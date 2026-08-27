using System;
using System.Net;
using System.ServiceModel;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.LogWriterServiceReference;
using VideoOS.LogServer.Communication.MIP;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public class LogWriterTokenServiceClientFactory : LogServiceClientFactoryBase<LogWriterTokenServiceClient>
{
	private readonly IEnvironmentWrapper _environmentWrapper;

	private readonly Uri _logServerUri;

	public LogWriterTokenServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger)
		: base(logger)
	{
		_environmentWrapper = environmentWrapper ?? throw new ArgumentNullException("environmentWrapper");
	}

	public LogWriterTokenServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger, Uri logServerUri)
		: base(logger)
	{
		_environmentWrapper = environmentWrapper;
		_logServerUri = logServerUri;
	}

	public override LogWriterTokenServiceClient Create()
	{
		if (_environmentWrapper != null)
		{
			EndpointAddress serverAddress = LookupServerAddress(_environmentWrapper, "LogWriterToken", isToken: true);
			return CreateClient(CredentialCache.DefaultNetworkCredentials, serverAddress);
		}
		if (_logServerUri != null)
		{
			EndpointAddress serverAddress2 = LookupServerAddress(_logServerUri, "LogWriterToken", isToken: true);
			return CreateClient(CredentialCache.DefaultNetworkCredentials, serverAddress2);
		}
		throw new InvalidOperationException("Couldn't create the instance of LogWriterServiceClient without MIP environment being initialized or Uri being provided.");
	}

	private static LogWriterTokenServiceClient CreateClient(NetworkCredential networkCredential, EndpointAddress serverAddress)
	{
		LogWriterTokenServiceClient logWriterTokenServiceClient = new LogWriterTokenServiceClient(LogServiceClientFactoryBase<LogWriterTokenServiceClient>.GetTokenBinding(), serverAddress);
		try
		{
			LogServiceClientFactoryBase<LogWriterTokenServiceClient>.UpdateEndPoint(logWriterTokenServiceClient.Endpoint);
			logWriterTokenServiceClient.ClientCredentials.Windows.ClientCredential = networkCredential;
			return logWriterTokenServiceClient;
		}
		catch (Exception)
		{
			logWriterTokenServiceClient.Close();
			throw;
		}
	}
}
