using System;

namespace VideoOS.Vms.Utilities.Oem;

public interface IAppResources
{
	string CompanyName { get; }

	string ProductName { get; }

	string VendorCode { get; }

	string ProductNameWithoutCompany { get; }

	Guid VendorId { get; }

	bool IsOem { get; }

	string Copyright { get; }

	string ManagementServerServiceName { get; }

	string LogServiceInstallDirectory { get; }

	string Manufacturer { get; }

	string AddRemoveProgramContactInfo { get; }

	string AddRemoveProgramHelpTelephone { get; }

	string AddRemoveProgramHelpUrl { get; }

	string AddRemoveProgramUpdateUrl { get; }

	string StartMenuShortcutAdminInstallationPageDisplayName { get; }

	string StartMenuShortcutInstallationPageDisplayName { get; }

	string StartMenuShortcutReadmeDisplayName { get; }

	string StartMenuShortcutFolderName { get; }

	string StartMenuShortcutGroupFolderName { get; }

	string ManagementServerNameSimple { get; }

	string FailoverManagementServerNameSimple { get; }

	string SystemRequirementsUrl { get; }

	string SupportedDeviceListUrl { get; }

	string ExternalProductName(ExternalProduct externalProduct);

	string ExternalProductInstallDirectory(ExternalProduct externalProduct);

	string LookupResourceString(string key);
}
