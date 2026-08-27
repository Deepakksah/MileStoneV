using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public class LogQuery
{
	private LogType _logType;

	private int _page;

	private int _pageSize;

	private DateTime _startDate;

	private DateTime _endDate;

	private LogQueryFormat _logQueryFormat;

	private LogFilter[] _logFilters;

	[DataMember]
	public LogType LogType
	{
		get
		{
			return _logType;
		}
		set
		{
			_logType = value;
		}
	}

	[DataMember]
	public string LanguageName { get; set; }

	[DataMember]
	public int Page
	{
		get
		{
			return _page;
		}
		set
		{
			_page = value;
		}
	}

	[DataMember]
	public int PageSize
	{
		get
		{
			return _pageSize;
		}
		set
		{
			_pageSize = value;
		}
	}

	[DataMember]
	public DateTime StartDate
	{
		get
		{
			return _startDate;
		}
		set
		{
			_startDate = value;
		}
	}

	[DataMember]
	public DateTime EndDate
	{
		get
		{
			return _endDate;
		}
		set
		{
			_endDate = value;
		}
	}

	[DataMember]
	public LogQueryFormat QueryFormat
	{
		get
		{
			return _logQueryFormat;
		}
		set
		{
			_logQueryFormat = value;
		}
	}

	[DataMember]
	public LogFilter[] LogFilters
	{
		get
		{
			if (_logFilters == null)
			{
				return new LogFilter[0];
			}
			return _logFilters;
		}
		private set
		{
			_logFilters = value;
		}
	}

	public LogQuery(LogType logType, string languageName)
	{
		_logType = logType;
		LanguageName = languageName;
	}

	public LogQuery(LogType logType, string languageName, int page, int pageSize)
		: this(logType, languageName)
	{
		_page = page;
		_pageSize = pageSize;
	}

	public LogQuery(LogType logType, string languageName, int page, int pageSize, LogFilter[] logFilters)
		: this(logType, languageName)
	{
		_page = page;
		_pageSize = pageSize;
		_logFilters = logFilters;
	}

	public void SetFilter(LogFilter logFilter)
	{
		if (logFilter == null)
		{
			throw new ArgumentNullException("logFilter");
		}
		if (string.IsNullOrEmpty(logFilter.Value))
		{
			RemoveFilter(logFilter);
			return;
		}
		if (_logFilters == null)
		{
			_logFilters = new LogFilter[1] { logFilter };
		}
		else
		{
			RemoveFilter(logFilter);
			AddFilter(logFilter);
		}
		logFilter.CultureName = LanguageName;
	}

	public void ClearLogFilters()
	{
		_logFilters = null;
	}

	private Dictionary<LogFilterEntry, LogFilter> BuildDictionary()
	{
		Dictionary<LogFilterEntry, LogFilter> dictionary = new Dictionary<LogFilterEntry, LogFilter>();
		if (_logFilters != null)
		{
			LogFilter[] logFilters = _logFilters;
			foreach (LogFilter logFilter in logFilters)
			{
				dictionary.Add(logFilter.LogFilterEntry, logFilter);
			}
		}
		return dictionary;
	}

	private void AddFilter(LogFilter logFilter)
	{
		Dictionary<LogFilterEntry, LogFilter> dictionary = BuildDictionary();
		dictionary.Add(logFilter.LogFilterEntry, logFilter);
		_logFilters = new LogFilter[dictionary.Count];
		dictionary.Values.CopyTo(_logFilters, 0);
	}

	private void RemoveFilter(LogFilter logFilter)
	{
		Dictionary<LogFilterEntry, LogFilter> dictionary = BuildDictionary();
		dictionary.Remove(logFilter.LogFilterEntry);
		_logFilters = new LogFilter[dictionary.Count];
		dictionary.Values.CopyTo(_logFilters, 0);
	}

	public LogQuery Clone()
	{
		LogQuery logQuery = new LogQuery(_logType, LanguageName);
		logQuery.Page = _page;
		logQuery.PageSize = _pageSize;
		logQuery.StartDate = _startDate;
		logQuery.EndDate = _endDate;
		logQuery.QueryFormat = _logQueryFormat;
		if (_logFilters != null)
		{
			LogFilter[] logFilters = _logFilters;
			foreach (LogFilter logFilter in logFilters)
			{
				logQuery.AddFilter(logFilter.Clone());
			}
		}
		return logQuery;
	}
}
