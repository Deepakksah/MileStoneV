using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.Installer.Common;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class InstallationFinishPageViewModel : FinishPageViewModel
{
	public InstallationFinishPageViewModel(bool singleComputerInstall, IInstallerManager installerManager)
		: base(new InstallationSummaryControlViewModel())
	{
		SetProperties(singleComputerInstall, installerManager);
	}

	public InstallationFinishPageViewModel(IEnumerable<string> installedComponents, bool singleComputerInstall, IInstallerManager installerManager)
		: base(new InstallationSummaryControlViewModel(installedComponents))
	{
		SetProperties(singleComputerInstall, installerManager);
	}

	private void SetProperties(bool singleComputerInstall, IInstallerManager installerManager)
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			base.Header = Resources.FinishInstallationPageHeader_HardwareManufacturerMode;
			base.PrepareFinalizeLabel = Resources.FinishInstallationPage_PrepareFinalize;
			base.PrepareFinalizeLabelExplanation = Resources.FinishInstallationPage_PrepareFinalizeExplanation;
			base.PrepareFinalizeVisibility = true;
			base.PrepareFinalize = true;
			base.RemoveInstallLogLabel = Resources.FinishInstallationPage_RemoveInstallLog;
			base.RemoveInstallLogVisibility = true;
			base.RemoveInstallLog = true;
		}
		else
		{
			base.Header = Resources.FinishInstallationPageHeader;
		}
		base.SummaryVisibility = true;
		base.OpenDownloadManagerVisibility = false;
		if (singleComputerInstall)
		{
			base.HeaderInfo = string.Format(CultureInfo.InvariantCulture, Resources.FinishInstallationFirstInstallPageHeaderInfo, OemResources.Instance.Manager.ApplicationNameShort);
			base.OpenClientVisibility = false;
			base.OpenClient = false;
			base.SummaryVisibility = true;
			base.WebMobileClientVisibility = true;
			base.WebClientHeader = Resources.FinishConfigurationPageWebClientHeader;
			base.MobileClientLabel = string.Format(CultureInfo.InvariantCulture, Resources.FinishPageMobileClientAddressLabel, OemResources.Instance.MobileServer.MobileClientNameShort);
			base.WebClientLabel = string.Format(CultureInfo.InvariantCulture, Resources.FinishPageWebClientUrlLabel, OemResources.Instance.MobileServer.WebClientNameShort);
			base.MobileClientAddress = FinishPageHelper.LookupMobileClientAddress(installerManager);
			base.WebClientAddress = FinishPageHelper.LookupWebClientAddress(installerManager);
		}
		else
		{
			base.HeaderInfo = Resources.FinishInstallationPageHeaderInfo;
			base.WebMobileClientVisibility = false;
			base.OpenClient = true;
			base.OpenClientVisibility = true;
		}
	}
}
