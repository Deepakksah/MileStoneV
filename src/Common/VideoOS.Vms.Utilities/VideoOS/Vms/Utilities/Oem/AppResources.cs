using System;
using VideoOS.Utilities.OemResource;

namespace VideoOS.Vms.Utilities.Oem;

public class AppResources : ApplicationResources, IAppResources
{
	public string CompanyName => GetResourceString("CompanyName");

	public string ProductName => GetResourceString("ProductName");

	public string MilestoneFreeLicenseSkuName => GetResourceString("MilestoneFreeLicenseSkuName");

	public string ManagerDownloadFreeLicenseUrl => GetResourceString("ManagerDownloadFreeLicenseUrl");

	public string VendorCode => GetResourceString("VendorCode");

	public bool IsOem => !string.Equals(GetResourceString("IsOemed"), bool.FalseString, StringComparison.OrdinalIgnoreCase);

	public string ProductNameWithoutCompany => GetResourceString("ProductNameWithoutCompany");

	public Guid VendorId => new Guid(GetResourceString("VendorId"));

	public string Copyright => GetResourceString("Copyright");

	public string ManagementServerServiceName => GetResourceString("ManagementServerServiceName");

	public string LogServiceInstallDirectory => GetResourceString("LogServiceInstallDirectory");

	public string LogServerServerInstallDir => GetResourceString("LogServerServerInstallDir");

	public string Manufacturer => GetResourceString("Manufacturer");

	public string AddRemoveProgramContactInfo => GetResourceString("AddRemoveProgramContactInfo");

	public string AddRemoveProgramHelpTelephone => GetResourceString("AddRemoveProgramHelpTelephone");

	public string AddRemoveProgramHelpUrl => GetResourceString("AddRemoveProgramHelpUrl");

	public string AddRemoveProgramUpdateUrl => GetResourceString("AddRemoveProgramUpdateUrl");

	public string RecorderSecurityKnowledgebaseUrl => GetResourceString("RecorderSecurityKnowledgebaseUrl");

	public string StartMenuShortcutAdminInstallationPageDisplayName => GetResourceString("InstallationPageAdminStartMenuShortcutDisplayName");

	public string StartMenuShortcutInstallationPageDisplayName => GetResourceString("InstallationPageStartMenuShortcutDisplayName");

	public string StartMenuShortcutReadmeDisplayName => GetResourceString("StartMenuShortcutReadmeDisplayName");

	public string StartMenuShortcutFolderName => GetResourceString("StartMenuShortcutFolderName");

	public string StartMenuShortcutGroupFolderName => GetResourceString("StartMenuShortcutGroupFolderName");

	public string ManagementServerNameSimple => GetResourceString("ManagementServerNameSimple");

	public string FailoverManagementServerNameSimple => GetResourceString("FailoverManagementServerNameSimple");

	public string SystemRequirementsUrl => GetResourceString("SystemRequirementsUrl");

	public string SupportedDeviceListUrl
	{
		get
		{
			string value = string.Empty;
			TryGetResourceString("SupportedDeviceListUrl", out value);
			return value;
		}
	}

	public AppResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}

	public string ExternalProductName(ExternalProduct externalProduct)
	{
		return externalProduct switch
		{
			ExternalProduct.EventServer => GetResourceString("EventServerApplicationName"), 
			ExternalProduct.LogServer => GetResourceString("LogServerApplicationName"), 
			_ => string.Empty, 
		};
	}

	public string ExternalProductInstallDirectory(ExternalProduct externalProduct)
	{
		return externalProduct switch
		{
			ExternalProduct.EventServer => GetResourceString("EventServerInstallDir"), 
			ExternalProduct.LogServer => GetResourceString("LogServerServerInstallDir"), 
			_ => string.Empty, 
		};
	}

	public string LookupResourceString(string key)
	{
		string value = null;
		TryGetResourceString(key, out value);
		return value;
	}
}
