using System.Globalization;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Common.DefaultValues;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RecordingServerMulticastPropertyDefinition
{
	public const string IPAddressStartKey = "IPAddressStart";

	public const string IPAddressEndKey = "IPAddressEnd";

	public const string PortRangeStartKey = "PortRangeStart";

	public const string PortRangeEndKey = "PortRangeEnd";

	public const string MTUKey = "MTU";

	public const string TTLKey = "TTL";

	public static Property IPAddressStartProperty(string IPAddressStart)
	{
		Property property = new Property("IPAddressStart", IPAddressStart)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMulticastIPAddressStart,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMulticastIPAddressStart"
		};
		ValueTypeInfo valueTypeInfo = new ValueTypeInfo();
		valueTypeInfo.Name = "MinValue";
		valueTypeInfo.Value = RecordingServerMulticast.IPv4MulticastAllowedRangeStart.ToString();
		ValueTypeInfo valueTypeInfo2 = new ValueTypeInfo();
		valueTypeInfo2.Name = "MinValue";
		valueTypeInfo2.Value = RecordingServerMulticast.IPv6MulticastAllowedRangeStart.ToString();
		ValueTypeInfo valueTypeInfo3 = new ValueTypeInfo();
		valueTypeInfo3.Name = "MaxValue";
		valueTypeInfo3.Value = RecordingServerMulticast.IPv4MulticastAllowedRangeEnd.ToString();
		ValueTypeInfo valueTypeInfo4 = new ValueTypeInfo();
		valueTypeInfo4.Name = "MaxValue";
		valueTypeInfo4.Value = RecordingServerMulticast.IPv6MulticastAllowedRangeEnd.ToString();
		property.ValueTypeInfos = new ValueTypeInfo[4] { valueTypeInfo, valueTypeInfo2, valueTypeInfo3, valueTypeInfo4 };
		return property;
	}

	public static Property IPAddressEndProperty(string IPAddressEnd)
	{
		Property property = new Property("IPAddressEnd", IPAddressEnd)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMulticastIPAddressEnd,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMulticastIPAddressEnd"
		};
		ValueTypeInfo valueTypeInfo = new ValueTypeInfo();
		valueTypeInfo.Name = "MinValue";
		valueTypeInfo.Value = RecordingServerMulticast.IPv4MulticastAllowedRangeEnd.ToString();
		ValueTypeInfo valueTypeInfo2 = new ValueTypeInfo();
		valueTypeInfo2.Name = "MinValue";
		valueTypeInfo2.Value = RecordingServerMulticast.IPv6MulticastAllowedRangeEnd.ToString();
		ValueTypeInfo valueTypeInfo3 = new ValueTypeInfo();
		valueTypeInfo3.Name = "MaxValue";
		valueTypeInfo3.Value = RecordingServerMulticast.IPv4MulticastAllowedRangeEnd.ToString();
		ValueTypeInfo valueTypeInfo4 = new ValueTypeInfo();
		valueTypeInfo4.Name = "MaxValue";
		valueTypeInfo4.Value = RecordingServerMulticast.IPv6MulticastAllowedRangeEnd.ToString();
		property.ValueTypeInfos = new ValueTypeInfo[4] { valueTypeInfo, valueTypeInfo2, valueTypeInfo3, valueTypeInfo4 };
		return property;
	}

	public static Property PortRangeStartProperty(string portRangeStart)
	{
		Property property = new Property("PortRangeStart", portRangeStart)
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMulticastPortRangeStart,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMulticastPortRangeStart"
		};
		ValueTypeInfo valueTypeInfo = new ValueTypeInfo();
		valueTypeInfo.Name = "MinValue";
		valueTypeInfo.Value = 1.ToString(CultureInfo.InvariantCulture);
		ValueTypeInfo valueTypeInfo2 = new ValueTypeInfo();
		valueTypeInfo2.Name = "MaxValue";
		valueTypeInfo2.Value = 65535.ToString(CultureInfo.InvariantCulture);
		property.ValueTypeInfos = new ValueTypeInfo[2] { valueTypeInfo, valueTypeInfo2 };
		return property;
	}

	public static Property PortRangeEndProperty(string portRangeEnd)
	{
		Property property = new Property("PortRangeEnd", portRangeEnd)
		{
			ValueType = "Int",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMulticastPortRangeEnd,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMulticastPortRangeEnd"
		};
		ValueTypeInfo valueTypeInfo = new ValueTypeInfo();
		valueTypeInfo.Name = "MinValue";
		valueTypeInfo.Value = 1.ToString(CultureInfo.InvariantCulture);
		ValueTypeInfo valueTypeInfo2 = new ValueTypeInfo();
		valueTypeInfo2.Name = "MaxValue";
		valueTypeInfo2.Value = 65535.ToString(CultureInfo.InvariantCulture);
		property.ValueTypeInfos = new ValueTypeInfo[2] { valueTypeInfo, valueTypeInfo2 };
		return property;
	}

	public static Property MTUProperty(string recorderAddress)
	{
		return new Property("MTU", recorderAddress)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMulticastMTU,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMulticastMTU",
			ToolTipTranslationId = "PropertyRecordingServerMulticastMTUTooltip"
		};
	}

	public static Property TTLProperty(string recorderAddress)
	{
		return new Property("TTL", recorderAddress)
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyRecordingServerMulticastTTL,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyRecordingServerMulticastTTL",
			ToolTipTranslationId = "PropertyRecordingServerMulticastTTLTooltip"
		};
	}
}
