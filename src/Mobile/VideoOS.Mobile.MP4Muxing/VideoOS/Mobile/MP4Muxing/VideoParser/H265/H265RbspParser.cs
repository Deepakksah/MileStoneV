using System;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.VideoParser.H265;

public class H265RbspParser : RbspParser
{
	public H265RbspParser(byte[] data, int startIndex, Func<byte[], byte[]> convertor)
		: base(data, startIndex, convertor)
	{
	}

	public void ReadNalUnit(H265NalUnit nalUnit)
	{
		if (ReadBit() != 0)
		{
			throw new InvalidDataException("forbidden_zero_bit of NAL Unit has value != 0");
		}
		nalUnit.NalUnitType = (byte)ReadBits(6);
		nalUnit.NuhLayerId = (byte)ReadBits(6);
		nalUnit.NuhTempId = (byte)ReadBits(3);
	}

	public H265NalUnit ReadNalUnit()
	{
		H265NalUnit h265NalUnit = new H265NalUnit();
		ReadNalUnit(h265NalUnit);
		return h265NalUnit;
	}
}
