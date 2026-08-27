using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct HeaderExtensionPlaybackInfo
{
	public ulong RequestedTimeStampUtcMs;

	public int Reserved0;

	public int Reserved1;
}
