using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[StructLayout(LayoutKind.Sequential, Pack = 8)]
public struct MFTInputStreamInfo
{
	public long hnsMaxLatency;

	public MFTInputStreamInfoFlags dwFlags;

	public int cbSize;

	public int cbMaxLookahead;

	public int cbAlignment;
}
