namespace VideoOS;

public static class BooleanExtensions
{
	public static string SafeToLowerString(this bool source)
	{
		if (!source)
		{
			return "false";
		}
		return "true";
	}

	public static string SafeToUpperString(this bool source)
	{
		if (!source)
		{
			return "FALSE";
		}
		return "TRUE";
	}
}
