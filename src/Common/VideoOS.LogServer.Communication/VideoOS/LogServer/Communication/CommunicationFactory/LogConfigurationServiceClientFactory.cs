using System;
using System.Globalization;
using System.Net;
using System.ServiceModel;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.LogConfigurationServiceReference;
using VideoOS.LogServer.Communication.MIP;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public class LogConfigurationServiceClientFactory : LogServiceClientFactoryBase<LogConfigurationServiceClient>
{
	private readonly IEnvironmentWrapper _environmentWrapper;

	private readonly LogServiceClientBindingParameters _bindingParameters = ConfigSettings.LogConfigurationServiceClientBindingParameters;

	private readonly Uri _logServerUri;

	public LogConfigurationServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger)
		: base(logger)
	{
		_environmentWrapper = environmentWrapper ?? throw new ArgumentNullException("environmentWrapper");
	}

	public LogConfigurationServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger, Uri logServerUri)
		: base(logger)
	{
		_logServerUri = logServerUri;
		_environmentWrapper = environmentWrapper;
	}

	public override LogConfigurationServiceClient Create()
	{
		LoggerExtensions.LogDebug(Logger, string.Format(CultureInfo.InvariantCulture, "Creating LogConfigurationServiceClient with binding parameters: {0}", _bindingParameters), Array.Empty<object>());
		if (_environmentWrapper != null)
		{
			EndpointAddress serverAddress = LookupServerAddress(_environmentWrapper, "LogConfiguration");
			if (_environmentWrapper.LoginSettings.IsOAuthIdentity)
			{
				throw new NotSupportedException("LogConfigurationServiceClientFactory beging called for an OAuth user - should not happen");
			}
			return CreateClient(_environmentWrapper.LoginSettings.NetworkCredential, serverAddress, _bindingParameters);
		}
		if (_logServerUri != null)
		{
			EndpointAddress serverAddress2 = LookupServerAddress(_logServerUri, "LogConfiguration");
			return CreateClient(CredentialCache.DefaultNetworkCredentials, serverAddress2, _bindingParameters);
		}
		throw new InvalidOperationException("Couldn't create the instance of LogRetentionServiceClient without MIP environment being initialized or Uri being provided.");
	}

	private static LogConfigurationServiceClient CreateClient(NetworkCredential networkCredential, EndpointAddress serverAddress, LogServiceClientBindingParameters bindingParameters)
	{
		LogConfigurationServiceClient logConfigurationServiceClient = new LogConfigurationServiceClient(LogServiceClientFactoryBase<LogConfigurationServiceClient>.GetBinding(bindingParameters), serverAddress);
		try
		{
			LogServiceClientFactoryBase<LogConfigurationServiceClient>.UpdateEndPoint(logConfigurationServiceClient.Endpoint);
			logConfigurationServiceClient.ClientCredentials.Windows.ClientCredential = networkCredential;
			return logConfigurationServiceClient;
		}
		catch (Exception)
		{
			logConfigurationServiceClient.Close();
			throw;
		}
	}
}
