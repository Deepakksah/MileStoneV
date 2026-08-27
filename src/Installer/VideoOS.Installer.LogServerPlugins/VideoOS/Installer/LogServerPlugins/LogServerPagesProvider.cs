using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Installer.DatabasePlugin;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.LogServerPlugins;

public sealed class LogServerPagesProvider : IWizardPageProvider, IDisposable
{
	private List<IWizardPage> _pages = new List<IWizardPage> { (IWizardPage)new DatabaseValidationControl() };

	public List<IWizardPage> GetWizardPages()
	{
		return _pages;
	}

	public IWizardPage CreateInstance(string name)
	{
		return _pages.FirstOrDefault((IWizardPage p) => string.Equals(name, p.PageName, StringComparison.Ordinal));
	}

	public void Dispose()
	{
		_pages.ForEach(delegate(IWizardPage p)
		{
			(p as IDisposable)?.Dispose();
		});
		_pages.Clear();
	}

	public List<IWizardPage> GetWizardPagesUninstallation()
	{
		return new List<IWizardPage>();
	}
}
