using System;
using System.Collections;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NSpring.Logging.Exceptions;
using NSpring.Logging.Loggers;

namespace NSpring.Logging;

public abstract class Logger : IExceptionHandler, IDisposable
{
	private class FlushingManager : IDisposable
	{
		private Logger logger;

		private bool interrupted;

		private int flushInterval;

		private object monitor = new object();

		private bool requestFlush;

		private bool flushTaskRunning;

		private Timer timer;

		public int FlushInterval
		{
			get
			{
				return flushInterval;
			}
			set
			{
				flushInterval = value;
				lock (monitor)
				{
					if (!interrupted)
					{
						timer.Change(-1, flushInterval);
					}
				}
			}
		}

		public FlushingManager(Logger _logger, int _flushInterval)
		{
			logger = _logger;
			flushInterval = _flushInterval;
			timer = new Timer(TimerCallback, null, -1, flushInterval);
		}

		private void TimerCallback(object state)
		{
			RequestFlush();
		}

		private void RequestFlush()
		{
			lock (monitor)
			{
				requestFlush = true;
				EnsureTaskRunning();
			}
		}

		private void EnsureTaskRunning()
		{
			lock (monitor)
			{
				if (requestFlush && !flushTaskRunning)
				{
					requestFlush = false;
					flushTaskRunning = true;
					Task.Factory.StartNew(delegate
					{
						RunFlush();
					}, CancellationToken.None, TaskCreationOptions.None).ContinueWith(delegate
					{
						EnsureTaskRunning();
					});
				}
			}
		}

		private void RunFlush()
		{
			if (!interrupted)
			{
				try
				{
					logger.Flush();
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
			lock (monitor)
			{
				flushTaskRunning = false;
			}
		}

		public void Stop()
		{
			lock (monitor)
			{
				if (!interrupted)
				{
					interrupted = true;
					timer.Change(-1, -1);
				}
			}
		}

		public bool IsRunning()
		{
			lock (monitor)
			{
				return flushTaskRunning;
			}
		}

		public void Awaken()
		{
			RequestFlush();
		}

		~FlushingManager()
		{
			Stop();
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && timer != null)
			{
				timer.Dispose();
				timer = null;
			}
		}
	}

	private class Registration
	{
		public readonly string Name;

		public LoggerRegistry Registry;

		private Registration()
		{
		}

		public Registration(string name, LoggerRegistry registry)
		{
			Name = name;
			Registry = registry;
		}
	}

	private static char[] HEX_CHARS = new char[16]
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F'
	};

	private static IExceptionHandler defaultExceptionHandler = new SilentExceptionHandler();

	private IExceptionHandler exceptionHandler = defaultExceptionHandler;

	private const int DEFAULT_BUFFER_SIZE = 100000;

	private const int MINIMUM_BUFFER_SIZE = 5;

	private const int DEFAULT_AUTO_FLUSH_INTERVAL = 15000;

	private const int MINIMUM_AUTO_FLUSH_INTERVAL = 1000;

	private static readonly LoggerRegistry registry = LoggerRegistry.DefaultInstance;

	private readonly Hashtable registrations = new Hashtable(100);

	protected bool isClosed = true;

	private Level defaultEventLevel = Level.Info;

	private Level level = Level.All;

	private int levelPriority = Level.All.Priority;

	private Logger[] children = new Logger[5];

	private int childCount;

	private readonly object childMonitor = new object();

	private static bool isContextEnabledByDefault = false;

	private bool isContextEnabled = isContextEnabledByDefault;

	private static bool isExceptionAutoFlushEnabledByDefault = false;

	private bool isExceptionAutoFlushEnabled = isExceptionAutoFlushEnabledByDefault;

	private bool isBufferingEnabled;

	private int bufferSize = 100000;

	private int maximumBufferSize = int.MaxValue;

	private int bufferCount;

	private Event[] writeBuffer;

	private int writeBufferCount;

	private readonly object writeBufferMonitor = new object();

	private int autoFlushInterval = 15000;

	private Context[] contextBuffer;

	private Level[] levelBuffer;

	private long[] timestampBuffer;

	private string[] messageBuffer;

	private object[] dataBuffer;

	private string[] categoryBuffer;

	private string[] codeBuffer;

	private string[] idBuffer;

	private Context[] contextWriteBuffer;

	private Level[] levelWriteBuffer;

	private long[] timestampWriteBuffer;

	private string[] messageWriteBuffer;

	private object[] dataWriteBuffer;

	private string[] categoryWriteBuffer;

	private string[] codeWriteBuffer;

	private string[] idWriteBuffer;

	private Context[] tempContextBuffer;

	private Level[] tempLevelBuffer;

	private long[] tempTimestampBuffer;

	private string[] tempMessageBuffer;

	private object[] tempDataBuffer;

	private string[] tempCategoryBuffer;

	private string[] tempCodeBuffer;

	private string[] tempIDBuffer;

	private FlushingManager flushingManager;

	protected EventFormatter eventFormatter;

	private bool isIDGenerationEnabled;

	private Random random1 = new Random((int)DateTime.UtcNow.Ticks);

	private byte[] byteArray1 = new byte[16];

	private char[] charArray1 = new char[32];

	private Random random2 = new Random((int)(DateTime.UtcNow.Ticks - 371));

	private byte[] byteArray2 = new byte[16];

	private char[] charArray2 = new char[32];

	protected Encoding encoding = Encoding.UTF8;

	protected readonly object monitor = new object();

	public static Logger[] Loggers => registry.Loggers;

	public static string[] LoggerNames => registry.LoggerNames;

