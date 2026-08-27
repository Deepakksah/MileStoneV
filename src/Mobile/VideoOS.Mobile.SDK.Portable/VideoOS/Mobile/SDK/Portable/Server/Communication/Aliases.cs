namespace VideoOS.Mobile.SDK.Portable.Server.Communication;

public class Aliases
{
	public static string BaseCommunicationAlias = "XProtectMobile";

	public static string ShareServiceAlias = "Share";

	public static string HttpCommunicationAlias => BaseCommunicationAlias + "/Communication";

	public static string HttpVideoAlias => BaseCommunicationAlias + "/Video";

	public static string HttpAudioAlias => BaseCommunicationAlias + "/Audio";

	public static string HttpStreamAlias => BaseCommunicationAlias + "/Stream";

	public static string HttpWebAlias => "";

	public static string HttpExportAlias => BaseCommunicationAlias + "/Export";

	public static string HttpDeviceDiscoverabilityAlias => BaseCommunicationAlias + "/DeviceDiscovery";

	public static string HttpDeviceConfigurationAlias => BaseCommunicationAlias + "/Configuration";

	public static string HttpVideoSegmentedAlias => BaseCommunicationAlias + "/Video/Segmented";

	public static string HttpVideoSegmentedPlaybackAlias => HttpVideoSegmentedAlias + "/Playback";

	public static string HttpVideoSegmentedLiveAlias => HttpVideoSegmentedAlias + "/Live";

	public static void InitBaseAlias(string baseAlias)
	{
		if (!string.IsNullOrWhiteSpace(baseAlias))
		{
			BaseCommunicationAlias = baseAlias;
		}
	}
}
