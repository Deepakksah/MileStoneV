using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[Flags]
public enum MFTInputStreamInfoFlags
{
	WholeSamples = 1,
	SingleSamplePerBuffer = 2,
	FixedSampleSize = 4,
	HoldsBuffers = 8,
	DoesNotAddRef = 0x100,
	Removable = 0x200,
	Optional = 0x400,
	ProcessesInPlace = 0x800
}
