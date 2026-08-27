using System;
using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public interface IMp4MuxerCodecImpl
{
	uint CodecType { get; }

	SampleEntry CreateVisualSampleEntry(IGbdVideoData gbdVideoData, Mp4MuxerCodecOutputParameters ouputParams);

	IList<Tuple<uint, uint>> GetStartOffsets(IGbdVideoData gbdVideoData);
}
