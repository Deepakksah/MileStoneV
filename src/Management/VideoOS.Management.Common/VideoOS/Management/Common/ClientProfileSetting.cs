namespace VideoOS.Management.Common;

public class ClientProfileSetting
{
	public string Key { get; private set; }

	public string Value { get; set; }

	public bool Locked { get; set; }

	public ClientProfileSetting(string key, string value, bool locked)
	{
		Key = key;
		Value = value;
		Locked = locked;
	}
}
