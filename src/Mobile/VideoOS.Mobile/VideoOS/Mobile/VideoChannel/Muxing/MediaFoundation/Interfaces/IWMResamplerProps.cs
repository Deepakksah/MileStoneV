using System.Runtime.InteropServices;

namespace VideoOS.Mobile.VideoChannel.Muxing.MediaFoundation.Interfaces;

[Guid("E7E9984F-F09F-4da4-903F-6E2E0EFE56B5")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IWMResamplerProps
{
	int SetHalfFilterLength(int outputQuality);

	int SetUserChannelMtx([In] float[] channelConversionMatrix);
}
