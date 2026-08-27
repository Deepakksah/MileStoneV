namespace VideoOS.Platform.SDK.Interface;

public interface ISmartClientVersion
{
	int Major { get; set; }

	int Minor { get; set; }

	string Revision { get; set; }

	int Beta { get; set; }

	bool ForceUpgrade { get; set; }

	string DisplayName { get; set; }

	IUpgradeInfo UpgradeInfo { get; set; }
}
