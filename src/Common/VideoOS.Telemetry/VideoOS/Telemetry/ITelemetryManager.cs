using System;
using System.Collections.Generic;

namespace VideoOS.Telemetry;

public interface ITelemetryManager
{
	void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null, IDictionary<string, string> properties = null);

	void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success, IDictionary<string, string> properties = null);

	void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

	void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

	void TrackMetric(string name, double value, IDictionary<string, string> properties = null);

	void TrackPageView(string name, string id, TimeSpan duration);

	void TrackPageView(string name, string id = null, TimeSpan? duration = null, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null);

	void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success, IDictionary<string, string> properties = null);

	void TrackTrace(string message, IDictionary<string, string> properties);

	void Flush();
}
