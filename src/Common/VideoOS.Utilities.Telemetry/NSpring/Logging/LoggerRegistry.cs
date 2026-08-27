using System;
using System.Collections;

namespace NSpring.Logging;

public class LoggerRegistry
{
	private static LoggerRegistry defaultInstance;

	private static ArrayList registries;

	private static object staticMonitor;

	private bool isCaseSensitive;

	private Hashtable loggers = new Hashtable(1000);

	private Logger[] loggersArray = new Logger[0];

	private string[] loggerNamesArray = new string[0];

	private object monitor = new object();

	public static LoggerRegistry DefaultInstance => defaultInstance;

	public bool IsCaseSensitive
	{
		get
		{
			return isCaseSensitive;
		}
		set
		{
			isCaseSensitive = value;
		}
	}

	public Logger[] Loggers
	{
		get
		{
			lock (monitor)
			{
				if (loggersArray == null)
				{
					int count = loggers.Values.Count;
					loggersArray = new Logger[count];
					loggers.Values.CopyTo(loggersArray, 0);
				}
				Logger[] array = new Logger[loggersArray.Length];
				Array.Copy(loggersArray, 0, array, 0, loggersArray.Length);
				return array;
			}
		}
	}

	public string[] LoggerNames
	{
		get
		{
			lock (monitor)
			{
				if (loggerNamesArray == null)
				{
					int count = loggers.Keys.Count;
					loggerNamesArray = new string[count];
					loggers.Keys.CopyTo(loggerNamesArray, 0);
					Array.Sort(loggerNamesArray);
				}
				string[] array = new string[loggerNamesArray.Length];
				Array.Copy(loggerNamesArray, 0, array, 0, loggerNamesArray.Length);
				return array;
			}
		}
	}

	static LoggerRegistry()
	{
		staticMonitor = new object();
		defaultInstance = new LoggerRegistry();
		registries = new ArrayList();
		registries.Add(defaultInstance);
	}

	public static LoggerRegistry CreateLoggerRegistry()
	{
		LoggerRegistry loggerRegistry = new LoggerRegistry();
		lock (staticMonitor)
		{
			registries.Add(loggerRegistry);
			return loggerRegistry;
		}
	}

	public void AddLogger(string name, Logger logger)
	{
		if (name == null || logger == null)
		{
			throw new ArgumentNullException();
		}
		name = name.Trim();
		if (!isCaseSensitive)
		{
			name = name.ToLower();
		}
		lock (monitor)
		{
			if (loggers.ContainsKey(name))
			{
				throw new ApplicationException("A logger is already registered with the name '" + name + "'");
			}
			loggers.Add(name, logger);
			loggersArray = null;
			loggerNamesArray = null;
		}
	}

	public void RemoveLogger(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException();
		}
		name = name.Trim();
		if (!isCaseSensitive)
		{
			name = name.ToLower();
		}
		lock (monitor)
		{
			loggers.Remove(name);
			loggersArray = null;
			loggerNamesArray = null;
		}
	}

	public void RemoveLogger(Logger logger)
	{
		if (logger == null)
		{
			throw new ArgumentNullException();
		}
		ArrayList arrayList = new ArrayList(20);
		lock (monitor)
		{
			IEnumerator enumerator = loggers.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				string text = (string)enumerator.Current;
				if (loggers[text] == logger)
				{
					arrayList.Add(text);
				}
			}
			for (int num = arrayList.Count - 1; num >= 0; num--)
			{
				loggers.Remove(arrayList[num]);
			}
			loggersArray = null;
			loggerNamesArray = null;
		}
	}

	public Logger GetLogger(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException();
		}
		name = name.Trim();
		if (!isCaseSensitive)
		{
			name = name.ToLower();
		}
		Logger logger;
		lock (monitor)
		{
			logger = (Logger)loggers[name];
		}
		if (logger == null)
		{
			throw new ApplicationException("No logger is registered with the name '" + name + "'");
		}
		return logger;
	}

	public bool LoggerNameExists(string name)
	{
		lock (monitor)
		{
			if (name == null)
			{
				throw new ArgumentNullException();
			}
			name = name.Trim();
			if (!isCaseSensitive)
			{
				name = name.ToLower();
			}
			return loggers.ContainsKey(name);
		}
	}

	public void Clear()
	{
		lock (monitor)
		{
			loggers.Clear();
			loggersArray = null;
			loggerNamesArray = null;
		}
	}

	public void Close()
	{
		lock (monitor)
		{
			IEnumerator enumerator = loggers.Values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				Logger logger = (Logger)enumerator.Current;
				try
				{
					logger.Close();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
			loggers.Clear();
			loggersArray = new Logger[0];
			loggerNamesArray = new string[0];
		}
	}

	public static void CloseAll()
	{
		lock (staticMonitor)
		{
			for (int num = registries.Count - 1; num >= 0; num--)
			{
				((LoggerRegistry)registries[num]).Close();
			}
		}
	}
}
