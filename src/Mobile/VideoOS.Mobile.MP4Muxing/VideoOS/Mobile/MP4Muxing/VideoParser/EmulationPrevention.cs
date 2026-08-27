namespace VideoOS.Mobile.MP4Muxing.VideoParser;

public static class EmulationPrevention
{
	public static byte[] Remove(byte[] input)
	{
		if (input == null || input.Length < 2)
		{
			return null;
		}
		byte[] array = new byte[input.Length];
		int num = 0;
		int num2 = 0;
		while (num < input.Length - 2)
		{
			if (input[num] == 0 && input[num + 1] == 0 && input[num + 2] == 3)
			{
				array[num2++] = input[num++];
				array[num2] = input[num];
				num++;
			}
			else
			{
				array[num2] = input[num];
			}
			num++;
			num2++;
		}
		array[num2++] = input[num++];
		array[num2] = input[num];
		return array;
	}
}
