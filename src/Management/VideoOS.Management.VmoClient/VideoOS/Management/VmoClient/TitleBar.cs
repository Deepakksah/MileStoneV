using System;

namespace VideoOS.Management.VmoClient;

public enum TitleBar
{
	None,
	Caption,
	[Obsolete("Ability to hide live indicator was removed in 10.0")]
	CaptionAndLive
}
