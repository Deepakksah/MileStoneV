#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIClientSettings")]
[EntityName("ConfigAPIClientSettingsFolder")]
public class ClientSettingsConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public const string RelatedKey = "Related";

	public const string ShortcutKey = "Shortcut";

	public const string MulticastEnabledKey = "MulticastEnabled";

	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path, requestContext);
		ThrowIfPathInvalid(parentPath, requestContext);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		Device device = Device.QueryDeviceById(requestContext, path.Id);
		List<ConfigurationItemPath> deviceConfigurationItemPaths;
		bool relatedUpdated;
		string shortcut;
		bool? multicastEnabled;
		ValidateResult validateResult = ValidateItemInternal(requestContext, item, path, out deviceConfigurationItemPaths, out relatedUpdated, out shortcut, out multicastEnabled);
		if (!validateResult.ValidatedOk)
		{
			return validateResult;
		}
		if (string.CompareOrdinal(device.Shortcut, shortcut) != 0 || (multicastEnabled.HasValue && device.MulticastEnabled != multicastEnabled))
		{
			device.Shortcut = shortcut;
			Device.UpdateDevice(requestContext, path.Id, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, shortcut, multicastEnabled, null, null, null, null, null);
		}
		if (relatedUpdated)
		{
			List<Guid> list = new List<Guid>();
			foreach (ConfigurationItemPath item2 in deviceConfigurationItemPaths)
			{
				list.Add(new Guid(item2.Id));
			}
			DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.First((DeviceTypeRelation r) => r.DeviceType == device.DeviceType);
			List<Guid> related = GetRelated(requestContext, path, deviceTypeRelation.ItemType);
			foreach (Guid item3 in related)
			{
				if (!list.Contains(item3))
				{
					Guid guid = item3;
					Trace.WriteLine("Remove related: " + guid.ToString());
					DeviceLink.RemoveDeviceLink(requestContext, path.Id, item3.ToString());
				}
			}
			foreach (Guid item4 in list)
			{
				if (!related.Contains(item4))
				{
					Guid guid = item4;
					Trace.WriteLine("Add related: " + guid.ToString());
					DeviceLink.AddDeviceLink(requestContext, path.Id, item4.ToString());
				}
			}
		}
		return validateResult;
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path, requestContext);
		ThrowIfPathInvalid(parentPath, requestContext);
		List<ConfigurationItemPath> deviceConfigurationItemPaths;
		bool relatedUpdated;
		string shortcut;
		bool? multicastEnabled;
		if (!path.IsFolder)
		{
			return ValidateItemInternal(requestContext, item, path, out deviceConfigurationItemPaths, out relatedUpdated, out shortcut, out multicastEnabled);
		}
		throw new NotSupportedException();
	}

	private static ValidateResult ValidateItemInternal(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, out List<ConfigurationItemPath> deviceConfigurationItemPaths, out bool relatedUpdated, out string shortcut, out bool? multicastEnabled)
	{
		deviceConfigurationItemPaths = new List<ConfigurationItemPath>();
		relatedUpdated = false;
		shortcut = "";
		multicastEnabled = null;
		try
		{
			if (!path.IsFolder)
			{
				string value = null;
				Property property = item.Properties.First((Property p) => p.Key == "Related");
				if (property != null && property.IsSettable && ((IEnumerable<Property>)item.Properties).TryGetPropertyValue("Related", out value))
				{
					string[] array = value.Split(',', ';');
					foreach (string text in array)
					{
						if (!string.IsNullOrEmpty(text))
						{
							ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreatePath(text);
							if (Device.QueryDeviceById(requestContext, configurationItemPath.Id) == null)
							{
								throw new PathNotFoundException(path.Text);
							}
							deviceConfigurationItemPaths.Add(configurationItemPath);
						}
					}
					relatedUpdated = true;
				}
				Property property2 = item.Properties.FirstOrDefault((Property p) => p.Key == "Shortcut");
				if (property2 != null)
				{
					shortcut = property2.Value;
					if (!string.IsNullOrEmpty(shortcut) && !int.TryParse(shortcut, out var _))
					{
						return ErrorHelper.ErrorResultPropertyInvalid(property2, string.Empty);
					}
				}
				Property property3 = item.Properties.FirstOrDefault((Property p) => p.Key == "MulticastEnabled");
				if (property3 != null)
				{
					if (!bool.TryParse(property3.Value, out var result2))
					{
						return ErrorHelper.ErrorResultPropertyInvalid(property3, string.Empty);
					}
					multicastEnabled = result2;
				}
				return new ValidateResult
				{
					ValidatedOk = true,
					ResultItem = item
				};
			}
			throw new NotSupportedException();
		}
		catch (ArgumentException ex)
		{
			ValidateResult validateResult = new ValidateResult();
			validateResult.ErrorResults = new ErrorResult[1]
			{
				new ErrorResult
				{
					ErrorProperty = "Related",
					ErrorText = ex.Message,
					ErrorTextId = "InvalidPath"
				}
			};
			validateResult.ValidatedOk = false;
			validateResult.ResultItem = null;
			return validateResult;
		}
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path, requestContext);
		if (path.IsFolder)
		{
			List<Guid> related = GetRelated(requestContext, path, path.ParentItemType);
			return new ConfigurationItem[1] { ConvertToClientSettingsConfigurationItem(requestContext, path, related) };
		}
		return new ConfigurationItem[0];
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path, requestContext);
		if (string.Equals(path.ItemType, "ClientSettingsFolder", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("ClientSettingsFolder", parentPath);
		}
		if (string.Equals(path.ItemType, "ClientSettings", StringComparison.OrdinalIgnoreCase))
		{
			Device device = Device.QueryDeviceById(requestContext, path.Id);
			DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.First((DeviceTypeRelation r) => r.DeviceType == device.DeviceType);
			List<Guid> related = GetRelated(requestContext, path, deviceTypeRelation.ItemType);
			ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateFolderPath(deviceTypeRelation.ItemType, path.Id, "ClientSettingsFolder");
			return ConvertToClientSettingsConfigurationItem(requestContext, parentPath2, related);
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string method)
	{
		throw new NotSupportedException();
	}

	private static List<Guid> GetRelated(IGeminiRequestContext requestContext, ConfigurationItemPath path, string parentItemType)
	{
		return new List<Guid>((parentItemType == "Camera") ? DeviceLink.QueryDeviceLink(requestContext, path.Id) : DeviceLink.QueryRelatedDeviceLink(requestContext, path.Id));
	}

	protected static void ThrowIfPathInvalid(ConfigurationItemPath path, IGeminiRequestContext requestContext)
	{
		if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
		string id = path.Id;
		if (!Device.DeviceExists(requestContext, id))
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem ConvertToClientSettingsConfigurationItem(IGeminiRequestContext requestContext, ConfigurationItemPath parentPath, List<Guid> deviceLinks)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "ClientSettings";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeClientSettingsProperties;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, parentPath.Id).Text;
		List<Property> list = new List<Property>();
		string text = "";
		bool isSettable = string.CompareOrdinal(parentPath.ParentItemType, "Camera") == 0;
		if (deviceLinks != null)
		{
			foreach (Guid deviceLink in deviceLinks)
			{
				if (!string.IsNullOrEmpty(text))
				{
					text += ",";
				}
				Device relatedDevice = Device.QueryDeviceById(requestContext, deviceLink.ToString());
				if (relatedDevice == null)
				{
					isSettable = false;
					continue;
				}
				ConfigurationItemPath configurationItemPath = ConfigurationItemPath.CreateItemPath(DeviceTypeRelation.Relations.First((DeviceTypeRelation r) => r.DeviceType == relatedDevice.DeviceType).ItemType, deviceLink.ToString());
				text += configurationItemPath.Text;
			}
		}
		list.Add(RelatedProperty(text, isSettable));
		Device device = Device.QueryDeviceById(requestContext, parentPath.Id);
		list.Add(ShortcutProperty(device.Shortcut, isSettable: true));
		list.Add(MulticastEnabledProperty(device.MulticastEnabled, isSettable: true));
		configurationItem.Properties = list.ToArray();
		configurationItem.MethodIds = new string[0];
		configurationItem.Children = new ConfigurationItem[0];
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}

	public static Property RelatedProperty(string val, bool isSettable)
	{
		Property property = new Property("Related", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "PathList",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyRelatedDevices,
			ItemReference = true,
			ServerValidation = false,
			TranslationId = "PropertyRelatedDevices",
			ToolTipTranslationId = "PropertyRelatedDevicesTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
		if (isSettable)
		{
			property.ValueTypeInfos = new ValueTypeInfo[1]
			{
				new ValueTypeInfo
				{
					Name = "PathItemType",
					Value = string.Format(CultureInfo.InvariantCulture, "{0},{1},{2}", "Microphone", "Speaker", "Metadata")
				}
			};
		}
		return property;
	}

	public static Property ShortcutProperty(string val, bool isSettable)
	{
		return new Property("Shortcut", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Int",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyClientSettingsShortcut,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyClientSettingsShortcut",
			ToolTipTranslationId = "PropertyClientSettingsShortcutTip",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}

	private static Property MulticastEnabledProperty(bool val, bool isSettable)
	{
		return new Property("MulticastEnabled", val.ToString(CultureInfo.InvariantCulture))
		{
			ValueType = "Tick",
			IsSettable = isSettable,
			DisplayName = ConfigurationApiResources.PropertyClientSettingsMulticastEnabled,
			ItemReference = false,
			ServerValidation = false,
			TranslationId = "PropertyClientSettingsMulticastEnabled",
			UIImportance = UIImportance.Summary,
			SortKey = 100
		};
	}
}
