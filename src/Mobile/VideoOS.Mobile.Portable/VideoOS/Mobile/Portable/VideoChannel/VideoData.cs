namespace VideoOS.Mobile.Portable.VideoChannel;

public class VideoData : VideoDataRaw, IVideoData, IVideoDataRaw, IStreamData, IRawData
{
	public TranscodedVideoData Transcoded { get; set; }

	public RawPlaybackVideoData RawPlayback { get; set; }

	public RawPlaybackSyncData RawSyncData { get; set; }

	public int SourceHeight { get; set; }

	public int SourceWidth { get; set; }

	public bool ContainsKeyFrame { get; set; }
}
