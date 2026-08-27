namespace VideoOS.Platform.SDK.Interface;

public interface ISmartClientSecurityInfo
{
	bool SmartClientLive { get; set; }

	bool SmartClientBrowse { get; set; }

	bool SmartClientSetup { get; set; }

	bool SmartClientReport { get; set; }
}
