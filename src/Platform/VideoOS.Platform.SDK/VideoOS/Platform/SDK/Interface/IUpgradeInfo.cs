namespace VideoOS.Platform.SDK.Interface;

public interface IUpgradeInfo
{
	string Hostname { get; set; }

	int Port { get; set; }

	string Path { get; set; }

	string Message { get; set; }
}
