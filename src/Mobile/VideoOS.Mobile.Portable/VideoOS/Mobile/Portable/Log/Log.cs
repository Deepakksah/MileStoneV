using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.Portable.Log;

public class Log
{
	private static readonly SimpleLogger Logger = new SimpleLogger
	{
		DaysToKeepLog = 3,
		FilePrefix = "MobileServer",
		FileSufix = "log",
		Separator = ";"
	};

	public static bool LoggerInitialized => Logger.LoggerInitialized;

	public static int DaysToKeepLogging
	{
		get
		{
			return Logger.DaysToKeepLog;
		}
		set
		{
			Logger.DaysToKeepLog = value;
		}
	}

	public static LogContent LogLevel
	{
		get
		{
			return Logger.LogLevel;
		}
		set
		{
			Logger.LogLevel = value;
		}
	}

	public static string DailyLogDirectory => Logger.DailyLogDirectory;

	public static string DailyLogFileName => Logger.DailyLogFileName;

	public static string Separator => Logger.Separator;

	public static string LogDateTime(DateTime dt)
	{
		return Logger.LogDateTime(dt);
	}

	public static void Exception(Exception exception)
	{
		Error(exception.GetType()?.ToString() + ": " + exception.Message, exception.StackTrace, null, sanitized: false);
	}

	public static void Internal(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Logger.Internal(origin, tx, multipleMessages);
	}

	public static void Error(string origin, string tx, IEnumerable<string> multipleMessages = null, bool sanitized = true)
	{
		Logger.Error(origin, tx, multipleMessages, sanitized);
	}

	public static void Warning(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Logger.Warning(origin, tx, multipleMessages);
	}

	public static void Info(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Logger.Info(origin, tx, multipleMessages);
	}

	public static void Debug(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Logger.Debug(origin, tx, multipleMessages);
	}

	public static void TraceBegin([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		TraceMessage("Begin", memberName, sourceFilePath, sourceLineNumber);
	}

	public static void TraceEnd([CallerMemberName] string memberName = "", [CallerFilePath] string sourceFilePath = "", [CallerLineNumber] int sourceLineNumber = 0)
	{
		TraceMessage("End", memberName, sourceFilePath, sourceLineNumber);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void TraceMessage(string message, [CallerMemberName] string memberName = "", [CallerFilePath] string srcFilePath = "", [CallerLineNumber] int srcLineNum = 0)
	{
		string fileName = Path.GetFileName(srcFilePath);
		Logger.Trace(fileName + " : " + memberName, $"{message} ({srcLineNum})");
	}

	public static void Trace(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Logger.Trace(origin, tx, multipleMessages);
	}

	public static void Communication(string origin, string tx, IEnumerable<string> multipleMessages = null)
	{
		Logger.Communication(origin, tx, multipleMessages);
	}

	public static void LogInit(string rootParm, int daysToKeep, string fileName = null)
	{
		if (!string.IsNullOrEmpty(fileName))
		{
			Logger.FilePrefix = fileName;
		}
		Logger.LogInit(rootParm);
		Logger.DaysToKeepLog = daysToKeep;
	}

	public static void Close()
	{
		Logger.Close();
	}
}
