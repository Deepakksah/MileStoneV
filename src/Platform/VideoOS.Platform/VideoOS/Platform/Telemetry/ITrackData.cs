namespace VideoOS.Platform.Telemetry;

public interface ITrackData
{
	void AddProperty(PropertyKeys property, string classFullName);

	void AddDeprecatedProperty();

	void Send(bool useSpamFilter);
}
