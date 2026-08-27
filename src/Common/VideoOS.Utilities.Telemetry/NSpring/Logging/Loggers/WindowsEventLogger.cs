using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using NSpring.Logging.EventFormatters;

namespace NSpring.Logging.Loggers;

public sealed class WindowsEventLogger : Logger
{
	private const string BLANK_STRING = "";

	private const string APPLICATION_LOG_SOURCE_NAME = "application";

	private const string SECURITY_LOG_SOURCE_NAME = "security";

	private const string SYSTEM_LOG_SOURCE_NAME = "system";

	private static Hashtable protectedLogSourceNames;

	private const string LOG_EXTENSION = " Log";

	private static object staticMonitor;

	private string eventLogApplicationName;

	private EventLog eventLog;

	private string applicationName;

	public string ApplicationName
	{
		get
		{
			return applicationName;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			value = value.Trim();
			if (value.Equals(""))
			{
				throw new ArgumentException("The name must contain non-whitespace characters");
			}
			lock (monitor)
			{
				if (!isClosed)
				{
					throw new InvalidOperationException("Cannot set this property while the logger is open");
				}
				applicationName = value;
			}
		}
	}

	static WindowsEventLogger()
	{
		protectedLogSourceNames = new Hashtable(100);
		staticMonitor = new object();
		protectedLogSourceNames.Add("application", "application");
		protectedLogSourceNames.Add("security", "security");
		protectedLogSourceNames.Add("system", "system");
	}

	public WindowsEventLogger(string _eventLogApplicationName)
	{
		ApplicationName = _eventLogApplicationName;
		base.IsBufferingEnabled = true;
		eventFormatter = new PatternEventFormatter("{message}");
	}

	public static bool DeleteEventLog(string eventLogApplicationName)
	{
		if (eventLogApplicationName == null)
		{
			return false;
		}
		lock (staticMonitor)
		{
			eventLogApplicationName = eventLogApplicationName.Trim();
			try
			{
				EventLog.DeleteEventSource(eventLogApplicationName);
				EventLog.Delete(eventLogApplicationName + " Log");
				return true;
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				return false;
			}
		}
	}

	protected override void AcquireResources()
	{
		if (applicationName == null)
		{
			throw new ApplicationException("No application name set-- cannot open");
		}
		string text = applicationName.Trim();
		string text2 = text.ToLower();
		if (text2.EndsWith(" Log".ToLower()))
		{
			text = text.Substring(0, text.Length - " Log".Length).Trim();
		}
		if (text.Equals(""))
		{
			throw new ApplicationException("Invalid application name");
		}
		if (protectedLogSourceNames.ContainsKey(text2))
		{
			throw new ApplicationException("The name '" + applicationName + "' is restricted from use for Windows event logging");
		}
		eventLogApplicationName = text;
		string text3 = eventLogApplicationName + " Log";
		try
		{
			if (!EventLog.SourceExists(eventLogApplicationName))
			{
				try
				{
					EventLog.CreateEventSource(eventLogApplicationName, text3);
					try
					{
						Thread.Sleep(500);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch
					{
					}
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			throw new ApplicationException("Couldn't create event source '" + eventLogApplicationName + "': " + ex4.GetType().FullName + ": " + ex4.Message);
		}
		try
		{
			eventLog = new EventLog();
			eventLog.Log = text3;
			eventLog.Source = eventLogApplicationName;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex6)
		{
			throw new ApplicationException("Couldn't create event log: " + ex6.GetType().FullName + ": " + ex6.Message);
		}
	}

	protected override void ReleaseResources()
	{
		try
		{
			eventLog.Close();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		eventLog = null;
	}

	protected override void Write(Event _event)
	{
		if (_event.Level == Level.Error)
		{
			eventLog.WriteEntry(eventFormatter.Format(_event), EventLogEntryType.Error);
		}
		else if (_event.Level == Level.Warning)
		{
			eventLog.WriteEntry(eventFormatter.Format(_event), EventLogEntryType.Warning);
		}
		else
		{
			eventLog.WriteEntry(eventFormatter.Format(_event), EventLogEntryType.Information);
		}
	}
}
