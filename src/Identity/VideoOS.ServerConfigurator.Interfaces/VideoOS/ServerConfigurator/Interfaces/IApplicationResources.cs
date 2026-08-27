using System.Drawing;

namespace VideoOS.ServerConfigurator.Interfaces;

public interface IApplicationResources
{
	string CompanyName { get; }

	string ProductNameWithoutCompany { get; }

	string ServerConfiguratorInstallFolder { get; }

	string ServerConfiguratorApplicationName { get; }

	Icon ServerConfiguratorIcon { get; }

	string CertificateGuideUri { get; }

	string RegisterTabLearnMoreLink { get; }
}
