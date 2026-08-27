namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public interface IKeyValueFactory<out TKey, out TValue>
{
	TKey CreateKey(string key);

	TValue CreateValue(string value);
}
