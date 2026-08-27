namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public class H264NaluHeaders
{
	public H264SpsRbsp Sps { get; set; }

	public H264PpsRbsp Pps { get; set; }

	public H264NaluHeaders(byte[] sps, byte[] pps)
	{
		Sps = H264SpsParser.Parse(sps);
		Pps = new H264PpsRbsp
		{
			Data = pps
		};
	}
}
