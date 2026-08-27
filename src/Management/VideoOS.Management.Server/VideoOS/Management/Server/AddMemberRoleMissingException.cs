using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class AddMemberRoleMissingException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 61000;

	public AddMemberRoleMissingException()
		: base(VmoResources.VMO61000)
	{
	}
}
