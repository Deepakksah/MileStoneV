using System;

namespace VideoOS.Management.Server;

public class RestrictedMediaLiveDataContract
{
	public string DeviceId { get; set; }

	public string UserName { get; set; }

	public DateTime StartTime { get; set; }

	public DateTime LastModified { get; set; }
}
