using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 10)]
public struct HeaderExtensionNativePlaybackMotionSeq
{
	public ulong SequenceId;

	public ushort HasMotion;
}
