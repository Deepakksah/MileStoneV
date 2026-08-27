namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public class H265Vps : H265NalUnit
{
	public static H265Vps FromData(byte[] data)
	{
		return new H265Vps
		{
			Data = data
		};
	}
}
