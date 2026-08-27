using System;
using System.Text.RegularExpressions;

namespace VideoOS.Platform.UI.Automation.Serialization;

public class PathSegmentsFactory
{
	private static readonly Regex PropertySplitRegex = new Regex("\\.(?![^(]*\\))", RegexOptions.Compiled);

	private static readonly Regex TypeFilterRegex = new Regex("^\\(([^)]*)\\)?(.*)$", RegexOptions.Compiled);

	private static readonly Regex CollectionTypeAndFilterRegex = new Regex(".*(\\[(.*)\\])+$", RegexOptions.Compiled);

	public static IPathSegment Create(AutomationObjectSerializationPath path)
	{
		string pattern = path.Pattern;
		if (string.IsNullOrWhiteSpace(pattern))
		{
			return null;
		}
		string[] array = PropertySplitRegex.Split(pattern, 2);
		string propertyName = array[0];
		string remainder = ((array.Length == 2) ? array[1] : null);
		PropertyInfoDetails propertyInfoDetails = GetPropertyInfoDetails(propertyName);
		string keyPrefix = path.OriginalPattern.Substring(0, path.OriginalPattern.Length - pattern.Length);
		if (!propertyInfoDetails.IsCollection)
		{
			return new PropertyPathSegment(keyPrefix, pattern, propertyInfoDetails, Create(path.Update(remainder)));
		}
		return new CollectionPathSegment(keyPrefix, pattern, propertyInfoDetails, Create(path.Update(remainder)));
	}

	private static PropertyInfoDetails GetPropertyInfoDetails(string propertyName)
	{
		string text = propertyName;
		bool flag = false;
		bool isRecursive = false;
		int num = text.IndexOf("{", StringComparison.Ordinal);
		int num2 = text.IndexOf("}", StringComparison.Ordinal);
		if ((num >= 0 && num2 < 0) || (num2 >= 0 && num < 0))
		{
			throw new InvalidOperationException("Recursion syntax error. Missing start/end element.");
		}
		if (num >= 0)
		{
			if (num2 >= 0 && num2 < num)
			{
				throw new InvalidOperationException("Recursion syntax error. End element before start element.");
			}
			if (num != 0 && num2 != text.Length - 1)
			{
				throw new InvalidOperationException("Recursion syntax error. Start/end element not at start/end of property path.");
			}
			isRecursive = true;
			text = text.Substring(1, text.Length - 2);
		}
		Func<Type, bool> itemFilter = null;
		Match match = CollectionTypeAndFilterRegex.Match(text);
		if (match.Success)
		{
			flag = true;
			text = text.Substring(0, text.Length - match.Groups[1].Length);
			if (match.Groups.Count == 3)
			{
				string value = match.Groups[2].Value;
				if (value != null && !string.IsNullOrWhiteSpace(value))
				{
					value = value.Substring(1, value.Length - 2);
					bool invert = false;
					if (value.StartsWith("!"))
					{
						value = value.Substring(1);
						invert = true;
					}
					Type type = Type.GetType(value);
					if (type == null)
					{
						throw new InvalidOperationException("Unable to load type from type collection item filter: '" + value + "'");
					}
					itemFilter = (Type t) => (!invert) ? type.IsAssignableFrom(t) : (!type.IsAssignableFrom(t));
				}
			}
		}
		Func<Type, bool> filter = null;
		if (text.StartsWith("("))
		{
			match = TypeFilterRegex.Match(text);
			if (!match.Success)
			{
				throw new InvalidOperationException("Invalid syntax for property: '" + propertyName + "'");
			}
			string text2 = match.Groups[1].Value;
			text = match.Groups[2].Value;
			bool invert2 = false;
			if (text2.StartsWith("!"))
			{
				text2 = text2.Substring(1);
				invert2 = true;
			}
			Type type2 = Type.GetType(text2);
			if (type2 == null)
			{
				throw new InvalidOperationException("Unable to load type from type filter: '" + text2 + "'");
			}
			filter = (Type t) => (!invert2) ? type2.IsAssignableFrom(t) : (!type2.IsAssignableFrom(t));
		}
		TypeFilterBase typeFilter = (flag ? ((TypeFilterBase)new CollectionItemFilter(filter, itemFilter)) : ((TypeFilterBase)new PropertyTypeFilter(filter)));
		PropertyInfoDetails propertyInfoDetails = new PropertyInfoDetails(text, flag, isRecursive, typeFilter);
		if (propertyInfoDetails.IsRecursive && propertyInfoDetails.IsSimpleTypeWildcard)
		{
			throw new InvalidOperationException("Recursion prefix '$' is now allowed in combination with property wildcard '*'. Wildcards only operate on simple types; thus no recursion.");
		}
		return propertyInfoDetails;
	}
}
