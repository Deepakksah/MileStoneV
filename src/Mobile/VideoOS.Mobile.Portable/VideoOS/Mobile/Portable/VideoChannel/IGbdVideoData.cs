using System;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.Portable.VideoChannel;

public interface IGbdVideoData : IVideoDataRaw, IStreamData, IRawData
{
	DateTime SyncTimeUtc { get; }

	DateTime PicTimeUtc { get; }

	DateTime NextFrameTimeUtc { get; }

	uint Duration { get; }

	bool IsSyncFrame { get; }

	GbdHeader GbdHeader { get; }

	ByteBuffer ByteBuffer { get; }
}
