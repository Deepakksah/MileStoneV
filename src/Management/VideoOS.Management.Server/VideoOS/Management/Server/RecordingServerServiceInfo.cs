namespace VideoOS.Management.Server;

public sealed class RecordingServerServiceInfo
{
	public enum ServiceServerType
	{
		Recorder,
		Failover
	}

	public string Id { get; set; }

	public ServiceServerType ServerType { get; set; }

	public string ServerId { get; set; }
}
