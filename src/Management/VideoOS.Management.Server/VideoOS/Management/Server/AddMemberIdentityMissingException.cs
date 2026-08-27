using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class AddMemberIdentityMissingException : SecuritySubsystemException
{
	protected override int ErrorCodeDefault => 61001;

	public AddMemberIdentityMissingException()
		: base(VmoResources.VMO61001)
	{
	}
}
