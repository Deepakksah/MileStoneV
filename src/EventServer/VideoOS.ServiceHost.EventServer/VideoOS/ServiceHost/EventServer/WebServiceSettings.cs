using System.Configuration;
using System.Globalization;
using VideoOS.ServiceHost.Common;
using VideoOS.Utilities.Validation;

namespace VideoOS.ServiceHost.EventServer;

public sealed class WebServiceSettings : IWebServiceSettings
{
	private readonly ICommunicationServiceHostLog _communicationServiceHostLog;

	private int _webServicePort = 22331;

	private bool _webServicePortRead;

	private static int _concurrentConnections = 100;

	private static bool _concurrentConnectionsRead;

	private static int _receiveTimeout = 120000;

	private static bool _receiveTimeoutRead;

	public int WebServicePort
	{
		get
		{
			if (!_webServicePortRead)
			{
				string text = ConfigurationManager.AppSettings["WebServicePort"];
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result) && result > 0 && result <= 65535)
				{
					_webServicePortRead = true;
					_webServicePort = result;
				}
				if (text != null && !_webServicePortRead)
				{
					_communicationServiceHostLog.Debug("MapWebService", string.Format(CultureInfo.InvariantCulture, "Error reading WebServicePort from app.config, using default: {0}", _webServicePort));
				}
				_webServicePortRead = true;
			}
			return _webServicePort;
		}
	}

	public int ConcurrentConnections
	{
		get
		{
			if (!_concurrentConnectionsRead)
			{
				string text = ConfigurationManager.AppSettings["WebServiceMaxConcurrentCalls"];
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result) && result > 0)
				{
					_concurrentConnectionsRead = true;
					_concurrentConnections = result;
				}
				if (text != null && !_concurrentConnectionsRead)
				{
					_communicationServiceHostLog.Debug("MapWebService", string.Format(CultureInfo.InvariantCulture, "Error reading WebServiceMaxConcurrentCalls from app.config, using default: {0}", _concurrentConnections));
				}
				_concurrentConnectionsRead = true;
			}
			return _concurrentConnections;
		}
	}

	public int ReceiveTimeout
	{
		get
		{
			if (!_receiveTimeoutRead)
			{
				string text = ConfigurationManager.AppSettings["WebServiceReceiveTimeout"];
				if (!string.IsNullOrEmpty(text) && int.TryParse(text, out var result) && result > 0)
				{
					_receiveTimeoutRead = true;
					_receiveTimeout = result;
				}
				if (text != null && !_receiveTimeoutRead)
				{
					_communicationServiceHostLog.Debug("MapWebService", string.Format(CultureInfo.InvariantCulture, "Error reading WebServiceReceiveTimeout from app.config, using default: {0}", _receiveTimeout));
				}
				_receiveTimeoutRead = true;
			}
			return _receiveTimeout;
		}
	}

	public WebServiceSettings(ICommunicationServiceHostLog communicationServiceHostLog)
		: this(communicationServiceHostLog, null)
	{
	}

	public WebServiceSettings(ICommunicationServiceHostLog communicationServiceHostLog, int? port = null)
	{
		communicationServiceHostLog.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Common\\VideoOS.ServiceHost.Common\\VideoOS.ServiceHost.EventServer\\WebServiceSettings.cs", 108);
		_communicationServiceHostLog = communicationServiceHostLog;
		if (port.HasValue)
		{
			_webServicePort = port.Value;
			_webServicePortRead = true;
		}
	}
}
