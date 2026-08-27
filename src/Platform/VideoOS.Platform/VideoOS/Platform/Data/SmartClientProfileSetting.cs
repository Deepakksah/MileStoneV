namespace VideoOS.Platform.Data;

public sealed class SmartClientProfileSetting
{
	public const int CustomExportPath = 0;

	public int Key { get; }

	public bool Locked { get; }

	public string Value { get; }

	public SmartClientProfileSetting(int key, bool locked, string value)
	{
		Key = key;
		Locked = locked;
		Value = value;
	}
}
