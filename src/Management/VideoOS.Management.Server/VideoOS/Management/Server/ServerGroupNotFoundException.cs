using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class ServerGroupNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61012;

	public ServerGroupNotFoundException()
		: base(VmoResources.VMO61012)
	{
	}
}
