namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public class H265Sps : H265NalUnit
{
	public byte VideoParamSetId;

	public byte MaxNumberSubLayers;

	public byte TempIdNestingFlag;

	public ProfileTierLevel ProfileTierLevel = new ProfileTierLevel();

	public byte SpsId;

	public byte ChromaFormatIdc;

	public ushort Width;

	public ushort Height;

	public byte BitDepthLuma;

	public byte BitDepthChroma;

	public static H265Sps FromData(byte[] data)
	{
		return H265SpsParser.Parse(data);
	}
}
