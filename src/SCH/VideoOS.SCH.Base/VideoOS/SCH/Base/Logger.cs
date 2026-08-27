using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace VideoOS.SCH.Base;

public abstract class Logger
{
	protected class LogEvent
	{
		public int ErrorNumber { get; set; }

		public string Message { get; set; }

		public string AdditionalInfo { get; set; }

		public string ExceptionClass { get; set; }

		public string AssemblyName { get; set; }

		public LogType Type { get; set; }

		public List<string> StackTrace { get; set; }
	}

	public virtual void Log(int errorNumber, string logText, LogType logType, StackTrace stackTrace)
	{
		LogEvent logEvent = new LogEvent();
		logEvent.ErrorNumber = errorNumber;
		logEvent.Message = logText;
		logEvent.StackTrace = UnwindStackTrace(stackTrace);
		logEvent.ExceptionClass = null;
		logEvent.AssemblyName = Assembly.GetCallingAssembly().GetName().Name;
		logEvent.Type = logType;
		logEvent.AdditionalInfo = null;
		WriteLogEntry(logEvent);
	}

	public virtual void Log(int errorNumber, string logText, LogType logType)
	{
		Log(errorNumber, logText, logType, null);
	}

	public virtual void Log(Exception ex, string additionalInfo, LogType logType)
	{
		LogEvent logEvent = new LogEvent();
		ServiceChannelException ex2 = ex as ServiceChannelException;
		if (ex2 != null && ex2.Errors.Count > 0)
		{
			logEvent.ErrorNumber = ex2.Errors[0].ErrorNumber;
		}
		logEvent.Message = ex.Message;
		logEvent.StackTrace = UnwindStackTrace(ex);
		logEvent.ExceptionClass = ex.GetType().FullName;
		logEvent.AssemblyName = Assembly.GetCallingAssembly().GetName().Name;
		logEvent.Type = logType;
		logEvent.AdditionalInfo = additionalInfo;
		WriteLogEntry(logEvent);
		if (ex2 != null)
		{
			ex2.Logged = true;
		}
	}

	public static List<string> UnwindStackTrace(StackTrace stackTrace)
	{
		if (stackTrace == null)
		{
			return null;
		}
		List<string> list = new List<string>();
		list.AddRange(stackTrace.ToString().Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
		return list;
	}

	public static List<string> UnwindStackTrace(Exception ex)
	{
		List<string> list = new List<string>();
		if (ex == null)
		{
			return null;
		}
		list.Add("[" + ex.GetType().FullName + "]: " + ex.Message);
		if (ex.StackTrace == null)
		{
			list.Add("{null StackTrace}");
		}
		else
		{
			list.AddRange(ex.StackTrace.Split(new string[1] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries));
		}
		if (ex.InnerException != null)
		{
			list.Add("Inner: ");
			list.AddRange(UnwindStackTrace(ex.InnerException));
		}
		return list;
	}

	protected abstract void WriteLogEntry(LogEvent logEvent);
}
