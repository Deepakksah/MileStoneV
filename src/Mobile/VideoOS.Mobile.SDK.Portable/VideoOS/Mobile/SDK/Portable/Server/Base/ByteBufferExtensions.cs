using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public static class ByteBufferExtensions
{
	public const int GuidLenBytes = 16;

	public static int FindSeparator(this ByteBuffer Buffer, byte[] VideoId)
	{
		int result = -1;
		for (int i = Buffer.offset; i < Buffer.end - 16; i++)
		{
			if (Buffer.IsSeparator(VideoId, i))
			{
				result = i;
				break;
			}
		}
		return result;
	}

	public static bool IsSeparator(this ByteBuffer Buffer, byte[] VideoId, int CheckOffset)
	{
		bool result = false;
		if (CheckOffset < Buffer.data.Length - 16)
		{
			result = Buffer.data[CheckOffset] == VideoId[0] && Buffer.data[CheckOffset + 1] == VideoId[1] && Buffer.data[CheckOffset + 2] == VideoId[2] && Buffer.data[CheckOffset + 3] == VideoId[3] && Buffer.data[CheckOffset + 4] == VideoId[4] && Buffer.data[CheckOffset + 5] == VideoId[5] && Buffer.data[CheckOffset + 6] == VideoId[6] && Buffer.data[CheckOffset + 7] == VideoId[7] && Buffer.data[CheckOffset + 8] == VideoId[8] && Buffer.data[CheckOffset + 9] == VideoId[9] && Buffer.data[CheckOffset + 10] == VideoId[10] && Buffer.data[CheckOffset + 11] == VideoId[11] && Buffer.data[CheckOffset + 12] == VideoId[12] && Buffer.data[CheckOffset + 13] == VideoId[13] && Buffer.data[CheckOffset + 14] == VideoId[14] && Buffer.data[CheckOffset + 15] == VideoId[15];
		}
		return result;
	}
}
