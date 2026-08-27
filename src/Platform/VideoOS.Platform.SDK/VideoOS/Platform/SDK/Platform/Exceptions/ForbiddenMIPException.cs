namespace VideoOS.Platform.SDK.Platform.Exceptions;

public class ForbiddenMIPException : MIPException
{
	public ForbiddenMIPException(string message)
		: base(message)
	{
	}
}
