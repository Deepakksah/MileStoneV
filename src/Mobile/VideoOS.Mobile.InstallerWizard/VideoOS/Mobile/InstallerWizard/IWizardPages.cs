using VideoOS.Installer.UI.Framework;

namespace VideoOS.Mobile.InstallerWizard;

public interface IWizardPages
{
	WizardPageBase PageControl { get; }

	WizardPageBase PageUninstall { get; }

	WizardPageBase PageServiceAccount { get; }

	WizardPageBase PageVmsAccount { get; }

	WizardPageBase PageKeyManagementPassword { get; }
}
