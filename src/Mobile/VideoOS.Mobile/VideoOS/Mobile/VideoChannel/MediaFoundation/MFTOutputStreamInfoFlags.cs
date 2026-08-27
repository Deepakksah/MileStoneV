using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[Flags]
public enum MFTOutputStreamInfoFlags
{
	None = 0,
	WholeSamples = 1,
	SingleSamplePerBuffer = 2,
	FixedSampleSize = 4,
	Discardable = 8,
	Optional = 0x10,
	ProvidesSamples = 0x100,
	CanProvideSamples = 0x200,
	LazyRead = 0x400,
	Removable = 0x800
}
