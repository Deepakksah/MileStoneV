namespace VideoOS.Mobile.Portable.VideoChannel;

public interface IVideoData : IVideoDataRaw, IStreamData, IRawData
{
	TranscodedVideoData Transcoded { get; }

	RawPlaybackVideoData RawPlayback { get; }

	RawPlaybackSyncData RawSyncData { get; }
}
