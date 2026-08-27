using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ConfigurationFinishPageViewModel : FinishPageViewModel
{
	public ConfigurationFinishPageViewModel(IInstallerManager installerManager)
		: base(new ConfigurationSummaryControlViewModel())
	{
		SetProperties(installerManager);
	}

	public ConfigurationFinishPageViewModel(IEnumerable<SingleSummaryControlViewModel> configurationSummeryItems, IInstallerManager installerManager)
		: base(new ConfigurationSummaryControlViewModel(configurationSummeryItems))
	{
		SetProperties(installerManager);
	}

	public void SetSummary(ConfigurationSummaryControlViewModel summary)
	{
		base.ContentViewModel = summary;
	}

	private void SetProperties(IInstallerManager installerManager)
	{
		base.Header = Resources.FinishConfigurationPageHeader;
		base.HeaderInfo = Resources.FinishConfigurationPageHeaderInfo;
		base.OpenDownloadManagerVisibility = false;
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
}
