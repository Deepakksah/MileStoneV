using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public class LogQueryResult
{
	private LogType _logType;

	private int _totalPages;

	private int _currentPage;

	private int _pageSize;

	private int _autualPageSize;

	private LogEntry[] _logEntries;

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
	public int TotalPages
	{
		get
		{
			return _totalPages;
		}
		set
		{
			_totalPages = value;
		}
	}

	[DataMember]
	public int CurrentPage
	{
		get
		{
			return _currentPage;
		}
		set
		{
			_currentPage = value;
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
	public int ActualPageSize
	{
		get
		{
			return _autualPageSize;
		}
		set
		{
			_autualPageSize = value;
		}
	}

	[DataMember]
	public LogEntry[] LogEntries
	{
		get
		{
			return _logEntries;
		}
		set
		{
			_logEntries = value;
		}
	}
}
