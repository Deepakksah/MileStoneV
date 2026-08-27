using System;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 40)]
public struct MainHeaderExt
{
	public Guid VideoID;

	public ulong TimeStampUtcMs;

	public uint FrameCount;

	public uint FrameSizeBytes;

	public ushort HeaderVersion;

	public ushort HeaderExtensionFlags;

	public uint HeaderSizeBytes;
}
