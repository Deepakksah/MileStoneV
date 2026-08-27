using System;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public class Fmp4FragmentInfo : IFmp4FragmentInfo
{
	public uint CodecType { get; set; }

	public DateTime FirstFrameTimeUtc { get; set; } = DateTime.MinValue;

	public TimeSpan WholeSegmentDuration { get; set; }

	public uint TimeScaleInFmp4 { get; set; }

	public uint TrackId { get; set; } = 1u;

	public Fmp4FragmenterState FragmenterState { get; set; } = new Fmp4FragmenterState();

	public Mp4MuxerCodecOutputParameters CodecOutputParams { get; set; } = new Mp4MuxerCodecOutputParameters();
}
