using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.UI;

public class FlowPage
{
	public bool PostInstall { get; set; }

	public IWizardPage Page { get; set; }

	public string Name { get; set; }

	public string Control { get; set; }

	public string NextStep { get; set; }

	public string Event { get; set; }

	public FlowPage(IWizardPage wizardPage, string control, string nextStep, string flowEvent)
	{
		Page = wizardPage;
		Name = wizardPage.PageName;
		Control = control;
		NextStep = nextStep;
		Event = flowEvent;
		PostInstall = false;
	}
}
