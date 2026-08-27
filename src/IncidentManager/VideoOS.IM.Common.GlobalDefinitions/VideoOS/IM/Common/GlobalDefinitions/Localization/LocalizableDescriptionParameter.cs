using System;

namespace VideoOS.IM.Common.GlobalDefinitions.Localization;

public class LocalizableDescriptionParameter
{
	public string Key { get; }

	public Type Type { get; }

	public string Value { get; }

	public LocalizableDescriptionParameter(string key, Type type, string value)
	{
		Key = key ?? throw new ArgumentNullException("key");
		Type = type ?? throw new ArgumentNullException("type");
		Value = value ?? throw new ArgumentNullException("value");
	}
}
