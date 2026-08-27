using System;

namespace VideoOS.Management.Server;

public class RestrictedMediaDataContract
{
	public string Id { get; set; }

	public string[] DeviceIds { get; set; }

	public string Header { get; set; }

	public string Description { get; set; }

	public string UserName { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime EndTime { get; set; }

	public DateTime LastModified { get; set; }
}
