using System.Drawing;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.Integration.Oem;

public class ManagementFailoverResources : ApplicationResources, IManagementFailoverResources
{
	public string FailoverManagementServerName => GetResourceString("FailoverManagementServerName");

	public string FailoverManagementServerNameSimple => GetResourceString("FailoverManagementServerNameSimple");

	public string FailoverManagementServerNameSimpleWithVersion => GetResourceString("FailoverManagementServerNameSimpleWithVersion");

	public string ProductNameWithoutCompany => GetResourceString("ProductNameWithoutCompany");

	public string CompanyNameShort => GetResourceString("CompanyNameShort");

	public string LogServiceNameSimple => GetResourceString("LogServiceNameSimple");

	public string ManagementServerNameSimple => GetResourceString("ManagementServerNameSimple");

	public string EventServerApplicationNameShort => GetResourceString("EventServerApplicationNameShort");

	public string SupportPortalUrl => GetResourceString("SupportPortalUrl");

	public string FailoverManagementServerConfigurationHelpUrl => GetResourceString("FailoverManagementServerConfigurationHelpUrl");

	public Icon FailoverManagementServerDesktopIcon => GetResourceIcon("FailoverManagementServerIcon");

	public ManagementFailoverResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
