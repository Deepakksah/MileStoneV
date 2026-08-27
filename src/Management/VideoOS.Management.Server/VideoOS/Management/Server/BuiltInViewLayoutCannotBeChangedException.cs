using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class BuiltInViewLayoutCannotBeChangedException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61035;

	[SqlError(61035)]
	public BuiltInViewLayoutCannotBeChangedException()
		: base(VmoResources.VMO61035)
	{
	}
}
