using System;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[Flags]
public enum MFByteStreamCapabilities
{
	None = 0,
	IsReadable = 1,
	IsWritable = 2,
	IsSeekable = 4,
	IsRemote = 8,
	IsDirectory = 0x80,
	HasSlowSeek = 0x100,
	IsPartiallyDownloaded = 0x200
}
