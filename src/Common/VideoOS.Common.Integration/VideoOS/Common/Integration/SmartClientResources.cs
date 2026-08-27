using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.Integration;

public class SmartClientResources : ApplicationResources
{
	public override string Version => string.Empty;

	public string BrandName => GetResourceString("BrandName");

	public string ClientName => GetResourceString("ClientName");

	public string ClientPlayerName => GetResourceString("ClientPlayerName");

	public string Company => GetResourceString("Company");

	public SmartClientResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
