using System;

namespace VideoOS.Common.Integration;

public class LogDefinition
{
	private LogType _logType;

	private string _name;

	private string _tableName;

	private string _dateTimeColumn;

	private int _millisecondsBetweenDatabaseWrites;

	private TimeSpan _timeToKeepLogEntries;

	private long _maxLogEntries;

	private TimeSpan _timeBetweenDatabaseWrites;

	private bool _enabled;

	private int _auditLogPlaybackFramesBeforeLogging;

	private int _auditLogPlaybackWindowSizeInSec;

	private bool _extendedUserLogging;

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

	public string Name => _name;

	public string TableName => _tableName;

	public string DateTimeColumn => _dateTimeColumn;

	public int MillisecondsBetweenDatabaseWrites => _millisecondsBetweenDatabaseWrites;

	public TimeSpan TimeToKeepLogEntries
	{
		get
		{
			return _timeToKeepLogEntries;
		}
		set
		{
			_timeToKeepLogEntries = value;
		}
	}

	public long MaxLogEntries
	{
		get
		{
			return _maxLogEntries;
		}
		set
		{
			_maxLogEntries = value;
		}
	}

	public bool CanUseLogLevels { get; set; }

	public LogLevel LogLevel { get; set; }

	public TimeSpan TimeBetweenDatabaseWrites
	{
		get
		{
			return new TimeSpan(0, 0, 0, 0, _millisecondsBetweenDatabaseWrites);
		}
		set
		{
			_timeBetweenDatabaseWrites = value;
		}
	}

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
		}
	}

	public int AuditLogPlaybackFramesBeforeLogging
	{
		get
		{
			return _auditLogPlaybackFramesBeforeLogging;
		}
		set
		{
			_auditLogPlaybackFramesBeforeLogging = value;
		}
	}

	public int AuditLogPlaybackWindowSizeInSec
	{
		get
		{
			return _auditLogPlaybackWindowSizeInSec;
		}
		set
		{
			_auditLogPlaybackWindowSizeInSec = value;
		}
	}

	public bool ExtendedUserLogging
	{
		get
		{
			return _extendedUserLogging;
		}
		set
		{
			_extendedUserLogging = value;
		}
	}

	public LogDefinition(LogType logType, string name, string tableName, string dateTimeColumn, int millisecondsBetweenDatabaseWrites, bool enabled, bool extendedUserLogging, int auditLogPlaybackWindowSizeInSec, int auditLogPlaybackFramesBeforeLogging)
	{
		_logType = logType;
		_name = name;
		_tableName = tableName;
		_dateTimeColumn = dateTimeColumn;
		_millisecondsBetweenDatabaseWrites = millisecondsBetweenDatabaseWrites;
		_enabled = enabled;
		_extendedUserLogging = extendedUserLogging;
		_auditLogPlaybackWindowSizeInSec = auditLogPlaybackWindowSizeInSec;
		_auditLogPlaybackFramesBeforeLogging = auditLogPlaybackFramesBeforeLogging;
	}
}
