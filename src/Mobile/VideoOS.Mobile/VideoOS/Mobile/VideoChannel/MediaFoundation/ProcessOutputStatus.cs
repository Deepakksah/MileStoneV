using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[Flags]
public enum ProcessOutputStatus
{
	None = 0,
	NewStreams = 0x100
}
