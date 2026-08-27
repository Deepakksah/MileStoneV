using System;
using System.Collections.Generic;
using VideoOS.Platform.Admin;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Proxy.ConfigApi;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.Util.GisMap;

public class GisMapLocationItemManager : ItemManager
{
	private ServerId _serverId;

	private Guid _myKind;

	private string _myItemType = "GisMapLocationFolder";

	private string _myAllName;

	public GisMapLocationItemManager()
	{
	}

	public override void Init()
	{
		if (_serverId == null)
		{
			_myKind = Kind.GisMapLocation;
			_serverId = EnvironmentManager.Instance.MasterSite.ServerId;
			_myAllName = Strings.AllGisMapLocations;
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
		if (fqid.ObjectId == _myKind)
		{
			if (fqid.ServerId.ServerType != "XPCO")
			{
				return null;
			}
			try
			{
				ConfigurationApiItemWrapper wrapper = new ConfigurationApiItemWrapper(fqid.ServerId, "/" + _myItemType);
				return new GisMapLocationItemWrapper(fqid.ServerId, Guid.Empty, _myKind, _myAllName, FolderType.SystemDefined, _myKind, wrapper);
			}
			catch (Exception)
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
		return new GisMapLocationItemWrapper(fqid, configurationApiItemWrapper.ConfigurationItem)
		{
			HasChildren = HasChildren.No
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
			return new GisMapLocationItemWrapper(_serverId, Guid.Empty, _myKind, _myAllName, FolderType.SystemDefined, _myKind, wrapper).GetChildren();
		}
		catch (Exception)
		{
			return new List<Item>();
		}
	}

	public override List<Item> GetItems(Item parentItem)
	{
		try
		{
			EnvironmentManager.Instance.Log(error: false, GetType().Name, "Unexpected call to GetItems(parentItem) - not implemented");
			return new List<Item>();
		}
		catch (Exception)
		{
			return new List<Item>();
		}
	}
}