	public static bool IsRegistryCaseSensitive
	{
		get
		{
			return registry.IsCaseSensitive;
		}
		set
		{
			registry.IsCaseSensitive = value;
		}
	}

	public int ChildCount
	{
		get
		{
			lock (childMonitor)
			{
				return childCount;
			}
		}
	}

	public Logger[] Children
	{
		get
		{
			lock (childMonitor)
			{
				Logger[] array = new Logger[childCount];
				if (childCount > 0)
				{
					Array.Copy(children, 0, array, 0, childCount);
				}
				return array;
			}
		}
	}

	public Encoding Encoding
	{
		get
		{
			return encoding;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			lock (monitor)
			{
				if (UpdateEncoding(value))
				{
					encoding = value;
					return;
				}
				throw new ApplicationException("Invalid encoding");
			}
		}
	}

	public EventFormatter EventFormatter
	{
		get
		{
			lock (monitor)
			{
				return eventFormatter;
			}
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			lock (monitor)
			{
				if (UpdateEventFormatter(value))
				{
					value.Logger = this;
					eventFormatter = value;
					return;
				}
				throw new ApplicationException("Invalid event formatter");
			}
		}
	}

	public static IExceptionHandler DefaultExceptionHandler
	{
		get
		{
			return defaultExceptionHandler;
		}
		set
		{
			if (value == null)
			{
				defaultExceptionHandler = new SilentExceptionHandler();
			}
			else
			{
				defaultExceptionHandler = value;
			}
		}
	}

	public IExceptionHandler ExceptionHandler
	{
		get
		{
			return exceptionHandler;
		}
		set
		{
			if (value == null)
			{
				exceptionHandler = new SilentExceptionHandler();
				return;
			}
			if (value == this)
			{
				throw new ArgumentException("A logger cannot handle its own exceptions");
			}
			lock (monitor)
			{
				if (!isClosed)
				{
					try
					{
						value.Open();
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception ex2)
					{
						throw new ApplicationException("Could not open the exception handler: " + ex2.GetType().FullName + ": " + ex2.Message);
					}
				}
				exceptionHandler = value;
			}
		}
	}

	public Level Level
	{
		get
		{
			return level;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			lock (monitor)
			{
				level = value;
				levelPriority = level.Priority;
			}
		}
	}

	public bool IsLoggingEnabled
	{
		get
		{
			if (!isClosed)
			{
				return level != Level.None;
			}
			return false;
		}
	}

	public Level DefaultEventLevel
	{
		get
		{
			return defaultEventLevel;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			defaultEventLevel = value;
		}
	}

	public static bool IsContextEnabledByDefault
	{
		get
		{
			return isContextEnabledByDefault;
		}
		set
		{
			isContextEnabledByDefault = value;
		}
	}

	public bool IsContextEnabled
	{
		get
		{
			return isContextEnabled;
		}
		set
		{
			isContextEnabled = value;
		}
	}

	public bool IsIDGenerationEnabled
	{
		get
		{
			return isIDGenerationEnabled;
		}
		set
		{
			isIDGenerationEnabled = value;
		}
	}

	public bool IsOpen
	{
		get
		{
			lock (monitor)
			{
				return !isClosed;
			}
		}
	}

	public bool IsBufferingEnabled
	{
		get
		{
			lock (monitor)
			{
				return isBufferingEnabled;
			}
		}
		set
		{
			lock (monitor)
			{
				if (!isClosed)
				{
					throw new InvalidOperationException("Cannot set property while the logger is open.");
				}
				isBufferingEnabled = value;
			}
		}
	}

	public bool IsExceptionAutoFlushEnabled
	{
		get
		{
			return isExceptionAutoFlushEnabled;
		}
		set
		{
			isExceptionAutoFlushEnabled = value;
		}
	}

	public static bool IsExceptionAutoFlushEnabledByDefault
	{
		get
		{
			return isExceptionAutoFlushEnabledByDefault;
		}
		set
		{
			isExceptionAutoFlushEnabledByDefault = value;
		}
	}

	public int BufferSize
	{
		get
		{
			return bufferSize;
		}
		set
		{
			lock (monitor)
			{
				if (IsOpen)
				{
					throw new InvalidOperationException("Cannot set property while the logger is open.");
				}
				bufferSize = Math.Max(value, 5);
				maximumBufferSize = Math.Max(maximumBufferSize, bufferSize);
			}
		}
	}

	public int MaximumBufferSize
	{
		get
		{
			return maximumBufferSize;
		}
		set
		{
			lock (monitor)
			{
				Flush();
				maximumBufferSize = Math.Max(value, bufferSize);
			}
		}
	}

	public int BufferCount
	{
		get
		{
			lock (monitor)
			{
				return bufferCount;
			}
		}
	}

	public int AutoFlushInterval
	{
		get
		{
			lock (monitor)
			{
				return autoFlushInterval;
			}
		}
		set
		{
			int num = Math.Max(value, 1000);
			lock (monitor)
			{
				autoFlushInterval = num;
				if (!isClosed && isBufferingEnabled)
				{
					int flushInterval = flushingManager.FlushInterval;
					flushingManager.FlushInterval = num;
					if (flushInterval > num)
					{
						Flush();
					}
				}
			}
		}
	}

	public static void AddLogger(string name, Logger logger)
	{
		registry.AddLogger(name, logger);
	}

	public static void RemoveLogger(string name)
	{
		registry.RemoveLogger(name);
	}

	public static void RemoveLogger(Logger logger)
	{
		registry.RemoveLogger(logger);
	}

	public static void CloseLoggers()
	{
		registry.Close();
	}

