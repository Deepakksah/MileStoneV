using System;
using System.Diagnostics;

namespace VideoOS.Platform.JsonHandling;

[DebuggerDisplay("{Key}={Value}")]
public class IntElement : IElement
{
	public string Key { get; set; }

	public string Value { get; set; }

	public TokenType TokenType { get; set; }

	public bool IsBasicElement => string.IsNullOrEmpty(Key);

	public IntElement(string key, string value)
	{
		Key = key;
		Value = value;
		TokenType = TokenType.Int;
	}

	public IElement Element(string key)
	{
		return null;
	}

	public void ToJson(JsonBuilder jsonBuilder)
	{
		if (jsonBuilder == null)
		{
			throw new ArgumentNullException("jsonBuilder");
		}
		if (IsBasicElement)
		{
			jsonBuilder.AppendElement(Value);
		}
		else
		{
			jsonBuilder.AppendElement(Key, Value);
		}
	}
}
