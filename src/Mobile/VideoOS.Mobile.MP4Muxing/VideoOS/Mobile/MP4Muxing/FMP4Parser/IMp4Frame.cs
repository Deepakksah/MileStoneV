using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public interface IMp4Frame
{
	IEnumerable<GenericMultiDataBoxData> Datas { get; }

	TrackFragmentRunBoxSample TrunSample { get; }

	uint FrameSize { get; }
}
