using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class ServerPropertyDefinitions
{
	public const string AddressesKey = "Addresses";

	public const string TypeKey = "Type";

	public static Property AddressesProperty(IEnumerable<string> addresses)
	{
		Property property = new Property("Addresses", addresses.ToArray());
		property.ValueType = "Array";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyServerAddresses;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyServerAddresses";
		property.ToolTipTranslationId = "PropertyServerAddressesToolTip";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 1;
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "ArrayValueType",
				Value = "String"
			}
		};
		return property;
	}

	public static Property TypeProperty(Guid type)
	{
		return new Property("Type", type.ToString())
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyServerType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyServerType",
			ToolTipTranslationId = "PropertyServerTypeToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 1
		};
	}
}
