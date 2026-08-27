#define TRACE
using System;
using System.Diagnostics;
using System.IO;

namespace VideoOS.Platform.SDK.Platform;

public class Logger
{
	private static string _currentFileName = "";

	private static int _dayToKeepLog = 30;

	private static bool _configFilePathInitialized = false;

	private static string _configFilePath = "";

	private static object _lockObject = new object();

	private static string company = "Milestone";

	public static string ConfigurationPath
	{
		get
		{
			string text = company;
			if (EnvironmentManager.Instance.EnvironmentOptions.ContainsKey("CompanyNameFolder"))
			{
				text = EnvironmentManager.Instance.EnvironmentOptions["CompanyNameFolder"];
			}
			if (!_configFilePathInitialized || company != text)
			{
				company = text;
				_configFilePath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData), company + "\\MIPSDK");
				_configFilePathInitialized = true;
			}
			return _configFilePath;
		}
	}

	private static string LogFilePrefix
	{
		get
		{
			if (EnvironmentManager.Instance.EnvironmentOptions.TryGetValue("LogPrefix", out var value))
			{
				return value;
			}
			return "MIP";
		}
	}

	private static string BuildFileName(DateTime date, string FirstName, string Ext)
	{
		string text = $"{date:yyyyMMdd}";
		return FirstName + text + "." + Ext;
	}

	private static void DebugLogLocation(string path, params string[] LogLine)
	{
		try
		{
			using StreamWriter streamWriter = File.AppendText(path);
			foreach (string text in LogLine)
			{
				streamWriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + " " + text);
				Trace.WriteLine("MIP: " + text);
			}
			streamWriter.Flush();
		}
		catch (Exception)
		{
		}
	}

	public static void DebugLog(params string[] LogLine)
	{
		lock (_lockObject)
		{
			string text = BuildFileName(DateTime.Now, LogFilePrefix, "log");
			if (text != _currentFileName)
			{
				CheckDeleteLogFiles();
				_currentFileName = text;
			}
			try
			{
				if (!Directory.Exists(ConfigurationPath))
				{
					Directory.CreateDirectory(ConfigurationPath);
				}
			}
			catch (Exception)
			{
			}
			DebugLogLocation(Path.Combine(ConfigurationPath, text), LogLine);
		}
	}

	private static void CheckDeleteLogFiles()
	{
		try
		{
			string strB = BuildFileName(DateTime.Now - TimeSpan.FromDays(_dayToKeepLog), LogFilePrefix, "log");
			string[] files = Directory.GetFiles(ConfigurationPath);
			foreach (string path in files)
			{
				string fileName = Path.GetFileName(path);
				if (fileName.StartsWith(LogFilePrefix) && fileName.CompareTo(strB) < 0)
				{
					try
					{
						File.Delete(path);
					}
					catch (Exception)
					{
					}
				}
			}
		}
		catch (Exception ex2)
		{
			Trace.WriteLine("Unable to delete old log files:" + ex2.Message);
		}
	}
}
