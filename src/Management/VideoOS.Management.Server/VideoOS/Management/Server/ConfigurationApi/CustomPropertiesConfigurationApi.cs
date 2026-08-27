using System;
using System.Collections.Generic;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

[EntityName("ConfigAPICustomProperties")]
[EntityName("ConfigAPICustomPropertiesFolder")]
public class CustomPropertiesConfigurationApi : CustomPropertiesConfigurationApiObjectBase
{
	public CustomPropertiesConfigurationApi(ICustomPropertiesRepository repository)
		: base(repository)
	{
	}

	public CustomPropertiesConfigurationApi()
		: this(null)
	{
	}

	public override ValidateResult SetItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path, requestContext);
		ThrowIfPathInvalid(parentPath, requestContext);
		if (path.IsFolder)
		{
			throw new NotSupportedException();
		}
		if (!string.Equals(item.ItemType, "CustomProperties", StringComparison.OrdinalIgnoreCase))
		{
			throw new NotSupportedException();
		}
		if (item.Children == null)
		{
			item.Children = new ConfigurationItem[0];
		}
		string id = parentPath.Id;
		try
		{
			List<CustomProperty> list = new List<CustomProperty>();
			Property[] properties = item.Properties;
			foreach (Property property in properties)
			{
				list.Add(new CustomProperty
				{
					Name = property.Key,
					Value = property.Value
				});
			}
			CustomProperty[] array = list.ToArray();
			base.Repository.UpdateCustomProperties(requestContext, id, array);
			return new ValidateResult
			{
				ValidatedOk = true,
				ResultItem = ConvertToCustomPropertyConfigurationItem(path, array)
			};
		}
		catch (ObjectNotFoundException)
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	public override ConfigurationItem[] GetChildItems(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path, requestContext);
		if (path.IsFolder)
		{
			CustomProperty[] customProperties = base.Repository.GetCustomProperties(requestContext, path.Id);
			ConfigurationItem configurationItem = ConvertToCustomPropertyConfigurationItem(path, customProperties);
			return new ConfigurationItem[1] { configurationItem };
		}
		return new ConfigurationItem[0];
	}

	public override ConfigurationItem GetItem(IGeminiRequestContext requestContext, ConfigurationItemPath path)
	{
		ConfigurationApiBase.ThrowIfArgumentsInvalid(requestContext, path);
		ThrowIfPathInvalid(path, requestContext);
		if (string.Equals(path.ItemType, "CustomPropertiesFolder", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath = ConfigurationItemPath.CreateItemPath(path.ParentItemType, path.Id);
			return GroupConfigurationItemFactory.CreateFolderItem("CustomPropertiesFolder", parentPath);
		}
		if (string.Equals(path.ItemType, "CustomProperties", StringComparison.OrdinalIgnoreCase))
		{
			ConfigurationItemPath parentPath2 = ConfigurationItemPath.CreateFolderPath(base.Repository.GetDeviceType(requestContext, path.Id), path.Id, "CustomPropertiesFolder");
			CustomProperty[] customProperties = base.Repository.GetCustomProperties(requestContext, path.Id);
			return ConvertToCustomPropertyConfigurationItem(parentPath2, customProperties);
		}
		throw new NotSupportedException();
	}

	public override ValidateResult ValidateItem(IGeminiRequestContext requestContext, ConfigurationItem item, ConfigurationItemPath path, ConfigurationItemPath parentPath)
	{
		ConfigurationApiBase.ThrowIfAnyParameterNull(requestContext, item, path, parentPath);
		ThrowIfPathInvalid(path, requestContext);
		ThrowIfPathInvalid(parentPath, requestContext);
		return new ValidateResult
		{
			ValidatedOk = true,
			ResultItem = item
		};
	}

	protected override void ThrowIfParentPathInvalid(ConfigurationItemPath path)
	{
		if (path.IsFolder && path.Id.IsGuid())
		{
			return;
		}
		throw new PathInvalidException("Invalid id. Path: " + path.Text);
	}

	protected override void ThrowIfPathInvalid(ConfigurationItemPath path, IGeminiRequestContext requestContext)
	{
		if (!path.Id.IsGuid())
		{
			throw new PathInvalidException("Invalid id. Path: " + path.Text);
		}
		string id = path.Id;
		if (!base.Repository.DeviceExists(requestContext, id))
		{
			throw new PathNotFoundException(path.Text);
		}
	}

	private static ConfigurationItem ConvertToCustomPropertyConfigurationItem(ConfigurationItemPath parentPath, CustomProperty[] customProperties)
	{
		ConfigurationItem configurationItem = new ConfigurationItem();
		configurationItem.ItemType = "CustomProperties";
		configurationItem.UIHint = "DirectionHorizontal";
		configurationItem.ItemCategory = ItemCategories.Item;
		configurationItem.DisplayName = ConfigurationApiResources.ItemTypeCustomProperties;
		configurationItem.ParentPath = parentPath.Text;
		configurationItem.Path = ConfigurationItemPath.CreateItemPath(configurationItem.ItemType, parentPath.Id).Text;
		List<Property> list = new List<Property>();
		configurationItem.MethodIds = new string[0];
		foreach (CustomProperty customProperty in customProperties)
		{
			Property item = new Property(customProperty.Name, customProperty.Value)
			{
				ValueType = "String",
				IsSettable = true
			};
			list.Add(item);
		}
		configurationItem.Properties = list.ToArray();
		configurationItem.Children = new ConfigurationItem[0];
		configurationItem.ChildrenFilled = true;
		return configurationItem;
	}
}
