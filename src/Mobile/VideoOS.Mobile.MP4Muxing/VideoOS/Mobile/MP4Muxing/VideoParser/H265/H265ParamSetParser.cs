using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public class H265ParamSetParser : IH265ParamsSetParser
{
	private readonly byte[] _naluIds = new byte[3] { 32, 33, 34 };

	private H265NaluHeaders _h265NaluHeaders;

	public H265NaluHeaders GetParamSetHeader(IGbdVideoData gbdFrame)
	{
		if (gbdFrame == null || !gbdFrame.IsSyncFrame)
		{
			return _h265NaluHeaders;
		}
		_h265NaluHeaders = GetParamSetHeader(gbdFrame.ByteBuffer);
		return _h265NaluHeaders;
	}

	public H265NaluHeaders GetParamSetHeader(ByteBuffer frameBuffer)
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
		return (byte)((naluStartByte >> 1) & 0x3F);
	}

	public static bool IsDataNalu(byte nalUnitType)
	{
		if ((uint)nalUnitType <= 9u || (uint)(nalUnitType - 16) <= 5u)
		{
			return true;
		}
		return false;
	}

	private H265NaluHeaders ParseAnnexBHeader(ByteBuffer frameBuffer)
	{
		NaluHeaders naluHeaders = ParamsSetParser.ParseAnnexBHeader(frameBuffer, _naluIds, NaluTypeExtractor);
		NaluInfo naluInfo = naluHeaders.Get(32);
		NaluInfo naluInfo2 = naluHeaders.Get(33);
		NaluInfo naluInfo3 = naluHeaders.Get(34);
		if (naluInfo == null || naluInfo2 == null || naluInfo3 == null)
		{
			return null;
		}
		byte[] vps = naluInfo.ExtractData(frameBuffer);
		byte[] sps = naluInfo2.ExtractData(frameBuffer);
		byte[] pps = naluInfo3.ExtractData(frameBuffer);
		return new H265NaluHeaders(vps, sps, pps);
	}
}
