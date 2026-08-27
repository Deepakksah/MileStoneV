namespace VideoOS.Platform.Util;

public static class JpegUtil
{
	public static bool FindWidthHeight(byte[] data, out int width, out int height)
	{
		width = 0;
		height = 0;
		int num = 0;
		if (data != null && data.Length > 10 && data[num] == byte.MaxValue && data[num + 1] == 216 && data[num + 2] == byte.MaxValue && data[num + 3] == 224)
		{
			num += 4;
			if (data[num + 2] == 74 && data[num + 3] == 70 && data[num + 4] == 73 && data[num + 5] == 70 && data[num + 6] == 0)
			{
				int num2 = data[num] * 256 + data[num + 1];
				while (num < data.Length)
				{
					num += num2;
					if (num + 8 >= data.Length)
					{
						return false;
					}
					if (data[num] != byte.MaxValue)
					{
						return false;
					}
					if (data[num + 1] == 192)
					{
						height = data[num + 5] * 256 + data[num + 6];
						width = data[num + 7] * 256 + data[num + 8];
						return true;
					}
					num += 2;
					num2 = data[num] * 256 + data[num + 1];
				}
			}
		}
		return false;
	}
}
