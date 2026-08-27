using System;
using System.Globalization;
using System.IO;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Admin;

public class LogComponent : ILogger
{
	private class NullLogScope : IDisposable
	{
		public void Dispose()
		{
		}
	}

	private string FilePath { get; }

	public LogComponent(string filePath)
	{
		FilePath = filePath;
	}

	private void WriteDebugLog(string log)
	{
		string value = string.Format(CultureInfo.InvariantCulture, "{0} {1}", DateTime.Now.ToString("G", CultureInfo.InvariantCulture), log);
		Console.WriteLine(value);
		if (!string.IsNullOrEmpty(FilePath))
		{
			using (StreamWriter streamWriter = File.AppendText(FilePath))
			{
				streamWriter.WriteLine(value);
				streamWriter.Flush();
			}
		}
	}

	public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (IsEnabled(logLevel))
		{
			string text = formatter(state, exception);
			if (((EventId)(ref eventId)).Id != 0)
			{
				text = string.Concat(eventId, " ", text);
			}
			WriteDebugLog(text);
		}
	}

	public bool IsEnabled(LogLevel logLevel)
	{
		return true;
	}

	public IDisposable BeginScope<TState>(TState state)
	{
		return new NullLogScope();
	}
}
