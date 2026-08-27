namespace VideoOS.SCH.Base;

public class IdentityTokenUtil
{
	public static string MaskToken(string token)
	{
		if (string.IsNullOrEmpty(token))
		{
			return "{NullOrEmpty}";
		}
		if (token.Length >= 10)
		{
			return token.Substring(0, 10) + "******";
		}
		return token;
	}
}
