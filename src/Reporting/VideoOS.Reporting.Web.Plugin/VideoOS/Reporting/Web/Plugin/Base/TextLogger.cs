using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using VideoOS.Diagnostics.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Reporting.Web.Plugin.Base;

public class TextLogger : Logger
{
	private readonly ILogger _logger;

	public string LoggerConfigName { get; private set; }

	public string LogFolder { get; private set; }

	public TextLogger(string loggerConfigName, string logFolder)
	{
		LoggerConfigName = loggerConfigName;
		LogFolder = logFolder;
		if (string.IsNullOrEmpty(LogFolder))
		{
			_logger = Telemetry.GetILogger(LoggerConfigName);
			return;
		}
		LogManager.ModifyLogParameter += HandleModifyLogParameter;
		_logger = Telemetry.GetILogger(LoggerConfigName);
		LogManager.ModifyLogParameter -= HandleModifyLogParameter;
	}

	private static string ReplaceFileNameMask(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			throw new ReportServerException("TextLogger.ReplaceFileNameMask(): fileName parameter cannot be null or empty.");
		}
		Match match = null;
		Regex regex = new Regex("%(?<DATE>\\w+)%");
		match = regex.Match(fileName);
		if (match.Success && match.Groups["DATE"].Success)
		{
			return regex.Replace(fileName, DateTime.UtcNow.ToString(match.Groups["DATE"].Value));
		}
		return fileName;
	}

	private void HandleModifyLogParameter(object sender, LogManager.LogInitializeParameterEventArgs e)
	{
		if (e.LogType == "File" && (e.Key == "Filename" || e.Key == "Archive"))
		{
			string text = (string)e.Value2;
			text = text.Replace("~", "").Replace("/", "\\");
			if (Path.IsPathRooted(text))
			{
				text = text.TrimStart('\\');
			}
			e.Value = Path.Combine(LogFolder, ReplaceFileNameMask(text));
		}
	}

	protected override void WriteLogEntry(LogEvent logEvent)
	{
		if (logEvent == null)
		{
			throw new ArgumentNullException("logEvent");
		}
		if (_logger != null)
		{
			switch (logEvent.LogType)
			{
			case LogType.Error:
			{
				string text = (logEvent.ExtendedLogging ? CreateExtendedLogMessage(logEvent) : logEvent.Message);
				_logger.LogError("{LogEvent}", text);
				break;
			}
			case LogType.Warning:
				_logger.LogWarning("{LogEvent}", logEvent.Message);
				break;
			case LogType.Information:
				_logger.LogInformation("{LogEvent}", logEvent.Message);
				break;
			default:
				_logger.LogDebug("{LogEvent}", logEvent.Message);
				break;
			}
		}
	}

	private static string CreateExtendedLogMessage(LogEvent logEvent)
	{
		string text = "Message: " + logEvent.Message;
		if (logEvent.ErrorNumber != 0)
		{
			text = text + Environment.NewLine + "Error number: " + logEvent.ErrorNumber.ToString(CultureInfo.InvariantCulture);
		}
		if (logEvent.ExceptionClass != null)
		{
			text = text + Environment.NewLine + "Exception class: " + logEvent.ExceptionClass;
		}
		text = text + Environment.NewLine + "Assembly name: " + logEvent.AssemblyName;
		if (logEvent.AdditionalInfo != null)
		{
			text = text + Environment.NewLine + "Additional info: " + logEvent.AdditionalInfo;
		}
		if (logEvent.StackTrace != null && logEvent.LogType == LogType.Error)
		{
			text = text + Environment.NewLine + "Stack trace: " + Environment.NewLine + string.Join(Environment.NewLine, logEvent.StackTrace);
		}
		return text;
	}
}
