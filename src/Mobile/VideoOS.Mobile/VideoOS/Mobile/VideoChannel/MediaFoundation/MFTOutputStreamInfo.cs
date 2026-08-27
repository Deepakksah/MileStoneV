using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.MediaFoundation;

[StructLayout(LayoutKind.Sequential, Pack = 4)]
public struct MFTOutputStreamInfo
{
	public MFTOutputStreamInfoFlags dwFlags;

	public int cbSize;

	public int cbAlignment;
}
