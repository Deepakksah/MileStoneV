using System.Drawing;

namespace VideoOS.Common.Integration.Oem;

public interface IManagementFailoverResources
{
	string FailoverManagementServerName { get; }

	string FailoverManagementServerNameSimple { get; }

	string FailoverManagementServerNameSimpleWithVersion { get; }

	string ProductNameWithoutCompany { get; }

	string LogServiceNameSimple { get; }

	string ManagementServerNameSimple { get; }

	string EventServerApplicationNameShort { get; }

	string CompanyNameShort { get; }

	string SupportPortalUrl { get; }

	string FailoverManagementServerConfigurationHelpUrl { get; }

	Icon FailoverManagementServerDesktopIcon { get; }
}
