using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Telemetry;

public abstract class TrackData : ITrackData
{
	protected readonly Dictionary<string, string> _properties = new Dictionary<string, string>();

	protected readonly string _name;

	protected static Dictionary<string, DateTime> _sentTelemetry = new Dictionary<string, DateTime>();

	protected static TimeSpan _filterTimeout = TimeSpan.FromMinutes(2.0);

	protected TrackData(string name)
	{
		_name = name;
	}

	public virtual void AddProperty(PropertyKeys property, string classFullName)
	{
		_properties.Add(property.ToString(), classFullName);
	}

	public virtual void AddDeprecatedProperty()
	{
		_properties.Add("Deprecated", bool.TrueString);
	}

	public abstract void Send(bool useSpamFilter = false);

	protected bool FilterOutTelemetry(string name)
	{
		if (!_sentTelemetry.ContainsKey(name))
		{
			_sentTelemetry.Add(name, DateTime.UtcNow);
			return false;
		}
		if (_sentTelemetry[name].Add(_filterTimeout) > DateTime.UtcNow)
		{
			return true;
		}
		_sentTelemetry[name] = DateTime.UtcNow;
		return false;
	}
}
