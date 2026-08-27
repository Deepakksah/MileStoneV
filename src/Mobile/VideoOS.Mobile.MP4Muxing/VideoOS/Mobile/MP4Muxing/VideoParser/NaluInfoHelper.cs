using System;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.MP4Muxing.VideoParser;

public static class NaluInfoHelper
{
	public static byte[] ExtractData(this NaluInfo naluInfo, ByteBuffer frameBuffer)
	{
		byte[] array = new byte[naluInfo.StrippedCodeLength];
		Buffer.BlockCopy(frameBuffer.data, frameBuffer.begin + naluInfo.StrippedCodeStart, array, 0, naluInfo.StrippedCodeLength);
		return array;
	}
}
