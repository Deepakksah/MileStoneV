using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class CustomPropertyNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61028;

	public CustomPropertyNotFoundException()
		: base(VmoResources.VMO61028)
	{
	}
}
