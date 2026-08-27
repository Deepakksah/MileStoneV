namespace VideoOS.SCH.Services.Business.Definitions.Custom;

public class MessageData
{
	public string Key { get; set; }

	public string Value { get; set; }

	public MessageData(string key, string value)
	{
		Key = key;
		Value = value;
	}
}
