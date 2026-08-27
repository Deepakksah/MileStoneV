using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using VideoOS.Installer.DatabasePlugin;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

[SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Created through reflection")]
public sealed class WizardPageProvider : IWizardPageProvider, IDisposable
{
	private List<IWizardPage> _pages = new List<IWizardPage>
	{
		new EnterpriseSlaveWarningPageControl(),
		new PrivacySettingsWizard(),
		new IISSetupCorporatePage(),
		new WizardPageConfirmBackup(),
		new DatabaseValidationControl(),
		new WizardPageDatabaseInstallType(),
		new WizardPageDatabaseExistingType()
	};

	public IWizardPage CreateInstance(string name)
	{
		return _pages.FirstOrDefault((IWizardPage p) => string.Compare(name, p.PageName, StringComparison.Ordinal) == 0);
	}

	public void Dispose()
	{
		_pages.ForEach(delegate(IWizardPage p)
		{
			(p as IDisposable)?.Dispose();
		});
		_pages.Clear();
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Must implement the interface correctly")]
	public List<IWizardPage> GetWizardPages()
	{
		return _pages;
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Must implement the interface correctly")]
	public List<IWizardPage> GetWizardPagesUninstallation()
	{
		return null;
	}
}
