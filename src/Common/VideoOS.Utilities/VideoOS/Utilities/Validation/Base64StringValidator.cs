namespace VideoOS.Utilities.Validation;

public class Base64StringValidator
{
	public unsafe static bool IsValidBase64String(string base64String)
	{
		if (base64String == null || base64String.Length % 4 != 0)
		{
			return false;
		}
		fixed (char* ptr = base64String)
		{
			int num = 0;
			int num2 = base64String.Length - 1;
			while (num2 >= 0 && ptr[num2] == '=')
			{
				num2--;
				if (++num == 3)
				{
					break;
				}
			}
			while (num2 >= 0)
			{
				int num3 = ptr[num2];
				if ((num3 < 65 || num3 > 90) && (num3 < 97 || num3 > 122) && (num3 < 48 || num3 > 57) && num3 != 43 && num3 != 47)
				{
					return false;
				}
				num2--;
			}
			return true;
		}
	}
}
