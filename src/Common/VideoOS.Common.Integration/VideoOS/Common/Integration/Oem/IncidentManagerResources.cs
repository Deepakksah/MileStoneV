using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.Integration.Oem;

public class IncidentManagerResources : ApplicationResources
{
	public string IncidentManagerApplicationName => GetResourceString("IncidentManagerApplicationName");

	public IncidentManagerResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
