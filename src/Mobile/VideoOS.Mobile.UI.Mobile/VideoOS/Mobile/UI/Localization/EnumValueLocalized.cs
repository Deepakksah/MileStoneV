using System;

namespace VideoOS.Mobile.UI.Localization;

public class EnumValueLocalized
{
	public Enum EnumValue { get; private set; }

	public string DisplayName { get; private set; }

	public EnumValueLocalized(Enum enumValue, string displayName)
	{
		EnumValue = enumValue;
		DisplayName = displayName;
	}

	public override string ToString()
	{
		return DisplayName;
	}
}
