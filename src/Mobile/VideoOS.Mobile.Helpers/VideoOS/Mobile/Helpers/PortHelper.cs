namespace VideoOS.Mobile.Helpers;

public static class PortHelper
{
	public static bool IsPortValid(int value)
	{
		if (value > 1024 && value < 65535)
		{
			return true;
		}
		return value switch
		{
			80 => true, 
			443 => true, 
			_ => false, 
		};
	}

	public static bool IsPortValid(string value)
	{
		if (int.TryParse(value, out var result))
		{
			return IsPortValid(result);
		}
		return false;
	}
}
