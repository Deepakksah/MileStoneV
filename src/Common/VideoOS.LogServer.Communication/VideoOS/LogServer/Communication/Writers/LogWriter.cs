using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.LogWriterServiceReference;
using VideoOS.LogServer.Communication.MIP;
using VideoOS.LogServer.Communication.QueueingClient;

namespace VideoOS.LogServer.Communication.Writers;

public class LogWriter : ILogWriter, IDisposable
{
	private readonly TimeSpan _loopTime = TimeSpan.FromMilliseconds(500.0);

	private readonly TimeSpan[] _retryDelays = new TimeSpan[3]
	{
		TimeSpan.FromMilliseconds(500.0),
		TimeSpan.FromSeconds(5.0),
		TimeSpan.FromSeconds(30.0)
	};

	private readonly int _queueLength = 10000;

	private readonly QueueingClientWrapper<LogWriterServiceClient> _clientWrapper;

	private readonly QueueingClientWrapper<LogWriterTokenServiceClient> _tokenClientWrapper;

	private readonly IEnvironmentWrapper _environmentWrapper;

	private bool disposedValue;

	public LogWriter(IClientWrapper<LogWriterServiceClient> clientWrapper, IClientWrapper<LogWriterTokenServiceClient> tokenClientWrapper, ILogger logger)
		: this(clientWrapper, tokenClientWrapper, null, logger)
	{
	}

	public LogWriter(IClientWrapper<LogWriterServiceClient> clientWrapper, IClientWrapper<LogWriterTokenServiceClient> tokenClientWrapper, IEnvironmentWrapper environmentWrapper, ILogger logger)
	{
		_clientWrapper = new QueueingClientWrapper<LogWriterServiceClient>(clientWrapper, _retryDelays, _loopTime, _queueLength, logger);
		_tokenClientWrapper = new QueueingClientWrapper<LogWriterTokenServiceClient>(tokenClientWrapper, _retryDelays, _loopTime, _queueLength, logger);
		_environmentWrapper = environmentWrapper;
	}

	public async Task RegisterLogMessagesAsync(IEnumerable<LogMessage> logMessages)
	{
		string text = QueryToken();
		if (text != null)
		{
			await RegisterLogMessagesTokenAsync(text, logMessages);
			return;
		}
		await _clientWrapper.CallWithRetryAsync((LogWriterServiceClient client) => client.RegisterLogMessagesAsync(new RegisterLogMessagesParams
		{
			LogMessages = logMessages.ToArray()
		}));
	}

	public async Task RegisterLogMessagesTokenAsync(string token, IEnumerable<LogMessage> logMessages)
	{
		await _tokenClientWrapper.CallWithRetryAsync((LogWriterTokenServiceClient client) => client.RegisterLogMessagesTokenAsync(token, new RegisterLogMessagesParams
		{
			LogMessages = logMessages.ToArray()
		}));
	}

	public async Task RegisterLegacyLogMessagesAsync(IEnumerable<LegacyLogMessage> logMessages)
	{
		string text = QueryToken();
		if (text != null)
		{
			await RegisterLegacyLogMessagesTokenAsync(text, logMessages);
			return;
		}
		await _clientWrapper.CallWithRetryAsync((LogWriterServiceClient client) => client.RegisterLegacyLogMessagesAsync(new RegisterLegacyLogMessagesParams
		{
			LogMessages = logMessages.ToArray()
		}));
	}

	public async Task RegisterLegacyLogMessagesTokenAsync(string token, IEnumerable<LegacyLogMessage> logMessages)
	{
		await _tokenClientWrapper.CallWithRetryAsync((LogWriterTokenServiceClient client) => client.RegisterLegacyLogMessagesTokenAsync(token, new RegisterLegacyLogMessagesParams
		{
			LogMessages = logMessages.ToArray()
		}));
	}

	public async Task RegisterCategoriesAsync(IEnumerable<LogMessageCategory> categories)
	{
		string text = QueryToken();
		if (text != null)
		{
			await RegisterCategoriesTokenAsync(text, categories);
			return;
		}
		await _clientWrapper.CallWithRetryAsync((LogWriterServiceClient client) => client.RegisterCategoryAsync(new RegisterCategoryParams
		{
			Categories = categories.ToArray()
		}));
	}

	public async Task RegisterCategoriesTokenAsync(string token, IEnumerable<LogMessageCategory> categories)
	{
		await _tokenClientWrapper.CallWithRetryAsync((LogWriterTokenServiceClient client) => client.RegisterCategoryTokenAsync(token, new RegisterCategoryParams
		{
			Categories = categories.ToArray()
		}));
	}

	public async Task RegisterSourceTypesAsync(IEnumerable<LogMessageSourceType> sourceTypes)
	{
		string text = QueryToken();
		if (text != null)
		{
			await RegisterSourceTypesTokenAsync(text, sourceTypes);
			return;
		}
		await _clientWrapper.CallWithRetryAsync((LogWriterServiceClient client) => client.RegisterSourceTypeAsync(new RegisterSourceTypeParams
		{
			SourceTypes = sourceTypes.ToArray()
		}));
	}

