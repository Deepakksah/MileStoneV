using System;
using System.Resources;

namespace VideoOS.Mobile.UI.Localization;

public static class ResourceLocalization
{
	public static string Localize(string resourceKey, Type resourceType, bool throwMissingResourceException = false)
	{
		ResourceManager resourceManager = new ResourceManager(resourceType);
		return Localize(resourceKey, resourceManager, throwMissingResourceException);
	}

	public static string Localize(string resourceKey, ResourceManager resourceManager, bool throwMissingResourceException = false)
	{
		if (!ValidateResourceKey(resourceKey))
		{
			throw new ArgumentException("Invalid resource key.");
		}
		string text = resourceManager.GetString(resourceKey);
		if (throwMissingResourceException && text == null)
		{
			throw new LocalizationException($"Resource key '{resourceKey}' not found while localizing.");
		}
		return text ?? resourceKey;
	}

	private static bool ValidateResourceKey(string resourceKey)
	{
		return !string.IsNullOrWhiteSpace(resourceKey);
	}
}
