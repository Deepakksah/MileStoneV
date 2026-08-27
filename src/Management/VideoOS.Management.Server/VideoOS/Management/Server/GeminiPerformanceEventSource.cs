using System;
using System.Diagnostics.Tracing;

namespace VideoOS.Management.Server;

public class GeminiPerformanceEventSource : EventSource
{
	public static GeminiPerformanceEventSource Log = new GeminiPerformanceEventSource();

	public void MethodStart(Guid uniqueidentifier, Guid hostId, string methodName)
	{
		WriteEvent(1, uniqueidentifier, hostId, methodName);
	}

	public void MethodStop(Guid uniqueidentifier, Guid hostId, string methodName, long duration)
	{
		WriteEvent(2, uniqueidentifier, hostId, methodName, duration);
	}
}
