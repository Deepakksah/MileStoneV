using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.LogServer.Communication.LogReaderServiceReference;
using VideoOS.LogServer.Communication.MIP;

namespace VideoOS.LogServer.Communication.Readers;

public class LogReader : ILogReader
{
	private readonly IClientWrapper<LogReaderServiceClient> _clientWrapper;

	private readonly IClientWrapper<LogReaderTokenServiceClient> _tokenClientWrapper;

	private readonly IEnvironmentWrapper _environmentWrapper;

	private Guid _querySessionId;

	public int BatchSize { get; set; } = 100;

	public int TotalLoglinesCount { get; private set; }

	public bool IsQuerySessionStarted => !_querySessionId.Equals(default(Guid));

	public LogReader(IClientWrapper<LogReaderServiceClient> clientWrapper, IClientWrapper<LogReaderTokenServiceClient> tokenClientWrapper)
		: this(clientWrapper, tokenClientWrapper, null)
	{
	}

	public LogReader(IClientWrapper<LogReaderServiceClient> clientWrapper, IClientWrapper<LogReaderTokenServiceClient> tokenClientWrapper, IEnvironmentWrapper environmentWrapper)
	{
		_clientWrapper = clientWrapper;
		_tokenClientWrapper = tokenClientWrapper;
		_environmentWrapper = environmentWrapper;
	}

	public async Task StartQuerySessionAsync(QueryFilters queryFilters, string language)
	{
		string text = QueryToken();
		if (text != null)
		{
			await StartQuerySessionTokenAsync(text, queryFilters, language);
			return;
		}
		StartQuerySessionResp startQuerySessionResp = await _clientWrapper.CallWithRetryAsync((LogReaderServiceClient client) => client.StartQuerySessionAsync(new StartQuerySessionParams
		{
			QueryFilters = queryFilters,
			Language = language
		}));
		_querySessionId = startQuerySessionResp.QuerySessionId;
		TotalLoglinesCount = startQuerySessionResp.TotalLoglinesCount;
	}

	public async Task StartQuerySessionTokenAsync(string token, QueryFilters queryFilters, string language)
	{
		StartQuerySessionResp startQuerySessionResp = await _tokenClientWrapper.CallWithRetryAsync((LogReaderTokenServiceClient client) => client.StartQuerySessionTokenAsync(token, new StartQuerySessionParams
		{
			QueryFilters = queryFilters,
			Language = language
		}));
		_querySessionId = startQuerySessionResp.QuerySessionId;
		TotalLoglinesCount = startQuerySessionResp.TotalLoglinesCount;
	}

	public async Task<IEnumerable<AuditLogline>> FetchAuditLoglinesAsync(int offset)
	{
		string text = QueryToken();
		if (text != null)
		{
			return await FetchAuditLoglinesTokenAsync(text, offset);
		}
		return (await _clientWrapper.CallWithRetryAsync((LogReaderServiceClient client) => client.FetchAuditLoglinesAsync(new FetchLoglinesParams
		{
			QuerySessionId = _querySessionId,
			BatchSize = BatchSize,
			Offset = offset
		}))).Loglines;
	}

	public async Task<IEnumerable<AuditLogline>> FetchAuditLoglinesTokenAsync(string token, int offset)
	{
		return (await _tokenClientWrapper.CallWithRetryAsync((LogReaderTokenServiceClient client) => client.FetchAuditLoglinesTokenAsync(token, new FetchLoglinesParams
		{
			QuerySessionId = _querySessionId,
			BatchSize = BatchSize,
			Offset = offset
		}))).Loglines;
	}

	public async Task<IEnumerable<SystemLogline>> FetchSystemLoglinesAsync(int offset)
	{
		string text = QueryToken();
		if (text != null)
		{
			return await FetchSystemLoglinesTokenAsync(text, offset);
		}
		return (await _clientWrapper.CallWithRetryAsync((LogReaderServiceClient client) => client.FetchSystemLoglinesAsync(new FetchLoglinesParams
		{
			QuerySessionId = _querySessionId,
			BatchSize = BatchSize,
			Offset = offset
		}))).Loglines;
	}

