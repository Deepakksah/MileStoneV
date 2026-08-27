using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace VideoOS.Reporting.Web.Plugin.Base;

public class EventLogger : Logger
{
	public string Source { get; private set; }

	public EventLogger(string sourceName)
	{
		if (string.IsNullOrEmpty(sourceName))
		{
			throw new ReportServerException("Source must not be null or empty in the Report Server 'EventLogger' constructor.");
		}
		Source = sourceName;
	}

	protected override void WriteLogEntry(LogEvent logEvent)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		string text = "Message: " + logEvent.Message;
		text = text + Environment.NewLine + "Error number: " + logEvent.ErrorNumber.ToString(CultureInfo.InvariantCulture);
		if (logEvent.ExceptionClass != null)
		{
			text = text + Environment.NewLine + "Exception class: " + logEvent.ExceptionClass;
		}
		text = text + Environment.NewLine + "Assembly name: " + logEvent.AssemblyName;
		if (logEvent.AdditionalInfo != null)
		{
			text = text + Environment.NewLine + "Additional info: " + logEvent.AdditionalInfo;
		}
		if (logEvent.StackTrace != null && logEvent.LogType == LogType.Error)
		{
			text = text + Environment.NewLine + "Stack trace: " + Environment.NewLine + string.Join(Environment.NewLine, logEvent.StackTrace.ToArray());
		}
		EventLog.WriteEntry(Source, text, (EventLogEntryType)logEvent.LogType, logEvent.ErrorNumber);
	}
}
