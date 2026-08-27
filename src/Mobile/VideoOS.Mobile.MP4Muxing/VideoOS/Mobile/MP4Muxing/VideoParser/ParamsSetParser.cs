using System;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.MP4Muxing.VideoParser;

public static class ParamsSetParser
{
	public static bool MatchStartCode(ByteBuffer frameBuffer, int position, byte naluType, out int startCodeLen, Func<ByteBuffer, int, byte> naluTypeExtractor = null)
	{
		startCodeLen = 0;
		if (frameBuffer == null || frameBuffer.lenWritten < position + 4 || position < 0)
		{
			return false;
		}
		int num = position;
		if (frameBuffer.data[frameBuffer.begin + num] != 0)
		{
			return false;
		}
		num++;
		if (frameBuffer.data[frameBuffer.begin + num] != 0)
		{
			return false;
		}
		num++;
		if (frameBuffer.data[frameBuffer.begin + num] == 0)
		{
			num++;
		}
		if (frameBuffer.data[frameBuffer.begin + num] != 1)
		{
			return false;
		}
		if (naluTypeExtractor == null || naluType == naluTypeExtractor(frameBuffer, num + 1))
		{
			startCodeLen = num - position + 1;
			return true;
		}
		return false;
	}

	public static bool FindNextZero(ByteBuffer frameBuffer, ref int position)
	{
		if (frameBuffer == null || frameBuffer.lenWritten < position || position < 0)
		{
			return false;
		}
		while (position <= frameBuffer.lenWritten)
		{
			if (frameBuffer.data[frameBuffer.begin + position] == 0)
			{
				return true;
			}
			position++;
		}
		return false;
	}

	public static NaluInfo GetSequenceHeaderInfo(ByteBuffer frameBuffer, ref int position, byte naluCode, Func<ByteBuffer, int, byte> naluTypeExtractor)
	{
		int startCodeLen;
		while (!MatchStartCode(frameBuffer, position, naluCode, out startCodeLen, naluTypeExtractor))
		{
			position++;
			if (!FindNextZero(frameBuffer, ref position))
			{
				return null;
			}
		}
		int start = position;
		position += startCodeLen;
		int startCodeLen2;
		while (!MatchStartCode(frameBuffer, position, 0, out startCodeLen2))
		{
			position++;
			if (!FindNextZero(frameBuffer, ref position))
			{
				return null;
			}
		}
		return new NaluInfo(naluCode, start, position, startCodeLen);
	}

	public static NaluHeaders ParseAnnexBHeader(ByteBuffer frameBuffer, byte[] naluTypes, Func<ByteBuffer, int, byte> naluTypeExtractor)
	{
		int position = 0;
		NaluHeaders naluHeaders = new NaluHeaders();
		for (int i = 0; i < naluTypes.Length; i++)
		{
			NaluInfo sequenceHeaderInfo = GetSequenceHeaderInfo(frameBuffer, ref position, naluTypes[i], naluTypeExtractor);
			if (sequenceHeaderInfo == null)
			{
				return null;
			}
			naluHeaders.Add(sequenceHeaderInfo);
		}
		return naluHeaders;
	}
}
