using System.Threading.Tasks;
using VideoOS.LogServer.Communication.LogConfigurationServiceReference;
using VideoOS.LogServer.Communication.MIP;

namespace VideoOS.LogServer.Communication.LogConfiguration;

public class LogConfigurationHandler : ILogConfigurationHandler
{
	private readonly ClientWrapper<LogConfigurationServiceClient> _clientWrapper;

	private readonly ClientWrapper<LogConfigurationTokenServiceClient> _tokenClientWrapper;

	private readonly IEnvironmentWrapper _environmentWrapper;

	public LogConfigurationHandler(ClientWrapper<LogConfigurationServiceClient> clientWrapper, ClientWrapper<LogConfigurationTokenServiceClient> tokenClientWrapper)
		: this(clientWrapper, tokenClientWrapper, null)
	{
	}

	public LogConfigurationHandler(ClientWrapper<LogConfigurationServiceClient> clientWrapper, ClientWrapper<LogConfigurationTokenServiceClient> tokenClientWrapper, IEnvironmentWrapper environmentWrapper)
	{
		_clientWrapper = clientWrapper;
		_tokenClientWrapper = tokenClientWrapper;
		_environmentWrapper = environmentWrapper;
	}

	public async Task<LogRetentions> GetLogRetentionsAsync()
	{
		string text = QueryToken();
		if (text != null)
		{
			return await GetLogRetentionsAsync(text);
		}
		return (await _clientWrapper.CallWithRetryAsync((LogConfigurationServiceClient client) => client.GetLogRetentionsAsync())).LogRetentions;
	}

	public async Task<LogRetentions> GetLogRetentionsAsync(string token)
	{
		return (await _tokenClientWrapper.CallWithRetryAsync((LogConfigurationTokenServiceClient client) => client.GetLogRetentionsTokenAsync(token))).LogRetentions;
	}

	public async Task UpdateLogRetentionsAsync(LogRetentions logRetentions)
	{
		string text = QueryToken();
		if (text != null)
		{
			await UpdateLogRetentionsAsync(logRetentions, text);
			return;
		}
		await _clientWrapper.CallWithRetryAsync((LogConfigurationServiceClient client) => client.UpdateLogRetentionsAsync(new UpdateLogRetentionsParams
		{
			LogRetentions = logRetentions
		}));
	}

	public async Task UpdateLogRetentionsAsync(LogRetentions logRetentions, string token)
	{
		await _tokenClientWrapper.CallWithRetryAsync((LogConfigurationTokenServiceClient client) => client.UpdateLogRetentionsTokenAsync(new UpdateLogRetentionsParams
		{
			LogRetentions = logRetentions
		}, token));
	}

	public async Task<bool> IsLegacyInterfaceLoggingEnabledAsync()
	{
		string text = QueryToken();
		if (text != null)
		{
			return await IsLegacyInterfaceLoggingEnabledAsync(text);
		}
		return (await _clientWrapper.CallWithRetryAsync((LogConfigurationServiceClient client) => client.GetAnonymousLoggingAsync())).Enabled;
	}

	public async Task<bool> IsLegacyInterfaceLoggingEnabledAsync(string token)
	{
		return (await _tokenClientWrapper.CallWithRetryAsync((LogConfigurationTokenServiceClient client) => client.GetAnonymousLoggingTokenAsync(token))).Enabled;
	}

	public async Task UpdateLegacyInterfaceLoggingAsync(bool enabled)
	{
		string text = QueryToken();
		if (text != null)
		{
			await UpdateLegacyInterfaceLoggingAsync(enabled, text);
			return;
		}
		await _clientWrapper.CallWithRetryAsync((LogConfigurationServiceClient client) => client.UpdateAnonymousLoggingAsync(new UpdateAnonymousLoggingParams
		{
			Enabled = enabled
		}));
	}

	public async Task UpdateLegacyInterfaceLoggingAsync(bool enabled, string token)
	{
		await _tokenClientWrapper.CallWithRetryAsync((LogConfigurationTokenServiceClient client) => client.UpdateAnonymousLoggingTokenAsync(new UpdateAnonymousLoggingParams
		{
			Enabled = enabled
		}, token));
	}

	private string QueryToken()
	{
		if (_environmentWrapper != null && _environmentWrapper.LoginSettings != null && (_environmentWrapper.LoginSettings.IsBasicUser || _environmentWrapper.LoginSettings.IsOAuthIdentity))
		{
			return _environmentWrapper.LoginSettings.Token;
		}
		return null;
	}
}
