using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public interface IH264ParamsSetParser
{
	H264NaluHeaders GetParamSetHeader(IGbdVideoData gbdFrame);
}
