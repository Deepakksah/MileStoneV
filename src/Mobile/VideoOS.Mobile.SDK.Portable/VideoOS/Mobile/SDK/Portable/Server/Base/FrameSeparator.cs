using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Binary;
using VideoOS.Mobile.SDK.Portable.Server.Base.Video;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class FrameSeparator
{
	public static void ProcessBuffer(ByteBuffer buffer, byte[] separator, OnVideoFrame newFrame, bool networkByteOrder, Action<ByteBuffer, uint> onRequireBiggerBuffer = null)
	{
		while (true)
		{
			int num = buffer.FindSeparator(separator);
			if (num < 0)
			{
				buffer.Clear();
				break;
			}
			int num2 = num - buffer.begin;
			buffer.offset += num2;
			buffer.lenWritten -= num2;
			if (buffer.lenWritten < 40)
			{
				break;
			}
			MainHeaderExt tHeader = FrameExtractor.ExtractMainHeader(buffer, networkByteOrder);
			uint num3 = tHeader.HeaderSizeBytes + tHeader.FrameSizeBytes;
			if (num3 > buffer.lenWritten - 16)
			{
				if (num3 > buffer.data.Length - 16)
				{
					onRequireBiggerBuffer?.Invoke(buffer, num3);
				}
				if (num3 <= buffer.data.Length - 16)
				{
					break;
				}
				buffer.MoveOffset(16);
			}
			else if (!buffer.IsSeparator(separator, (int)(buffer.begin + num3)))
			{
				buffer.MoveOffset(16);
			}
			else
			{
				FrameExtractor.ExtractFrame(buffer, newFrame, tHeader, networkByteOrder);
				buffer.MoveOffset((int)num3);
			}
		}
	}
}
