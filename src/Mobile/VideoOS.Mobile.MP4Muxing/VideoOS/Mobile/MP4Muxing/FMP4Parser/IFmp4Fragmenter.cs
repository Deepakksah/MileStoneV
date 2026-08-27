using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public interface IFmp4Fragmenter
{
	byte[] CreateFragment(RootBox rootBox, IReadOnlyList<IGbdVideoData> listFrames, IFmp4FragmentInfo fmp4FragmentInfo);
}
