using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class MonitorNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61026;

	public MonitorNotFoundException()
		: base(VmoResources.VMO61026)
	{
	}
}
