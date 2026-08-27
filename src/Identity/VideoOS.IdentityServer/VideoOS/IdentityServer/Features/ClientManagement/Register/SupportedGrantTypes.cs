using System.Linq;

namespace VideoOS.IdentityServer.Features.ClientManagement.Register;

public static class SupportedGrantTypes
{
	public static readonly string[] Values = new string[3] { "authorization_code", "client_credentials", "refresh_token" };

	public static bool IsSupported(string grantType)
	{
		return Values.Contains(grantType);
	}
}
