using System;
using System.Collections.Generic;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Util.VideoWall;

namespace VideoOS.Platform.Util.GisMap;

public class GisMapLocationItemWrapper : Item
{
	private ConfigurationApiItemWrapper _wrapper;

	public override HasChildren HasChildren
	{
		get
		{
			if (base.FQID.FolderType != FolderType.No)
			{
				return HasChildren.Maybe;
			}
			return HasChildren.No;
		}
		set
		{
		}
	}

	public GisMapLocationItemWrapper(ServerId serverId, Guid parentId, Guid objectId, string name, FolderType folderType, Guid kind, ConfigurationApiItemWrapper wrapper)
		: base(serverId, parentId, objectId, name, folderType, kind)
	{
		_wrapper = wrapper;
	}

	public GisMapLocationItemWrapper(FQID fqid, ConfigurationItem configurationItem)
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
			try
			{
				GisMapLocationItemWrapper gisMapLocationItemWrapper = new GisMapLocationItemWrapper(FQID, configurationItem);
				gisMapLocationItemWrapper.HasChildren = HasChildren.No;
				list.Add(gisMapLocationItemWrapper);
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: true, "ItemWrapper", ex.Message);
			}
		}
		return list;
	}

	public override Item GetParent()
	{
		return null;
	}
}