	public async Task<IEnumerable<SystemLogline>> FetchSystemLoglinesTokenAsync(string token, int offset)
	{
		return (await _tokenClientWrapper.CallWithRetryAsync((LogReaderTokenServiceClient client) => client.FetchSystemLoglinesTokenAsync(token, new FetchLoglinesParams
		{
			QuerySessionId = _querySessionId,
			BatchSize = BatchSize,
			Offset = offset
		}))).Loglines;
	}

	public async Task<IEnumerable<RuleLogline>> FetchRuleLoglinesAsync(int offset)
	{
		string text = QueryToken();
		if (text != null)
		{
			return await FetchRuleLoglinesTokenAsync(text, offset);
		}
		return (await _clientWrapper.CallWithRetryAsync((LogReaderServiceClient client) => client.FetchRuleLoglinesAsync(new FetchLoglinesParams
		{
			QuerySessionId = _querySessionId,
			BatchSize = BatchSize,
			Offset = offset
		}))).Loglines;
	}

	public async Task<IEnumerable<RuleLogline>> FetchRuleLoglinesTokenAsync(string token, int offset)
	{
		return (await _tokenClientWrapper.CallWithRetryAsync((LogReaderTokenServiceClient client) => client.FetchRuleLoglinesTokenAsync(token, new FetchLoglinesParams
		{
			QuerySessionId = _querySessionId,
			BatchSize = BatchSize,
			Offset = offset
		}))).Loglines;
	}

	public async Task EndQuerySessionAsync()
	{
		if (!IsQuerySessionStarted)
		{
			return;
		}
		string text = QueryToken();
		if (text != null)
		{
			await EndQuerySessionTokenAsync(text);
			return;
		}
		EndQuerySessionParams endQuerySessionParams = new EndQuerySessionParams
		{
			QuerySessionId = _querySessionId
		};
		await _clientWrapper.CallWithRetryAsync((LogReaderServiceClient client) => client.EndQuerySessionAsync(endQuerySessionParams));
		_querySessionId = default(Guid);
	}

	public async Task EndQuerySessionTokenAsync(string token)
	{
		if (IsQuerySessionStarted)
		{
			EndQuerySessionParams endQuerySessionParams = new EndQuerySessionParams
			{
				QuerySessionId = _querySessionId
			};
			await _tokenClientWrapper.CallWithRetryAsync((LogReaderTokenServiceClient client) => client.EndQuerySessionTokenAsync(token, endQuerySessionParams));
			_querySessionId = default(Guid);
		}
	}

	public async Task<IDictionary<FilterType, FilterValue[]>> GetFilterValuesAsync(string language, LogType logType, IEnumerable<FilterType> filterTypes)
	{
		string text = QueryToken();
		if (text != null)
		{
			return await GetFilterValuesTokenAsync(text, language, logType, filterTypes);
		}
		return (await _clientWrapper.CallWithRetryAsync((LogReaderServiceClient client) => client.GetFilterValuesAsync(new GetFilterValuesParams
		{
			Language = language,
			LogType = logType,
			FilterTypes = filterTypes.ToArray()
		}))).FilterValues;
	}

	public async Task<IDictionary<FilterType, FilterValue[]>> GetFilterValuesTokenAsync(string token, string language, LogType logType, IEnumerable<FilterType> filterTypes)
	{
		return (await _tokenClientWrapper.CallWithRetryAsync((LogReaderTokenServiceClient client) => client.GetFilterValuesTokenAsync(token, new GetFilterValuesParams
		{
			Language = language,
			LogType = logType,
			FilterTypes = filterTypes.ToArray()
		}))).FilterValues;
	}

	public async Task<IDictionary<FilterType, FilterValue[]>> GetFilterValuesAsync(string language, LogType logType, DateTime fromTime, DateTime toTime, IEnumerable<FilterType> filterTypes)
	{
		string text = QueryToken();
		if (text != null)
		{
			return await GetFilterValuesTokenAsync(text, language, logType, filterTypes);
		}
		return (await _clientWrapper.CallWithRetryAsync((LogReaderServiceClient client) => client.GetFilterValuesTimestampAsync(new GetFilterValuesTimestampParams
		{
			Language = language,
			LogType = logType,
			FromTime = fromTime,
			ToTime = toTime,
			FilterTypes = filterTypes.ToArray()
		}))).FilterValues;
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
