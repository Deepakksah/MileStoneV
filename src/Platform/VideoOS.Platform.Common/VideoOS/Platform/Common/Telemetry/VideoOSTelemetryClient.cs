using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VideoOS.Platform.Telemetry;
using VideoOS.Telemetry;

namespace VideoOS.Platform.Common.Telemetry;

public class VideoOSTelemetryClient : TelemetryClient
{
	private readonly ITelemetryManager _telemetryManager;

	private readonly string _environment;

	public VideoOSTelemetryClient(ITelemetryManager manager, string environment)
	{
		if (manager == null)
		{
			throw new ArgumentNullException("Telemetry Manager not found");
		}
		_telemetryManager = manager;
		_environment = environment;
	}

	public override void TrackAvailability(string name, DateTimeOffset timeStamp, TimeSpan duration, string runLocation, bool success, string message = null)
	{
		IDictionary<string, string> properties = AddEnvironmentToProperties(null);
		_telemetryManager.TrackAvailability(name, timeStamp, duration, runLocation, success, message, properties);
	}

	public override void TrackDependency(string dependencyTypeName, string dependencyName, string data, DateTimeOffset startTime, TimeSpan duration, bool success)
	{
		IDictionary<string, string> properties = AddEnvironmentToProperties(null);
		_telemetryManager.TrackDependency(dependencyTypeName, dependencyName, data, startTime, duration, success, properties);
	}

	public override void TrackEvent(string eventName, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
		IDictionary<string, string> properties2 = AddEnvironmentToProperties(properties);
		_telemetryManager.TrackEvent(eventName, properties2, metrics);
	}

	public override void TrackException(Exception exception, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
		IDictionary<string, string> properties2 = AddEnvironmentToProperties(properties);
		_telemetryManager.TrackException(exception, properties2, metrics);
	}

	public override void TrackMetric(string name, double value, IDictionary<string, string> properties = null)
	{
		IDictionary<string, string> properties2 = AddEnvironmentToProperties(properties);
		_telemetryManager.TrackMetric(name, value, properties2);
	}

	public override void TrackPageView(string name, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
	{
		IDictionary<string, string> properties2 = AddEnvironmentToProperties(properties);
		_telemetryManager.TrackPageView(name, null, null, properties2, metrics);
	}

	public override void TrackPageView(string name, string id, TimeSpan duration)
	{
		IDictionary<string, string> properties = AddEnvironmentToProperties(null);
		_telemetryManager.TrackPageView(name, id, duration, properties);
	}

	public override void TrackRequest(string name, DateTimeOffset startTime, TimeSpan duration, string responseCode, bool success)
	{
		IDictionary<string, string> properties = AddEnvironmentToProperties(null);
		_telemetryManager.TrackRequest(name, startTime, duration, responseCode, success, properties);
	}

	public override void TrackTrace(string message, IDictionary<string, string> properties)
	{
		IDictionary<string, string> properties2 = AddEnvironmentToProperties(properties);
		_telemetryManager.TrackTrace(message, properties2);
	}

	public override void Flush()
	{
		_telemetryManager.Flush();
	}

	public override void FlushWithMaximumWait(int maximumWaitMilliseconds)
	{
		Task.Run(async () => await Task.WhenAny(Task.Run(delegate
		{
			Flush();
		}), Task.Delay(TimeSpan.FromMilliseconds(maximumWaitMilliseconds)))).GetAwaiter().GetResult();
	}

	private IDictionary<string, string> AddEnvironmentToProperties(IDictionary<string, string> properties)
	{
		IDictionary<string, string> dictionary = properties ?? new Dictionary<string, string>();
		if (dictionary.ContainsKey("environmentID"))
		{
			dictionary["environmentID"] = _environment;
			EnvironmentManager.Instance.Log(error: false, GetType().FullName, "environmentID has already been set.");
		}
		else
		{
			dictionary.Add(new KeyValuePair<string, string>("environmentID", _environment));
		}
		return dictionary;
	}
}
