using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Windows.Markup;

namespace VideoOS.Mobile.UI.Localization;

public class EnumBindingSourceExtension : MarkupExtension
{
	private Type _enumType;

	private ResourceManager _resourceManager;

	public bool ThrowMissingResourceException { get; set; }

	public Type EnumType
	{
		get
		{
			return _enumType;
		}
		private set
		{
			Type type = null;
			if (value != null)
			{
				type = Nullable.GetUnderlyingType(value) ?? value;
				if (!type.IsEnum)
				{
					throw new ArgumentException("Type must be for an Enum.");
				}
			}
			_enumType = type;
		}
	}

	public Dictionary<Enum, string> Mapping { get; private set; }

	public EnumBindingSourceExtension(Type enumType, Type resourceType)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		if (resourceType == null)
		{
			throw new ArgumentNullException("resourceType");
		}
		ThrowMissingResourceException = false;
		EnumType = enumType;
		_resourceManager = new ResourceManager(resourceType);
	}

	public EnumBindingSourceExtension(Type enumType, Dictionary<Enum, string> mapping)
	{
		if (enumType == null)
		{
			throw new ArgumentNullException("enumType");
		}
		if (mapping == null)
		{
			throw new ArgumentNullException("mapping");
		}
		ThrowMissingResourceException = false;
		EnumType = enumType;
		Mapping = mapping;
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		List<Enum> source = Enum.GetValues(EnumType).OfType<Enum>().ToList();
		if (Mapping != null)
		{
			if (ThrowMissingResourceException)
			{
				Enum obj = source.FirstOrDefault((Enum val) => !Mapping.ContainsKey(val));
				if (obj != null)
				{
					throw new LocalizationException($"Mapping key '{obj}' not found while localizing.");
				}
			}
			return source.Select((Enum enumVal) => new EnumValueLocalized(enumVal, Mapping.ContainsKey(enumVal) ? Mapping[enumVal] : enumVal.ToString())).ToList();
		}
		string enumResourcePrefix = $"{EnumType.Name}_";
		return source.Select((Enum enumValue) => new EnumValueLocalized(enumValue, ResourceLocalization.Localize(enumResourcePrefix + enumValue, _resourceManager, ThrowMissingResourceException))).ToList();
	}
}
