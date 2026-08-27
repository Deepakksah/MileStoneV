using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace VideoOS.SCH.Base;

public class TextLoggerBase
{
	private sealed class LogFile
	{
		public string LogFileName { get; set; }

		public List<LogEntry> LogEntries { get; set; }

		public LogFile(string logFileName)
		{
			LogFileName = logFileName;
			LogEntries = new List<LogEntry>();
		}
	}

	private sealed class LogEntry
	{
		public string LogFileName { get; set; }

		public DateTime Timestamp { get; set; }

		public string LogLine { get; set; }

		public bool PendingWrite { get; set; }

		public bool WrittenToFile { get; set; }

		public LogEntry(string logFileName, DateTime timestamp, string logLine)
		{
			LogFileName = logFileName;
			Timestamp = timestamp;
			LogLine = logLine;
			PendingWrite = false;
			WrittenToFile = false;
		}
	}

	public const string LineItemSeparatorDefault = ";";

	public const int WriteTimeIntervalInMilliSecDefault = 1000;

	private static TextLoggerBase _theLogger;

	private Mutex _loggerLock;

	private Timer _logWriteTimer;

	private bool _useLogWriteTimer;

	private bool _logWriteTimerActive;

	private int _writeTimeIntervalInMilliSec;

	private Dictionary<string, LogFile> _logFiles;

	public static TextLoggerBase Current => _theLogger;

	public int WriteTimeIntervalInMilliSec
	{
		get
		{
			_loggerLock.WaitOne(-1);
			try
			{
				return _writeTimeIntervalInMilliSec;
			}
			finally
			{
				_loggerLock.ReleaseMutex();
			}
		}
		set
		{
			_loggerLock.WaitOne(-1);
			try
			{
				_writeTimeIntervalInMilliSec = value;
				if (_logWriteTimerActive)
				{
					_logWriteTimer.Change(_writeTimeIntervalInMilliSec, _writeTimeIntervalInMilliSec);
				}
			}
			finally
			{
				_loggerLock.ReleaseMutex();
			}
		}
	}

	public bool DirectWriteMode
	{
		get
		{
			_loggerLock.WaitOne(-1);
			try
			{
				return !_useLogWriteTimer;
			}
			finally
			{
				_loggerLock.ReleaseMutex();
			}
		}
	}

	static TextLoggerBase()
	{
		_theLogger = new TextLoggerBase();
	}

	private void InitData()
	{
		_logFiles = new Dictionary<string, LogFile>();
		_logWriteTimer = new Timer(HandleLogWriteTimerCallback, null, -1, -1);
		_logWriteTimerActive = false;
		_useLogWriteTimer = true;
		_writeTimeIntervalInMilliSec = 1000;
	}

	private TextLoggerBase()
	{
		_loggerLock = new Mutex(initiallyOwned: false);
		InitData();
	}

	private bool PersistLogLines(LogFile logToWrite)
	{
		if (logToWrite.LogEntries.Count == 0)
		{
			return true;
		}
		bool result = false;
		StringBuilder stringBuilder = new StringBuilder();
		StreamWriter streamWriter = null;
		try
		{
			streamWriter = new StreamWriter(logToWrite.LogFileName, append: true);
			foreach (LogEntry logEntry in logToWrite.LogEntries)
			{
				if (logEntry.PendingWrite)
				{
					stringBuilder.Append(logEntry.LogLine + streamWriter.NewLine);
				}
			}
			streamWriter.Write(stringBuilder.ToString());
			streamWriter.Flush();
		}
		catch
		{
		}
		finally
		{
			try
			{
				if (streamWriter != null)
				{
					streamWriter.Close();
					result = true;
				}
			}
			catch
			{
			}
		}
		return result;
	}

	private bool PersistLogLines(List<LogFile> logsToWrite)
	{
		foreach (LogFile item in logsToWrite)
		{
			if (!PersistLogLines(item))
			{
				continue;
			}
			foreach (LogEntry logEntry in item.LogEntries)
			{
				if (logEntry.PendingWrite)
				{
					logEntry.PendingWrite = false;
					logEntry.WrittenToFile = true;
				}
			}
		}
		return true;
	}

