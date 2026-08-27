using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPIHardwareDriver")]
[EntityName("ConfigAPIHardwareDriverFolder")]
public class HardwareDriverConfigurationApi : ConfigurationApiBase, IConfigurationApiSupport
{
	public ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		throw new NotSupportedException();
	}

	public ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			if (string.Equals(path.ItemType, "HardwareDriverFolder", StringComparison.Ordinal))
			{
				string id = path.Id;
				HardwareDriver[] array = HardwareDriver.QueryActiveHardwareDrivers(requestContext, id);
				List<ConfigurationItem> list = new List<ConfigurationItem>();
				HardwareDriver[] array2 = array;
				foreach (HardwareDriver hardwareDriver in array2)
				{
					ConfigurationItemPath configurationItemPath = new ConfigurationItemPathBuilder("HardwareDriverFolder", id, "RecordingServer").CreatePath();
					list.Add(ConvertToConfigurationItem(configurationItemPath.Text, hardwareDriver));
				}
				return list.ToArray();
			}
		}
		else if (string.Equals(path.ItemType, "HardwareDriver", StringComparison.Ordinal))
		{
			return new ConfigurationItem[0];
		}
		throw new NotSupportedException();
	}

	public ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (path == null)
		{
			throw new ArgumentNullException("path");
		}
		ThrowIfPathInvalid(path);
		if (path.IsFolder)
		{
			if (string.Equals(path.ParentItemType, "RecordingServer", StringComparison.Ordinal))
			{
				string id = path.Id;
				if (!RecordingServer.RecordingServerExists(requestContext, id))
				{
					throw new PathNotFoundException(path.Text);
				}
				ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath("RecordingServer", path.Id);
				return GroupConfigurationItemFactory.CreateFolderItem(path.ItemType, parentPath);
			}
			throw new NotSupportedException();
		}
		string.Equals(path.ItemType, "HardwareDriver", StringComparison.Ordinal);
		throw new NotSupportedException();
	}

	public ConfigurationItem InvokeMethod(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath, string methodId)
	{
		throw new NotSupportedException();
	}

	private static void ThrowIfPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder)
		{
			if (!string.Equals(path.ParentItemType, "RecordingServer", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Invalid parent. " + path.Text);
			}
			if (!string.Equals(path.ItemType, "HardwareDriverFolder", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Parent type and item type mismatch. " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Id format invalid. " + path.Text);
			}
		}
		else
		{
			if (!string.Equals(path.ItemType, "HardwareDriver", StringComparison.Ordinal))
			{
				throw new PathInvalidException("Unsupported item type. " + path.Text);
			}
			if (!path.Id.IsGuid())
			{
				throw new PathInvalidException("Id format invalid. " + path.Text);
			}
		}
	}

	private static ConfigurationItem ConvertToConfigurationItem(string parentPath, HardwareDriver hardwareDriver)
	{
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPathBuilder("HardwareDriver", hardwareDriver.Id).CreatePath();
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "HardwareDriver";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = hardwareDriver.Name;
		configurationItem.ParentPath = parentPath;
		configurationItem.Path = configurationItemPath.Text;
		configurationItem.Children = new ConfigurationItem[0];
		configurationItem.ChildrenFilled = true;
		configurationItem.Properties = new List<Property>
		{
			GeneralPropertyDefinitions.GetIdPropertyGuid(hardwareDriver.Id),
			GeneralPropertyDefinitions.GetNameProperty(hardwareDriver.Name),
			HardwareDriverPropertyDefinition.GroupNameProperty(hardwareDriver.GroupName),
			HardwareDriverPropertyDefinition.DriverNumberProperty(hardwareDriver.Number),
			HardwareDriverPropertyDefinition.UseCountProperty(hardwareDriver.UseCount),
			HardwareDriverPropertyDefinition.DriverVersionProperty(hardwareDriver.DriverVersion),
			HardwareDriverPropertyDefinition.DriverRevisionProperty(hardwareDriver.DriverRevision),
			HardwareDriverPropertyDefinition.DriverTypeProperty(hardwareDriver.DriverType)
		}.ToArray();
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		configurationItem.Children = list.ToArray();
		return configurationItem;
	}
}
