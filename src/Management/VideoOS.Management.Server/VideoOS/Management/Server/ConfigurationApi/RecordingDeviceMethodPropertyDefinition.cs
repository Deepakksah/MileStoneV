using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class RecordingDeviceMethodPropertyDefinition
{
	public const string IdentifierPropertyName = "Identifier";

	public const string RelatedTasksPropertyName = "RelatedTasks";

	public const string UserNamePropertyName = "UserName";

	public const string CreationTimePropertyName = "CreationTime";

	public const string DevicePropertyName = "Device";

	public static Property StartTimeProperty(DateTime startTime)
	{
		return new Property("startTime", PropertyTypesHelper.GetValue(startTime))
		{
			ValueType = "DateTime",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceRecordingStartTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceRecordingStartTime"
		};
	}

	public static Property EndTimeProperty(DateTime endTime)
	{
		return new Property("endTime", PropertyTypesHelper.GetValue(endTime))
		{
			ValueType = "DateTime",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceRecordingEndTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceRecordingEndTime"
		};
	}

	public static Property IdentifierProperty(Guid identifier)
	{
		return new Property("Identifier", PropertyTypesHelper.GetValue(identifier))
		{
			ValueType = "String",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceEdgeRetrievalIdentifier,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceEdgeRetrievalIdentifier",
			ToolTipTranslationId = "PropertyDeviceEdgeRetrievalIdentifierToolTip"
		};
	}

	public static Property UserNameProperty(string userName)
	{
		return new Property("UserName", userName)
		{
			ValueType = "String",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyDeviceEdgeRetrievalUserName,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceEdgeRetrievalUserName"
		};
	}

	public static Property CreationTimeProperty(DateTime creationTime)
	{
		return new Property("CreationTime", PropertyTypesHelper.GetValue(creationTime))
		{
			ValueType = "DateTime",
			IsSettable = false,
			DisplayName = ConfigurationApiResources.PropertyDeviceRecordingCreationTime,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceRecordingCreationTime"
		};
	}

	public static Property DeviceProperty(ConfigurationItemPath devicePath)
	{
		Property property = new Property("Device", devicePath.Text);
		property.ValueType = "Path";
		property.IsSettable = false;
		property.DisplayName = ConfigurationApiResources.PropertyDeviceEdgeRetrievalDevicePath;
		property.ItemReference = true;
		property.ServerValidation = false;
		property.TranslationId = "PropertyDeviceEdgeRetrievalDevicePath";
		property.ToolTipTranslationId = "PropertyDeviceEdgeRetrievalDevicePathToolTip";
		property.ValueTypeInfos = new ValueTypeInfo[1]
		{
			new ValueTypeInfo
			{
				Name = "PathItemType",
				Value = "Camera"
			}
		};
		return property;
	}

	public static Property RelatedTasksProperty(IEnumerable<Guid> relatedTasks)
	{
		Property property = new Property("RelatedTasks", "")
		{
			ValueType = "Array",
			IsSettable = true,
			DisplayName = ConfigurationApiResources.PropertyDeviceEdgeRetrievalRelatedTasks,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyDeviceEdgeRetrievalRelatedTasks",
			ToolTipTranslationId = "PropertyDeviceEdgeRetrievalRelatedTasksToolTip",
			UIImportance = UIImportance.Default,
			SortKey = 100
		};
		List<string> list = new List<string>();
		foreach (Guid relatedTask in relatedTasks)
		{
			list.Add(relatedTask.ToString());
		}
		property.ValueArray = list.ToArray();
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
}
