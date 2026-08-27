#define TRACE
using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace VideoOS.Mobile.Portable.Log;

public class BaseLogger
{
	public delegate void LogOnEvent(StreamWriter stream);

	public delegate void SeparatorChange(string separator);

	private readonly Queue _baseEntries;

	private readonly Queue _entries;

	private Thread _thread;

	private string _root = "";

	private bool _shutDown;

	private string _separator;

	private string _dateTimeFormat;

	private string _newLogFormat;

	public bool TraceFlag = true;

	private Queue _internalMessagesQueue = new Queue();

	private Queue _internalMessagesSynced;

	private LogContent _logLevel;

	private int _daysToKeepLog = 3;

	public string FilePrefix = "Mobile";

	public string FileSufix = "log";

	public LogOnEvent OnNewDayLog;

	public LogOnEvent OnLogEnd;

	public SeparatorChange OnSeparatorChange;

	private string _dailyLogFileName = "";

	private string _dailyLogDirectory = "";

	public bool LoggingEnabled;

	public LogContent LogLevel
	{
		get
		{
			return _logLevel;
		}
		set
		{
			_internalMessagesSynced.Enqueue($"{LogDateTime(DateTime.UtcNow)}{_separator}LogLevel set to: {value}");
			_logLevel = value;
		}
	}

	public string DailyLogFileName => _dailyLogFileName;

	public int DaysToKeepLog
	{
		get
		{
			return _daysToKeepLog;
		}
		set
		{
			bool num = value != _daysToKeepLog;
			_daysToKeepLog = ((value < 1) ? (_daysToKeepLog = Math.Max(value, 1)) : value);
			if (num)
			{
				DeleteLog(_daysToKeepLog);
			}
		}
	}

	public string DailyLogDirectory => _dailyLogDirectory;

	public bool LoggerInitialized { get; private set; }

	public string Separator
	{
		get
		{
			return _separator;
		}
		set
		{
			_separator = value;
			_dateTimeFormat = "{0}" + _separator + "{1}";
			_newLogFormat = "{0}" + _separator + "{1}" + _separator + "{2}" + _separator + "{3}";
			if (OnSeparatorChange != null)
			{
				OnSeparatorChange(_separator);
			}
		}
	}

	public BaseLogger()
	{
		_baseEntries = new Queue();
		_entries = Queue.Synchronized(_baseEntries);
		_internalMessagesSynced = Queue.Synchronized(_internalMessagesQueue);
		Separator = " ";
		OnNewDayLog = NewDayLog;
		OnLogEnd = LogClose;
	}

	public void LogInit(string rootPath)
	{
		if (_root != rootPath && CheckFolderIsValid(rootPath) && rootPath != string.Empty)
		{
			Close();
			_root = rootPath;
			_dailyLogDirectory = _root;
			LoggerInitialized = true;
			LogLevel = LogContent.Info;
			Start();
		}
	}

