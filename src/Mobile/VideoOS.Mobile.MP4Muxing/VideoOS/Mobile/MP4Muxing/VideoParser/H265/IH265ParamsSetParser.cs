using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public interface IH265ParamsSetParser
{
	H265NaluHeaders GetParamSetHeader(IGbdVideoData gbdFrame);
}
