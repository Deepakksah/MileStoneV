using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Threading;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class GeminiEventLog
{
	private string _eventLogSource;

	private string _defaultAssemblyName;

	private EventLogEntryType _defaultLogLevel = EventLogEntryType.Error;

	public string EventLogSource => _eventLogSource;

	public string AssemblyName
	{
		get
		{
			if (!string.IsNullOrEmpty(_defaultAssemblyName))
			{
				return _defaultAssemblyName;
			}
			return GeminiEventLogHeader.AssemblyDisplayName(Assembly.GetCallingAssembly());
		}
		set
		{
			_defaultAssemblyName = value;
		}
	}

	public GeminiEventLog(string eventSource)
		: this(eventSource, null)
	{
	}

	public GeminiEventLog(string eventLogSource, Assembly reportingAssembly)
	{
		if (string.IsNullOrEmpty(eventLogSource))
		{
			throw new ArgumentNullException(Resources.EventSourceMissingException);
		}
		_eventLogSource = eventLogSource;
		if (reportingAssembly != null)
		{
			_defaultAssemblyName = GeminiEventLogHeader.AssemblyDisplayName(reportingAssembly);
		}
	}

	public void Log(string message)
	{
		Log(message, EventLogEntryType.Information);
	}

	public void Log(string message, int eventId)
	{
		Log(message, eventId, EventLogEntryType.Information);
	}

	public void Log(string message, EventLogEntryType level)
	{
		Log(message, GeminiEventId.DefaultInformationalEventId, level);
	}

	public void Log(string message, int eventId, EventLogEntryType level)
	{
		Log(message, eventId, level, isFormatted: false);
	}

	public void Log(string messageBrief, string message, int eventId, EventLogEntryType level)
	{
		GeminiEventLogHeader geminiEventLogHeader = new GeminiEventLogHeader(message, AssemblyName)
		{
			MessageBrief = messageBrief
		};
		Log(geminiEventLogHeader.ToString(), eventId, level, isFormatted: true);
	}

	private void Log(string message, int eventId, EventLogEntryType level, bool isFormatted)
	{
		try
		{
			if (!isFormatted)
			{
				message = ((level != EventLogEntryType.Warning && level != EventLogEntryType.Error) ? string.Format(CultureInfo.InvariantCulture, Resources.InformationalEventFormatNoHost, message, Thread.GetDomain().FriendlyName) : new GeminiEventLogHeader(message, AssemblyName).ToString());
			}
			if (message.Length > 16384)
			{
				EventLog.WriteEntry(_eventLogSource, message.Substring(0, 16384), level, eventId);
			}
			else
			{
				EventLog.WriteEntry(_eventLogSource, message, level, eventId);
			}
		}
		catch
		{
		}
	}

	public void LogException(IGeminiRequestContext requestContext, string message, Exception ex)
	{
		LogException(requestContext, message, ex, GetEventId(ex));
	}

	public void LogException(IGeminiRequestContext requestContext, string message, Exception ex, EventLogEntryType level)
	{
		LogException(requestContext, message, ex, GetEventId(ex), level);
	}

	public void LogException(IGeminiRequestContext requestContext, string message, Exception ex, int eventId)
	{
		EventLogEntryType level = _defaultLogLevel;
		if (ex is GeminiServiceException ex2)
		{
			level = ex2.LogLevel;
		}
		LogException(requestContext, message, ex, eventId, level);
	}

	public void LogException(IGeminiRequestContext requestContext, string message, Exception ex, int eventId, EventLogEntryType level)
	{
		if (requestContext == null && ex is GeminiServiceException)
		{
			requestContext = ((GeminiServiceException)ex).RequestContext;
		}
		LogException(new GeminiEventLogHeader(message, AssemblyName), ex, eventId, level);
	}

	private void LogException(GeminiEventLogHeader logHeader, Exception ex, int eventId, EventLogEntryType level)
	{
		StringBuilder stringBuilder = new StringBuilder(logHeader.ToString());
		stringBuilder.AppendLine(GeminiExceptionFormatter.FormatException(ex, SkipExceptionDetails(level)));
		if (ex.GetType() != typeof(ThreadAbortException))
		{
			Log(stringBuilder.ToString(), eventId, level, isFormatted: true);
		}
		if (GeminiTrace.IsTracing(TraceType.General, TraceLevel.Error))
		{
			GeminiTrace.Error(stringBuilder.ToString());
		}
	}

	public static bool SkipExceptionDetails(EventLogEntryType level)
	{
		if (level != EventLogEntryType.Warning)
		{
			return level != EventLogEntryType.Error;
		}
		return false;
	}

	public static int GetEventId(Exception ex)
	{
		if (ex is GeminiServiceException ex2)
		{
			return ex2.EventId;
		}
		if (ex is ThreadAbortException)
		{
			return GeminiEventId.ThreadAbortException;
		}
		if (ex is DatabaseException)
		{
			return GeminiEventId.UnexpectedDatabaseResultException;
		}
		return GeminiEventId.DefaultExceptionEventId;
	}
}
