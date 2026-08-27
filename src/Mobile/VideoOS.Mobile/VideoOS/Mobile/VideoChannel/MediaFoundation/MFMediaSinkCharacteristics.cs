using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[Flags]
public enum MFMediaSinkCharacteristics
{
	None = 0,
	FixedStreams = 1,
	CannotMatchClock = 2,
	Rateless = 4,
	ClockRequired = 8,
	CanPreroll = 0x10,
	RequireReferenceMediaType = 0x20
}
