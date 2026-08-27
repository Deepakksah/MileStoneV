using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.Manager;

public class Plugin
{
	public IWizardPageProvider WizardProvider { get; set; }

	public InstallerPluginTypeFlowStep[] PageFlow { get; set; }

	public InstallerPluginTypeWizardPage[] WizardPages { get; set; }
}
