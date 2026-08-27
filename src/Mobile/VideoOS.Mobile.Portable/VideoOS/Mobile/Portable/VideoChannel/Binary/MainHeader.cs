using System;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 36)]
public struct MainHeader
{
	public Guid VideoID;

	public ulong TimeStampUtcMs;

	public uint FrameCount;

	public uint FrameSizeBytes;

	public ushort HeaderSizeBytes;

	public ushort HeaderExtensionFlags;
}
