using System.Collections.Generic;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public interface IMp4FrameGroup
{
	ulong BaseMediaDecodeTime { get; }

	IReadOnlyList<IMp4Frame> Frames { get; }
}
