namespace VideoOS.Platform.Search.Errors;

public class DeviceSearchError : SearchError
{
	public string DeviceName { get; private set; }

	public DeviceSearchError(string deviceName)
	{
		DeviceName = deviceName;
	}
}
