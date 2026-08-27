namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public class H264PpsRbsp : H264NalUnit
{
	public static H264PpsRbsp FromData(byte[] data)
	{
		return new H264PpsRbsp
		{
			Data = data
		};
	}
}
