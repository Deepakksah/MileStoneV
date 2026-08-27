using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config.Svc;

public class KeyValue
{
	public string Key { get; set; }

	public string Value { get; set; }

	public KeyValue(IKeyValue keyValue)
	{
		Key = keyValue.Key;
		Value = keyValue.Value;
	}

	public KeyValue()
	{
	}
}
