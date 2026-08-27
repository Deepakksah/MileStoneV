#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Web;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Definition.Schema;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class SettingsUtil
{
	public static ConfigurationItem ConstructDriverSettingsItemAndChildren(IGeminiRequestContext requestContext, MainDefinition mainDefinition, Hwid hwid, string itemType, DriverSettings driverSettings, ConfigurationItemPath parentPath)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		ConfigurationItem configurationItem = ConstructDriverSettingsConfigurationItem(parentPath, itemType);
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		string path = configurationItem.Path;
		string parentPath2 = configurationItem.ParentPath;
		string[] sectionNames = driverSettings.GetSectionNames();
		foreach (string text in sectionNames)
		{
			switch (text.Split(':')[0])
			{
			case "hardware":
			{
				ConfigurationItem item = MakeChildItem("HardwareDriverSettings", ConfigurationApiResources.ItemTypeDeviceDriverSettingsGeneral, path, parentPath2, BuildProperties(requestContext, mainDefinition, driverSettings, new HardwareHwid(text)));
				list.Add(item);
				break;
			}
			case "hardwareptz":
			{
				ConfigurationItem item = MakeChildItem("PTZ", ConfigurationApiResources.ItemTypePTZ, path, parentPath2, BuildProperties(requestContext, mainDefinition, driverSettings, new HardwarePtzHwid(text)));
				list.Add(item);
				break;
			}
			case "device":
			{
				ConfigurationItem item = MakeChildItem("DeviceDriverSettings", ConfigurationApiResources.ItemTypeDeviceDriverSettingsGeneral, path, parentPath2, BuildProperties(requestContext, mainDefinition, driverSettings, new DeviceHwid(text)));
				list.Add(item);
				break;
			}
			case "deviceptz":
			{
				ConfigurationItem item = MakeChildItem("PTZ", ConfigurationApiResources.ItemTypePTZ, path, parentPath2, BuildProperties(requestContext, mainDefinition, driverSettings, new DevicePtzHwid(text)));
				list.Add(item);
				break;
			}
			case "stream":
			{
				StreamHwid streamHwid = new StreamHwid(text);
				StreamDefinition streamDefinition = mainDefinition.HardwareDefinition.DeviceDefinitions[new DeviceHwid(text)].Streams[streamHwid];
				string displayNameResolved2 = streamDefinition.DisplayNameResolved;
				List<Property> list2 = BuildProperties(requestContext, mainDefinition, driverSettings, streamHwid);
				list2.Add(DeviceDriverSettingsProperties.StreamReferenceIdProperty(streamDefinition.ReferenceId));
				ConfigurationItem item = MakeChildItem("Stream", displayNameResolved2, path, parentPath2, list2);
				list.Add(item);
				break;
			}
			case "metadataitem":
			{
				MetadataItemHwid metadataItemHwid = new MetadataItemHwid(text);
				DeviceHwid key = new DeviceHwid(text);
				string displayNameResolved = mainDefinition.HardwareDefinition.DeviceDefinitions[key].MetadataItems[metadataItemHwid].DisplayNameResolved;
				ConfigurationItem item = MakeChildItem("DeviceDriverSettings", displayNameResolved, path, parentPath2, BuildProperties(requestContext, mainDefinition, driverSettings, metadataItemHwid));
				list.Add(item);
				break;
			}
			default:
				throw new InternalServerException("Unknown driver section:" + hwid.Text);
			}
		}
		configurationItem.Children = list.ToArray();
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	private static ConfigurationItem MakeChildItem(string itemType, string name, string path, string parentPath, List<Property> properties)
	{
		return new ConfigurationItem
		{
			Children = null,
			ChildrenFilled = true,
			DisplayName = name,
			ItemType = itemType,
			ItemCategory = ItemCategories.ChildItem,
			Path = path,
			ParentPath = parentPath,
			Properties = properties.ToArray()
		};
	}

	private static List<Property> BuildProperties(IGeminiRequestContext requestContext, MainDefinition mainDefinition, DriverSettings driverSettings, Hwid hwid)
	{
		int sort = 1;
		List<Property> list = new List<Property>();
		CapabilitiesCollection capabilities = null;
		if (!mainDefinition.TryGetCapabilitiesByHwid(hwid, out capabilities))
		{
			return new List<Property>();
		}
		foreach (CapabilityDefinition item in capabilities)
		{
			if (!DriverSettingsBusiness.InternalHardwareSettings.Contains(item.TypeDefinition.ReferenceId) && item.TypeDefinition.GetActiveTypeContent() != null)
			{
				list.AddRange(HandleCapability(requestContext, mainDefinition, item, driverSettings, ref sort));
			}
		}
		return list;
	}

	public static bool ShouldOverrideReadOnlyFlag(MainDefinition mainDefinition, Guid capabilityTypeId)
	{
		if (new Guid("338D6DB1-160E-4CAC-A5D4-3FCF8068A955") == capabilityTypeId && mainDefinition.HardwareDefinition.PtzSettings != null)
		{
			return mainDefinition.HardwareDefinition.PtzSettings.Enabled;
		}
		return false;
	}

	public static List<Property> HandleCapability(IGeminiRequestContext requestContext, MainDefinition mainDefinition, CapabilityDefinition capability, DriverSettings driverSettings, ref int sort)
	{
		List<Property> list = new List<Property>();
		try
		{
			string name = capability.Name;
			TypeDefinition typeDefinition = capability.TypeDefinition;
			bool visible = typeDefinition.Visible;
			bool flag = typeDefinition.Access == access.readwrite || typeDefinition.Access == access.write;
			if (flag && (typeDefinition.ActiveDataType == datatype.ptzcontrol || typeDefinition.ActiveDataType == datatype.ptzpresets))
			{
				flag = false;
			}
			if (ShouldOverrideReadOnlyFlag(mainDefinition, typeDefinition.ReferenceId))
			{
				flag = true;
			}
			TypeContent activeTypeContent = typeDefinition.GetActiveTypeContent();
			string typeId = driverSettings.GetTypeId(capability.Hwid.Text, capability.Name);
			string valueByTypeId = driverSettings.GetValueByTypeId(capability.Hwid.Text, typeId);
			string key = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", capability.Hwid.Text, HttpUtility.UrlEncode(name), capability.TypeDefinition.ReferenceId);
			Property property = new Property();
			visible = ConstructPropertyValueType(requestContext, activeTypeContent, visible, valueByTypeId, property, activeTypeContent.DefaultValue);
			property.Key = key;
			property.DisplayName = (string.IsNullOrEmpty(typeDefinition.DisplayNameResolved) ? name : typeDefinition.DisplayNameResolved);
			try
			{
				property.TranslationId = typeDefinition.DisplayNameReferenceId.ToString();
			}
			catch (ArgumentNullException)
			{
				property.TranslationId = Guid.Empty.ToString();
			}
			catch (FormatException)
			{
				property.TranslationId = Guid.Empty.ToString();
			}
			property.Value = valueByTypeId;
			property.SortKey = sort++;
			property.UIImportance = ((!visible) ? 1 : 0);
			property.IsSettable = flag;
			list.Add(property);
			return list;
		}
		catch (Exception ex3)
		{
			Trace.WriteLine("Capability unpack exception:" + ex3.Message);
			throw;
		}
	}

	private static bool ConstructPropertyValueType(IGeminiRequestContext requestContext, TypeContent typeContent, bool visible, string currentValue, Property property, string defaultValue)
	{
		List<ValueTypeInfo> valueTypes = new List<ValueTypeInfo>();
		if (typeContent.IsData)
		{
			visible &= ConstructPropertyValueTypeData(typeContent, currentValue, property, defaultValue, valueTypes);
		}
		else if (typeContent.IsEnumeration)
		{
			ConstructPropertyValueTypeEnum(requestContext, typeContent, currentValue, property, defaultValue, valueTypes);
		}
		return visible;
	}

	private static bool ConstructPropertyValueTypeData(TypeContent typeContent, string currentValue, Property property, string defaultValue, List<ValueTypeInfo> valueTypes)
	{
		bool result = true;
		switch (typeContent.AsData.DataType)
		{
		case datatype.@double:
		{
			property.ValueType = "Double";
			datacontentType datacontentType3 = (datacontentType)typeContent.Base;
			if (datacontentType3.minvalue != null && datacontentType3.maxvalue != null)
			{
				valueTypes.Add(new ValueTypeInfo
				{
					Name = "MinValue",
					Value = datacontentType3.minvalue
				});
				valueTypes.Add(new ValueTypeInfo
				{
					Name = "MaxValue",
					Value = datacontentType3.maxvalue
				});
			}
			if (datacontentType3.resolution != null)
			{
				valueTypes.Add(new ValueTypeInfo
				{
					Name = "StepValue",
					Value = datacontentType3.resolution
				});
			}
			break;
		}
		case datatype.@int:
		{
			property.ValueType = "Int";
			datacontentType datacontentType2 = (datacontentType)typeContent.Base;
			if (datacontentType2.minvalue != null && datacontentType2.maxvalue != null)
			{
				valueTypes.Add(new ValueTypeInfo
				{
					Name = "MinValue",
					Value = datacontentType2.minvalue
				});
				valueTypes.Add(new ValueTypeInfo
				{
					Name = "MaxValue",
					Value = datacontentType2.maxvalue
				});
				if (datacontentType2.minvalue == "0" && datacontentType2.maxvalue == "100")
				{
					property.ValueType = "Slider";
				}
			}
			if (datacontentType2.resolution != null)
			{
				valueTypes.Add(new ValueTypeInfo
				{
					Name = "StepValue",
					Value = datacontentType2.resolution
				});
			}
			break;
		}
		case datatype.@string:
			property.ValueType = "String";
			break;
		case datatype.@bool:
			property.ValueType = "Tick";
			valueTypes.AddRange(PropertyTypesHelper.GetBoolValueTypeInfos());
			break;
		case datatype.ptzcontrol:
			property.ValueType = "String";
			result = false;
			break;
		case datatype.ptzpresets:
			property.ValueType = "String";
			result = false;
			break;
		}
		if (string.IsNullOrEmpty(currentValue))
		{
			property.Value = defaultValue;
		}
		property.ValueTypeInfos = valueTypes.ToArray();
		return result;
	}

	private static void ConstructPropertyValueTypeEnum(IGeminiRequestContext requestContext, TypeContent typeContent, string currentValue, Property property, string defaultValue, List<ValueTypeInfo> valueTypes)
	{
		bool flag = requestContext.FeatureCacheManager.IsFeatureLicensed(requestContext, "AACAudioSupport");
		property.ValueType = "Enum";
		string text = currentValue;
		string text2 = defaultValue;
		foreach (EnumerationValue value in typeContent.AsEnumeration.Values)
		{
			if (flag || !DefinitionUtility.IsEnumerationValueAac(value))
			{
				valueTypes.Add(new ValueTypeInfo
				{
					Name = value.DisplayNameResolved,
					TranslationId = value.DisplayNameReferenceId.ToString(),
					Value = value.Name
				});
				if (currentValue == value.DisplayNameResolved)
				{
					text = value.Name;
				}
				if (defaultValue == value.DisplayNameResolved)
				{
					text2 = value.Name;
				}
			}
		}
		if (text != currentValue || defaultValue != text2)
		{
			Trace.WriteLine("Property current-value changed from " + currentValue + " to " + text + ", default-value changed from " + defaultValue + " to " + text2);
			currentValue = text;
			defaultValue = text2;
		}
		if (string.IsNullOrEmpty(currentValue))
		{
			property.Value = defaultValue;
		}
		else
		{
			property.Value = currentValue;
		}
		property.ValueTypeInfos = valueTypes.ToArray();
	}

	private static ConfigurationItem ConstructDriverSettingsConfigurationItem(ConfigurationItemPath parentPath, string itemType)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = itemType;
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeDeviceDriverSettings;
		configurationItem.ParentPath = parentPath.Text;
		ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath(itemType, parentPath.Id);
		configurationItem.Path = configurationItemPath.Text;
		configurationItem.EnableProperty = null;
		configurationItem.Properties = new Property[0];
		configurationItem.ChildrenFilled = false;
		return configurationItem;
	}

	public static List<ErrorResult> ConvertToDriverSettings(ConfigurationItem item, MainDefinition mainDefinition, out DriverSettings updatedDriverSettings)
	{
		List<ErrorResult> list = new List<ErrorResult>();
		updatedDriverSettings = new DriverSettings();
		if (item.ItemType == "DeviceDriverSettings" || item.ItemType == "HardwareDriverSettings" || item.ItemType == "HardwarePtzSettings")
		{
			if (item.Children != null)
			{
				ConfigurationItem[] children = item.Children;
				foreach (ConfigurationItem configurationItem in children)
				{
					if (configurationItem.ItemType != "PTZSessionTimeout")
					{
						list.AddRange(ConvertProperties(configurationItem, mainDefinition, updatedDriverSettings));
					}
				}
			}
		}
		else
		{
			list.AddRange(ConvertProperties(item, mainDefinition, updatedDriverSettings));
		}
		return list;
	}

	private static List<ErrorResult> ConvertProperties(ConfigurationItem configurationItem, MainDefinition mainDefinition, DriverSettings updatedDriverSettings)
	{
		List<ErrorResult> list = new List<ErrorResult>();
		Property[] properties = configurationItem.Properties;
		foreach (Property property in properties)
		{
			if (!property.IsSettable)
			{
				continue;
			}
			string[] array = property.Key.Split('/');
			if (array.Length != 3)
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorPropertyKeyInvalid, property.Key, property.DisplayName),
					ErrorTextId = "ErrorPropertyKeyInvalid"
				});
				continue;
			}
			Hwid hwid = Hwid.ParseHwid(array[0]);
			CapabilitiesCollection capabilitiesByHwid;
			try
			{
				capabilitiesByHwid = mainDefinition.GetCapabilitiesByHwid(hwid);
			}
			catch (ArgumentException)
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorPropertyKeyInvalid, property.Key, property.DisplayName),
					ErrorTextId = "ErrorPropertyKeyInvalid"
				});
				continue;
			}
			string name = HttpUtility.UrlDecode(array[1]);
			if (!capabilitiesByHwid.TryGetValue(name, out var capabilityDefinition))
			{
				list.Add(new ErrorResult
				{
					ErrorProperty = property.Key,
					ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorPropertyKeyInvalid, property.Key, property.DisplayName),
					ErrorTextId = "ErrorPropertyKeyInvalid"
				});
				continue;
			}
			if (!ValidateProperty(capabilityDefinition, property, out var errorResult))
			{
				list.Add(errorResult);
				continue;
			}
			string text = property.Value;
			if (capabilityDefinition.TypeDefinition.ActiveTypeName == typename.data && capabilityDefinition.TypeDefinition.ActiveDataType == datatype.@bool)
			{
				text = text.ToLower(CultureInfo.InvariantCulture);
			}
			updatedDriverSettings.CreateOrUpdate(hwid.Text, name, text, array[2]);
		}
		return list;
	}

	private static bool ValidateProperty(CapabilityDefinition capabilityDefinition, Property property, out ErrorResult errorResult)
	{
		errorResult = null;
		try
		{
			if (capabilityDefinition.TypeDefinition.ActiveTypeName == typename.enumeration)
			{
				TypeContent activeTypeContent = capabilityDefinition.TypeDefinition.GetActiveTypeContent();
				if (activeTypeContent == null)
				{
					errorResult = ConstructErrorResult(ConfigurationApiResources.ErrorPropertyInvalidDefinition, "ErrorPropertyInvalidDefinition");
					return false;
				}
				if (!activeTypeContent.AsEnumeration.GetActiveEnumerationValues().Any((enumValueType t) => t.name == property.Value))
				{
					errorResult = new ErrorResult
					{
						ErrorProperty = property.Key,
						ErrorText = string.Format(CultureInfo.InvariantCulture, ConfigurationApiResources.ErrorPropertyInvalid, property.DisplayName, "invalid value"),
						ErrorTextId = "ErrorPropertyInvalid"
					};
					return false;
				}
			}
		}
		catch (ActiveTypeDefinitionMissingException)
		{
			errorResult = ConstructErrorResult(ConfigurationApiResources.ErrorPropertyInvalidDefinition, "ErrorPropertyInvalidDefinition");
			return false;
		}
		return true;
	}

	private static ErrorResult ConstructErrorResult(string resourceText, string resourceTextId, UpdateDriverSettingsException updateDriverSettingsException)
	{
		return ConstructErrorResult(resourceText, resourceTextId, updateDriverSettingsException.Hwid, updateDriverSettingsException.CapabilityName, updateDriverSettingsException.TypeReferenceId);
	}

	private static ErrorResult ConstructErrorResult(string resourceText, string resourceTextId, string hwid, string capabilityName, Guid displayNameReferenceId)
	{
		string errorProperty = string.Format(CultureInfo.InvariantCulture, "{0}/{1}/{2}", hwid, capabilityName, displayNameReferenceId.ToString());
		return new ErrorResult
		{
			ErrorProperty = errorProperty,
			ErrorText = string.Format(CultureInfo.InvariantCulture, resourceText, capabilityName),
			ErrorTextId = resourceTextId
		};
	}

	public static ErrorResult ConstructErrorResult(string resourceText, string resourceTextId)
	{
		return new ErrorResult
		{
			ErrorProperty = "",
			ErrorText = resourceText,
			ErrorTextId = resourceTextId
		};
	}

	public static ErrorResult ConvertException(UpdateDriverSettingsException e)
	{
		if (e is UpdateDriverSettingsValueOutOfRangeException)
		{
			return ConstructErrorResult(ConfigurationApiResources.ErrorOutOfRange, "ErrorOutOfRange", e);
		}
		if (e is UpdateDriverSettingsNotSupportedException)
		{
			return ConstructErrorResult(ConfigurationApiResources.ErrorPropertyTypeNotSupported, "ErrorPropertyTypeNotSupported", e);
		}
		if (e is UpdateDriverSettingsInvalidSettingNameException)
		{
			return ConstructErrorResult(ConfigurationApiResources.ErrorPropertyInvalidDefinition, "ErrorPropertyInvalidDefinition", e);
		}
		if (e is UpdateDriverSettingsSettingReadOnlyException)
		{
			return ConstructErrorResult(ConfigurationApiResources.ErrorPropertyInvalidDefinition, "ErrorPropertyInvalidDefinition", e);
		}
		if (e is UpdateDriverSettingsValueNotLicensedException)
		{
			return ConstructErrorResult(ConfigurationApiResources.ErrorPropertyNotLicensed, "ErrorPropertyNotLicensed", e);
		}
		throw new InternalServerException("Exception handling not implemented for " + e.GetType());
	}
}
