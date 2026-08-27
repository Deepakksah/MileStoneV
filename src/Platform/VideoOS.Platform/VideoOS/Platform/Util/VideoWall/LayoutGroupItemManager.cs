using System;
using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Admin;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.Util.VideoWall;

public class LayoutGroupItemManager : ItemManager
{
	private ServerId _serverId;

	private Guid _myKind;

	private string _myItemType = "LayoutGroupFolder";

	private string _myAllName;

	public LayoutGroupItemManager()
	{
	}

	public override void Init()
	{
		if (_serverId == null)
		{
			_serverId = EnvironmentManager.Instance.MasterSite.ServerId;
			_myKind = Kind.LayoutGroup;
			_myAllName = Strings.AllLayoutGroups;
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
		if (fqid.ServerId.ServerType != "XPCO")
		{
			return null;
		}
		if (fqid.ObjectId == _myKind)
		{
			try
			{
				ConfigurationApiItemWrapper wrapper = new ConfigurationApiItemWrapper(fqid.ServerId, "/" + _myItemType);
				return new ItemWrapper(fqid.ServerId, Guid.Empty, _myKind, _myAllName, FolderType.SystemDefined, _myKind, wrapper)
				{
					HasChildren = HasChildren.Yes
				};
			}
			catch (FaultException<ArgumentExceptionFault>)
			{
				return null;
			}
		}
		ConfigurationItemPath configurationItemPath = new ConfigurationItemPath(fqid);
		ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(fqid.ServerId, configurationItemPath.Path);
		if (!configurationApiItemWrapper.IsValid)
		{
			return null;
		}
		return new ItemWrapper(fqid, configurationApiItemWrapper.ConfigurationItem)
		{
			HasChildren = ConfigurationApiClassConverter.HasChildren(_myKind)
		};
	}

	public override List<Item> GetItems()
	{
		if (!Factory.IsLoginEstablished(_serverId))
		{
			return new List<Item>();
		}
		try
		{
			ConfigurationApiItemWrapper wrapper = new ConfigurationApiItemWrapper(_serverId, "/" + _myItemType);
			return new ItemWrapper(_serverId, Guid.Empty, _myKind, _myAllName, FolderType.SystemDefined, _myKind, wrapper).GetChildren();
		}
		catch (FaultException<ArgumentExceptionFault>)
		{
			return new List<Item>();
		}
	}

	public override List<Item> GetItems(Item parentItem)
	{
		EnvironmentManager.Instance.Log(error: false, GetType().Name, "Unexpected call to GetItems(parentItem) - not implemented");
		return new List<Item>();
	}
}
