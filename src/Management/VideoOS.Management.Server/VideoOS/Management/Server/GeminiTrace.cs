#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class GeminiTrace
{
	private static GeminiTraceSettings _traceSettings;

	[ThreadStatic]
	private static Stack<long> _traceStack;

	public static GeminiTraceSettings TraceSettings
	{
		get
		{
			if (_traceSettings == null)
			{
				string[] obj = new string[4] { "General", "Database", "Security", "Configuration" };
				List<TraceSwitch> list = new List<TraceSwitch>(obj.Length);
				string[] array = obj;
				foreach (string text in array)
				{
					list.Add(new TraceSwitch(text, string.Format(CultureInfo.InvariantCulture, "VideoOS Server Trace: {0}", text)));
				}
				_traceSettings = new GeminiTraceSettings(list);
				Trace.AutoFlush = true;
			}
			return _traceSettings;
		}
	}

	private static Stack<long> TraceStack
	{
		get
		{
			if (_traceStack == null)
			{
				_traceStack = new Stack<long>();
			}
			return _traceStack;
		}
	}

	static GeminiTrace()
	{
		_traceStack = new Stack<long>();
	}

	public static void Enter(TraceType traceCategory, string str)
	{
		Enter(traceCategory, TraceLevel.Verbose, str);
	}

	public static void Enter(TraceType traceCategory, TraceLevel level, string str)
	{
		if (!string.IsNullOrEmpty(str) && IsTracing(traceCategory, level))
		{
			try
			{
				WriteLine(level, str + "{");
				TraceStack.Push(HighResTimer.Timestamp);
			}
			catch
			{
			}
		}
	}

	public static void Exit(TraceType traceCategory, string str)
	{
		Exit(traceCategory, TraceLevel.Verbose, str);
	}

	public static void Exit(TraceType traceCategory, TraceLevel level, string str)
	{
		if (!string.IsNullOrEmpty(str) && IsTracing(traceCategory, level))
		{
			try
			{
				long timestamp = Stopwatch.GetTimestamp();
				string str2 = string.Format(arg1: HighResTimer.ElapsedTime((TraceStack.Count <= 0) ? timestamp : _traceStack.Pop(), timestamp), provider: CultureInfo.InvariantCulture, format: "{0}}} - Time: ({1} usecs)", arg0: str);
				WriteLine(level, str2);
			}
			catch
			{
			}
		}
	}

	public static void Info(string str)
	{
		Info(TraceType.General, str);
	}

	public static void Info(string format, params object[] args)
	{
		Info(TraceType.General, format, args);
	}

	public static void Info(TraceType traceCategory, string format, params object[] args)
	{
		Info(traceCategory, string.Format(CultureInfo.InvariantCulture, format, args));
	}

	public static void Info(TraceType traceCategory, string str)
	{
		if (TraceSettings.GetTraceCategory(traceCategory).TraceInfo)
		{
			WriteLine(TraceLevel.Info, str);
		}
	}

	public static void Warning(string str)
	{
		Warning(TraceType.General, str);
	}

	public static void Warning(string format, params object[] args)
	{
		Warning(TraceType.General, format, args);
	}

	public static void Warning(TraceType traceCategory, string format, params object[] args)
	{
		Warning(traceCategory, string.Format(CultureInfo.InvariantCulture, format, args));
	}

	public static void Warning(TraceType traceCategory, string str)
	{
		if (TraceSettings.GetTraceCategory(traceCategory).TraceWarning)
		{
			WriteLine(TraceLevel.Warning, str);
		}
	}

	public static void Error(string str)
	{
		Error(TraceType.General, str);
	}

	public static void Error(string format, params object[] args)
	{
		Error(TraceType.General, format, args);
	}

	public static void Error(TraceType traceCategory, string format, params object[] args)
	{
		Error(traceCategory, string.Format(CultureInfo.InvariantCulture, format, args));
	}

	public static void Error(TraceType traceCategory, string str)
	{
		if (TraceSettings.GetTraceCategory(traceCategory).TraceError)
		{
			WriteLine(TraceLevel.Error, str);
		}
	}

	public static void Error(string str, Exception ex)
	{
		ReportError(TraceType.General, str, null, null, ex);
	}

	public static void Error(TraceType traceCategory, string category, string method, Exception ex)
	{
		if (!TraceSettings.GetTraceCategory(traceCategory).TraceError)
		{
			ReportError(TraceType.General, null, category, method, ex);
		}
		else
		{
			ReportError(traceCategory, null, category, method, ex);
		}
	}

	public static void Verbose(string str)
	{
		Verbose(TraceType.General, str);
	}

	public static void Verbose(string format, params object[] args)
	{
		Verbose(TraceType.General, format, args);
	}

	public static void Verbose(TraceType traceCategory, string format, params object[] args)
	{
		Verbose(traceCategory, string.Format(CultureInfo.InvariantCulture, format, args));
	}

	public static void Verbose(TraceType traceCategory, string str)
	{
		if (TraceSettings.GetTraceCategory(traceCategory).TraceVerbose)
		{
			WriteLine(TraceLevel.Verbose, str);
		}
	}

	public static void TraceException(Exception ex)
	{
		Error(null, ex);
	}

	public static void TraceException(TraceType traceCategory, Exception ex)
	{
		Error(traceCategory, null, null, ex);
	}

	public static bool IsTracing(TraceType traceCategory, TraceLevel level)
	{
		return TraceSettings.GetTraceCategory(traceCategory).Level >= level;
	}

	private static void WriteLine(TraceLevel traceLevel, string str)
	{
		if (!string.IsNullOrEmpty(str))
		{
			try
			{
				Trace.WriteLine(string.Format(CultureInfo.InvariantCulture, "{0} [{1}] {2} - {3}", DateTime.UtcNow, NativeMethods.GetCurrentThreadId(), traceLevel.ToString().ToUpper(CultureInfo.InvariantCulture), str));
			}
			catch (GeminiServerException)
			{
				throw;
			}
			catch (Exception)
			{
			}
		}
	}

	private static void ReportError(TraceType traceCategory, string message, string category, string method, Exception ex)
	{
		if (!TraceSettings.GetTraceCategory(traceCategory).TraceError)
		{
			return;
		}
		using StringWriter stringWriter = new StringWriter(CultureInfo.InvariantCulture);
		if (!string.IsNullOrEmpty(message))
		{
			stringWriter.WriteLine(message);
		}
		stringWriter.WriteLine();
		if (!string.IsNullOrEmpty(category))
		{
			stringWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, Resources.Category, category));
		}
		if (!string.IsNullOrEmpty(method))
		{
			stringWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, Resources.Method, method));
		}
		WriteLine(TraceLevel.Error, "{ " + stringWriter.ToString());
		WriteLine(TraceLevel.Error, Resources.Exception + " {" + GeminiExceptionFormatter.FormatException(ex, includeDetails: true) + Environment.NewLine + " }");
		WriteLine(TraceLevel.Error, "}");
	}
}
