using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.Log;

public class SimpleLogger : BaseLogger, ILog
{
	public SimpleLogger()
	{
	}

	public SimpleLogger(string rootPath, string logFileName)
	{
		base.DaysToKeepLog = 3;
		if (!string.IsNullOrEmpty(logFileName))
		{
			FilePrefix = logFileName;
		}
		FileSufix = "log";
		base.Separator = ";";
		LogInit(rootPath);
	}

	public void Log(LogContent content, string origin, string text, object dump = null)
	{
		Enqueue(new SimpleLogEntry(content, text, DateTime.UtcNow, origin));
	}

	public void Exception(Exception exception)
	{
		Error(exception.GetType()?.ToString() + ": " + exception.Message, exception.StackTrace, null, sanitized: false);
	}

	public void Internal(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Enqueue(new SimpleLogEntry(LogContent.Disabled, tx, DateTime.UtcNow, origin, multipleMessages));
	}

	public void Error(string origin, string tx, IEnumerable<string> multipleMessages = null, bool sanitized = true)
	{
		Enqueue(new SimpleLogEntry(LogContent.Error, tx, DateTime.UtcNow, origin, multipleMessages, sanitized));
	}

	public void Warning(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Enqueue(new SimpleLogEntry(LogContent.Warning, tx, DateTime.UtcNow, origin, multipleMessages));
	}

	public void Info(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Enqueue(new SimpleLogEntry(LogContent.Info, tx, DateTime.UtcNow, origin, multipleMessages));
	}

	public void Debug(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Enqueue(new SimpleLogEntry(LogContent.Debug, tx, DateTime.UtcNow, origin, multipleMessages));
	}

	public void Trace(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Enqueue(new SimpleLogEntry(LogContent.Trace, tx, DateTime.UtcNow, origin, multipleMessages));
	}

	public void Communication(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Enqueue(new SimpleLogEntry(LogContent.Communication, tx, DateTime.UtcNow, origin, multipleMessages));
	}
}
