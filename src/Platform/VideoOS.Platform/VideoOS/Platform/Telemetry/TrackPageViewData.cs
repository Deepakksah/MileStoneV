using System;

namespace VideoOS.Platform.Telemetry;

public class TrackPageViewData : TrackData
{
	public TrackPageViewData(Type classType, bool deprecated = false)
		: base($"{classType.Name} Initialized")
	{
		AddProperty(PropertyKeys.Identifier, classType.FullName);
		if (deprecated)
		{
			AddDeprecatedProperty();
		}
	}

	public TrackPageViewData(Type classType, bool deprecated, string subIdentifier)
		: base(classType.Name)
	{
		AddProperty(PropertyKeys.Identifier, classType.FullName);
		if (deprecated)
		{
			AddDeprecatedProperty();
		}
		if (!string.IsNullOrEmpty(subIdentifier))
		{
			AddProperty(PropertyKeys.SubIdentifier, subIdentifier);
		}
	}

	public override void Send(bool useSpamFilter = false)
	{
		if ((!useSpamFilter || !FilterOutTelemetry(_name)) && ClientControl.Instance.TelemetryClient != null)
		{
			ClientControl.Instance.TelemetryClient.TrackPageView(_name, (_properties.Count > 0) ? _properties : null);
		}
	}
}
