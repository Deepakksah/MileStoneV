using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.Logging;

namespace VideoOS.Common.Integration;

public abstract class StatusManagerBase : IStatusProviderBase
{
	private ILogger logger;

	private Timer cleanupTimer;

	private const int EXPERATIONTIMEMINUTES = 2;

	private const int RUNCLEANMINUTES = 1;

	private volatile bool isRunning;

	private Dictionary<Guid, StatusHandlerBase> currentHandlers;

	protected ILogger Logger
	{
		get
		{
			return logger;
		}
		set
		{
			logger = value;
		}
	}

	protected Dictionary<Guid, StatusHandlerBase> CurrentHandlers
	{
		get
		{
			return currentHandlers;
		}
		set
		{
			currentHandlers = value;
		}
	}

	protected bool IsRunning
	{
		get
		{
			return isRunning;
		}
		set
		{
			isRunning = value;
		}
	}

	public StatusManagerBase(ILogger logger)
	{
		this.logger = logger;
		currentHandlers = new Dictionary<Guid, StatusHandlerBase>();
		cleanupTimer = new Timer(CleanupTimerCallback, null, -1, -1);
	}

	public virtual void Start()
	{
		IsRunning = true;
	}

	public virtual void Stop()
	{
		IsRunning = false;
		if (cleanupTimer != null)
		{
			cleanupTimer.Dispose();
			cleanupTimer = null;
		}
		int count;
		lock (currentHandlers)
		{
			count = currentHandlers.Count;
			foreach (StatusHandlerBase value in currentHandlers.Values)
			{
				value.Dispose();
			}
			currentHandlers.Clear();
		}
		LoggerExtensions.LogInformation(logger, "StatusManager stopped ({HandleCount} searches cancelled)", new object[1] { count });
	}

	public virtual Guid StartStatusSession()
	{
		Guid guid = Guid.NewGuid();
		LoggerExtensions.LogInformation(logger, "StartStatusSession, StatusID = {StatusId}", new object[1] { guid });
		cleanupTimer.Change(0, 60000);
		return guid;
	}

	public virtual void StopStatusSession(Guid StatusSessionId)
	{
		LoggerExtensions.LogInformation(logger, "StopStatusSession, StatusID = {StatusSessionId}", new object[1] { StatusSessionId });
		lock (currentHandlers)
		{
			StatusHandlerBase statusHandlerBase = currentHandlers[StatusSessionId];
			if (statusHandlerBase != null)
			{
				currentHandlers.Remove(StatusSessionId);
				statusHandlerBase.Dispose();
			}
		}
	}

	public StatusBase[] GetStatus(Guid statusSessionId, int timeoutMs)
	{
		StatusHandlerBase statusHandlerBase;
		lock (currentHandlers)
		{
			statusHandlerBase = currentHandlers[statusSessionId];
		}
		if (statusHandlerBase == null)
		{
			throw new StatusException("No StatusHandler found with id: " + statusSessionId.ToString());
		}
		LoggerExtensions.LogDebug(logger, "GetStatus, StatusID {StatusSessionId}= ", new object[1] { statusSessionId });
		return statusHandlerBase.GetStatus(timeoutMs);
	}

	private void CleanupTimerCallback(object dummy)
	{
		try
		{
			if (!isRunning)
			{
				return;
			}
			Dictionary<Guid, StatusHandlerBase> dictionary;
			lock (currentHandlers)
			{
				dictionary = new Dictionary<Guid, StatusHandlerBase>(currentHandlers);
			}
			if (dictionary.Count > 0)
			{
				DateTime dateTime = DateTime.UtcNow - new TimeSpan(0, 2, 0);
				foreach (StatusHandlerBase value in dictionary.Values)
				{
					if (value.LastRequestTime < dateTime)
					{
						LoggerExtensions.LogInformation(logger, "Status expired, SearchID = {SearchId}", new object[1] { value.StatusHandleId });
						StopStatusSession(value.StatusHandleId);
					}
				}
			}
			lock (currentHandlers)
			{
				if (currentHandlers.Count == 0)
				{
					cleanupTimer.Change(-1, -1);
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			LoggerExtensions.LogError(logger, "CleanupTimerCallback error, ex: {Exception}", new object[1] { ex2 });
		}
	}
}
