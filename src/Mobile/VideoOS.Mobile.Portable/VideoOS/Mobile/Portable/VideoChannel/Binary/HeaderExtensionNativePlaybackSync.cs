using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct HeaderExtensionNativePlaybackSync
{
	public ulong PrevousSyncTimeStamp;

	public ulong NextSyncTimeStamp;
}
