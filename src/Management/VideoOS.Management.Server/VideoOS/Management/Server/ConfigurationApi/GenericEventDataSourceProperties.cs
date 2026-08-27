using System.Globalization;
using System.Text;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class GenericEventDataSourceProperties
{
	public const string ProtocolKeyBoth = "Both";

	public const string ProtocolKeyTcp = "Tcp";

	public const string ProtocolKeyUdp = "Udp";

	public const string AddressFamilyKeyIPv4 = "InterNetwork";

	public const string AddressFamilyKeyIPv6 = "InterNetworkV6";

	public const string AddressFamilyKeyBoth = "Both";

	public const string PropertyEchoKeyFull = "Full";

	public const string PropertyEchoKeyNone = "None";

	public const string PropertyEchoKeyStatistics = "Statistics";

	public static Property PortProperty(int val)
	{
		return new Property(GenericEventDataSourceConfigurationApi.DataSourcePortKey, val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourcePort,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourcePort",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	public static Property SeparatorProperty(byte[] val)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte value in val)
		{
			stringBuilder.Append(value);
			stringBuilder.Append(',');
		}
		return new Property(GenericEventDataSourceConfigurationApi.DataSourceSeparatorKey, stringBuilder.ToString().TrimEnd(','))
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourceSeparator,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourceSeparator",
			UIImportance = UIImportance.Summary,
			SortKey = 102
		};
	}

	public static Property EncodingProperty(int val)
	{
		return new Property(GenericEventDataSourceConfigurationApi.DataSourceEncodingKey, val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourceEncoding,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourceEncoding",
			UIImportance = UIImportance.Summary,
			SortKey = 103
		};
	}

	public static Property LogProperty(bool val)
	{
		return new Property(GenericEventDataSourceConfigurationApi.DataSourceLogKey, val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourceLog,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourceLog",
			UIImportance = UIImportance.Summary,
			SortKey = 104
		};
	}

	public static Property EchoProperty(string val)
	{
		Property property = new Property(GenericEventDataSourceConfigurationApi.DataSourceEchoKey, val)
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourceEcho,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourceEcho",
			UIImportance = UIImportance.Summary,
			SortKey = 105
		};
		property.ValueTypeInfos = new ValueTypeInfo[3]
		{
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceEchoFull,
				Value = "Full",
				TranslationId = "GenericEventDataSourceEchoFull"
			},
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceEchoStatistics,
				Value = "Statistics",
				TranslationId = "GenericEventDataSourceEchoStatistics"
			},
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceEchoNone,
				Value = "None",
				TranslationId = "GenericEventDataSourceEchoNone"
			}
		};
		return property;
	}

	public static Property AllowedProperty(string val)
	{
		return new Property(GenericEventDataSourceConfigurationApi.DataSourceAllowedKey, val)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourceAllowed,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourceAllowed",
			UIImportance = UIImportance.Summary,
			SortKey = 106
		};
	}

	public static Property Allowed6Property(string val)
	{
		return new Property(GenericEventDataSourceConfigurationApi.DataSourceAllowed6Key, val)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourceAllowed6,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourceAllowed6",
			UIImportance = UIImportance.Summary,
			SortKey = 106
		};
	}

	public static Property ProtocolProperty(string val)
	{
		Property property = new Property(GenericEventDataSourceConfigurationApi.DataSourceProtocolKey, val)
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourceProtocol,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourceProtocol",
			UIImportance = UIImportance.Summary,
			SortKey = 110
		};
		property.ValueTypeInfos = new ValueTypeInfo[3]
		{
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceProtocolTCP,
				TranslationId = "GenericEventDataSourceProtocolTCP",
				Value = "Tcp"
			},
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceProtocolUDP,
				TranslationId = "GenericEventDataSourceProtocolUDP",
				Value = "Udp"
			},
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceProtocolBoth,
				TranslationId = "GenericEventDataSourceProtocolBoth",
				Value = "Both"
			}
		};
		return property;
	}

	public static Property AddressFamilyProperty(string val)
	{
		Property property = new Property(GenericEventDataSourceConfigurationApi.DataSourceAddressFamilyKey, val)
		{
			ValueType = "Enum",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyGenericEventDataSourceAddressFamily,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyGenericEventDataSourceAddressFamily",
			UIImportance = UIImportance.Summary,
			SortKey = 112
		};
		property.ValueTypeInfos = new ValueTypeInfo[3]
		{
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceAddressFamily4,
				TranslationId = "GenericEventDataSourceAddressFamily4",
				Value = "InterNetwork"
			},
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceAddressFamily6,
				TranslationId = "GenericEventDataSourceAddressFamily6",
				Value = "InterNetworkV6"
			},
			new ValueTypeInfo
			{
				Name = ConfigurationApiResources.GenericEventDataSourceAddressFamilyBoth,
				TranslationId = "GenericEventDataSourceAddressFamilyBoth",
				Value = "Both"
			}
		};
		return property;
	}
}
