namespace VideoOS.Platform.Search.Errors;

public class UnauthorizedDeviceAccessSearchError : SearchError
{
	public string DeviceName { get; private set; }

	public UnauthorizedDeviceAccessSearchError(string deviceName)
	{
		DeviceName = deviceName;
	}
}
