using System.Collections.Generic;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ModifiedInstallationFinishPageViewModel : InstallationFinishPageViewModel
{
	public ModifiedInstallationFinishPageViewModel(IInstallerManager installerManager)
		: base(singleComputerInstall: false, installerManager)
	{
		SetProperties();
	}

	public ModifiedInstallationFinishPageViewModel(IEnumerable<string> installedComponents, IInstallerManager installerManager)
		: base(installedComponents, singleComputerInstall: false, installerManager)
	{
		SetProperties();
	}

	private void SetProperties()
	{
		base.Header = Resources.FinishInstallationPageModifyHeader;
		base.HeaderInfo = Resources.FinishInstallationPageModifyHeaderInfo;
		base.DownloadManagerLabel = Resources.FinishInstallationPageOpenDownloadManager;
		base.SummaryVisibility = true;
		base.WebMobileClientVisibility = false;
		base.OpenClient = true;
		base.OpenClientVisibility = true;
		base.OpenDownloadManagerVisibility = true;
	}
}
