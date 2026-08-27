using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VideoOS.Utilities.CommandLine;

public class Arguments : IEnumerable<KeyValuePair<string, string>>, IEnumerable
{
	private const string SPLIT_REGEX = "^-{1,2}|^/|=";

	private const string REMOVE_REGEX = "^['\"]?(.*?)['\"]?$";

	protected StringDictionary Parameters { get; set; }

	public string this[string parameter] => Parameters[parameter];

	public Arguments(string[] args)
	{
		Parameters = new StringDictionary();
		Regex regex = new Regex("^-{1,2}|^/|=", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		Regex regex2 = new Regex("^['\"]?(.*?)['\"]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
		string text = null;
		foreach (string input in args)
		{
			string[] array = regex.Split(input, 3);
			switch (array.Length)
			{
			case 1:
				if (text != null)
				{
					if (!Parameters.ContainsKey(text))
					{
						array[0] = regex2.Replace(array[0], "$1");
						Parameters.Add(text, array[0]);
					}
					text = null;
				}
				break;
			case 2:
				if (text != null && !Parameters.ContainsKey(text))
				{
					Parameters.Add(text, "true");
				}
				text = array[1];
				break;
			case 3:
				if (text != null && !Parameters.ContainsKey(text))
				{
					Parameters.Add(text, "true");
				}
				text = array[1];
				if (!Parameters.ContainsKey(text))
				{
					array[2] = regex2.Replace(array[2], "$1");
					Parameters.Add(text, array[2]);
				}
				text = null;
				break;
			}
		}
		if (text != null && !Parameters.ContainsKey(text))
		{
			Parameters.Add(text, "true");
		}
	}

	private Arguments()
	{
		Parameters = new StringDictionary();
	}

	public Arguments Clone(Dictionary<string, string> replacements)
	{
		Arguments arguments = new Arguments();
		foreach (string key in Parameters.Keys)
		{
			if (replacements.ContainsKey(key))
			{
				arguments.Parameters[key] = replacements[key];
			}
			else
			{
				arguments.Parameters[key] = Parameters[key];
			}
		}
		return arguments;
	}

	public string BuildCommandLineArgs(IList includeOnlyKeys, IList excludeKeys)
	{
		List<string> list = new List<string>();
		if (includeOnlyKeys != null)
		{
			foreach (string includeOnlyKey in includeOnlyKeys)
			{
				list.Add(includeOnlyKey.ToLower());
			}
		}
		List<string> list2 = new List<string>();
		if (excludeKeys != null)
		{
			foreach (string excludeKey in excludeKeys)
			{
				list2.Add(excludeKey.ToLower());
			}
		}
		StringBuilder stringBuilder = new StringBuilder("");
		foreach (string key in Parameters.Keys)
		{
			string item = key.ToLower();
			if ((list.Count <= 0 || list.Contains(item)) && (list2 == null || !list2.Contains(item)))
			{
				stringBuilder.Append("-" + key + " " + Parameters[key] + " ");
			}
		}
		return stringBuilder.ToString();
	}

	public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
	{
		return (from DictionaryEntry e in Parameters
			select new KeyValuePair<string, string>(e.Key as string, e.Value as string)).GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
