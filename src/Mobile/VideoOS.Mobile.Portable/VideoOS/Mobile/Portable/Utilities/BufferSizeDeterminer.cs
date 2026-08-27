namespace VideoOS.Mobile.Portable.Utilities;

public class BufferSizeDeterminer
{
	private static readonly int[] Threshold = new int[4] { 131072, 524288, 1048576, 5242880 };

	public static int GetBufferSize(long contentLen)
	{
		long num = contentLen;
		int[] threshold = Threshold;
		foreach (int num2 in threshold)
		{
			if (num2 > contentLen)
			{
				break;
			}
			num = num2;
		}
		return (int)num;
	}
}
