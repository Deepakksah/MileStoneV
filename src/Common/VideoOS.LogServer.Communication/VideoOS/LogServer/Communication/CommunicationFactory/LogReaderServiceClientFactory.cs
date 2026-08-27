using System;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.LogReaderServiceReference;
using VideoOS.LogServer.Communication.MIP;

namespace VideoOS.LogServer.Communication.CommunicationFactory;

public class LogReaderServiceClientFactory : LogServiceClientFactoryBase<LogReaderServiceClient>
{
	private readonly IEnvironmentWrapper _environmentWrapper;

	private readonly LogServiceClientBindingParameters _bindingParameters = ConfigSettings.LogReaderServiceClientBindingParameters;

	public LogReaderServiceClientFactory(IEnvironmentWrapper environmentWrapper, ILogger logger)
		: base(logger)
	{
		_environmentWrapper = environmentWrapper ?? throw new ArgumentNullException("environmentWrapper");
	}

	public override LogReaderServiceClient Create()
	{
		LoggerExtensions.LogDebug(Logger, string.Format(CultureInfo.InvariantCulture, "Creating LogReaderServiceClient with binding parameters: {0}", _bindingParameters), Array.Empty<object>());
		LogReaderServiceClient logReaderServiceClient = new LogReaderServiceClient(LogServiceClientFactoryBase<LogReaderServiceClient>.GetBinding(_bindingParameters), LookupServerAddress(_environmentWrapper, "LogReader"));
		try
		{
			LogServiceClientFactoryBase<LogReaderServiceClient>.UpdateEndPoint(logReaderServiceClient.Endpoint);
			if (_environmentWrapper.LoginSettings.IsOAuthIdentity)
			{
				throw new NotSupportedException("LogReaderServiceClientFactory being called for an OAuth user - should not happen");
			}
			logReaderServiceClient.ClientCredentials.Windows.ClientCredential = _environmentWrapper.LoginSettings.NetworkCredential;
			return logReaderServiceClient;
		}
		catch (Exception)
		{
			logReaderServiceClient.Close();
			throw;
		}
	}
}
