using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class FailoverServerNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 60307;

	public FailoverServerNotFoundException()
		: base(VmoResources.VMO60307)
	{
	}
}
