#define TRACE
using System;
using System.Collections;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Xml;
using NSpring.Logging;
using NSpring.Logging.Loggers;
using VideoOS.Diagnostics.Logging.Configuration;
using VideoOS.Diagnostics.Logging.Loggers;

namespace VideoOS.Diagnostics.Logging;

public sealed class LogManager
{
	public class LogInitializeParameterEventArgs : EventArgs
	{
		private string _key;

		private object _value;

		private object _value2;

		private string _logtype;

		private string _logName;

		public string Key => _key;

		public object Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
			}
		}

		public object Value2
		{
			get
			{
				return _value2;
			}
			set
			{
				_value2 = value;
			}
		}

		public string LogType => _logtype;

		public string LogName => _logName;

		public LogInitializeParameterEventArgs(string key, object value, object value2, string logType, string logName)
		{
			_key = key;
			_value = value;
			_value2 = value2;
			_logtype = logType;
			_logName = logName;
		}
	}

	public delegate void InitializeLogParameterEventHandler(object sender, LogInitializeParameterEventArgs e);

	private const string _defaultLoggerConfigXml = "<Logger name=\"{0}\"><Appender type=\"File\" level=\"warning\" filename=\"{1}\\Logs\\{0}.log\" archive=\"{1}\\Logs\\Archive\" maxsize=\"10\"></Appender></Logger>";

	private const string _defaultFileLogFormat = "{ts} [{ti:6w;ra}] {levelName:uc;10w} - {msg}\r";

	private const int _defaultBufferSize = 1000;

	private const bool _defaultIsBufferingEnabled = true;

	private const int _defaultAutoFlushInterval = 300;

	private static volatile bool _enableConsoleLoggerFallback;

	private static Hashtable loggerTable = new Hashtable();

	public static bool EnableConsoleLoggerFallback
	{
		get
		{
			return _enableConsoleLoggerFallback;
		}
		set
		{
			_enableConsoleLoggerFallback = value;
		}
	}

	public static bool LoggingEnabled { get; set; } = true;

	public static event InitializeLogParameterEventHandler ModifyLogParameter;

	private LogManager()
	{
	}

	public static LoggerBase GetLogger(string name)
	{
		LoggerBase loggerBase = null;
		lock (LoggerRegistry.DefaultInstance)
		{
			if (LoggerRegistry.DefaultInstance.LoggerNameExists(name))
			{
				loggerBase = loggerTable[name] as LoggerBase;
			}
			else
			{
				LogSettings logSettings = ConfigurationManager.GetSection("VideoOS.Diagnostics.Logging") as LogSettings;
				if (logSettings == null)
				{
					logSettings = new LogSettings();
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.LoadXml(string.Format(CultureInfo.InvariantCulture, "<Logger name=\"{0}\"><Appender type=\"File\" level=\"warning\" filename=\"{1}\\Logs\\{0}.log\" archive=\"{1}\\Logs\\Archive\" maxsize=\"10\"></Appender></Logger>", name, string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VideoOS")));
					logSettings.Loggers.Add(name, new VideoOS.Diagnostics.Logging.Configuration.Logger(xmlDocument.SelectSingleNode("Logger")));
				}
				VideoOS.Diagnostics.Logging.Configuration.Logger logger = logSettings.Loggers[name];
				if (logger == null || logger.Appenders.Count == 0)
				{
					XmlDocument xmlDocument2 = new XmlDocument();
					xmlDocument2.LoadXml(string.Format(CultureInfo.InvariantCulture, "<Logger name=\"{0}\"><Appender type=\"File\" level=\"warning\" filename=\"{1}\\Logs\\{0}.log\" archive=\"{1}\\Logs\\Archive\" maxsize=\"10\"></Appender></Logger>", name, string.Format(CultureInfo.InvariantCulture, "{0}\\{1}", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "VideoOS")));
					logger = new VideoOS.Diagnostics.Logging.Configuration.Logger(xmlDocument2.SelectSingleNode("Logger"));
				}
				if (logger.Appenders.Count == 1)
				{
					switch (logger.Appenders[0].AppenderType)
					{
					case AppenderTypes.File:
					{
						string text = Convert.ToString(logger.Appenders[0].Properties["FileName"], CultureInfo.InvariantCulture);
						string value = Convert.ToString(logger.Appenders[0].Properties["UnprocessedFileName"], CultureInfo.InvariantCulture);
						string text2 = Convert.ToString(logger.Appenders[0].Properties["Archive"], CultureInfo.InvariantCulture);
						string value2 = Convert.ToString(logger.Appenders[0].Properties["UnprocessedArchive"], CultureInfo.InvariantCulture);
						try
						{
							if (LogManager.ModifyLogParameter != null)
							{
								LogInitializeParameterEventArgs e = new LogInitializeParameterEventArgs("Filename", text, value, logger.Appenders[0].AppenderType.ToString(), name);
								LogManager.ModifyLogParameter(null, e);
								text = e.Value as string;
								e = new LogInitializeParameterEventArgs("Archive", text2, value2, logger.Appenders[0].AppenderType.ToString(), name);
								LogManager.ModifyLogParameter(null, e);
								text2 = e.Value as string;
							}
						}
						catch (Exception arg)
						{
							Trace.WriteLine($"VideoOS: Error while notifying components about creating log {name}. ERROR: {arg}");
						}
						NSpring.Logging.Loggers.FileLogger fileLogger = NSpring.Logging.Logger.CreateFileLogger(text, "{ts} [{ti:6w;ra}] {levelName:uc;10w} - {msg}\r");
						fileLogger.Level = logger.Appenders[0].LogLevel;
						fileLogger.IsContextEnabled = true;
						fileLogger.IsBufferingEnabled = true;
						fileLogger.BufferSize = 1000;
						fileLogger.AutoFlushInterval = 300;
						if (!string.IsNullOrEmpty(text2))
						{
							fileLogger.IsArchivingEnabled = true;
							fileLogger.ArchiveDirectoryPath = text2;
							try
							{
								fileLogger.MaximumFileSize = Convert.ToInt32(logger.Appenders[0].Properties["MaxSize"], CultureInfo.InvariantCulture) * 1024 * 1024;
							}
							catch (OutOfMemoryException)
							{
								throw;
							}
							catch
							{
							}
						}
						try
						{
							fileLogger.Open();
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch
						{
							if (_enableConsoleLoggerFallback)
							{
								loggerBase = CreateConsoleLogger(name, logger);
								break;
							}
							throw;
						}
						LoggerRegistry.DefaultInstance.AddLogger(name, fileLogger);
						loggerBase = new VideoOS.Diagnostics.Logging.Loggers.FileLogger(fileLogger);
						break;
					}
					case AppenderTypes.Console:
						loggerBase = CreateConsoleLogger(name, logger);
						break;
					case AppenderTypes.EventLog:
						loggerBase = CreateEventLogLogger(name, logger);
						break;
					}
				}
				else
				{
					NSpring.Logging.Logger logger2 = null;
					VideoOS.Diagnostics.Logging.Loggers.CompositeLogger compositeLogger = new VideoOS.Diagnostics.Logging.Loggers.CompositeLogger(new NSpring.Logging.Loggers.CompositeLogger());
					foreach (Appender appender in logger.Appenders)
					{
						switch (appender.AppenderType)
						{
						case AppenderTypes.File:
						{
							string text3 = Convert.ToString(appender.Properties["FileName"], CultureInfo.InvariantCulture);
							string value3 = Convert.ToString(logger.Appenders[0].Properties["UnprocessedFileName"], CultureInfo.InvariantCulture);
							string text4 = Convert.ToString(logger.Appenders[0].Properties["Archive"], CultureInfo.InvariantCulture);
							if (LogManager.ModifyLogParameter != null)
							{
								LogInitializeParameterEventArgs e2 = new LogInitializeParameterEventArgs("Filename", text3, value3, logger.Appenders[0].AppenderType.ToString(), name);
								LogManager.ModifyLogParameter(null, e2);
								text3 = e2.Value as string;
							}
							logger2 = NSpring.Logging.Logger.CreateFileLogger(text3, "{ts} [{ti:6w;ra}] {levelName:uc;10w} - {msg}\r");
							if (!string.IsNullOrEmpty(text4))
							{
								NSpring.Logging.Loggers.FileLogger fileLogger2 = (NSpring.Logging.Loggers.FileLogger)logger2;
								fileLogger2.IsArchivingEnabled = true;
								fileLogger2.ArchiveDirectoryPath = text4;
								try
								{
									fileLogger2.MaximumFileSize = Convert.ToInt32(logger.Appenders[0].Properties["MaxSize"], CultureInfo.InvariantCulture) * 1024 * 1024;
								}
								catch (OutOfMemoryException)
								{
									throw;
								}
								catch
								{
								}
							}
							logger2.Level = appender.LogLevel;
							compositeLogger.AddChild(logger2);
							break;
						}
						case AppenderTypes.Console:
							logger2 = NSpring.Logging.Logger.CreateConsoleLogger("{ts} [{ti:6w;ra}] {levelName:uc;10w} - {msg}\r");
							logger2.Level = appender.LogLevel;
							compositeLogger.AddChild(logger2);
							break;
						case AppenderTypes.EventLog:
							logger2 = NSpring.Logging.Logger.CreateWindowsEventLogger(Convert.ToString(appender.Properties["LogName"], CultureInfo.InvariantCulture));
							logger2.Level = appender.LogLevel;
							compositeLogger.AddChild(logger2);
							break;
						}
					}
					LoggerRegistry.DefaultInstance.AddLogger(name, compositeLogger.LoggerInstance);
					compositeLogger.LoggerInstance.IsContextEnabled = true;
					compositeLogger.LoggerInstance.IsBufferingEnabled = true;
					compositeLogger.LoggerInstance.BufferSize = 1000;
					logger2.AutoFlushInterval = 300;
					try
					{
						compositeLogger.LoggerInstance.Open();
						loggerBase = compositeLogger;
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch
					{
						if (!_enableConsoleLoggerFallback)
						{
							throw;
						}
						loggerBase = CreateConsoleLogger(name, logger);
					}
				}
				loggerTable.Add(name, loggerBase);
			}
		}
		return loggerBase;
	}

	private static VideoOS.Diagnostics.Logging.Loggers.ConsoleLogger CreateConsoleLogger(string name, VideoOS.Diagnostics.Logging.Configuration.Logger loggerConfig)
	{
		NSpring.Logging.Loggers.ConsoleLogger consoleLogger = NSpring.Logging.Logger.CreateConsoleLogger("{ts} [{ti:6w;ra}] {levelName:uc;10w} - {msg}\r");
		try
		{
			consoleLogger.Level = loggerConfig.Appenders[0].LogLevel;
			consoleLogger.IsContextEnabled = true;
			consoleLogger.IsBufferingEnabled = true;
			consoleLogger.BufferSize = 1000;
			consoleLogger.AutoFlushInterval = 300;
			consoleLogger.Open();
			LoggerRegistry.DefaultInstance.AddLogger(name, consoleLogger);
		}
		catch
		{
			consoleLogger.Dispose();
			throw;
		}
		return new VideoOS.Diagnostics.Logging.Loggers.ConsoleLogger(consoleLogger);
	}

	private static VideoOS.Diagnostics.Logging.Loggers.WindowsEventLogger CreateEventLogLogger(string name, VideoOS.Diagnostics.Logging.Configuration.Logger loggerConfig)
	{
		NSpring.Logging.Loggers.WindowsEventLogger windowsEventLogger = NSpring.Logging.Logger.CreateWindowsEventLogger(Convert.ToString(loggerConfig.Appenders[0].Properties["LogName"], CultureInfo.InvariantCulture));
		try
		{
			windowsEventLogger.Level = loggerConfig.Appenders[0].LogLevel;
			windowsEventLogger.IsContextEnabled = true;
			windowsEventLogger.IsBufferingEnabled = true;
			windowsEventLogger.BufferSize = 1000;
			windowsEventLogger.AutoFlushInterval = 300;
			windowsEventLogger.Open();
			LoggerRegistry.DefaultInstance.AddLogger(name, windowsEventLogger);
		}
		catch
		{
			windowsEventLogger.Dispose();
			throw;
		}
		return new VideoOS.Diagnostics.Logging.Loggers.WindowsEventLogger(windowsEventLogger);
	}

	public static void CloseAllLoggers()
	{
		LoggerRegistry.CloseAll();
		loggerTable.Clear();
	}

	public static void FlushAllLoggers()
	{
		lock (LoggerRegistry.DefaultInstance)
		{
			NSpring.Logging.Logger[] loggers = LoggerRegistry.DefaultInstance.Loggers;
			for (int i = 0; i < loggers.Length; i++)
			{
				loggers[i].Flush();
			}
		}
	}
}
