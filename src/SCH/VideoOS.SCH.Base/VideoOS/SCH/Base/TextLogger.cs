using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using VideoOS.Diagnostics.Logging;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.SCH.Base;

public class TextLogger : Logger
{
	public string LoggerConfigName { get; private set; }

	public string LogFolder { get; private set; }

	public ILogger Logger { get; }

	public TextLogger(string loggerConfigName, string logFolder)
	{
		LoggerConfigName = loggerConfigName;
		LogFolder = logFolder;
		if (string.IsNullOrEmpty(LogFolder))
		{
			Logger = Telemetry.GetILogger(LoggerConfigName);
			return;
		}
		LogManager.ModifyLogParameter += HandleModifyLogParameter;
		Logger = Telemetry.GetILogger(LoggerConfigName);
		LogManager.ModifyLogParameter -= HandleModifyLogParameter;
	}

	private string ReplaceFileNameMask(string fileName)
	{
		if (string.IsNullOrEmpty(fileName))
		{
			throw new ServiceChannelException("TextLogger.ReplaceFileNameMask(): fileName parameter cannot be null or empty.");
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
		if (Logger != null)
		{
			string text = Environment.NewLine + "Message: " + logEvent.Message;
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
			if (logEvent.StackTrace != null && logEvent.Type == LogType.Error)
			{
				text = text + Environment.NewLine + "Stack trace: " + Environment.NewLine + string.Join(Environment.NewLine, logEvent.StackTrace.ToArray());
			}
			if (logEvent.Type == LogType.Error)
			{
				Logger.LogError(text);
			}
			else if (logEvent.Type == LogType.Warning)
			{
				Logger.LogWarning(text);
			}
			else if (logEvent.Type == LogType.Information)
			{
				Logger.LogInformation(text);
			}
			else
			{
				Logger.LogDebug(text);
			}
		}
	}
}
