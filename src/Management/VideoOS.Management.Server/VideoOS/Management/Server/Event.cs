using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using VideoOS.Common.Integration.Threading;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

public class Event
{
	private const string CONFIG_KEY_MAX_THREADS = "VideoOS.Management.Server.Event.HighPriorityMaxThreads";

	private const int DEFAULT_MAX_THREADS = 10;

	private static SmartThreadPool _eventProcessingThreadPoolThrottle;

	private static SmartThreadPool _eventProcessingHighPriorityThreadPoolThrottle;

	static Event()
	{
		_eventProcessingThreadPoolThrottle = new SmartThreadPool(new SmartThreadPoolStartInfo
		{
			MinWorkerThreads = 0,
			MaxWorkerThreads = 3,
			ThreadPriority = ThreadPriority.BelowNormal
		})
		{
			Name = "Management.Server.Event (Low priority)"
		};
		SmartThreadPoolStartInfo smartThreadPoolStartInfo = new SmartThreadPoolStartInfo
		{
			MinWorkerThreads = 0,
			MaxWorkerThreads = 10
		};
		try
		{
			string appSetting = Config.Instance.GetAppSetting("VideoOS.Management.Server.Event.HighPriorityMaxThreads", "VideoOS.Management.Server.Event.HighPriorityMaxThreads");
			if (!string.IsNullOrWhiteSpace(appSetting) && int.TryParse(appSetting, out var result))
			{
				smartThreadPoolStartInfo.MaxWorkerThreads = result;
				GeminiTrace.Info(FormattableString.Invariant(FormattableStringFactory.Create("{0} set to {1}", "VideoOS.Management.Server.Event.HighPriorityMaxThreads", result)));
			}
		}
		catch (Exception ex)
		{
			GeminiTrace.Warning(FormattableString.Invariant(FormattableStringFactory.Create("Error setting {0}, using default value of {1}.{2}{3}", "VideoOS.Management.Server.Event.HighPriorityMaxThreads", 10, Environment.NewLine, ex.StackTrace)));
		}
		smartThreadPoolStartInfo.ThreadPriority = ThreadPriority.Normal;
		_eventProcessingHighPriorityThreadPoolThrottle = new SmartThreadPool(smartThreadPoolStartInfo)
		{
			Name = "Management.Server.Event (Normal priority)"
		};
	}

	public void FireAsyncEventNoPermissionCheck(IGeminiRequestContext requestContext, string eventTypeName, string eventObjectId, EventAction action)
	{
		FireAsyncEventNoPermissionCheck(requestContext, eventTypeName, eventObjectId, action, null);
	}

	public void FireAsyncEventNoPermissionCheck(IGeminiRequestContext requestContext, string eventTypeName, string eventObjectId, EventAction action, Dictionary<string, string> data, bool isHighPriority = false)
	{
		try
		{
			GeminiTrace.Verbose("Processing asynchronous event of the following type: '{0}'.", eventTypeName);
			EventObject eventObject = CreateEventObject(requestContext, eventTypeName, eventObjectId, action, data);
			SaveEvent(eventObject);
			Notify(eventObject, isHighPriority);
		}
		catch (Exception)
		{
			throw;
		}
	}

	public static EventObject CreateEventObject(IGeminiRequestContext requestContext, string eventTypeName, string eventObjectId, EventAction action, Dictionary<string, string> data)
	{
		return new EventObject(requestContext, eventTypeName, eventObjectId, action, data);
	}

	private static int SaveEvent(EventObject eventObject)
	{
		return EventObject.Save();
	}

	private void Notify(EventObject eventObject, bool isHighPriority)
	{
		if (isHighPriority)
		{
			_eventProcessingHighPriorityThreadPoolThrottle.QueueWorkItem((WorkItemCallback)DoNotification, (object)eventObject);
		}
		else
		{
			_eventProcessingThreadPoolThrottle.QueueWorkItem((WorkItemCallback)DoNotification, (object)eventObject);
		}
	}

	private object DoNotification(object theEvent)
	{
		EventObject eventObject = (EventObject)theEvent;
		try
		{
			NotificationManager.Notify(eventObject);
		}
		catch (Exception ex)
		{
			if (ex is VmoServiceException)
			{
				GeminiApplicationCore.LogException(eventObject.RequestContext, VmoResources.VMO63013, ex);
			}
			else
			{
				GeminiApplicationCore.LogException(eventObject.RequestContext, VmoResources.VMO63013, ex, GeminiEventId.UnhandledExceptionOccurred, EventLogEntryType.Error);
			}
		}
		return null;
	}
}
