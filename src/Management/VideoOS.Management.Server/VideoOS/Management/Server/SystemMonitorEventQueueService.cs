using System;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class SystemMonitorEventQueueService : IGeminiServerService, ISystemMonitorEventQueueService, IDisposable
{
	private static BulkProcessor<PluginEventInfo> _eventQueue;

	public BulkProcessor<PluginEventInfo> EventQueue => _eventQueue;

	public void StartService(IGeminiRequestContext requestContext)
	{
		if (_eventQueue == null)
		{
			_eventQueue = new BulkProcessor<PluginEventInfo>(SystemMonitorEvent.ProcessQueuedEvents, FailedProcessedEvents, 1000);
		}
	}

	private void FailedProcessedEvents(Exception ex, PluginEventInfo[] failedEvents)
	{
		if (failedEvents != null && failedEvents.Length != 0)
		{
			GeminiTrace.Error(TraceType.General, "Failed to sent {0} system monitor event to server.", failedEvents.Length);
			if (ex is RecordingServerCommunicationException)
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
			}
			else if (ex is ManagementServerServiceUnavailableException)
			{
				GeminiTrace.Error(TraceType.Configuration, "Unable to connect to the Management Server service.");
				GeminiTrace.TraceException(ex);
			}
			GeminiTrace.TraceException(ex);
		}
	}

	public void StopService(IGeminiRequestContext requestContext)
	{
		if (_eventQueue != null)
		{
			_eventQueue.Close();
			_eventQueue = null;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _eventQueue != null)
		{
			_eventQueue.Dispose();
			_eventQueue = null;
		}
	}
}
