namespace VideoOS.Platform.SDK.Platform;

public class LoginFailedInternalMIPException : MIPException
{
	public LoginFailedInternalMIPException(string server)
		: base(server)
	{
	}
}
