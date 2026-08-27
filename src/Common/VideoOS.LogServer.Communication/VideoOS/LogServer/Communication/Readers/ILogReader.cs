using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.LogServer.Communication.LogReaderServiceReference;

namespace VideoOS.LogServer.Communication.Readers;

public interface ILogReader
{
	int BatchSize { get; set; }

	int TotalLoglinesCount { get; }

	bool IsQuerySessionStarted { get; }

	Task StartQuerySessionAsync(QueryFilters queryFilters, string language);

	Task StartQuerySessionTokenAsync(string token, QueryFilters queryFilters, string language);

	Task<IEnumerable<AuditLogline>> FetchAuditLoglinesAsync(int offset);

	Task<IEnumerable<AuditLogline>> FetchAuditLoglinesTokenAsync(string token, int offset);

	Task<IEnumerable<SystemLogline>> FetchSystemLoglinesAsync(int offset);

	Task<IEnumerable<SystemLogline>> FetchSystemLoglinesTokenAsync(string token, int offset);

	Task<IEnumerable<RuleLogline>> FetchRuleLoglinesAsync(int offset);

	Task<IEnumerable<RuleLogline>> FetchRuleLoglinesTokenAsync(string token, int offset);

	Task EndQuerySessionAsync();

	Task EndQuerySessionTokenAsync(string token);

	Task<IDictionary<FilterType, FilterValue[]>> GetFilterValuesAsync(string language, LogType logType, IEnumerable<FilterType> filterTypes);

	Task<IDictionary<FilterType, FilterValue[]>> GetFilterValuesAsync(string language, LogType logType, DateTime fromTime, DateTime toTime, IEnumerable<FilterType> filterTypes);

	Task<IDictionary<FilterType, FilterValue[]>> GetFilterValuesTokenAsync(string token, string language, LogType logType, IEnumerable<FilterType> filterTypes);
}
