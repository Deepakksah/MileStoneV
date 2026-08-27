namespace VideoOS.Platform.Messaging;

public class DriverResponseData
{
	public object SyncObject { get; set; }

	public string Response { get; set; }

	public bool Success { get; set; }

	public string ErrorText { get; set; }
}