	public static Logger GetLogger(string name)
	{
		return registry.GetLogger(name);
	}

	public static ConsoleLogger CreateConsoleLogger()
	{
		return new ConsoleLogger();
	}

	public static ConsoleLogger CreateConsoleLogger(string _pattern)
	{
		return new ConsoleLogger(_pattern);
	}

	public static FileLogger CreateFileLogger(string _filePath)
	{
		return new FileLogger(_filePath);
	}

	public static FileLogger CreateFileLogger(string _filePath, string _outputPattern)
	{
		return new FileLogger(_filePath, _outputPattern);
	}

	public static WindowsEventLogger CreateWindowsEventLogger(string _eventLogApplicationName)
	{
		return new WindowsEventLogger(_eventLogApplicationName);
	}

	public void RegisterAs(string name)
	{
		if (name == null)
		{
			throw new ArgumentNullException();
		}
		lock (monitor)
		{
			if (isClosed)
			{
				Open();
			}
			registry.AddLogger(name, this);
			registrations.Add(name + "_" + registry.GetHashCode(), new Registration(name, registry));
		}
	}

	public void RegisterAs(string name, LoggerRegistry _registry)
	{
		if (name == null)
		{
			throw new ArgumentNullException();
		}
		lock (monitor)
		{
			if (isClosed)
			{
				Open();
			}
			_registry.AddLogger(name, this);
			registrations.Add(name + "_" + _registry.GetHashCode(), new Registration(name, _registry));
		}
	}

	public void Deregister()
	{
		lock (monitor)
		{
			IEnumerator enumerator = registrations.Keys.GetEnumerator();
			while (enumerator.MoveNext())
			{
				((Registration)registrations[enumerator.Current]).Registry.RemoveLogger(this);
			}
			registrations.Clear();
		}
	}

	public void AddChild(Logger child)
	{
		if (child == null)
		{
			throw new ArgumentNullException();
		}
		lock (monitor)
		{
			lock (childMonitor)
			{
				for (int i = 0; i < childCount; i++)
				{
					if (children[i] == child)
					{
						return;
					}
				}
				if (!isClosed && !child.IsOpen)
				{
					child.Open();
				}
				if (childCount == children.Length)
				{
					Logger[] destinationArray = new Logger[childCount * 2];
					Array.Copy(children, 0, destinationArray, 0, childCount);
					children = destinationArray;
				}
				children[childCount++] = child;
				if (child.Level.Priority < Level.Priority)
				{
					Level = child.Level;
				}
			}
		}
	}

	public void RemoveChild(Logger child)
	{
		if (child == null)
		{
			throw new ArgumentNullException();
		}
		lock (childMonitor)
		{
			for (int i = 0; i < childCount; i++)
			{
				if (children[i] == child)
				{
					if (i < childCount - 1)
					{
						Array.Copy(children, i + 1, children, i, childCount - i - 1);
					}
					children[--childCount] = null;
					break;
				}
			}
		}
	}

	public void ClearChildren()
	{
		lock (childMonitor)
		{
			for (int i = 0; i < childCount; i++)
			{
				children[i] = null;
			}
			childCount = 0;
		}
	}

	public bool HasChild(Logger child)
	{
		if (child == null)
		{
			throw new ArgumentNullException();
		}
		lock (childMonitor)
		{
			for (int i = 0; i < childCount; i++)
			{
				if (children[i] == child)
				{
					return true;
				}
			}
			return false;
		}
	}

	public Logger GetChildAt(int index)
	{
		if (index < 0)
		{
			throw new IndexOutOfRangeException();
		}
		lock (childMonitor)
		{
			if (index > childCount)
			{
				throw new IndexOutOfRangeException();
			}
			return children[index];
		}
	}

	protected virtual bool UpdateEncoding(Encoding _encoding)
	{
		return true;
	}

	protected virtual bool UpdateEventFormatter(EventFormatter _eventFormatter)
	{
		return true;
	}

