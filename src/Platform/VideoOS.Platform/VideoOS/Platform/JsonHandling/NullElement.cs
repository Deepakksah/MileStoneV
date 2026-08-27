using System;
using System.Diagnostics;

namespace VideoOS.Platform.JsonHandling;

[DebuggerDisplay("{Key}=null")]
public class NullElement : IElement
{
	public string Key { get; set; }

	public TokenType TokenType { get; set; }

	public bool IsBasicElement => string.IsNullOrEmpty(Key);

	public NullElement(string key)
	{
		Key = key;
		TokenType = TokenType.Null;
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
			jsonBuilder.AppendElement("null");
		}
		else
		{
			jsonBuilder.AppendKeyString(Key, null);
		}
	}
}
