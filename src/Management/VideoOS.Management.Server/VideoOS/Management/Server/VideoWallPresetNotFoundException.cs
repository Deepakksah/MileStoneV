using System;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class VideoWallPresetNotFoundException : AdministrationSubsystemException
{
	protected override int ErrorCodeDefault => 61027;

	public VideoWallPresetNotFoundException()
		: base(VmoResources.VMO61027)
	{
	}
}
