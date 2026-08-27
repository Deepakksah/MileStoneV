namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public class H265NaluHeaders
{
	public H265Vps Vps { get; set; }

	public H265Sps Sps { get; set; }

	public H265Pps Pps { get; set; }

	public H265NaluHeaders(byte[] vps, byte[] sps, byte[] pps)
	{
		Vps = H265Vps.FromData(vps);
		Sps = H265Sps.FromData(sps);
		Pps = H265Pps.FromData(pps);
	}
}
