using System;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 24)]
public struct HeaderExtensionCarouselInfo
{
	public uint HeaderSize;

	public uint Version;

	public Guid ItemID;
}
