namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public class H264SpsRbsp : H264NalUnit
{
	public byte ProfileIdc;

	public byte LevelIdc;

	public byte Constrains;

	public ushort Width;

	public ushort Height;

	public static H264SpsRbsp FromData(byte[] data)
	{
		return H264SpsParser.Parse(data);
	}
}
