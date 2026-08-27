namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public class H265Pps : H265NalUnit
{
	public static H265Pps FromData(byte[] data)
	{
		return new H265Pps
		{
			Data = data
		};
	}
}
