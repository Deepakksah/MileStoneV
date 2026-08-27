using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 52)]
public struct HeaderExtensionStreamInfo
{
	public uint HeaderSize;

	public uint Version;

	public uint Reserved;

	public uint Fields;

	public uint TimeBetweenFrames;

	public uint DataType;

	public uint Flags;

	public uint Profile;

	public uint Level;

	public uint Compatibility;

	public ulong Constrains;

	public uint FramesCount;
}
