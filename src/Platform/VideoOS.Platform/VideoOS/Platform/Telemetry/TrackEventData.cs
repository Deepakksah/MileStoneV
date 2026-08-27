namespace VideoOS.Platform.Telemetry;

public class TrackEventData : TrackData
{
	public TrackEventData(string messageId, bool deprecated = false, string subIdentifier = null)
		: base($"{messageId} Event handled")
	{
		AddProperty(PropertyKeys.Identifier, messageId);
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
		if (!useSpamFilter || !FilterOutTelemetry(_name))
		{
			ClientControl.Instance.TelemetryClient.TrackEvent(_name, (_properties.Count > 0) ? _properties : null);
		}
	}
}
