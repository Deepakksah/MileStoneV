using System;
using System.Collections.Generic;
using System.Globalization;
using System.Resources;
using VideoOS.Mobile.UI.Converters;

namespace VideoOS.Mobile.UI.Localization;

public class EnumLocalizationConverter : ValueConverterBase
{
	private ResourceManager _resourceManager;

	private const string _defaultResourceKeyFormat = "{0}_{1}";

	public string ResourceKeyFormat { get; set; }

	public Dictionary<Enum, string> Mapping { get; set; }

	public bool ThrowMissingResourceException { get; set; }

	public EnumLocalizationConverter(Type resourceType)
	{
		_resourceManager = new ResourceManager(resourceType);
		ResourceKeyFormat = "{0}_{1}";
		ThrowMissingResourceException = false;
	}

	public EnumLocalizationConverter(Dictionary<Enum, string> mapping)
	{
		if (mapping == null)
		{
			throw new ArgumentNullException("mapping");
		}
		Mapping = mapping;
		ThrowMissingResourceException = false;
	}

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is Enum obj)
		{
			if (Mapping != null)
			{
				if (Mapping.ContainsKey(obj))
				{
					return Mapping[obj];
				}
				if (ThrowMissingResourceException)
				{
					throw new LocalizationException($"Mapping key '{obj}' not found while localizing.");
				}
				return value;
			}
			return ResourceLocalization.Localize(string.Format(ResourceKeyFormat, obj.GetType().Name, obj), _resourceManager, ThrowMissingResourceException);
		}
		return null;
	}
}
