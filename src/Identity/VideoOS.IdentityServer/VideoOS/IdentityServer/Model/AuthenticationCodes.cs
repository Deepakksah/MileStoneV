using System.Collections.Generic;

namespace VideoOS.IdentityServer.Model;

public static class AuthenticationCodes
{
	public const string LockedOut = "LockedOut";

	public const string InvalidCredentials = "InvalidCredentials";

	public const string ChangePassword = "ChangePassword";

	public static Dictionary<string, object> LockedOutResponse => Response("LockedOut");

	public static Dictionary<string, object> InvalidCredentialsResponse => Response("InvalidCredentials");

	public static Dictionary<string, object> ChangePasswordResponse => Response("ChangePassword");

	private static Dictionary<string, object> Response(string errorCode)
	{
		return new Dictionary<string, object> { { "error_code", errorCode } };
	}
}
