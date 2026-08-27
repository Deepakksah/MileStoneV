namespace VideoOS.Platform.SDK.Interface;

public interface IClientProfileSetting
{
	string Key { get; set; }

	string Value { get; set; }

	bool Locked { get; set; }
}
