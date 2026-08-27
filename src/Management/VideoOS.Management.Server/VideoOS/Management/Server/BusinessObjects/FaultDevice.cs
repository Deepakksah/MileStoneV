namespace VideoOS.Management.Server.BusinessObjects;

public class FaultDevice
{
	public string DeviceId { get; set; }

	public string Message { get; set; }

	public int ErrorCode { get; set; }

	public FaultSource Source { get; set; }

	public FaultDevice(string deviceId, string message, int code, FaultSource source)
	{
		DeviceId = deviceId;
		Message = message;
		ErrorCode = code;
		Source = source;
	}
}
