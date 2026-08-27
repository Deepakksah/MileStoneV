using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;

namespace VideoOS.Platform.JsonHandling;

[DebuggerDisplay("{Key} has {Elements.Count} elements")]
public class ElementList : IElement
{
	public Collection<IElement> Elements { get; } = new Collection<IElement>();

	public string Key { get; set; }

	public TokenType TokenType { get; set; }

	public bool IsBasicElement => string.IsNullOrEmpty(Key);

	public ElementList(string key, TokenType tokenType)
	{
		Key = key;
		TokenType = tokenType;
	}

	public ElementList(string key, TokenType tokenType, Collection<IElement> elements)
	{
		Key = key;
		TokenType = tokenType;
		Elements = elements;
	}

	public IElement Element(string key)
	{
		foreach (IElement element in Elements)
		{
			if (string.Compare(element.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return element;
			}
		}
		return null;
	}

	public void AddElement(IElement element)
	{
		Elements.Add(element);
	}

	public void Remove(string key)
	{
		foreach (IElement element in Elements)
		{
			if (string.Compare(element.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
			{
				Elements.Remove(element);
				return;
			}
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Key not found: {0}", key), "key");
	}

	public void Set(string key, string value)
	{
		foreach (IElement element in Elements)
		{
			if (string.Compare(element.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (element is StringElement stringElement)
				{
					stringElement.Value = value;
				}
				return;
			}
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Key not found: {0}", key), "key");
	}

	public void Set(string key, int value)
	{
		foreach (IElement element in Elements)
		{
			if (string.Compare(element.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (element is IntElement intElement)
				{
					intElement.Value = value.ToString(CultureInfo.InvariantCulture);
				}
				return;
			}
		}
		throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Key not found: {0}", key), "key");
	}

	public string GetStringElementValue(string key)
	{
		foreach (IElement element in Elements)
		{
			if (string.Compare(element.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (element is StringElement stringElement)
				{
					return stringElement.Value;
				}
				break;
			}
		}
		return "";
	}

	public int GetIntElementValue(string key)
	{
		foreach (IElement element in Elements)
		{
			if (string.Compare(element.Key, key, StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (element is IntElement intElement)
				{
					if (int.TryParse(intElement.Value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var result))
					{
						return result;
					}
					return 0;
				}
				break;
			}
		}
		return 0;
	}

	public void ToJson(JsonBuilder jsonBuilder)
	{
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		if (TokenType == TokenType.ArrayStart)
		{
			JsonBuilder jsonBuilder2 = new JsonBuilder(array: true);
			foreach (IElement element in Elements)
			{
				if (element.IsBasicElement)
				{
					element.ToJson(jsonBuilder2);
					continue;
				}
				JsonBuilder jsonBuilder3 = new JsonBuilder(array: false);
				element.ToJson(jsonBuilder3);
				jsonBuilder2.AppendElement(jsonBuilder3.ToJson());
			}
			if (IsBasicElement)
			{
				jsonBuilder.AppendElement(jsonBuilder2.ToJson());
			}
			else
			{
				jsonBuilder.AppendKeyValue(Key, jsonBuilder2.ToJson());
			}
			return;
		}
		JsonBuilder jsonBuilder4 = new JsonBuilder(array: false);
		foreach (IElement element2 in Elements)
		{
			element2.ToJson(jsonBuilder4);
		}
		if (IsBasicElement)
		{
			jsonBuilder.AppendElement(jsonBuilder4.ToJson());
		}
		else
		{
			jsonBuilder.AppendElement(Key, jsonBuilder4.ToJson());
		}
	}
}
