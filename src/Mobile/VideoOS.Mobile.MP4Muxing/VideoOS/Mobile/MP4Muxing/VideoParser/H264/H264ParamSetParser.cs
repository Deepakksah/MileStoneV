using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public class H264ParamSetParser : IH264ParamsSetParser
{
	public const byte NaluIdIdr = 5;

	public const byte NaluIdSps = 7;

	public const byte NaluIdPps = 8;

	private readonly byte[] _naluIds = new byte[2] { 7, 8 };

	private H264NaluHeaders _h264NaluHeaders;

	public H264NaluHeaders GetParamSetHeader(IGbdVideoData gbdFrame)
	{
		if (gbdFrame == null || !gbdFrame.IsSyncFrame)
		{
			return _h264NaluHeaders;
		}
		_h264NaluHeaders = GetParamSetHeader(gbdFrame.ByteBuffer);
		return _h264NaluHeaders;
	}

	public H264NaluHeaders GetParamSetHeader(ByteBuffer frameBuffer)
	{
		if (frameBuffer == null || frameBuffer.lenWritten <= 0)
		{
			return null;
		}
		return ParseAnnexBHeader(frameBuffer);
	}

	public static byte NaluTypeExtractor(ByteBuffer byteBuffer, int offset)
	{
		byte result = 0;
		if (byteBuffer != null && byteBuffer.data != null && byteBuffer.lenWritten - offset > 1)
		{
			result = NaluTypeExtractor(byteBuffer.data[byteBuffer.begin + offset]);
		}
		return result;
	}

	public static byte NaluTypeExtractor(byte naluStartByte)
	{
		return (byte)(naluStartByte & 0x1F);
	}

	public static bool IsDataNalu(byte nalUnitType)
	{
		if ((uint)(nalUnitType - 1) <= 4u)
		{
			return true;
		}
		return false;
	}

	private H264NaluHeaders ParseAnnexBHeader(ByteBuffer frameBuffer)
	{
		NaluHeaders naluHeaders = ParamsSetParser.ParseAnnexBHeader(frameBuffer, _naluIds, NaluTypeExtractor);
		NaluInfo naluInfo = naluHeaders.Get(7);
		NaluInfo naluInfo2 = naluHeaders.Get(8);
		if (naluInfo == null || naluInfo2 == null)
		{
			return null;
		}
		byte[] sps = naluInfo.ExtractData(frameBuffer);
		byte[] pps = naluInfo2.ExtractData(frameBuffer);
		return new H264NaluHeaders(sps, pps);
	}
}
