using System;
using System.Diagnostics;

namespace VideoOS.Platform.JsonHandling;

[DebuggerDisplay("{Key}={Value}")]
public class BooleanElement : IElement
{
	public string Key { get; set; }

	public bool Value { get; set; }

	public TokenType TokenType { get; set; }

	public bool IsBasicElement => string.IsNullOrEmpty(Key);

	public BooleanElement(string key, bool value)
	{
		Key = key;
		Value = value;
		TokenType = TokenType.Boolean;
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
			jsonBuilder.AppendElement(Value.ToString().ToLowerInvariant());
		}
		else
		{
			jsonBuilder.AppendElement(Key, Value.ToString().ToLowerInvariant());
		}
	}
}
