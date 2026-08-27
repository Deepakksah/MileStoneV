namespace VideoOS.Mobile.Portable.VideoChannel;

public interface IVideoDataRaw : IStreamData, IRawData
{
	RawVideoData RawVideo { get; }
}
