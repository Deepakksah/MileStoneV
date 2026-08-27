using System;

namespace VideoOS.Management.Common;

public class ClientSetting
{
	public bool Live { get; set; }

	public bool Playback { get; set; }

	public bool Setup { get; set; }

	public Guid RoleId { get; private set; }

	public ClientSetting(Guid roleId)
	{
		RoleId = roleId;
	}
}
