namespace VideoOS.Mobile.Portable.Utilities;

public class ArrayHelper
{
	public static int Compare(byte[] b1, byte[] b2)
	{
		if (b1 == b2)
		{
			return 0;
		}
		if (b1 == null)
		{
			return -1;
		}
		if (b2 == null)
		{
			return 1;
		}
		int num = b1.Length - b2.Length;
		if (num != 0)
		{
			return num;
		}
		int num2 = b1.Length;
		for (int i = 0; i < num2; i++)
		{
			num = b1[i] - b2[i];
			if (num != 0)
			{
				return num;
			}
		}
		return 0;
	}
}
