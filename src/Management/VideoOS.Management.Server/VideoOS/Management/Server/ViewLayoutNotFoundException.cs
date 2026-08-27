using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class ViewLayoutNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61030;

	[SqlError(61030)]
	public ViewLayoutNotFoundException()
		: base(VmoResources.VMO61030)
	{
	}
}
