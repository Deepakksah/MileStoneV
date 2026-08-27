using System.Collections.Generic;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.UI.Framework;
using VideoOS.Mobile.InstallerWizard.KeyManagement;

namespace VideoOS.Mobile.InstallerWizard;

public class WizardPageProvider : IWizardPageProvider, IWizardPages
{
	private readonly WizardPageControl _pageControl = new WizardPageControl();

	private readonly WizardPageUninstall _pageUninstall = new WizardPageUninstall();

	private readonly WizardPageServiceAccount _pageServiceAccount = new WizardPageServiceAccount();

	private readonly WizardPageVmsAccount _pageVmsAccount = new WizardPageVmsAccount();

	private readonly KeyManagementPasswordWizardPage _pageKeyManagementPassword = new KeyManagementPasswordWizardPage();

	public WizardPageBase PageControl => _pageControl;

	public WizardPageBase PageUninstall => _pageUninstall;

	public WizardPageBase PageServiceAccount => _pageServiceAccount;

	public WizardPageBase PageVmsAccount => _pageVmsAccount;

	public WizardPageBase PageKeyManagementPassword => _pageKeyManagementPassword;

	public WizardPageProvider()
	{
		_pageControl.WizardPagesParent = this;
	}

	public List<IWizardPage> GetWizardPages()
	{
		return new List<IWizardPage> { _pageControl, _pageServiceAccount, _pageVmsAccount, _pageKeyManagementPassword };
	}

	public IWizardPage CreateInstance(string name)
	{
		return name switch
		{
			"ServerPage" => _pageControl, 
			"UninstallPage" => _pageUninstall, 
			"AccountPage" => _pageServiceAccount, 
			"VmsAccountPage" => _pageVmsAccount, 
			"MobileServerKeyManagementPasswordPage" => _pageKeyManagementPassword, 
			_ => null, 
		};
	}

	public List<IWizardPage> GetWizardPagesUninstallation()
	{
		return new List<IWizardPage> { _pageUninstall };
	}

	public void Release()
	{
	}
}
