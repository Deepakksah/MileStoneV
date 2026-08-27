using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Util;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Converters;

public class StringCollectionConverter : IStringCollectionConverter
{
	private static readonly char[] _separators = new char[2] { ';', ',' };

	private readonly IStringSplitter _splitter;

	public static IReadOnlyList<char> Separators => _separators;

	public StringCollectionConverter(IStringSplitter splitter)
	{
		_splitter = splitter;
	}

	public string EnumerableToString(IEnumerable<string> enumerable)
	{
		enumerable.NullCheck("", "EnumerableToString", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\Converters\\StringCollectionConverter.cs", 29);
		string[] enumerable2 = enumerable.Select((string topLevel) => _splitter.Escape(topLevel, _separators)).ToArray();
		return _splitter.JoinString(enumerable2, new char[1] { Separators[0] });
	}

	public string EnumerableToString(IEnumerable<IEnumerable<string>> enumerable)
	{
		enumerable.NullCheck("", "EnumerableToString", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\Converters\\StringCollectionConverter.cs", 36);
		string[][] enumerable2 = enumerable.Select((IEnumerable<string> topLevel) => topLevel.Select((string innerLevel) => _splitter.Escape(innerLevel, _separators)).ToArray()).ToArray();
		return _splitter.JoinString(enumerable2, _separators);
	}

	public string[] StringToArray(string input)
	{
		input.NullCheck("", "StringToArray", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\Converters\\StringCollectionConverter.cs", 44);
		return (from string s in _splitter.SplitString(input, new char[1] { Separators[0] })
			select _splitter.Unescape(s, _separators)).ToArray();
	}

	public string[][] StringTo2DimensionalArray(string input)
	{
		input.NullCheck("", "StringTo2DimensionalArray", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\Converters\\StringCollectionConverter.cs", 51);
		IList list = _splitter.SplitString(input, _separators);
		List<string[]> list2 = new List<string[]>(list.Count);
		foreach (IEnumerable item in list)
		{
			list2.Add((from string s in item
				select _splitter.Unescape(s, _separators)).ToArray());
		}
		return list2.ToArray();
	}
}
