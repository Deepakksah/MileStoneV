namespace VideoOS.ConfigurationAPI;

public class TokenHelper
{
	public virtual string GetToken()
	{
		return "";
	}

	public virtual bool ValidateToken(string token)
	{
		return false;
	}
}
