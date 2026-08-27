namespace VideoOS.IdentityServer.Services;

public class LoginProviderRestrictions : ILoginProviderRestrictions
{
	public bool AllowOnlyOneProvider => true;
}
