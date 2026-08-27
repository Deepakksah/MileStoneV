using System.Collections.Generic;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public interface IMp4Muxer : IControl
{
	byte[] Mux(IReadOnlyList<GbdVideoData> h264Input, Fmp4FragmentInfo fmp4FragmentInfo);
}
