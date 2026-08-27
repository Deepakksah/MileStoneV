namespace VideoOS.Mobile.Portable.VideoChannel;

public class VideoDataRaw : StreamData, IVideoDataRaw, IStreamData, IRawData
{
	public RawVideoData RawVideo { get; set; }
}
