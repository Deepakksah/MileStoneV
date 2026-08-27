using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class GeminiTextWriterTraceListener : TextWriterTraceListener
{
	private const string _fileDateFormat = "yyyyMMdd.HHmm";

	private static string _traceFileDirectoryName;

	private static Process _process;

	private FileStream _stream;

	private bool _startingNewLine;

	private bool _reportAbsoluteTime;

	private long _startTicks = DateTime.UtcNow.Ticks;

	public static string TraceFileDirectoryName
	{
		get
		{
			if (string.IsNullOrEmpty(_traceFileDirectoryName))
			{
				return "%TEMP%";
			}
			return _traceFileDirectoryName;
		}
		set
		{
			_traceFileDirectoryName = value;
		}
	}

	public static string TraceLogFileName => "ManagementServer - " + DateTime.UtcNow.ToString("yyyyMMdd.HHmm", CultureInfo.InvariantCulture) + ".log";

	public bool ReportAbsoluteTime
	{
		get
		{
			return _reportAbsoluteTime;
		}
		set
		{
			_reportAbsoluteTime = value;
		}
	}

	private long MillisecondsElapsed => (DateTime.UtcNow.Ticks - _startTicks) / 10000;

	private static Process CurrentProcess
	{
		get
		{
			if (_process == null)
			{
				_process = Process.GetCurrentProcess();
			}
			return _process;
		}
	}

	public GeminiTextWriterTraceListener()
		: this(Path.Combine(Environment.ExpandEnvironmentVariables(TraceFileDirectoryName), TraceLogFileName))
	{
	}

	public GeminiTextWriterTraceListener(string fileName)
	{
		_startingNewLine = true;
		_startTicks = DateTime.UtcNow.Ticks;
		InitializeFile(fileName);
	}

	public override void Write(string message)
	{
		WriteInternal(message, appendNewLine: false);
	}

	public override void WriteLine(string message)
	{
		WriteInternal(message, appendNewLine: true);
	}

	public override void Fail(string message, string detailMessage)
	{
		WriteLine("[FAIL]" + message + Environment.NewLine + detailMessage + new StackTrace(fNeedFileInfo: true));
	}

	public void WriteFormatted(string message)
	{
		WriteInternal(message, appendNewLine: true, includeHeader: false);
	}

	private void WriteInternal(string message, bool appendNewLine)
	{
		WriteInternal(message, appendNewLine, includeHeader: true);
	}

	private void WriteInternal(string message, bool appendNewLine, bool includeHeader)
	{
		if (_stream != null)
		{
			_stream.Seek(0L, SeekOrigin.End);
			if (includeHeader && _startingNewLine && message.Length > 0)
			{
				message = string.Format(CultureInfo.InvariantCulture, Resources.LogOutputString, DateTime.Now.ToString("G", CultureInfo.InvariantCulture), CurrentProcess.Id, NativeMethods.GetCurrentThreadId(), _reportAbsoluteTime ? DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) : (MillisecondsElapsed.ToString(CultureInfo.InvariantCulture) + " ms"), message);
			}
			if (appendNewLine)
			{
				base.WriteLine(message);
			}
			else
			{
				base.Write(message);
			}
			Flush();
			_startingNewLine = appendNewLine;
		}
	}

	private void InitializeFile(string filename)
	{
		try
		{
			string directoryName = Path.GetDirectoryName(filename);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			if (_stream != null)
			{
				_stream.Dispose();
				_stream = null;
			}
			_stream = new FileStream(Environment.ExpandEnvironmentVariables(filename), FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
			base.Writer = new StreamWriter(_stream);
		}
		catch (Exception ex)
		{
			Console.Error.WriteLine("The file {0} specified to store the trace output could not be found or created. Confirm that the file name is correct and that your user account has permission to access the file: {1}.", filename, ex.Message);
			Console.Error.WriteLine();
		}
	}
}
