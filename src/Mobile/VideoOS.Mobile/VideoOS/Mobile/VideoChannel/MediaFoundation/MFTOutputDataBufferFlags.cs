using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[Flags]
public enum MFTOutputDataBufferFlags
{
	None = 0,
	Incomplete = 0x1000000,
	FormatChange = 0x100,
	StreamEnd = 0x200,
	NoSample = 0x300
}
