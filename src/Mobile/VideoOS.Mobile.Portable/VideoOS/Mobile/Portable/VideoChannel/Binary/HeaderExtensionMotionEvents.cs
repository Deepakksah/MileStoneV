using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct HeaderExtensionMotionEvents
{
	public uint MotionDataHeaderSize;

	public uint MotionAmmount;
}