	public void Log(Level _level, string _message)
	{
		try
		{
			if (_message == null)
			{
				return;
			}
			if (_level == null)
			{
				_level = defaultEventLevel;
			}
			if (_level.Priority < levelPriority)
			{
				return;
			}
			Context context = ((!isContextEnabled) ? null : Context.CurrentContext.Copy());
			long ticks = DateTime.UtcNow.Ticks;
			Monitor.Enter(monitor);
			if (isClosed)
			{
				Monitor.Exit(monitor);
				return;
			}
			if (isBufferingEnabled)
			{
				if (bufferCount < bufferSize)
				{
					contextBuffer[bufferCount] = context;
					levelBuffer[bufferCount] = _level;
					timestampBuffer[bufferCount] = ticks;
					messageBuffer[bufferCount] = _message;
					bufferCount++;
					Monitor.Exit(monitor);
					flushingManager.Awaken();
				}
				else if (bufferCount >= maximumBufferSize)
				{
					Monitor.Exit(monitor);
				}
				else if (bufferCount == contextBuffer.Length)
				{
					int num = contextBuffer.Length * 2;
					Context[] destinationArray = new Context[num];
					Level[] destinationArray2 = new Level[num];
					long[] destinationArray3 = new long[num];
					string[] destinationArray4 = new string[num];
					object[] destinationArray5 = new object[num];
					string[] destinationArray6 = new string[num];
					string[] destinationArray7 = new string[num];
					string[] destinationArray8 = new string[num];
					Array.Copy(contextBuffer, 0, destinationArray, 0, bufferCount);
					Array.Copy(levelBuffer, 0, destinationArray2, 0, bufferCount);
					Array.Copy(timestampBuffer, 0, destinationArray3, 0, bufferCount);
					Array.Copy(messageBuffer, 0, destinationArray4, 0, bufferCount);
					Array.Copy(dataBuffer, 0, destinationArray5, 0, bufferCount);
					Array.Copy(categoryBuffer, 0, destinationArray6, 0, bufferCount);
					Array.Copy(codeBuffer, 0, destinationArray7, 0, bufferCount);
					Array.Copy(idBuffer, 0, destinationArray8, 0, bufferCount);
					contextBuffer = destinationArray;
					levelBuffer = destinationArray2;
					timestampBuffer = destinationArray3;
					messageBuffer = destinationArray4;
					dataBuffer = destinationArray5;
					categoryBuffer = destinationArray6;
					codeBuffer = destinationArray7;
					idBuffer = destinationArray8;
					contextBuffer[bufferCount] = context;
					levelBuffer[bufferCount] = _level;
					timestampBuffer[bufferCount] = ticks;
					messageBuffer[bufferCount] = _message;
					bufferCount++;
					if (bufferCount == bufferSize)
					{
						flushingManager.Awaken();
					}
					Monitor.Exit(monitor);
				}
				else
				{
					contextBuffer[bufferCount] = context;
					levelBuffer[bufferCount] = _level;
					timestampBuffer[bufferCount] = ticks;
					messageBuffer[bufferCount] = _message;
					bufferCount++;
					if (bufferCount == bufferSize)
					{
						flushingManager.Awaken();
					}
					Monitor.Exit(monitor);
				}
				return;
			}
			string id = null;
			if (isIDGenerationEnabled)
			{
				random1.NextBytes(byteArray1);
				charArray1[0] = HEX_CHARS[byteArray1[0] & 0xF];
				charArray1[1] = HEX_CHARS[byteArray1[0] >> 4];
				charArray1[2] = HEX_CHARS[byteArray1[1] & 0xF];
				charArray1[3] = HEX_CHARS[byteArray1[1] >> 4];
				charArray1[4] = HEX_CHARS[byteArray1[2] & 0xF];
				charArray1[5] = HEX_CHARS[byteArray1[2] >> 4];
				charArray1[6] = HEX_CHARS[byteArray1[3] & 0xF];
				charArray1[7] = HEX_CHARS[byteArray1[3] >> 4];
				charArray1[8] = HEX_CHARS[byteArray1[4] & 0xF];
				charArray1[9] = HEX_CHARS[byteArray1[4] >> 4];
				charArray1[10] = HEX_CHARS[byteArray1[5] & 0xF];
				charArray1[11] = HEX_CHARS[byteArray1[5] >> 4];
				charArray1[12] = HEX_CHARS[byteArray1[6] & 0xF];
				charArray1[13] = HEX_CHARS[byteArray1[6] >> 4];
				charArray1[14] = HEX_CHARS[byteArray1[7] & 0xF];
				charArray1[15] = HEX_CHARS[byteArray1[7] >> 4];
				charArray1[16] = HEX_CHARS[byteArray1[8] & 0xF];
				charArray1[17] = HEX_CHARS[byteArray1[8] >> 4];
				charArray1[18] = HEX_CHARS[byteArray1[9] & 0xF];
				charArray1[19] = HEX_CHARS[byteArray1[9] >> 4];
				charArray1[20] = HEX_CHARS[byteArray1[10] & 0xF];
				charArray1[21] = HEX_CHARS[byteArray1[10] >> 4];
				charArray1[22] = HEX_CHARS[byteArray1[11] & 0xF];
				charArray1[23] = HEX_CHARS[byteArray1[11] >> 4];
				charArray1[24] = HEX_CHARS[byteArray1[12] & 0xF];
				charArray1[25] = HEX_CHARS[byteArray1[12] >> 4];
				charArray1[26] = HEX_CHARS[byteArray1[13] & 0xF];
				charArray1[27] = HEX_CHARS[byteArray1[13] >> 4];
				charArray1[28] = HEX_CHARS[byteArray1[14] & 0xF];
				charArray1[29] = HEX_CHARS[byteArray1[14] >> 4];
				charArray1[30] = HEX_CHARS[byteArray1[15] & 0xF];
				charArray1[31] = HEX_CHARS[byteArray1[15] >> 4];
				id = new string(charArray1);
			}
			Event obj = new Event(new DateTime(ticks, DateTimeKind.Utc), _level, _message, null, null, null, id);
			try
			{
				Write(obj);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception exception)
			{
				exceptionHandler.Handle(new LoggerException(this, obj, exception));
			}
			if (childCount > 0)
			{
				Monitor.Enter(childMonitor);
				Monitor.Exit(monitor);
				for (int i = 0; i < childCount; i++)
				{
					children[i].LogEvent(_level, _message, null, null, null, id);
				}
				Monitor.Exit(childMonitor);
			}
			else
			{
				Monitor.Exit(monitor);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (ThreadInterruptedException)
		{
		}
	}

	public void Handle(LoggerException e)
	{
		if (e == null)
		{
			return;
		}
		if (e.IsBatchException)
		{
			for (int i = 0; i < e.Exceptions.Length; i++)
			{
				Event obj = e.Event;
				if (obj == null)
				{
					continue;
				}
				try
				{
					lock (monitor)
					{
						LogEvent(obj.Level, obj.Message, obj.Data, obj.Category, obj.Code, obj.ID);
					}
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (LoggerException exception)
				{
					try
					{
						exceptionHandler.Handle(exception);
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
			return;
		}
		Event obj3 = e.Event;
		try
		{
			LogEvent(obj3.Level, obj3.Message, obj3.Data, obj3.Category, obj3.Code, obj3.ID);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (LoggerException exception2)
		{
			try
			{
				exceptionHandler.Handle(exception2);
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

	private void LogEvent(Level _level, string _message, object _data, string _category, string _code, string _id)
	{
		if (_level.Priority < levelPriority)
		{
			return;
		}
		Context context = ((!isContextEnabled) ? null : Context.CurrentContext.Copy());
		long ticks = DateTime.UtcNow.Ticks;
		Monitor.Enter(monitor);
		if (isClosed)
		{
			Monitor.Exit(monitor);
			return;
		}
		if (isBufferingEnabled)
		{
			if (bufferCount < bufferSize)
			{
				contextBuffer[bufferCount] = context;
				levelBuffer[bufferCount] = _level;
				timestampBuffer[bufferCount] = ticks;
				messageBuffer[bufferCount] = _message;
				dataBuffer[bufferCount] = _data;
				categoryBuffer[bufferCount] = _category;
				codeBuffer[bufferCount] = _code;
				idBuffer[bufferCount] = _id;
				bufferCount++;
				Monitor.Exit(monitor);
			}
			else if (bufferCount >= maximumBufferSize)
			{
				Monitor.Exit(monitor);
			}
			else if (bufferCount == contextBuffer.Length)
			{
				int num = contextBuffer.Length * 2;
				Context[] destinationArray = new Context[num];
				Level[] destinationArray2 = new Level[num];
				long[] destinationArray3 = new long[num];
				string[] destinationArray4 = new string[num];
				object[] destinationArray5 = new object[num];
				string[] destinationArray6 = new string[num];
				string[] destinationArray7 = new string[num];
				string[] destinationArray8 = new string[num];
				Array.Copy(contextBuffer, 0, destinationArray, 0, bufferCount);
				Array.Copy(levelBuffer, 0, destinationArray2, 0, bufferCount);
				Array.Copy(timestampBuffer, 0, destinationArray3, 0, bufferCount);
				Array.Copy(messageBuffer, 0, destinationArray4, 0, bufferCount);
				Array.Copy(dataBuffer, 0, destinationArray5, 0, bufferCount);
				Array.Copy(categoryBuffer, 0, destinationArray6, 0, bufferCount);
				Array.Copy(codeBuffer, 0, destinationArray7, 0, bufferCount);
				Array.Copy(idBuffer, 0, destinationArray8, 0, bufferCount);
				contextBuffer = destinationArray;
				levelBuffer = destinationArray2;
				timestampBuffer = destinationArray3;
				messageBuffer = destinationArray4;
				dataBuffer = destinationArray5;
				categoryBuffer = destinationArray6;
				codeBuffer = destinationArray7;
				idBuffer = destinationArray8;
				contextBuffer[bufferCount] = context;
				levelBuffer[bufferCount] = _level;
				timestampBuffer[bufferCount] = ticks;
				messageBuffer[bufferCount] = _message;
				dataBuffer[bufferCount] = _data;
				categoryBuffer[bufferCount] = _category;
				codeBuffer[bufferCount] = _code;
				idBuffer[bufferCount] = _id;
				bufferCount++;
				if (bufferCount == bufferSize)
				{
					flushingManager.Awaken();
				}
				Monitor.Exit(monitor);
			}
			else
			{
				contextBuffer[bufferCount] = context;
				levelBuffer[bufferCount] = _level;
				timestampBuffer[bufferCount] = ticks;
				messageBuffer[bufferCount] = _message;
				dataBuffer[bufferCount] = _data;
				categoryBuffer[bufferCount] = _category;
				codeBuffer[bufferCount] = _code;
				idBuffer[bufferCount] = _id;
				bufferCount++;
				if (bufferCount == bufferSize)
				{
					flushingManager.Awaken();
				}
				Monitor.Exit(monitor);
			}
			return;
		}
		if (isIDGenerationEnabled && _id == null)
		{
			random1.NextBytes(byteArray1);
			charArray1[0] = HEX_CHARS[byteArray1[0] & 0xF];
			charArray1[1] = HEX_CHARS[byteArray1[0] >> 4];
			charArray1[2] = HEX_CHARS[byteArray1[1] & 0xF];
			charArray1[3] = HEX_CHARS[byteArray1[1] >> 4];
			charArray1[4] = HEX_CHARS[byteArray1[2] & 0xF];
			charArray1[5] = HEX_CHARS[byteArray1[2] >> 4];
			charArray1[6] = HEX_CHARS[byteArray1[3] & 0xF];
			charArray1[7] = HEX_CHARS[byteArray1[3] >> 4];
			charArray1[8] = HEX_CHARS[byteArray1[4] & 0xF];
			charArray1[9] = HEX_CHARS[byteArray1[4] >> 4];
			charArray1[10] = HEX_CHARS[byteArray1[5] & 0xF];
			charArray1[11] = HEX_CHARS[byteArray1[5] >> 4];
			charArray1[12] = HEX_CHARS[byteArray1[6] & 0xF];
			charArray1[13] = HEX_CHARS[byteArray1[6] >> 4];
			charArray1[14] = HEX_CHARS[byteArray1[7] & 0xF];
			charArray1[15] = HEX_CHARS[byteArray1[7] >> 4];
			charArray1[16] = HEX_CHARS[byteArray1[8] & 0xF];
			charArray1[17] = HEX_CHARS[byteArray1[8] >> 4];
			charArray1[18] = HEX_CHARS[byteArray1[9] & 0xF];
			charArray1[19] = HEX_CHARS[byteArray1[9] >> 4];
			charArray1[20] = HEX_CHARS[byteArray1[10] & 0xF];
			charArray1[21] = HEX_CHARS[byteArray1[10] >> 4];
			charArray1[22] = HEX_CHARS[byteArray1[11] & 0xF];
			charArray1[23] = HEX_CHARS[byteArray1[11] >> 4];
			charArray1[24] = HEX_CHARS[byteArray1[12] & 0xF];
			charArray1[25] = HEX_CHARS[byteArray1[12] >> 4];
			charArray1[26] = HEX_CHARS[byteArray1[13] & 0xF];
			charArray1[27] = HEX_CHARS[byteArray1[13] >> 4];
			charArray1[28] = HEX_CHARS[byteArray1[14] & 0xF];
			charArray1[29] = HEX_CHARS[byteArray1[14] >> 4];
			charArray1[30] = HEX_CHARS[byteArray1[15] & 0xF];
			charArray1[31] = HEX_CHARS[byteArray1[15] >> 4];
			_id = new string(charArray1);
		}
		Event obj = new Event(new DateTime(ticks, DateTimeKind.Utc), _level, _message, _data, _category, _code, _id);
		try
		{
			Write(obj);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception exception)
		{
			exceptionHandler.Handle(new LoggerException(this, obj, exception));
		}
		if (childCount > 0)
		{
			Monitor.Enter(childMonitor);
			Monitor.Exit(monitor);
			for (int i = 0; i < childCount; i++)
			{
				children[i].LogEvent(_level, _message, _data, _category, _code, _id);
			}
			Monitor.Exit(childMonitor);
		}
		else
		{
			Monitor.Exit(monitor);
		}
	}

	public void Open()
	{
		lock (monitor)
		{
			if (!isClosed)
			{
				return;
			}
			if (eventFormatter == null)
			{
				throw new InvalidOperationException("No event formatter set-- cannot start");
			}
			eventFormatter.Initialize();
			try
			{
				exceptionHandler.Open();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception innerException)
			{
				throw new ApplicationException("Could not open exception handler", innerException);
			}
			int num = bufferSize * 2;
			contextBuffer = new Context[num];
			levelBuffer = new Level[num];
			timestampBuffer = new long[num];
			messageBuffer = new string[num];
			dataBuffer = new object[num];
			categoryBuffer = new string[num];
			codeBuffer = new string[num];
			idBuffer = new string[num];
			contextWriteBuffer = new Context[num];
			levelWriteBuffer = new Level[num];
			timestampWriteBuffer = new long[num];
			messageWriteBuffer = new string[num];
			dataWriteBuffer = new object[num];
			categoryWriteBuffer = new string[num];
			codeWriteBuffer = new string[num];
			idWriteBuffer = new string[num];
			writeBuffer = new Event[num];
			AcquireResources();
			if (isBufferingEnabled)
			{
				flushingManager = new FlushingManager(this, autoFlushInterval);
			}
			isClosed = false;
			lock (childMonitor)
			{
				for (int i = 0; i < childCount; i++)
				{
					children[i].Open();
				}
			}
		}
	}

	public void Close()
	{
		lock (monitor)
		{
			if (isClosed)
			{
				return;
			}
			if (isBufferingEnabled)
			{
				flushingManager.Stop();
			}
			Flush();
			lock (writeBufferMonitor)
			{
			}
			lock (childMonitor)
			{
				for (int i = 0; i < childCount; i++)
				{
					try
					{
						children[i].Close();
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
			try
			{
				ReleaseResources();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		if (isBufferingEnabled)
		{
			while (flushingManager.IsRunning())
			{
				try
				{
					Thread.Sleep(10);
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
		lock (monitor)
		{
			try
			{
				exceptionHandler.Close();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			try
			{
				Deregister();
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
			contextBuffer = null;
			levelBuffer = null;
			timestampBuffer = null;
			messageBuffer = null;
			dataBuffer = null;
			categoryBuffer = null;
			codeBuffer = null;
			idBuffer = null;
			contextWriteBuffer = null;
			levelWriteBuffer = null;
			timestampWriteBuffer = null;
			messageWriteBuffer = null;
			dataWriteBuffer = null;
			categoryWriteBuffer = null;
			codeWriteBuffer = null;
			idWriteBuffer = null;
			isClosed = true;
		}
	}

	public void Flush()
	{
		Monitor.Enter(monitor);
		if (isClosed || bufferCount == 0)
		{
			Monitor.Exit(monitor);
			return;
		}
		Monitor.Enter(writeBufferMonitor);
		if (contextWriteBuffer == null)
		{
			int num = bufferSize * 2;
			tempContextBuffer = new Context[num];
			tempLevelBuffer = new Level[num];
			tempTimestampBuffer = new long[num];
			tempMessageBuffer = new string[num];
			tempDataBuffer = new object[num];
			tempCategoryBuffer = new string[num];
			tempCodeBuffer = new string[num];
			tempIDBuffer = new string[num];
		}
		else
		{
			tempContextBuffer = contextWriteBuffer;
			tempLevelBuffer = levelWriteBuffer;
			tempTimestampBuffer = timestampWriteBuffer;
			tempMessageBuffer = messageWriteBuffer;
			tempDataBuffer = dataWriteBuffer;
			tempCategoryBuffer = categoryWriteBuffer;
			tempCodeBuffer = codeWriteBuffer;
			tempIDBuffer = idWriteBuffer;
		}
		contextWriteBuffer = contextBuffer;
		levelWriteBuffer = levelBuffer;
		timestampWriteBuffer = timestampBuffer;
		messageWriteBuffer = messageBuffer;
		dataWriteBuffer = dataBuffer;
		categoryWriteBuffer = categoryBuffer;
		codeWriteBuffer = codeBuffer;
		idWriteBuffer = idBuffer;
		writeBufferCount = bufferCount;
		contextBuffer = tempContextBuffer;
		levelBuffer = tempLevelBuffer;
		timestampBuffer = tempTimestampBuffer;
		messageBuffer = tempMessageBuffer;
		dataBuffer = tempDataBuffer;
		categoryBuffer = tempCategoryBuffer;
		codeBuffer = tempCodeBuffer;
		idBuffer = tempIDBuffer;
		bufferCount = 0;
		tempContextBuffer = null;
		tempLevelBuffer = null;
		tempTimestampBuffer = null;
		tempMessageBuffer = null;
		tempDataBuffer = null;
		tempCategoryBuffer = null;
		tempCodeBuffer = null;
		tempIDBuffer = null;
		Monitor.Exit(monitor);
		if (writeBuffer == null || writeBuffer.Length < writeBufferCount)
		{
			writeBuffer = new Event[Math.Max(writeBufferCount, bufferSize)];
		}
		int length = writeBufferCount;
		for (int i = 0; i < writeBufferCount; i++)
		{
			Event obj = new Event(new DateTime(timestampWriteBuffer[i], DateTimeKind.Utc), levelWriteBuffer[i], messageWriteBuffer[i], dataWriteBuffer[i], categoryWriteBuffer[i], codeWriteBuffer[i]);
			Context context = contextWriteBuffer[i];
			if (context != null)
			{
				obj.Context = context;
			}
			if (isIDGenerationEnabled)
			{
				string text = idWriteBuffer[i];
				if (text == null)
				{
					random2.NextBytes(byteArray2);
					charArray2[0] = HEX_CHARS[byteArray2[0] & 0xF];
					charArray2[1] = HEX_CHARS[byteArray2[0] >> 4];
					charArray2[2] = HEX_CHARS[byteArray2[1] & 0xF];
					charArray2[3] = HEX_CHARS[byteArray2[1] >> 4];
					charArray2[4] = HEX_CHARS[byteArray2[2] & 0xF];
					charArray2[5] = HEX_CHARS[byteArray2[2] >> 4];
					charArray2[6] = HEX_CHARS[byteArray2[3] & 0xF];
					charArray2[7] = HEX_CHARS[byteArray2[3] >> 4];
					charArray2[8] = HEX_CHARS[byteArray2[4] & 0xF];
					charArray2[9] = HEX_CHARS[byteArray2[4] >> 4];
					charArray2[10] = HEX_CHARS[byteArray2[5] & 0xF];
					charArray2[11] = HEX_CHARS[byteArray2[5] >> 4];
					charArray2[12] = HEX_CHARS[byteArray2[6] & 0xF];
					charArray2[13] = HEX_CHARS[byteArray2[6] >> 4];
					charArray2[14] = HEX_CHARS[byteArray2[7] & 0xF];
					charArray2[15] = HEX_CHARS[byteArray2[7] >> 4];
					charArray2[16] = HEX_CHARS[byteArray2[8] & 0xF];
					charArray2[17] = HEX_CHARS[byteArray2[8] >> 4];
					charArray2[18] = HEX_CHARS[byteArray2[9] & 0xF];
					charArray2[19] = HEX_CHARS[byteArray2[9] >> 4];
					charArray2[20] = HEX_CHARS[byteArray2[10] & 0xF];
					charArray2[21] = HEX_CHARS[byteArray2[10] >> 4];
					charArray2[22] = HEX_CHARS[byteArray2[11] & 0xF];
					charArray2[23] = HEX_CHARS[byteArray2[11] >> 4];
					charArray2[24] = HEX_CHARS[byteArray2[12] & 0xF];
					charArray2[25] = HEX_CHARS[byteArray2[12] >> 4];
					charArray2[26] = HEX_CHARS[byteArray2[13] & 0xF];
					charArray2[27] = HEX_CHARS[byteArray2[13] >> 4];
					charArray2[28] = HEX_CHARS[byteArray2[14] & 0xF];
					charArray2[29] = HEX_CHARS[byteArray2[14] >> 4];
					charArray2[30] = HEX_CHARS[byteArray2[15] & 0xF];
					charArray2[31] = HEX_CHARS[byteArray2[15] >> 4];
					text = new string(charArray2);
				}
				obj.ID = text;
			}
			writeBuffer[i] = obj;
		}
		if (writeBufferCount > 0)
		{
			try
			{
				Write(writeBuffer, writeBufferCount);
			}
			catch (LoggerException exception)
			{
				try
				{
					exceptionHandler.Handle(exception);
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
			catch (Exception exception2)
			{
				exceptionHandler.Handle(LoggerException.CreateBatchException(this, writeBuffer, exception2));
			}
		}
		if (contextWriteBuffer.Length >= Math.Min(bufferSize * 10, maximumBufferSize))
		{
			contextWriteBuffer = null;
			levelWriteBuffer = null;
			timestampWriteBuffer = null;
			messageWriteBuffer = null;
			dataWriteBuffer = null;
			categoryWriteBuffer = null;
			codeWriteBuffer = null;
			idWriteBuffer = null;
		}
		else
		{
			Array.Clear(contextWriteBuffer, 0, length);
			Array.Clear(levelWriteBuffer, 0, length);
			Array.Clear(timestampWriteBuffer, 0, length);
			Array.Clear(messageWriteBuffer, 0, length);
			Array.Clear(dataWriteBuffer, 0, length);
			Array.Clear(categoryWriteBuffer, 0, length);
			Array.Clear(codeWriteBuffer, 0, length);
			Array.Clear(idWriteBuffer, 0, length);
		}
		if (childCount > 0 && writeBufferCount > 0)
		{
			lock (childMonitor)
			{
				Event[] array = new Event[writeBufferCount];
				Array.Copy(writeBuffer, 0, array, 0, writeBufferCount);
				if (writeBuffer.Length > bufferSize * 10)
				{
					writeBuffer = null;
				}
				else
				{
					Array.Clear(writeBuffer, 0, writeBufferCount);
				}
				writeBufferCount = 0;
				Monitor.Exit(writeBufferMonitor);
				for (int j = 0; j < childCount; j++)
				{
					children[j].Log(array, array.Length);
				}
				return;
			}
		}
		if (writeBuffer.Length > bufferSize * 10)
		{
			writeBuffer = null;
		}
		else
		{
			Array.Clear(writeBuffer, 0, writeBufferCount);
		}
		writeBufferCount = 0;
		Monitor.Exit(writeBufferMonitor);
	}

	private void Log(Event[] writeBuffer, int writeBufferCount)
	{
		Event[] array = new Event[writeBufferCount];
		int num = 0;
		Monitor.Enter(monitor);
		if (isIDGenerationEnabled)
		{
			for (int i = 0; i < writeBufferCount; i++)
			{
				Event obj = writeBuffer[i];
				if (obj.Level.Priority >= level.Priority)
				{
					if (obj.ID == null)
					{
						random2.NextBytes(byteArray2);
						charArray2[0] = HEX_CHARS[byteArray2[0] & 0xF];
						charArray2[1] = HEX_CHARS[byteArray2[0] >> 4];
						charArray2[2] = HEX_CHARS[byteArray2[1] & 0xF];
						charArray2[3] = HEX_CHARS[byteArray2[1] >> 4];
						charArray2[4] = HEX_CHARS[byteArray2[2] & 0xF];
						charArray2[5] = HEX_CHARS[byteArray2[2] >> 4];
						charArray2[6] = HEX_CHARS[byteArray2[3] & 0xF];
						charArray2[7] = HEX_CHARS[byteArray2[3] >> 4];
						charArray2[8] = HEX_CHARS[byteArray2[4] & 0xF];
						charArray2[9] = HEX_CHARS[byteArray2[4] >> 4];
						charArray2[10] = HEX_CHARS[byteArray2[5] & 0xF];
						charArray2[11] = HEX_CHARS[byteArray2[5] >> 4];
						charArray2[12] = HEX_CHARS[byteArray2[6] & 0xF];
						charArray2[13] = HEX_CHARS[byteArray2[6] >> 4];
						charArray2[14] = HEX_CHARS[byteArray2[7] & 0xF];
						charArray2[15] = HEX_CHARS[byteArray2[7] >> 4];
						charArray2[16] = HEX_CHARS[byteArray2[8] & 0xF];
						charArray2[17] = HEX_CHARS[byteArray2[8] >> 4];
						charArray2[18] = HEX_CHARS[byteArray2[9] & 0xF];
						charArray2[19] = HEX_CHARS[byteArray2[9] >> 4];
						charArray2[20] = HEX_CHARS[byteArray2[10] & 0xF];
						charArray2[21] = HEX_CHARS[byteArray2[10] >> 4];
						charArray2[22] = HEX_CHARS[byteArray2[11] & 0xF];
						charArray2[23] = HEX_CHARS[byteArray2[11] >> 4];
						charArray2[24] = HEX_CHARS[byteArray2[12] & 0xF];
						charArray2[25] = HEX_CHARS[byteArray2[12] >> 4];
						charArray2[26] = HEX_CHARS[byteArray2[13] & 0xF];
						charArray2[27] = HEX_CHARS[byteArray2[13] >> 4];
						charArray2[28] = HEX_CHARS[byteArray2[14] & 0xF];
						charArray2[29] = HEX_CHARS[byteArray2[14] >> 4];
						charArray2[30] = HEX_CHARS[byteArray2[15] & 0xF];
						charArray2[31] = HEX_CHARS[byteArray2[15] >> 4];
						obj.ID = new string(charArray2);
					}
					array[num++] = obj;
				}
			}
		}
		else
		{
			for (int j = 0; j < writeBufferCount; j++)
			{
				Event obj = writeBuffer[j];
				if (obj.Level.Priority >= level.Priority)
				{
					array[num++] = obj;
				}
			}
		}
		if (num > 0)
		{
			try
			{
				Write(array, num);
			}
			catch (LoggerException exception)
			{
				exceptionHandler.Handle(exception);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception exception2)
			{
				exceptionHandler.Handle(LoggerException.CreateBatchException(this, array, exception2));
			}
			lock (childMonitor)
			{
				Monitor.Exit(monitor);
				for (int k = 0; k < childCount; k++)
				{
					children[k].Log(array, num);
				}
				return;
			}
		}
		Monitor.Exit(monitor);
	}

	protected virtual void AcquireResources()
	{
	}

	protected virtual void ReleaseResources()
	{
	}

	protected abstract void Write(Event _event);

	protected virtual void Write(Event[] events, int length)
	{
		for (int i = 0; i < length; i++)
		{
			try
			{
				Write(events[i]);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception exception)
			{
				exceptionHandler.Handle(new LoggerException(this, events[i], exception));
			}
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && flushingManager != null)
		{
			flushingManager.Dispose();
			flushingManager = null;
		}
	}

	~Logger()
	{
		try
		{
			Close();
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
