using System;
using System.Collections.Generic;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.Util.VideoWall;

public class ItemWrapper : Item
{
	private ConfigurationApiItemWrapper _wrapper;

	public ItemWrapper(ServerId serverId, Guid parentId, Guid objectId, string name, FolderType folderType, Guid kind, ConfigurationApiItemWrapper wrapper)
		: base(serverId, parentId, objectId, name, folderType, kind)
	{
		_wrapper = wrapper;
	}

	public ItemWrapper(FQID fqid, ConfigurationItem configurationItem)
		: base(ConfigurationApiClassConverter.ConvertToFQID(fqid.ServerId, configurationItem), configurationItem.DisplayName)
	{
		_wrapper = new ConfigurationApiItemWrapper(base.FQID.ServerId, configurationItem);
		Property[] properties = configurationItem.Properties;
		foreach (Property property in properties)
		{
			if (property.Key != "Id")
			{
				base.Properties[property.Key] = property.Value;
			}
		}
	}

	public override List<Item> GetChildren()
	{
		List<Item> list = new List<Item>();
		ConfigurationItem[] array = _wrapper.Children();
		foreach (ConfigurationItem configurationItem in array)
		{
			if (!configurationItem.ItemType.EndsWith("Folder"))
			{
				try
				{
					ItemWrapper itemWrapper = new ItemWrapper(FQID, configurationItem);
					itemWrapper.HasChildren = ConfigurationApiClassConverter.HasChildren(itemWrapper.FQID.Kind);
					list.Add(itemWrapper);
				}
				catch (Exception ex)
				{
					EnvironmentManager.Instance.Log(error: true, "ItemWrapper", ex.Message);
				}
				continue;
			}
			if (!configurationItem.ChildrenFilled)
			{
				IConfigurationClientDecorator configurationClient = ConfigurationApiUtil.GetConfiguration(FQID.ServerId).ConfigurationApiManager.GetConfigurationClient(FQID.ServerId, configurationItem.ItemType);
				configurationItem.Children = configurationClient.GetChildItems(configurationItem.Path);
			}
			ConfigurationItem[] children = configurationItem.Children;
			foreach (ConfigurationItem configurationItem2 in children)
			{
				try
				{
					ItemWrapper itemWrapper2 = new ItemWrapper(FQID, configurationItem2);
					itemWrapper2.HasChildren = ConfigurationApiClassConverter.HasChildren(itemWrapper2.FQID.Kind);
					list.Add(itemWrapper2);
				}
				catch (Exception ex2)
				{
					EnvironmentManager.Instance.Log(error: true, "ItemWrapper", ex2.Message);
				}
			}
		}
		return list;
	}

	public override Item GetParent()
	{
		if (FQID.ParentId == Guid.Empty)
		{
			return null;
		}
		IConfigurationClientDecorator configurationClient = ConfigurationApiUtil.GetConfiguration(FQID.ServerId).ConfigurationApiManager.GetConfigurationClient(FQID.ServerId, _wrapper.ConfigurationItem.ItemType);
		string[] array = _wrapper.ConfigurationItem.ParentPath.Split('/');
		ConfigurationItem item = configurationClient.GetItem(array[0]);
		if (item != null)
		{
			return new ItemWrapper(ConfigurationApiClassConverter.ConvertToFQID(FQID.ServerId, item), item);
		}
		return null;
	}
}
