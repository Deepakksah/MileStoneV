namespace VideoOS.Management.Server.BusinessObjects;

public class WarningDevice
{
	public string DeviceId { get; set; }

	public string Message { get; set; }

	public int ErrorCode { get; set; }

	public WarningDevice(string deviceId, string message, int code)
	{
		DeviceId = deviceId;
		Message = message;
		ErrorCode = code;
	}
}
