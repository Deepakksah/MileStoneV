using System;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public static class Constants
{
	public const string BaseCommunicationAlias = "XProtectMobile";

	public const string CommunicationAliasExtension = "/Communication";

	public const string HttpCommunicationAlias = "XProtectMobile/Communication";

	public const string VideoAliasExtension = "/Video";

	public const string HttpVideoAlias = "XProtectMobile/Video";

	public const string AudioAliasExtension = "/Audio";

	public const string HttpAudioAlias = "XProtectMobile/Audio";

	public const string StreamAliasExtension = "/Stream";

	public const string HttpStreamAlias = "XProtectMobile/Stream";

	public const string HttpWebAlias = "";

	public const string ExportAliasExtension = "/Export";

	public const string HttpExportAlias = "XProtectMobile/Export";

	public const string DeviceDiscoverabilityAliasExtension = "/DeviceDiscovery";

	public const string HttpDeviceDiscoverabilityAlias = "XProtectMobile/DeviceDiscovery";

	public const string DeviceConfigurationAliasExtension = "/Configuration";

	public const string HttpDeviceConfigurationAlias = "XProtectMobile/Configuration";

	public const string VideoSegmentedAliasExtension = "/Video/Segmented";

	public const string HttpVideoSegmentedAlias = "XProtectMobile/Video/Video/Segmented";

	public const string VideoSegmentedPlaybackAliasExtension = "/Playback";

	public const string HttpVideoSegmentedPlaybackAlias = "XProtectMobile/Video/Video/Segmented/Playback";

	public const string VideoSegmentedLiveAliasExtension = "/Live";

	public const string HttpVideoSegmentedLiveAlias = "XProtectMobile/Video/Video/Segmented/Live";

	public const string ChangePasswordRelativePathExtension = "/idp/Identity/Account/Manage/ChangePassword";

	public const string ShareServiceAlias = "Share";

	public const string WcfTrayServiceAlias = "TrayIconService";

	public static readonly Guid MobileServiceType = new Guid("{7510BBAE-DBA5-4C2E-B037-74A0222484C6}");
}