	private void HandleLogWriteTimerCallback(object data)
	{
		_loggerLock.WaitOne(-1);
		List<LogFile> logEntriesToWrite;
		try
		{
			logEntriesToWrite = GetLogEntriesToWrite();
			if (!_useLogWriteTimer)
			{
				_logWriteTimer.Change(-1, -1);
				_logWriteTimerActive = false;
				if (logEntriesToWrite.Count == 0)
				{
					return;
				}
			}
		}
		finally
		{
			_loggerLock.ReleaseMutex();
		}
		DateTime utcNow = DateTime.UtcNow;
		if (PersistLogLines(logEntriesToWrite))
		{
			DateTime.UtcNow.Subtract(utcNow);
		}
		_loggerLock.WaitOne(-1);
		try
		{
			UpdateWriteQueue(logEntriesToWrite);
			if (_useLogWriteTimer && _logFiles.Count == 0)
			{
				_logWriteTimer.Change(-1, -1);
				_logWriteTimerActive = false;
			}
		}
		finally
		{
			_loggerLock.ReleaseMutex();
		}
	}

	private void UpdateWriteQueue(List<LogFile> logsToWrite)
	{
		foreach (LogFile item in logsToWrite)
		{
			if (!_logFiles.ContainsKey(item.LogFileName))
			{
				continue;
			}
			foreach (LogEntry logEntry in item.LogEntries)
			{
				if (logEntry.WrittenToFile)
				{
					_logFiles[item.LogFileName].LogEntries.Remove(logEntry);
				}
				else if (logEntry.PendingWrite)
				{
					logEntry.PendingWrite = false;
				}
			}
			if (_logFiles[item.LogFileName].LogEntries.Count == 0)
			{
				_logFiles.Remove(item.LogFileName);
			}
		}
	}

	private List<LogFile> GetLogEntriesToWrite()
	{
		List<LogFile> list = new List<LogFile>();
		bool flag = true;
		foreach (LogFile value in _logFiles.Values)
		{
			LogFile logFile = new LogFile(value.LogFileName);
			list.Add(logFile);
			foreach (LogEntry logEntry in value.LogEntries)
			{
				if (!logEntry.WrittenToFile && !logEntry.PendingWrite)
				{
					logEntry.PendingWrite = true;
					logFile.LogEntries.Add(logEntry);
					flag = false;
				}
			}
		}
		if (flag)
		{
			list.Clear();
		}
		return list;
	}

	private string ReplaceFileNameMask(DateTime timestamp, string fileNameMask)
	{
		Match match = null;
		Regex regex = new Regex("%(?<DATE>\\w+)%");
		match = regex.Match(fileNameMask);
		if (match.Success && match.Groups["DATE"].Success)
		{
			return regex.Replace(fileNameMask, timestamp.ToString(match.Groups["DATE"].Value));
		}
		return fileNameMask;
	}

	public void ActivateDirectWriteMode()
	{
		_loggerLock.WaitOne(-1);
		try
		{
			_useLogWriteTimer = false;
		}
		finally
		{
			_loggerLock.ReleaseMutex();
		}
		HandleLogWriteTimerCallback(null);
	}

	public void ActivateIndirectWriteMode()
	{
		_loggerLock.WaitOne(-1);
		try
		{
			_useLogWriteTimer = true;
			_logWriteTimer.Change(WriteTimeIntervalInMilliSec, WriteTimeIntervalInMilliSec);
			_logWriteTimerActive = true;
		}
		finally
		{
			_loggerLock.ReleaseMutex();
		}
	}

	public void WriteLine(string logFileNameMask, IEnumerable<string> lineItems, string itemSeparator)
	{
		DateTime utcNow = DateTime.UtcNow;
		LogEntry logEntry = new LogEntry(null, utcNow, utcNow.ToString("yyyy-MM-dd HH:mm:ss.fff 'GMT'") + itemSeparator + string.Join(itemSeparator, lineItems.ToArray()));
		logEntry.LogFileName = ReplaceFileNameMask(logEntry.Timestamp, logFileNameMask);
		_loggerLock.WaitOne(-1);
		try
		{
			if (!_logFiles.ContainsKey(logEntry.LogFileName))
			{
				_logFiles.Add(logEntry.LogFileName, new LogFile(logEntry.LogFileName));
			}
			_logFiles[logEntry.LogFileName].LogEntries.Add(logEntry);
			if (_useLogWriteTimer)
			{
				if (!_logWriteTimerActive)
				{
					_logWriteTimer.Change(WriteTimeIntervalInMilliSec, WriteTimeIntervalInMilliSec);
					_logWriteTimerActive = true;
				}
				return;
			}
		}
		finally
		{
			_loggerLock.ReleaseMutex();
		}
		HandleLogWriteTimerCallback(null);
	}

	public void WriteLine(string logFileNameMask, IEnumerable<string> lineItems)
	{
		WriteLine(logFileNameMask, lineItems, ";");
	}

	public void WriteLine(string logFileNameMask, string line)
	{
		WriteLine(logFileNameMask, new string[1] { line });
	}
}
