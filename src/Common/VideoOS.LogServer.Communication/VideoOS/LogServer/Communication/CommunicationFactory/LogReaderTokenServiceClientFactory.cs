using System;
using System.Net;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.LogReaderServiceReference;
using VideoOS.LogServer.Communication.MIP;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public class LogReaderTokenServiceClientFactory : LogServiceClientFactoryBase<LogReaderTokenServiceClient>
{
	private readonly IEnvironmentWrapper _environmentWrapper;

	public LogReaderTokenServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger)
		: base(logger)
	{
		_environmentWrapper = environmentWrapper ?? throw new ArgumentNullException("environmentWrapper");
	}

	public override LogReaderTokenServiceClient Create()
	{
		LogReaderTokenServiceClient logReaderTokenServiceClient = new LogReaderTokenServiceClient(LogServiceClientFactoryBase<LogReaderTokenServiceClient>.GetTokenBinding(), LookupServerAddress(_environmentWrapper, "LogReaderToken", isToken: true));
		try
		{
			LogServiceClientFactoryBase<LogReaderTokenServiceClient>.UpdateEndPoint(logReaderTokenServiceClient.Endpoint);
			logReaderTokenServiceClient.ClientCredentials.Windows.ClientCredential = CredentialCache.DefaultNetworkCredentials;
			return logReaderTokenServiceClient;
		}
		catch (Exception)
		{
			logReaderTokenServiceClient.Close();
			throw;
		}
	}
}
