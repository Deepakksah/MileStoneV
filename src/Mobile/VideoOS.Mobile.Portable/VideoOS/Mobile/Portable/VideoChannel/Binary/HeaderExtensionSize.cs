using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 48)]
public struct HeaderExtensionSize
{
	public int SrcStrideX;

	public int SrcStrideY;

	public int SrcLeft;

	public int SrcTop;

	public int SrcRight;

	public int SrcBottom;

	public int DestStrideX;

	public int DestStrideY;

	public uint ResamplingTag;

	public int Reserved0;

	public int Reserved1;

	public int Reserved2;
}
