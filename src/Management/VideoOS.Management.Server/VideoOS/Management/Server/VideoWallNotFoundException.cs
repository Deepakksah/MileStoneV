using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class VideoWallNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61025;

	public VideoWallNotFoundException()
		: base(VmoResources.VMO61025)
	{
	}
}
