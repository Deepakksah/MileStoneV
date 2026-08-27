using System;
using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Admin;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.Util.VideoWall;

public class LayoutItemManager : ItemManager
{
	private ServerId _serverId;

	public LayoutItemManager()
	{
	}

	public override void Init()
	{
		if (_serverId == null)
		{
			_serverId = EnvironmentManager.Instance.MasterSite.ServerId;
		}
	}

	public override void Close()
	{
	}

	public override Item GetItem(FQID fqid)
	{
		if (fqid == null)
		{
			throw new ArgumentNullException("fqid");
		}
		if (!Factory.IsLoginEstablished(fqid.ServerId))
		{
			return null;
		}
		if (fqid.ObjectId == Kind.Layout)
		{
			return new Item(fqid, Strings.AllLayouts);
		}
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(fqid);
		ConfigurationApiItemWrapper configurationApiItemWrapper;
		try
		{
			configurationApiItemWrapper = new ConfigurationApiItemWrapper(fqid.ServerId, configurationItemPath.Path);
		}
		catch (FaultException<ArgumentExceptionFault>)
		{
			return null;
		}
		if (!configurationApiItemWrapper.IsValid)
		{
			return null;
		}
		ItemWrapper itemWrapper = new ItemWrapper(fqid, configurationApiItemWrapper.ConfigurationItem);
		itemWrapper.HasChildren = ConfigurationApiClassConverter.HasChildren(itemWrapper.FQID.Kind);
		return itemWrapper;
	}

	public override List<Item> GetItems()
	{
		return new List<Item>();
	}

	public override List<Item> GetItems(Item parentItem)
	{
		EnvironmentManager.Instance.Log(error: false, GetType().Name, "Unexpected call to GetItems(parentItem) - not implemented");
		return new List<Item>();
	}
}
