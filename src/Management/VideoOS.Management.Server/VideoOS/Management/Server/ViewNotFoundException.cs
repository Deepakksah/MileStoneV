using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class ViewNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61031;

	public ViewNotFoundException()
		: base(VmoResources.VMO61031)
	{
	}
}
