using System;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public interface IFmp4FragmentInfo
{
	uint CodecType { get; }

	DateTime FirstFrameTimeUtc { get; }

	TimeSpan WholeSegmentDuration { get; }

	uint TimeScaleInFmp4 { get; }

	uint TrackId { get; }

	Fmp4FragmenterState FragmenterState { get; }

	Mp4MuxerCodecOutputParameters CodecOutputParams { get; }
}
