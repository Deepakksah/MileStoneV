using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct HeaderExtensionDynamicInfo
{
	public uint HeaderSize;

	public uint Version;

	public uint DataCount;

	public uint DataType;
}
