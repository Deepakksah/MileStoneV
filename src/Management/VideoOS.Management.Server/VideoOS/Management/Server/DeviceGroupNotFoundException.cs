using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class DeviceGroupNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61015;

	public DeviceGroupNotFoundException()
		: base(VmoResources.VMO61015)
	{
	}
}
