namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class StringStringFactory : IKeyValueFactory<string, string>
{
	public string CreateKey(string key)
	{
		return key;
	}

	public string CreateValue(string value)
	{
		return value;
	}
}
