using System;
using VideoOS.Common.Integration;

namespace VideoOS.Management.Server;

public class ForceUpgrade
{
	public ForceType Type { get; set; }

	public SCVersion Version { get; set; }

	public ForceUpgrade(string type)
	{
		if (StringComparer.OrdinalIgnoreCase.Equals("major", type))
		{
			Type = ForceType.Below;
		}
		else if (StringComparer.OrdinalIgnoreCase.Equals("forcespecific", type))
		{
			Type = ForceType.Specific;
		}
	}
}