	public async Task RegisterSourceTypesTokenAsync(string token, IEnumerable<LogMessageSourceType> sourceTypes)
	{
		await _tokenClientWrapper.CallWithRetryAsync((LogWriterTokenServiceClient client) => client.RegisterSourceTypeTokenAsync(token, new RegisterSourceTypeParams
		{
			SourceTypes = sourceTypes.ToArray()
		}));
	}

	public async Task WriteLoglineAsync(Guid messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, string serviceName, string ruleName, string eventType, LogLevel logLevel, PermissionType permissionType)
	{
		string text = QueryToken();
		if (text != null)
		{
			await WriteLoglineTokenAsync(text, messageId, parameters, clientTimestamp, source, user, userLocation, serviceName, ruleName, eventType, logLevel, permissionType);
			return;
		}
		await _clientWrapper.CallWithRetryAsync((LogWriterServiceClient client) => client.WriteLoglineAsync(new WriteLoglineParams
		{
			MessageId = messageId,
			Parameters = ((parameters == null) ? new Dictionary<string, string>() : new Dictionary<string, string>(parameters)),
			ClientTimestamp = clientTimestamp,
			Source = source,
			User = user,
			UserLocation = userLocation,
			ServiceName = serviceName,
			RuleName = ruleName,
			EventType = eventType,
			LogLevel = logLevel,
			PermissionType = permissionType
		}));
	}

	public async Task WriteLoglineTokenAsync(string token, Guid messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, string serviceName, string ruleName, string eventType, LogLevel logLevel, PermissionType permissionType)
	{
		await _tokenClientWrapper.CallWithRetryAsync((LogWriterTokenServiceClient client) => client.WriteLoglineTokenAsync(token, new WriteLoglineParams
		{
			MessageId = messageId,
			Parameters = ((parameters == null) ? new Dictionary<string, string>() : new Dictionary<string, string>(parameters)),
			ClientTimestamp = clientTimestamp,
			Source = source,
			User = user,
			UserLocation = userLocation,
			ServiceName = serviceName,
			RuleName = ruleName,
			EventType = eventType,
			LogLevel = logLevel,
			PermissionType = permissionType
		}));
	}

	public async Task WriteLegacyLoglineAsync(string messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, string serviceName, string ruleName, string eventType, LogLevel logLevel, PermissionType permissionType)
	{
		string text = QueryToken();
		if (text != null)
		{
			await WriteLegacyLoglineTokenAsync(text, messageId, parameters, clientTimestamp, source, user, userLocation, serviceName, ruleName, eventType, logLevel, permissionType);
			return;
		}
		await _clientWrapper.CallWithRetryAsync((LogWriterServiceClient client) => client.WriteLegacyLoglineAsync(new WriteLegacyLoglineParams
		{
			MessageId = messageId,
			Parameters = ((parameters == null) ? new Dictionary<string, string>() : new Dictionary<string, string>(parameters)),
			ClientTimestamp = clientTimestamp,
			Source = source,
			User = user,
			UserLocation = userLocation,
			ServiceName = serviceName,
			RuleName = ruleName,
			EventType = eventType,
			LogLevel = logLevel,
			PermissionType = permissionType
		}));
	}

	public async Task WriteLegacyLoglineTokenAsync(string token, string messageId, IDictionary<string, string> parameters, DateTime clientTimestamp, string source, string user, string userLocation, string serviceName, string ruleName, string eventType, LogLevel logLevel, PermissionType permissionType)
	{
		await _tokenClientWrapper.CallWithRetryAsync((LogWriterTokenServiceClient client) => client.WriteLegacyLoglineTokenAsync(token, new WriteLegacyLoglineParams
		{
			MessageId = messageId,
			Parameters = ((parameters == null) ? new Dictionary<string, string>() : new Dictionary<string, string>(parameters)),
			ClientTimestamp = clientTimestamp,
			Source = source,
			User = user,
			UserLocation = userLocation,
			ServiceName = serviceName,
			RuleName = ruleName,
			EventType = eventType,
			LogLevel = logLevel,
			PermissionType = permissionType
		}));
	}

	private string QueryToken()
	{
		if (_environmentWrapper != null && _environmentWrapper.LoginSettings != null && (_environmentWrapper.LoginSettings.IsBasicUser || _environmentWrapper.LoginSettings.IsOAuthIdentity))
		{
			return _environmentWrapper.LoginSettings.Token;
		}
		return null;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposedValue)
		{
			return;
		}
		if (disposing)
		{
			if (_clientWrapper != null)
			{
				_clientWrapper.Dispose();
			}
			if (_tokenClientWrapper != null)
			{
				_tokenClientWrapper.Dispose();
			}
		}
		disposedValue = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
