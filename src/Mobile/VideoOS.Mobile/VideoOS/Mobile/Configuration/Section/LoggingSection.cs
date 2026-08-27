using System;
using System.Collections.Specialized;
using VideoOS.Mobile.Helpers;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Configuration.Section;

public class LoggingSection : IConfigSection
{
	private string _logLevel;

	private int _daysToKeepLogging;

	private string _logFilePath;

	public const string NameLogLevel = "LogLevel";

	public const string NameDaysToKeepLog = "LoggingDaysToKeep";

	public const string NameLogFilePath = "LoggingFilePath";

	public LogContent LogLevel
	{
		get
		{
			LogContent result = LogContent.Info;
			try
			{
				if (Enum.IsDefined(typeof(LogContent), int.Parse(_logLevel)))
				{
					result = (LogContent)Enum.Parse(typeof(LogContent), _logLevel);
				}
			}
			catch (Exception exception)
			{
				Log.Exception(exception);
			}
			return result;
		}
		private set
		{
			_logLevel = value.ToString();
		}
	}

	public int DaysToKeepLogging
	{
		get
		{
			return _daysToKeepLogging;
		}
		private set
		{
			_daysToKeepLogging = value;
		}
	}

	public string LogFilePath
	{
		get
		{
			return _logFilePath;
		}
		private set
		{
			_logFilePath = value;
		}
	}

	public string SectionName => "Logging";

	public LoggingSection()
	{
		LogLevel = LogContent.Info;
	}

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			_logLevel = section["LogLevel"].Sanitized() ?? string.Empty;
			int.TryParse(section["LoggingDaysToKeep"].Sanitized(), out _daysToKeepLogging);
			_logFilePath = section["LoggingFilePath"].SanitizeFolder();
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			result = false;
		}
		return result;
	}

	public bool ConvToCollection(NameValueCollection serverSection)
	{
		bool result = true;
		try
		{
			serverSection["LogLevel"] = _logLevel;
			serverSection["LoggingDaysToKeep"] = _daysToKeepLogging.ToString();
			serverSection["LoggingFilePath"] = _logFilePath;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
			result = false;
		}
		return result;
	}
}
