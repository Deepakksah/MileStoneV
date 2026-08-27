using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Security;
using System.Text;
using System.Web;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class GeminiTraceListener : DefaultTraceListener
{
	public class FrameInformation
	{
		private string _method;

		private string _frameDetails;

		public string Method => _method;

		public string FrameDetails => _frameDetails;

		public FrameInformation(string method, string frameDetails)
		{
			_method = method;
			_frameDetails = frameDetails;
		}
	}

	private const string _enableTracingConfigKey = "enableGeminiTraceWriter";

	private const string _traceDirectoryConfigKey = "GeminiTraceDirectoryName";

	private static string _traceDirectoryName;

	private bool _useTraceWriter;

	private TextWriterTraceListener _traceWriter;

	private static bool _assertUiEnabled;

	private bool _launchDebugger = true;

	public static GeminiTraceListener Instance = new GeminiTraceListener();

	private static List<FrameInformation> StackInformation
	{
		get
		{
			List<FrameInformation> list = new List<FrameInformation>();
			StackTrace stackTrace = new StackTrace(2, fNeedFileInfo: true);
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				FrameInformation frameInformation = FrameDetails(stackTrace.GetFrame(i));
				if (frameInformation != null)
				{
					list.Add(frameInformation);
				}
			}
			return list;
		}
	}

	private static string TraceDirectoryName
	{
		get
		{
			if (string.IsNullOrEmpty(_traceDirectoryName))
			{
				string appSetting = Config.Instance.GetAppSetting("GeminiTraceDirectoryName", string.Empty);
				if (appSetting.Trim().Length != 0)
				{
					_traceDirectoryName = appSetting;
				}
				else
				{
					_traceDirectoryName = string.Format(CultureInfo.InvariantCulture, "{0}\\Logs", OemResources.Instance.Server.CommonApplicationDataFolder);
				}
			}
			return _traceDirectoryName;
		}
	}

	private bool UseTraceWriter
	{
		get
		{
			return _useTraceWriter;
		}
		set
		{
			if (value != _useTraceWriter)
			{
				if (value)
				{
					EnableTraceWriter();
				}
				else
				{
					DisableTraceWriter();
				}
			}
			_useTraceWriter = value;
		}
	}

	public TextWriterTraceListener TraceWriter
	{
		get
		{
			return _traceWriter;
		}
		set
		{
			_traceWriter = value;
			if (_traceWriter is GeminiTextWriterTraceListener)
			{
				((GeminiTextWriterTraceListener)_traceWriter).ReportAbsoluteTime = true;
			}
		}
	}

	public static bool EnableUiAssert
	{
		get
		{
			return _assertUiEnabled;
		}
		set
		{
			_assertUiEnabled = value;
		}
	}

	private static bool IsLocalRequest
	{
		get
		{
			if (HttpContext.Current == null)
			{
				return false;
			}
			HttpRequest request = HttpContext.Current.Request;
			if (!request.UserHostAddress.Equals("127.0.0.1"))
			{
				return request.UserHostAddress.Equals(request.ServerVariables.Get("LOCAL_ADDR"));
			}
			return true;
		}
	}

	public bool LaunchDebugger
	{
		get
		{
			return _launchDebugger;
		}
		set
		{
			_launchDebugger = value;
		}
	}

	public static void SetDefaultListener()
	{
		InitializeTraceListener();
	}

	[Localizable(false)]
	public override void Write(string message)
	{
		OutputMessage(message, includeNewline: false);
	}

	[Localizable(false)]
	public override void WriteLine(string message)
	{
		OutputMessage(message, includeNewline: true);
	}

	[Localizable(false)]
	public override void Fail(string message)
	{
		Fail(message, null);
	}

	private static void InitializeTraceListener()
	{
		for (int num = Trace.Listeners.Count - 1; num >= 0; num--)
		{
			if (Trace.Listeners[num] is GeminiTraceListener)
			{
				Trace.Listeners.RemoveAt(num);
			}
			else if (Trace.Listeners[num] is DefaultTraceListener)
			{
				_assertUiEnabled = ((DefaultTraceListener)Trace.Listeners[num]).AssertUiEnabled;
				Trace.Listeners.RemoveAt(num);
			}
		}
		Trace.Listeners.Insert(0, Instance);
		bool traceWriterState = false;
		string appSetting = Config.Instance.GetAppSetting("enableGeminiTraceWriter", "false");
		if (!string.IsNullOrEmpty(appSetting))
		{
			traceWriterState = bool.Parse(appSetting);
		}
		SetTraceWriterState(traceWriterState);
	}

	private static void SetTraceWriterState(bool useTraceWriter)
	{
		Instance.UseTraceWriter = useTraceWriter;
	}

	private static void EnableTraceWriter()
	{
		GeminiTextWriterTraceListener.TraceFileDirectoryName = TraceDirectoryName;
		Instance.TraceWriter = new GeminiTextWriterTraceListener();
		Instance.OutputMessage("# VideoOS Management Server Trace started.", includeNewline: true);
		Instance.OutputMessage(string.Format(CultureInfo.InvariantCulture, "# Trace listener: {0}:", Instance.TraceWriter.ToString()), includeNewline: true);
		Instance.OutputMessage(string.Format(CultureInfo.InvariantCulture, "# Config settings: Assert UI Enabled={0}, Launch Debugger={1}", _assertUiEnabled.ToString(), Instance.LaunchDebugger.ToString()), includeNewline: true);
		Instance.OutputMessage(string.Format(CultureInfo.InvariantCulture, "# {0}", GeminiTrace.TraceSettings.ToString()), includeNewline: true);
	}

	private static void DisableTraceWriter()
	{
		if (Instance.TraceWriter != null)
		{
			Instance.OutputMessage("# VideoOS Management Server Trace stopped.", includeNewline: true);
			Instance.TraceWriter.Dispose();
			Instance.TraceWriter = null;
		}
	}

	[Localizable(false)]
	private void OutputMessage(string message, bool includeNewline)
	{
		OutputMessage(message, null, includeNewline);
	}

	[Localizable(false)]
	private void OutputMessage(string message, string detailedMessage, bool includeNewline)
	{
		OutputMessage(message, detailedMessage, includeNewline, isAssert: false);
	}

	[Localizable(false)]
	private void OutputMessage(string message, string detailedMessage, bool includeNewline, bool isAssert)
	{
		StringBuilder stringBuilder = new StringBuilder(message);
		if (includeNewline)
		{
			stringBuilder.Append(Environment.NewLine);
		}
		if (!string.IsNullOrEmpty(detailedMessage))
		{
			stringBuilder.AppendLine(detailedMessage);
		}
		if (isAssert)
		{
			stringBuilder.AppendLine("-----------------------");
			stringBuilder.AppendLine(Resources.AssertionFailureHeader);
			stringBuilder.AppendLine("-----------------------");
			foreach (FrameInformation item in StackInformation)
			{
				if (!string.IsNullOrEmpty(item.Method))
				{
					stringBuilder.Append(item.Method);
				}
				if (!string.IsNullOrEmpty(item.FrameDetails))
				{
					stringBuilder.Append(item.FrameDetails);
				}
			}
		}
		if (Debugger.IsLogging())
		{
			Debugger.Log(0, null, stringBuilder.ToString());
		}
		NativeMethods.OutputDebugString(stringBuilder.ToString());
		if (_traceWriter != null)
		{
			if (includeNewline)
			{
				_traceWriter.WriteLine(stringBuilder.ToString());
			}
			else
			{
				_traceWriter.Write(stringBuilder.ToString());
			}
			_traceWriter.Flush();
		}
		foreach (TraceListener listener in Trace.Listeners)
		{
			if (listener is GeminiTraceListener)
			{
				continue;
			}
			try
			{
				if (includeNewline)
				{
					listener.WriteLine(stringBuilder.ToString());
				}
				else
				{
					listener.Write(stringBuilder.ToString());
				}
			}
			catch
			{
			}
		}
		if (HttpContext.Current != null)
		{
			HttpContext.Current.Trace.Write(stringBuilder.ToString());
		}
		if (!isAssert)
		{
			return;
		}
		bool flag = false;
		if (_launchDebugger)
		{
			try
			{
				if (Debugger.IsAttached)
				{
					Debugger.Break();
					flag = true;
				}
				else if (Environment.UserInteractive || (_assertUiEnabled && IsLocalRequest))
				{
					flag = Debugger.Launch();
				}
			}
			catch (SecurityException)
			{
			}
		}
		if (!flag)
		{
			throw new GeminiServerException(stringBuilder.ToString());
		}
	}

	private static FrameInformation FrameDetails(StackFrame frame)
	{
		MethodBase method = frame.GetMethod();
		if (method == null)
		{
			return null;
		}
		string name = method.ReflectedType.Name;
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, Resources.StackFrameHeader, name, method.Name));
		stringBuilder.Append("(");
		ParameterInfo[] parameters = method.GetParameters();
		for (int i = 0; i < parameters.Length; i++)
		{
			stringBuilder.Append(string.Format(CultureInfo.InvariantCulture, "{0} {1}", parameters[i].ParameterType.Name, parameters[i].Name));
			if (parameters.Length - 1 != i)
			{
				stringBuilder.Append(", ");
			}
		}
		stringBuilder.Append(") ");
		StringBuilder stringBuilder2 = new StringBuilder();
		if (frame.GetFileName() != null)
		{
			if (frame.GetFileLineNumber() == 0)
			{
				stringBuilder2.Append(frame.GetFileName());
			}
			else
			{
				stringBuilder2.Append(string.Format(CultureInfo.InvariantCulture, Resources.StackFrameLineFormat, frame.GetFileName(), frame.GetFileLineNumber().ToString(CultureInfo.InvariantCulture)));
			}
		}
		stringBuilder2.Append(Environment.NewLine);
		return new FrameInformation(stringBuilder2.ToString(), stringBuilder.ToString());
	}
}