	public bool CheckFolderIsValid(string rootPath)
	{
		bool result = false;
		try
		{
			Directory.CreateDirectory(rootPath);
			result = true;
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
		return result;
	}

	private void Start()
	{
		if (!IsRunning())
		{
			_shutDown = false;
			_thread = new Thread(Run)
			{
				Name = "LogThread"
			};
			_thread.Start();
		}
	}

	public void NewDayLog(StreamWriter stream)
	{
		DateTime utcNow = DateTime.UtcNow;
		DateTime now = DateTime.Now;
		long num = (now.Ticks - utcNow.Ticks) / 10000000;
		stream.WriteLine(LogDateTime(utcNow) + _separator + "-------------------------- Version" + _separator + "25.3.17737.1");
		stream.WriteLine($"{LogDateTime(utcNow)}{_separator}Local time is now: {now.ToLongTimeString()}{_separator}Time Zone: UTC{num / 3600:+0#;-0#}:{num / 60 % 60:0#}");
	}

	public void LogClose(StreamWriter stream)
	{
		DateTime utcNow = DateTime.UtcNow;
		stream.WriteLine(LogDateTime(utcNow) + _separator + "-------------------------- Session End");
	}

	public void Enqueue(object entry)
	{
		if (IsRunning() && entry is ILogEntry logEntry && logEntry.LogLevel <= _logLevel)
		{
			_entries.Enqueue(entry);
		}
	}

	private string TryCreateStreamWriter(out StreamWriter writer)
	{
		writer = null;
		string dateStr = GetDateStr(DateTime.Now);
		string text = FilePrefix + "." + dateStr + "." + FileSufix;
		for (int i = 0; i < 3; i++)
		{
			try
			{
				writer = new StreamWriter(_root + "\\" + text, append: true);
			}
			catch (UnauthorizedAccessException)
			{
				text = FilePrefix + "." + dateStr + $".{i}." + FileSufix;
				continue;
			}
			break;
		}
		if (writer == null)
		{
			throw new UnauthorizedAccessException("Failed 3 attempts to write in todays log file.");
		}
		return text;
	}

	private void Run()
	{
		int num = 0;
		StreamWriter writer = null;
		try
		{
			while (true)
			{
				if (num != DateTime.Now.Day)
				{
					writer?.Close();
					num = DateTime.Now.Day;
					DirectoryInfo directoryInfo = Directory.CreateDirectory(_root);
					if (!_root.Equals(directoryInfo.FullName))
					{
						_root = directoryInfo.FullName;
						_dailyLogDirectory = _root;
					}
					string text = TryCreateStreamWriter(out writer);
					_dailyLogFileName = _root + (_root.Trim().EndsWith("\\") ? text : ("\\" + text));
					OnNewDayLog?.Invoke(writer);
					WriteInternalMessages(writer);
					DeleteLog(DaysToKeepLog);
				}
				while (_entries.Count > 0 && writer != null)
				{
					object? obj = _entries.Dequeue();
					string text2 = null;
					ILogEntry logEntry = obj as ILogEntry;
					if (logEntry?.LogLevel <= LogLevel)
					{
						text2 = logEntry.GetLogEntry();
					}
					if (text2 != null)
					{
						writer.WriteLine(text2);
						Trace.WriteLine(text2);
					}
				}
				WriteInternalMessages(writer);
				writer.Flush();
				if (_shutDown)
				{
					break;
				}
				Thread.Sleep(50);
			}
			if (OnLogEnd != null)
			{
				OnLogEnd(writer);
				writer.Flush();
			}
		}
		catch (Exception)
		{
		}
		writer?.Close();
	}

	private void WriteInternalMessages(StreamWriter logWriter)
	{
		while (_internalMessagesSynced.Count > 0)
		{
			object value = _internalMessagesSynced.Dequeue();
			logWriter.WriteLine(value);
		}
	}

	public void DeleteLog(int daysAgo)
	{
		try
		{
			FileInfo[] files = new DirectoryInfo(_root).GetFiles("*." + FileSufix);
			foreach (FileInfo fileInfo in files)
			{
				try
				{
					if ((DateTime.Now - fileInfo.LastWriteTime).TotalDays > (double)daysAgo)
					{
						fileInfo.Delete();
					}
				}
				catch (UnauthorizedAccessException)
				{
				}
			}
		}
		catch (Exception)
		{
		}
	}

	public string LogDateTime(DateTime dt)
	{
		DateTime dateTime = dt.ToLocalTime();
		long num = (dateTime.Ticks - dt.Ticks) / 10000000;
		return string.Format(_dateTimeFormat, dateTime.ToString("yyyy-MM-dd"), dateTime.ToLongTimeString(), num / 3600, num / 60 % 60);
	}

	public static string GetDateStr(DateTime t)
	{
		return $"{t.Year:000#}-{t.Month:0#}-{t.Day:0#}";
	}

	public static string GetTimeStr(DateTime t)
	{
		return $"{t.Hour:0#}-{t.Minute:0#}-{t.Second:0#}";
	}

	public void Close()
	{
		_shutDown = true;
		if (IsRunning())
		{
			_thread.Join();
		}
		_thread = null;
		_root = "";
		LoggerInitialized = false;
	}

	public bool IsRunning()
	{
		if (_thread != null)
		{
			return _thread.IsAlive;
		}
		return false;
	}
}
