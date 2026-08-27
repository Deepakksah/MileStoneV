namespace VideoOS.Platform.UI.Automation.Serialization;

public class DecorationResult
{
	public bool WasObjectCreated { get; }

	public object ObjectValue { get; }

	public object HostObject { get; }

	public DecorationResult(bool wasObjectCreated, object objectValue = null, object hostObject = null)
	{
		WasObjectCreated = wasObjectCreated;
		ObjectValue = objectValue;
		HostObject = hostObject;
	}
}
