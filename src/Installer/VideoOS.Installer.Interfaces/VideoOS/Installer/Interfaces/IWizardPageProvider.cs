using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Interfaces;

[CLSCompliant(true)]
public interface IWizardPageProvider
{
	List<IWizardPage> GetWizardPages();

	IWizardPage CreateInstance(string name);

	List<IWizardPage> GetWizardPagesUninstallation();
}
