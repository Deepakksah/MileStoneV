using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace VideoOS.Mobile.Http;

public static class NamedValueCollectionExtensions
{
	public static IEnumerable<KeyValuePair<string, string[]>> AsEnumerable(this NameValueCollection that)
	{
		return that.Cast<string>().Select((string item, int index) => new KeyValuePair<string, string[]>(item, that.GetValues(index)));
	}

	public static IEnumerable<KeyValuePair<string, string>> AsKeyValuePairs(this IEnumerable<KeyValuePair<string, string[]>> that)
	{
		return that.SelectMany((KeyValuePair<string, string[]> item) => item.Value.Select((string value) => new KeyValuePair<string, string>(item.Key, value)));
	}

	public static NameValueCollection ToNameValueCollection(this IEnumerable<KeyValuePair<string, string[]>> that)
	{
		return that.AsKeyValuePairs().ToNameValueCollection();
	}

	public static NameValueCollection ToNameValueCollection(this IEnumerable<KeyValuePair<string, string>> that)
	{
		NameValueCollection nameValueCollection = new NameValueCollection();
		foreach (KeyValuePair<string, string> item in that)
		{
			nameValueCollection.Add(item.Key, item.Value);
		}
		return nameValueCollection;
	}
}
