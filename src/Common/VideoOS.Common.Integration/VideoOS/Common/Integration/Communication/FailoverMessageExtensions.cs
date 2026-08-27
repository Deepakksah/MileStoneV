namespace VideoOS.Common.Integration.Communication;

public static class FailoverMessageExtensions
{
	public static bool IsAuthorisationOrSllError(this FailoverMessage message)
	{
		if (message != FailoverMessage.AUTHORIZATION_ERROR_ALERT)
		{
			return message == FailoverMessage.SSL_HANDSHAKE_ERROR_ALERT;
		}
		return true;
	}
}
