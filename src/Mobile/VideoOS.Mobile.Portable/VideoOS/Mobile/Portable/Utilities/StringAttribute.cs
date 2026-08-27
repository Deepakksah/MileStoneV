using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class StringAttribute : Attribute, EnumsHelper.IAttributeValue<string>
{
	private readonly string _value;

	public StringAttribute(string value)
	{
		_value = value;
	}

	public string GetValue()
	{
		return _value;
	}
}
