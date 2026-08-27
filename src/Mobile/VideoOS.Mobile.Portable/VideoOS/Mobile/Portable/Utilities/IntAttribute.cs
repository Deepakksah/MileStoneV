using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class IntAttribute : Attribute, EnumsHelper.IAttributeValue<int>
{
	private readonly int _value;

	public IntAttribute(int value)
	{
		_value = value;
	}

	public int GetValue()
	{
		return _value;
	}
}
