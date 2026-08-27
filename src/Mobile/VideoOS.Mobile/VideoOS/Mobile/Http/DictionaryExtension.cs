using System.Collections.Generic;
using System.Collections.Specialized;

namespace VideoOS.Mobile.Http;

public static class DictionaryExtension
{
	public static NameValueCollection ToNameValueCollection<TKey, TValue>(this IDictionary<TKey, TValue> dict)
	{
		NameValueCollection nameValueCollection = new NameValueCollection();
		foreach (KeyValuePair<TKey, TValue> item in dict)
		{
			string value = "";
			if (item.Value != null)
			{
				value = item.Value.ToString();
			}
			nameValueCollection.Add(item.Key.ToString(), value);
		}
		return nameValueCollection;
	}
}
