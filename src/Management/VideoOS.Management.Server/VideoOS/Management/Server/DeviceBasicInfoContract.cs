using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public class DeviceBasicInfoContract
{
	public string Id { get; set; }

	public string Name { get; set; }

	public DeviceType DeviceType { get; set; }

	public string HardwareId { get; set; }
}
