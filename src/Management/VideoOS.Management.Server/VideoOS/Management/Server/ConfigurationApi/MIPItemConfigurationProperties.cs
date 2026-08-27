using System;
using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MIPItemConfigurationProperties
{
	public static string PluginIdKey = "PluginId";

	public static string PluginNameKey = "PluginName";

	public static string DataVersionKey = "DataVersion";

	public static string ModifiedUserKey = "ModifiedUser";

	public static string KindTypeKey = "KindType";

	public static string SecurityActionKey = "SecurityAction";

	public static string SecurityActionNamesKey = "SecurityActionNames";

	public static string DisplayOnGisMapKey = "DisplayOnGisMap";

	public static string ParentKindKey = "ParentKind";

	public static string FQIDKindKey = "FQID.Kind";

	public static string FQIDParentIdKey = "FQID.ParentId";

	public static string FQIDServerIdKey = "FQID.ServerId";

	public static string FQIDSchemeKey = "FQID.Scheme";

	public static string FQIDHostKey = "FQID.Host";

	public static string FQIDPortKey = "FQID.Port";

	public static string FQIDTypeKey = "FQID.Type";

	public static string GisPointKey = "GisPoint";

	public static string EncryptKey = "__Encrypt";

	public static string Licenses = "licenses";

	public static string Registrations = "registrations";

	public static string OptionIdKey = "OptionId";

	public static string OptionXmlKey = "XML";

	public static string OptionDataVersionKey = "DataVersion";

	public static string OptionUserKey = "UserPrivate";

	public static string MIPItemDataVersionSumKey = "MIPItemDataVersionSum";

	public static Property FQIDKindProperty(Guid kind, bool isSettable)
	{
		return new Property(FQIDKindKey, kind.ToString())
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPItemKind,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemKind",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FQIDServerIdProperty(Guid serverId, bool isSettable)
	{
		return new Property(FQIDServerIdKey, serverId.ToString())
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPItemServerId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemServerId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FQIDParentIdProperty(Guid parentId, bool isSettable)
	{
		return new Property(FQIDParentIdKey, parentId.ToString())
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPItemParentId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemParentId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FQIDSchemeProperty(string scheme, bool isSettable)
	{
		return new Property(FQIDSchemeKey, scheme)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPItemScheme,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemScheme",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FQIDHostProperty(string host, bool isSettable)
	{
		return new Property(FQIDHostKey, host)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPItemHost,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemHost",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FQIDPortProperty(int port, bool isSettable)
	{
		return new Property(FQIDPortKey, port.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPItemPort,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemPort",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property FQIDTypeProperty(string type, bool isSettable)
	{
		return new Property(FQIDTypeKey, type)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPItemType,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemType",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property GisPointProperty(string gisPoint, bool isSettable)
	{
		return new Property(GisPointKey, gisPoint)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPItemGisPointId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemGisPointId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property EncryptProperty(bool propertyValue)
	{
		return new Property(EncryptKey, propertyValue.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMIPItemEncrypt,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemEncrypt",
			UIImportance = UIImportance.Default,
			SortKey = 107
		};
	}

	public static Property PluginIdProperty(Guid pluginId, bool isSettable)
	{
		return new Property(PluginIdKey, pluginId.ToString())
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPKindPluginId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPKindPluginId",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property PluginNameProperty(string pluginName, bool isSettable)
	{
		return new Property(PluginNameKey, pluginName)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPKindPluginName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPKindPluginName",
			UIImportance = UIImportance.Default,
			SortKey = 102
		};
	}

	public static Property DataVersionProperty(int dataVersion)
	{
		return new Property(DataVersionKey, dataVersion.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyMIPItemDataVersionSum,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemDataVersionSum",
			UIImportance = UIImportance.Default,
			SortKey = 106
		};
	}

	public static Property ModifiedUserProperty(string name)
	{
		return new Property(ModifiedUserKey, name)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyMIPItemModifiedUser,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemModifiedUser",
			UIImportance = UIImportance.Default,
			SortKey = 107
		};
	}

	public static Property KindTypeProperty(string name, bool isSettable)
	{
		Property property = new Property(KindTypeKey, name);
		property.ValueType = "Enum";
		property.IsSettable = isSettable;
		property.DisplayName = ConfigurationApiResources.PropertyMIPKindKindType;
		property.ItemReference = false;
		property.ServerValidation = false;
		property.TranslationId = "PropertyMIPKindKindType";
		property.UIImportance = UIImportance.Default;
		property.SortKey = 107;
		property.ValueTypeInfos = new ValueTypeInfo[2]
		{
			new ValueTypeInfo
			{
				Name = "Item",
				Value = "ITEM"
			},
			new ValueTypeInfo
			{
				Name = "Option",
				Value = "OPTION"
			}
		};
		return property;
	}

	public static Property SecurityActionProperty(string name, bool isSettable)
	{
		return new Property(SecurityActionKey, name)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPKindSecurityAction,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPKindSecurityAction",
			ToolTipTranslationId = "PropertyMIPKindSecurityActionToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 107
		};
	}

	public static Property SecurityActionNamesProperty(string name, bool isSettable)
	{
		return new Property(SecurityActionNamesKey, name)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPKindSecurityActionNames,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPKindSecurityActionNames",
			ToolTipTranslationId = "PropertyMIPKindSecurityActionNamesToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 108
		};
	}

	public static Property DisplayOnGisMapProperty(bool propertyValue)
	{
		return new Property(DisplayOnGisMapKey, propertyValue.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMIPKindDisplayOnGisMap,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPKindDisplayOnGisMap",
			UIImportance = UIImportance.Default,
			SortKey = 107
		};
	}

	public static Property ParentKindProperty(Guid kind, bool isSettable)
	{
		return new Property(ParentKindKey, kind.ToString())
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPKindParentKind,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPKindParentKind",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property MIPItemDataVersionSumProperty(int sum)
	{
		return new Property(MIPItemDataVersionSumKey, sum.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyMIPItemDataVersionSum,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPItemDataVersionSum",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
	}

	public static Property OptionIdProperty(Guid optionId, bool isSettable)
	{
		return new Property(OptionIdKey, optionId.ToString())
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPOptionId,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPOptionId",
			UIImportance = UIImportance.Default,
			SortKey = 102
		};
	}

	public static Property OptionXmlProperty(string xml, bool isSettable)
	{
		return new Property(OptionXmlKey, xml)
		{
			ValueType = "String",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPOptionXml,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPOptionXml",
			UIImportance = UIImportance.Default,
			SortKey = 103
		};
	}

	public static Property OptionUserProperty(bool value)
	{
		return new Property(OptionUserKey, value.ToString())
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyMIPOptionUser,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPOptionUser",
			ToolTipTranslationId = "PropertyMIPOptionUserToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 104
		};
	}

	public static Property OptionDataVersionProperty(int dataVersion, bool isSettable)
	{
		return new Property(OptionDataVersionKey, dataVersion.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyMIPOptionDataVersion,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyMIPOptionDataVersion",
			UIImportance = UIImportance.Default,
			SortKey = 104
		};
	}
}
