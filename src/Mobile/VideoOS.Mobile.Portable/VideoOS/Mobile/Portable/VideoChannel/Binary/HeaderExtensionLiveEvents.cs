using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 8)]
public struct HeaderExtensionLiveEvents
{
	public uint CurrentFlags;

	public uint ChangedFlags;
}
