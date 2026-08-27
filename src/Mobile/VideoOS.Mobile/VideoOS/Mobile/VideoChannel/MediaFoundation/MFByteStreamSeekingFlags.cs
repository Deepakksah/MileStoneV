using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[Flags]
public enum MFByteStreamSeekingFlags
{
	None = 0,
	CancelPendingIO = 1
}
