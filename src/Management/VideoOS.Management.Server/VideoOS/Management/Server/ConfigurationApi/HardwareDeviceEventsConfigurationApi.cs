using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIHardwareDeviceEvent")]
[EntityName("ConfigAPIHardwareDeviceEventFolder")]
public class HardwareDeviceEventsConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		ValidateResult validateResult;
		DateTime? lastModified;
		try
		{
			validateResult = ValidateItemInternal(requestContext, item, path, out lastModified);
		}
		catch (UnknownDeviceException)
		{
			throw new PathNotFoundException(path.Text);
		}
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		DeviceEvents.UpdateDeviceEvents(requestContext, path.Id, ConvertConfigurationItemToDeviceEvents(path, item), lastModified);
		return new ValidateResult
		{
			ValidatedOk = true
		};
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path);
		ThrowIfPathInvalid(parentPath);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (!DeviceEvents.EventExists(requestContext, path.Id))
		{
			throw new PathNotFoundException(path.Text);
		}
		try
		{
			DateTime? lastModified;
			return ValidateItemInternal(requestContext, item, path, out lastModified);
		}
		catch (UnknownDeviceException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ThrowIfAnyParameterNull(requestContext, path);
		ThrowIfPathInvalid(path);
		if (!path.IsFolder)
		{
			return new ConfigurationItem[0];
		}
		return new ConfigurationItem[1] { GetHardwareDeviceEventsConfigurationItem(requestContext, path) };
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ThrowIfAnyParameterNull(requestContext, path);
		ThrowIfPathInvalid(path);
		if (string.Equals(path.ItemType, "HardwareDeviceEventFolder", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("HardwareDeviceEventFolder", parentPath);
		}
		if (string.Equals(path.ItemType, "HardwareDeviceEvent", StringComparison.OrdinalIgnoreCase))
		{
			return GetHardwareDeviceEventsConfigurationItem(requestContext, path);
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotImplementedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out DateTime? lastModified)
	{
		if (item.Children == null)
		{
			throw new ArgumentException("The children property of the item is null.", "item");
		}
		lastModified = null;
		try
		{
			if (((IEnumerable<Property>)item.Properties).TryGetPropertyValue("LastModified", out lastModified))
			{
				lastModified = lastModified.Value.ToUniversalTime();
			}
			if (string.Equals(path.ItemType, "HardwareDeviceEvent", StringComparison.Ordinal))
			{
				ConfigurationItem[] children = item.Children;
				foreach (ConfigurationItem configurationItem in children)
				{
					string value = null;
					string value2 = null;
					bool? value3 = null;
					if (configurationItem.Properties != null)
					{
						((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Name", out value);
						((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Description", out value2);
						((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("EventUsed", out value3);
					}
					DeviceEvents.UpdateDeviceEventValidation(requestContext, path.Id, value, value2, value3);
				}
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = item
				};
			}
			throw new NotSupportedException();
		}
		catch (ArgumentNullException exception)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
		catch (ArgumentException exception2)
		{
			ValidateResult validateResult = ErrorHelper.CreateValidateResult(exception2, item.Properties);
			if (validateResult != null)
			{
				return validateResult;
			}
			throw;
		}
	}

	private static ConfigurationItem GetHardwareDeviceEventsConfigurationItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		try
		{
			DateTime lastModified;
			IEnumerable<DeviceEvent> deviceEvents = (DeviceEvents.IsDeviceHardware(requestContext, path.Id) ? DeviceEvents.QueryHardwareEvents(requestContext, path, out lastModified) : DeviceEvents.QueryDeviceEvents(requestContext, path, out lastModified));
			ConfigurationItemPath parentPath;
			if (path.ItemType == "HardwareDeviceEventFolder")
			{
				parentPath = path;
			}
			else if (Hardware.QueryHardwareById(requestContext, path.Id) != null)
			{
				parentPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateItemPath("Hardware", path.Id), "HardwareDeviceEventFolder");
			}
			else
			{
				Device device = Device.QueryDeviceById(requestContext, path.Id);
				if (device == null)
				{
					throw new PathNotFoundException();
				}
				parentPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateItemPath(DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == device.DeviceType).ItemType, path.Id), "HardwareDeviceEventFolder");
			}
			return ConvertToHardwareDeviceEventConfigurationItem(path, parentPath, deviceEvents, lastModified);
		}
		catch (DeviceNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem ConvertToHardwareDeviceEventConfigurationItem(ConfigurationItemPath path, ConfigurationItemPath parentPath, IEnumerable<DeviceEvent> deviceEvents, DateTime lastModified)
	{
		ConfigurationItemPath hardwareDeviceEventPath = ConfigurationItemPath.CreateItemPath("HardwareDeviceEvent", path.Id);
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "HardwareDeviceEvent";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeHardwareDeviceEventFolder;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = hardwareDeviceEventPath.Text;
		configurationItem.Properties = new Property[1] { GeneralPropertyDefinitions.GetLastModifiedProperty(lastModified) };
		configurationItem.MethodIds = new string[0];
		configurationItem.ChildrenFilled = true;
		List<ConfigurationItem> list = deviceEvents.Select((DeviceEvent deviceEvent) => ConvertToEventChildConfigurationItem(parentPath, hardwareDeviceEventPath, deviceEvent)).ToList();
		configurationItem.Children = list.ToArray();
		return configurationItem;
	}

	private static ConfigurationItem ConvertToEventChildConfigurationItem(ConfigurationItemPath parentPath, ConfigurationItemPath path, DeviceEvent deviceEvent)
	{
		List<Property> list = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(deviceEvent.EventId),
			HardwareDeviceEventPropertyDefinition.EventUsedProperty(deviceEvent.IsUsed)
		};
		if (deviceEvent.EventIndex != null)
		{
			list.Add(HardwareDeviceEventPropertyDefinition.EventIndexProperty(deviceEvent.EventIndex, deviceEvent.EventIndexDisplayName));
		}
		if (deviceEvent.DriverEventId != null)
		{
			list.Add(HardwareDeviceEventPropertyDefinition.DriverEventIdProperty(deviceEvent.DriverEventId));
		}
		return new ConfigurationItem
		{
			ItemCategory = ItemCategories.ChildItem,
			ItemType = "HardwareDeviceEvent",
			Path = path.Text,
			DisplayName = deviceEvent.EventName,
			EnableProperty = new EnablePropertyInfo
			{
				Enabled = deviceEvent.IsEnabled,
				DisplayName = deviceEvent.EventName
			},
			ParentPath = parentPath.Text,
			UIHint = "DirectionHorizontal",
			Properties = list.ToArray()
		};
	}

	private static IEnumerable<DeviceEvent> ConvertConfigurationItemToDeviceEvents(ConfigurationItemPath path, ConfigurationItem item)
	{
		List<DeviceEvent> list = new List<DeviceEvent>();
		ConfigurationItem[] children = item.Children;
		foreach (ConfigurationItem configurationItem in children)
		{
			if (configurationItem.Properties != null)
			{
				((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("Id", out string value);
				((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("EventUsed", out bool? value2);
				((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("EventIndex", out string value3);
				((IEnumerable<Property>)configurationItem.Properties).TryGetPropertyValue("DriverEventId", out string value4);
				DeviceEvent item2 = new DeviceEvent
				{
					EventId = value,
					DeviceId = path.Id,
					EventName = configurationItem.DisplayName,
					IsUsed = (value2 == true),
					IsEnabled = configurationItem.EnableProperty.Enabled,
					EventIndex = value3,
					DriverEventId = value4
				};
				list.Add(item2);
			}
		}
		return list;
	}

	private static void ThrowIfAnyParameterNull(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!CheckIfParentPathSupported(path))
			{
				throw new PathInvalidException("Invalid parent. Path: " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Invalid id. Path: " + path.Text);
			}
		}
		else if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
	}

	private static bool CheckIfParentPathSupported(ConfigurationItemPath folderPath)
	{
		switch (folderPath.ParentItemType)
		{
		case "Hardware":
		case "Metadata":
		case "Camera":
		case "Output":
		case "InputEvent":
		case "Microphone":
		case "Speaker":
			return true;
		default:
			return false;
		}
	}
}
