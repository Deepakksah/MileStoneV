using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public sealed class ServerLogReceiver : ILogReceiver
{
	private BundleMessageProcessor<LogEntry> logWriter;

	private Thread _logQueueMgrThread;

	private object _logQueueMgrWaitObject = new object();

	private TimeSpan _logQueueMgrSleepTime = new TimeSpan(0, 0, 10);

	private volatile bool _logQueueMgrStopped;

	private DateTime _logQueueMgrLastSysLogEntry = DateTime.MinValue;

	private long _logQueueMgrNumberOfExceptions;

	private IReadOnlyDictionary<LogType, LogDefinition> _logDefinitions;

	private readonly int _numberOfLogsToRead;

	public ServerLogReceiver(IReadOnlyDictionary<LogType, LogDefinition> logDefinitions, int numberOfLogsToRead)
	{
		_logDefinitions = logDefinitions;
		_numberOfLogsToRead = numberOfLogsToRead;
		_logQueueMgrThread = new Thread(LogQueueManagerThread);
		_logQueueMgrThread.IsBackground = true;
		_logQueueMgrThread.Name = "Log queue manager Thread";
		_logQueueMgrThread.Start();
	}

	private void LogQueueManagerThread()
	{
		LogDataQueueManager logDataQueueManager = null;
		try
		{
			while (true)
			{
				bool flag = false;
				try
				{
					if (Server.Instance.SqlConnectionManager != null)
					{
						foreach (LogDefinition value in _logDefinitions.Values)
						{
							if (!_logQueueMgrStopped)
							{
								using (logDataQueueManager = new LogDataQueueManager(_numberOfLogsToRead))
								{
									flag |= logDataQueueManager.PopQueueAndSendToLogSvc(value.LogType);
								}
								continue;
							}
							break;
						}
					}
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception ex2)
				{
					if (_logQueueMgrLastSysLogEntry.AddMinutes(6.0) < DateTime.UtcNow)
					{
						Server.Instance.Logger.LogWarning("Exception caught in work loop in LogQueueManagerThread(): {Exception}", ex2);
						Server.Instance.Logger.LogWarning("Above message repeated {NumberOfExceptions} times.", _logQueueMgrNumberOfExceptions);
						_logQueueMgrNumberOfExceptions = 0L;
						_logQueueMgrLastSysLogEntry = DateTime.UtcNow;
					}
					_logQueueMgrNumberOfExceptions++;
				}
				lock (_logQueueMgrWaitObject)
				{
					if (_logQueueMgrStopped)
					{
						break;
					}
					if (!flag)
					{
						Monitor.Wait(_logQueueMgrWaitObject, _logQueueMgrSleepTime);
						if (_logQueueMgrStopped)
						{
							break;
						}
					}
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			Server.Instance.Logger.LogWarning("Exception caught in outer loop in LogQueueManagerThread(): {Exception}", ex4);
		}
	}

	public void Initialize()
	{
		LogDataAccessor.Initialize();
		BundleMessageProcessor<LogEntry>.ProcessDelegate processDelegate = WriteToDatabase;
		FileMessagePersister<LogEntry> messagePersister = new FileMessagePersister<LogEntry>(ServerConfig.Instance.LogConfiguration.CacheFile);
		logWriter = new BundleMessageProcessor<LogEntry>(processDelegate, messagePersister);
		logWriter.MaxMessagesPerCall = ServerConfig.Instance.LogConfiguration.BundleMaxCount;
		logWriter.MillisecondsBetweenCalls = Convert.ToInt32(ServerConfig.Instance.LogConfiguration.BundleInterval.TotalMilliseconds);
		logWriter.Start(enable: true);
	}

	public void Close()
	{
		lock (_logQueueMgrWaitObject)
		{
			_logQueueMgrStopped = true;
			Monitor.Pulse(_logQueueMgrWaitObject);
		}
		logWriter.Stop();
		_logQueueMgrThread.Join();
		LogDataAccessor.Close();
	}

	private static bool WriteToDatabase(LogEntry[] logEntries)
	{
		if (logEntries == null)
		{
			return true;
		}
		List<LogEntry> list = new List<LogEntry>();
		for (int i = 0; i < logEntries.Length; i++)
		{
			if (ServerLogManager.ShouldEntryBeLogged(logEntries[i]) && (!LogEntryDefinitions.IsExtendedLog((LogEntryId)logEntries[i].LogId) || ServerConfig.Instance.LogConfiguration.Logs[LogType.AuditLog].ExtendedUserLogging))
			{
				list.Add(logEntries[i]);
			}
		}
		if (list.Count == 0)
		{
			return true;
		}
		try
		{
			if (logEntries.Length == 1)
			{
				LogDataAccessor.CreateLogEntry(list[0]);
			}
			else
			{
				LogDataAccessor.CreateLogEntries(list.ToArray());
			}
			return true;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogError("Exception caught in ServerLogReceiver.WriteToDatabase(): {Exception}", ex2);
			return false;
		}
	}

	public void AddLogEntry(LogEntry logEntry)
	{
		if (logEntry != null)
		{
			AddLogEntry(logEntry, skipQueue: false);
		}
	}

	public void AddLogEntry(LogEntry logEntry, bool skipQueue)
	{
		if (logEntry != null)
		{
			if (skipQueue)
			{
				WriteToDatabase(new LogEntry[1] { logEntry });
			}
			else
			{
				logWriter.QueueMessage(logEntry);
			}
		}
	}

	public void AddLogEntries(LogEntry[] logEntry)
	{
		AddLogEntries(logEntry, skipQueue: false);
	}

	public void AddLogEntries(LogEntry[] logEntry, bool skipQueue)
	{
		if (logEntry != null)
		{
			if (skipQueue)
			{
				WriteToDatabase(logEntry);
			}
			else
			{
				logWriter.QueueMessages(logEntry);
			}
		}
	}
}
