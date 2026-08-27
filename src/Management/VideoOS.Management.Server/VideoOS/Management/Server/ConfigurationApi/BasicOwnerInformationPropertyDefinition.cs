using System.Collections.Generic;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class BasicOwnerInformationPropertyDefinition
{
	public const string TagTypeProperty = "TagType";

	public const string SelectToRemoveKey = "SelectToRemove";

	public static Property BasicOwnerTagTypeProperty()
	{
		Property property = new Property("TagType", "");
		property.Key = "TagType";
		property.ValueType = "Enum";
		property.IsSettable = true;
		property.DisplayName = ConfigurationApiResources.BasicOwnerTagTypeDisplayName;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "";
		property.ToolTipTranslationId = "";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 100;
		property.ValueTypeInfos = new ValueTypeInfo[10]
		{
			new ValueTypeInfo
			{
				Name = GetName("Address", "Name"),
				Value = "address.Name"
			},
			new ValueTypeInfo
			{
				Name = GetName("Address", "Address"),
				Value = "address.Address"
			},
			new ValueTypeInfo
			{
				Name = GetName("Address", "State"),
				Value = "address.State"
			},
			new ValueTypeInfo
			{
				Name = GetName("Address", "Phone"),
				Value = "address.Phone"
			},
			new ValueTypeInfo
			{
				Name = GetName("Address", "Country"),
				Value = "address.Country"
			},
			new ValueTypeInfo
			{
				Name = GetName("Address", "ZipCode"),
				Value = "address.ZipCode"
			},
			new ValueTypeInfo
			{
				Name = GetName("Admin", "Name"),
				Value = "admin.Name"
			},
			new ValueTypeInfo
			{
				Name = GetName("Admin", "Address"),
				Value = "admin.Address"
			},
			new ValueTypeInfo
			{
				Name = GetName("Admin", "Phone"),
				Value = "admin.Phone"
			},
			new ValueTypeInfo
			{
				Name = GetName("Additional", "AdditionalInfo"),
				Value = "additional.AdditionalInfo"
			}
		};
		return property;
	}

	public static Property BasicOwnerTagIdProperty(string id, string group, string fullKey, string key, string value, int order)
	{
		return new Property(FormatKey(id, fullKey), value)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = GetName(group, key),
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "",
			ToolTipTranslationId = "BasicOwnerTagPropertyToolTip",
			UIImportance = UIImportance.Default,
			SortKey = order
		};
	}

	public static Property BasicOwnerTagIdProperty(string key, string value, int order)
	{
		return new Property(key, value)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.BasicInfoAddValue,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "",
			ToolTipTranslationId = "BasicOwnerTagPropertyToolTip",
			UIImportance = UIImportance.Default,
			SortKey = order
		};
	}

	public static Property SelectToRemoveProperty(BasicOwnerInfo[] basicOwnerInfoArray)
	{
		List<ValueTypeInfo> list = new List<ValueTypeInfo>();
		foreach (BasicOwnerInfo basicOwnerInfo in basicOwnerInfoArray)
		{
			list.Add(new ValueTypeInfo
			{
				Value = basicOwnerInfo.Id,
				Name = string.Format(CultureInfo.InvariantCulture, "{0} = {1}", basicOwnerInfo.Name, basicOwnerInfo.Property)
			});
		}
		return new Property("ItemSelection", "")
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.BasicOwnerRemoveTagDisplayName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "BasicOwnerRemoveTagDisplayName",
			ToolTipTranslationId = "",
			UIImportance = UIImportance.Default,
			ValueTypeInfos = list.ToArray(),
			SortKey = 100
		};
	}

	private static string GetName(string group, string key)
	{
		string arg = "";
		string text = "";
		switch (group)
		{
		case "Address":
			arg = ConfigurationApiResources.BasicIfnoGroupAddress;
			break;
		case "Admin":
			arg = ConfigurationApiResources.BasicIfnoGroupAdmin;
			break;
		case "Additional":
			arg = ConfigurationApiResources.BasicIfnoGroupAdditional;
			break;
		}
		switch (key)
		{
		case "Address":
			text = ConfigurationApiResources.BasicInfoAddress;
			break;
		case "State":
			text = ConfigurationApiResources.BasicInfoState;
			break;
		case "ZipCode":
			text = ConfigurationApiResources.BasicInfoZipCode;
			break;
		case "Name":
			text = ConfigurationApiResources.BasicInfoName;
			break;
		case "Country":
			text = ConfigurationApiResources.BasicInfoCountry;
			break;
		case "Email":
			text = ConfigurationApiResources.BasicInfoEmail;
			break;
		case "Phone":
			text = ConfigurationApiResources.BasicInfoPhone;
			break;
		case "AdditionalInfo":
			text = ConfigurationApiResources.BasicInfoAdditionalInfo;
			break;
		case "NEW":
		case "TagValue":
			text = ConfigurationApiResources.BasicInfoAddValue;
			break;
		default:
			text = "No translation for: " + key;
			break;
		}
		return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", arg, text);
	}

	private static string FormatKey(string id, string fullKey)
	{
		return string.Format(CultureInfo.InvariantCulture, "[{0}]/{1}", id, fullKey);
	}

	public static bool UnformatKey(string input, out string id, out string fullKey)
	{
		id = "";
		fullKey = "";
		if (input == null || input.Length < 44)
		{
			return false;
		}
		if (input[0] != '[' || input[37] != ']' || input[38] != '/')
		{
			return false;
		}
		id = input.Substring(1, 36);
		fullKey = input.Substring(39);
		return true;
	}
}
