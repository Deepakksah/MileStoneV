using System;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.IM.Common.Service.ExternalAgents;
using VideoOS.IM.Server.Logger;
using VideoOS.IM.Server.Service.Licenses.DTOs;
using VideoOS.IM.Server.Service.WindowsRegistries;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Service.Licenses.Providers;

public sealed class LicenseProvider : ILicenseProvider
{
	[NotNull]
	private IWinRegistryService _winRegistryService;

	[NotNull]
	private IWebApiClient _webApiClient;

	private const string ManagementServerAddress = "ManagementServerAddress";

	public LicenseProvider(IWinRegistryService winRegistryService, IWebApiClient webApiClient)
	{
		winRegistryService.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\Licenses\\Providers\\LicenseProvider.cs", 22);
		webApiClient.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Service\\Licenses\\Providers\\LicenseProvider.cs", 23);
		_winRegistryService = winRegistryService;
		_webApiClient = webApiClient;
	}

	public async Task<LicenseInformationResponse> GetLicenseAsync()
	{
		Uri msBaseUri = new Uri(_winRegistryService.ReadRegistryValue("ManagementServerAddress", string.Empty));
		SitesResponse sitesResponse = await _webApiClient.ExecuteGetRequestAsync<SitesResponse>(msBaseUri, "managementServer/rest/sites").ConfigureAwait(continueOnCapturedContext: false);
		if (sitesResponse.Sites.Count == 0)
		{
			throw new InvalidOperationException($"Call to '{msBaseUri}managementServer/rest/sites' did not return any site.");
		}
		Site site = sitesResponse.Sites.First();
		LicenseInformationResponse obj = await _webApiClient.ExecuteGetRequestAsync<LicenseInformationResponse>(msBaseUri, $"managementServer/rest/licenseInformations/{site.Id}/licenseInstalledProducts").ConfigureAwait(continueOnCapturedContext: false);
		if (obj.LicenseInstalledProducts.Count == 0)
		{
			throw new InvalidOperationException($"Call to '{msBaseUri}managementServer/rest/licenseInformations/{site.Id}/licenseInstalledProducts' did not return any license Installed Product.");
		}
		return obj;
	}
}
