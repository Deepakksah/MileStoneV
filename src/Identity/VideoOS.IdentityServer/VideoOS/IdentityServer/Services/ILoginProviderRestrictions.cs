namespace VideoOS.IdentityServer.Services;

public interface ILoginProviderRestrictions
{
	bool AllowOnlyOneProvider { get; }
}
