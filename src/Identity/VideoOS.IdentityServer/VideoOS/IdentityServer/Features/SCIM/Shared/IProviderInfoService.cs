namespace VideoOS.IdentityServer.Features.SCIM.Shared;

public interface IProviderInfoService
{
	(string ProviderId, string ProviderDisplayName) GetProviderInfo();
}
