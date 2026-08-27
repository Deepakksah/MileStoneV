using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.Installer.ServerPagesSetupPlugin.Properties;
using VideoOS.Vms.Utilities.Oem;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class PrivacySettingsViewModel : INotifyPropertyChanged
{
	private bool _shareUsageData;

	public bool ShareUsageData
	{
		get
		{
			return _shareUsageData;
		}
		set
		{
			_shareUsageData = value;
			OnPropertyChanged("ShareUsageData");
		}
	}

	[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Binding to this property from the view")]
	public string UsageInfo => string.Format(CultureInfo.InvariantCulture, Resources.PrivacySettingsPageShareUsageDataInfo, OemResources.Instance.MobileServer.MobileServerName, OemResources.Instance.MobileServer.MobileProductName, OemResources.Instance.MobileServer.WebClientName);

	[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Binding to this property from the view")]
	public string LegalInfo => string.Format(CultureInfo.InvariantCulture, Resources.PrivacySettingsPageShareUsageDataLegalInfo, OemResources.Instance.App.CompanyName);

	[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Binding to this property from the view")]
	public string OnlineInfo => OemResources.Instance.Manager.CustomerExperienceProgramUrl?.AbsoluteUri;

	[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Binding to this property from the view")]
	public string ShareUsageDataString => Resources.PrivacySettingsPageShareUsageData;

	[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Binding to this property from the view")]
	public string DoNotShareUsageDataString => string.Format(CultureInfo.InvariantCulture, Resources.PrivacySettingsPageDoNotShareUsageData, OemResources.Instance.MobileServer.MobileServerName, OemResources.Instance.MobileServer.MobileProductName, OemResources.Instance.MobileServer.WebClientName);

	[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Binding to this property from the view")]
	public string PageInfoString => Resources.PrivacySettingsPageInfo;

	[SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Binding to this property from the view")]
	public string PageHeaderString => Resources.PrivacySettingsPageHeader;

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string propertyName)
	{
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}

	public void ReloadResources()
	{
		OnPropertyChanged("UsageInfo");
		OnPropertyChanged("LegalInfo");
		OnPropertyChanged("OnlineInfo");
		OnPropertyChanged("ShareUsageDataString");
		OnPropertyChanged("DoNotShareUsageDataString");
		OnPropertyChanged("PageInfoString");
		OnPropertyChanged("PageHeaderString");
	}
}
