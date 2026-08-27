using System;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H264;

public class H264RbspParser : RbspParser
{
	public H264RbspParser(byte[] data, int startIndex, Func<byte[], byte[]> convertor)
		: base(data, startIndex, convertor)
	{
	}

	public void ReadNalUnit(H264NalUnit nalUnit)
	{
		if (ReadBit() != 0)
		{
			throw new InvalidDataException("forbidden_zero_bit of NAL Unit has value != 0");
		}
		nalUnit.NalRefIdc = (byte)ReadBits(2);
		nalUnit.NalUnitType = (byte)ReadBits(5);
	}

	public H264NalUnit ReadNalUnit()
	{
		H264NalUnit h264NalUnit = new H264NalUnit();
		ReadNalUnit(h264NalUnit);
		return h264NalUnit;
	}
}
