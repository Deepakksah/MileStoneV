using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class RemoveMemberIdentityMissingException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 61002;

	public RemoveMemberIdentityMissingException()
		: base(VmoResources.VMO61002)
	{
	}
}
