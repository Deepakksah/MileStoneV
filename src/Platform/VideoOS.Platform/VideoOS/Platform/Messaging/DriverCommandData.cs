namespace VideoOS.Platform.Messaging;

public class DriverCommandData
{
	public object SyncObject { get; set; }

	public string Command { get; set; }

	public string Parameter { get; set; }
}
