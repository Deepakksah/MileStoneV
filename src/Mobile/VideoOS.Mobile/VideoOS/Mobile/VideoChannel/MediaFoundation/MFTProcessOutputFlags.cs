using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[Flags]
public enum MFTProcessOutputFlags
{
	None = 0,
	DiscardWhenNoBuffer = 1
}
