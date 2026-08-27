using VideoOS.Utilities.OemResource;

namespace VideoOS.Vms.Utilities.Oem;

public class MobileServerResources : ApplicationResources
{
	public string MobileServerName => GetResourceString("MobileServerApplicationNameShort");

	public string MobileProductName => GetResourceString("MobileProductName");

	public string MobileClientNameShort => GetResourceString("MobileClientProductNameShort");

	public string WebClientName => GetResourceString("WebClientProductName");

	public string WebClientNameShort => GetResourceString("WebClientProductNameShort");

	public MobileServerResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
