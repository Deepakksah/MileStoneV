using System.Collections.Generic;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.RecorderWarningPlugin;

public class RecorderWarningPage : IWizardPageProvider
{
	private RecorderWarningPageControl _page = new RecorderWarningPageControl();

	public IWizardPage CreateInstance(string name)
	{
		if (name == "RecorderWarningPage")
		{
			return _page;
		}
		return null;
	}

	public List<IWizardPage> GetWizardPages()
	{
		return new List<IWizardPage> { _page };
	}

	public List<IWizardPage> GetWizardPagesUninstallation()
	{
		return null;
	}
}
