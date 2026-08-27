using Microsoft.AspNetCore.Http;

namespace VideoOS.IdentityServer.Helper;

public static class SessionExtensions
{
	public const string ExpireSession = "ExpireSession";

	public const string CancelSession = "CancelSession";

	public static void SetSessionIntValue(this ISession session, string key, int value)
	{
		session.SetInt32(key, value);
	}

	public static bool GetSessionBoolValue(this ISession session, string key)
	{
		return session.GetInt32(key) == 1;
	}

	public static void DeleteSessionValue(this ISession session, string key)
	{
		session.Remove(key);
	}
}
