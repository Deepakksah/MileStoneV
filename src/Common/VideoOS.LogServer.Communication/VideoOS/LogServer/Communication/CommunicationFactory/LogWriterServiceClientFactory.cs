using System;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.LogWriterServiceReference;
using VideoOS.LogServer.Communication.MIP;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public class LogWriterServiceClientFactory : LogServiceClientFactoryBase<LogWriterServiceClient>
{
	private readonly IEnvironmentWrapper _environmentWrapper;

	private readonly Uri _logServerUri;

	private readonly LogServiceClientBindingParameters _bindingParameters = ConfigSettings.LogWriterServiceClientBindingParameters;

	public LogWriterServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger)
		: base(logger)
	{
		_environmentWrapper = environmentWrapper ?? throw new ArgumentNullException("environmentWrapper");
	}

	public LogWriterServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger, Uri logServerUri)
		: base(logger)
	{
		_environmentWrapper = environmentWrapper;
		_logServerUri = logServerUri;
	}

	public override LogWriterServiceClient Create()
	{
		LoggerExtensions.LogDebug(Logger, string.Format(CultureInfo.InvariantCulture, "Creating LogWriterServiceClient with binding parameters: {0}", _bindingParameters), Array.Empty<object>());
		if (_environmentWrapper != null)
		{
			EndpointAddress serverAddress = LookupServerAddress(_environmentWrapper, "LogWriter");
			if (_environmentWrapper.LoginSettings.IsOAuthIdentity)
			{
				throw new NotSupportedException("LogWriterServiceClientFactory being called for an OAuth user - should not happen");
			}
			return CreateClient(_environmentWrapper.LoginSettings.NetworkCredential, serverAddress, _bindingParameters);
		}
		if (_logServerUri != null)
		{
			EndpointAddress serverAddress2 = LookupServerAddress(_logServerUri, "LogWriter");
			return CreateClient(CredentialCache.DefaultNetworkCredentials, serverAddress2, _bindingParameters);
		}
		throw new InvalidOperationException("Couldn't create the instance of LogWriterServiceClient without MIP environment being initialized or Uri being provided.");
	}

	private static LogWriterServiceClient CreateClient(NetworkCredential networkCredential, EndpointAddress serverAddress, LogServiceClientBindingParameters bindingParameters)
	{
		LogWriterServiceClient logWriterServiceClient = new LogWriterServiceClient(LogServiceClientFactoryBase<LogWriterServiceClient>.GetBinding(bindingParameters), serverAddress);
		try
		{
			LogServiceClientFactoryBase<LogWriterServiceClient>.UpdateEndPoint(logWriterServiceClient.Endpoint);
			logWriterServiceClient.ClientCredentials.Windows.ClientCredential = networkCredential;
			return logWriterServiceClient;
		}
		catch (Exception)
		{
			logWriterServiceClient.Close();
			throw;
		}
	}
}
