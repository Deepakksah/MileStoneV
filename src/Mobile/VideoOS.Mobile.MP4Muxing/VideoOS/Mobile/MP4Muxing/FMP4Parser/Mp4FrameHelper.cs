using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser;

public static class Mp4FrameHelper
{
	public static IList<Tuple<uint, uint>> GetStartOffsets(IGbdVideoData gbdVideoData, Func<byte, byte> naluTypeExtractor, Func<byte, bool> isDataNaluChecker)
	{
		if (gbdVideoData == null)
		{
			return null;
		}
		return GetStartOffsetsTillData(gbdVideoData.ByteBuffer, naluTypeExtractor, isDataNaluChecker);
	}

	public static IList<Tuple<uint, uint>> GetStartOffsetsTillData(ByteBuffer byteBuffer, Func<byte, byte> naluTypeExtractor, Func<byte, bool> isDataNaluChecker)
	{
		byte[] data = byteBuffer.data;
		uint begin = (uint)byteBuffer.begin;
		int lenWritten = byteBuffer.lenWritten;
		List<Tuple<uint, uint>> list = new List<Tuple<uint, uint>>();
		if (GetStartOffset(data, begin) == 0)
		{
			list.Add(new Tuple<uint, uint>(0u, 0u));
		}
		else
		{
			for (uint num = 0u; num < lenWritten; num++)
			{
				uint startOffset = GetStartOffset(data, begin + num);
				if (startOffset != 0)
				{
					GetNalUnitType(data, begin + num + startOffset, naluTypeExtractor);
					list.Add(new Tuple<uint, uint>(num, startOffset));
					num += startOffset;
				}
			}
		}
		return list;
	}

	public static byte GetNalUnitType(byte[] data, uint startIndex, Func<byte, byte> naluTypeExtractor)
	{
		if (data == null || data.Length < startIndex)
		{
			return 0;
		}
		return naluTypeExtractor(data[startIndex]);
	}

	public static IList<Tuple<uint, uint>> GetStartOffsets(ByteBuffer byteBuffer)
	{
		return new List<Tuple<uint, uint>>
		{
			new Tuple<uint, uint>(0u, GetStartOffset(byteBuffer))
		};
	}

	public static uint GetStartOffset(ByteBuffer byteBuffer)
	{
		return GetStartOffset(byteBuffer.data, (uint)byteBuffer.begin);
	}

	public static uint GetStartOffset(byte[] data)
	{
		return GetStartOffset(data, 0u);
	}

	public static uint GetStartOffset(byte[] data, uint startIndex)
	{
		if (data == null || data.Length < startIndex + 4)
		{
			return 0u;
		}
		if (data[startIndex] == 0 && data[startIndex + 1] == 0 && data[startIndex + 2] == 1)
		{
			return 3u;
		}
		if (data[startIndex] == 0 && data[startIndex + 1] == 0 && data[startIndex + 2] == 0 && data[startIndex + 3] == 1)
		{
			return 4u;
		}
		return 0u;
	}
}
