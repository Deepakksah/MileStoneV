using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Telemetry;

public class TelemetryClient
{
	public virtual void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null)
	{
	}

	public virtual void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
	{
	}

	public virtual void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
	}

	public virtual void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
	}

	public virtual void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
	{
	}

	public virtual void TrackPageView(string name, string id, TimeSpan duration)
	{
	}

	public virtual void TrackPageView(string name, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
	}

	public virtual void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
	{
	}

	public virtual void TrackTrace(string message, IDictionary<string, string> properties)
	{
	}

	public virtual void Flush()
	{
	}

	public virtual void FlushWithMaximumWait(int maximumWaitMilliseconds)
	{
	}
}
