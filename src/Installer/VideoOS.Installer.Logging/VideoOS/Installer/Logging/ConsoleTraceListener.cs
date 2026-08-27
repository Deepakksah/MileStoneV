using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace VideoOS.Installer.Logging;

[ConfigurationElementType(typeof(CustomTraceListenerData))]
public class ConsoleTraceListener : CustomTraceListener
{
	public static bool DisableConsoleLogging { get; set; }

	public override void Write(string message)
	{
	}

	public override void WriteLine(string message)
	{
	}

	public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Expected O, but got Unknown
		if (data is LogEntry && ((CustomTraceListener)this).Formatter != null && !DisableConsoleLogging)
		{
			LogEntry val = (LogEntry)data;
			try
			{
				Console.WriteLine(((CustomTraceListener)this).Formatter.Format(val));
			}
			catch (Exception ex)
			{
				throw new LoggingException("Failed to write to log service.", ex.InnerException);
			}
		}
	}
}
