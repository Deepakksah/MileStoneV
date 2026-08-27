using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveMemberRoleMissingException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 61003;

	public RemoveMemberRoleMissingException()
		: base(VmoResources.VMO61003)
	{
	}
}
