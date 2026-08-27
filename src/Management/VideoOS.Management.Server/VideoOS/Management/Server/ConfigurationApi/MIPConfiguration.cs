using System;
using System.Collections.Generic;
using VideoOS.Platform;

namespace VideoOS.Management.Server.ConfigurationApi;

public class MIPConfiguration : Configuration
{
	private FQID _fqid;

	private string _name;

	public override FQID ServerFQID => _fqid;

	public override string ServerName => _name;

	public MIPConfiguration(Guid mgtServerId, string name, string machineName, int port)
	{
		_fqid = new FQID(new ServerId("XPCO", machineName, port, mgtServerId), Guid.Empty, Guid.Empty, FolderType.No, Kind.Server);
		_name = name;
	}

	public override void DeleteItemConfiguration(Guid platformDefinitionId, Item item)
	{
		throw new NotImplementedException();
	}

	public override Item GetItem(FQID fqid)
	{
		throw new NotImplementedException();
	}

	public override Item GetItem(Guid objectId, Guid kind)
	{
		throw new NotImplementedException();
	}

	public override Item GetItem(ServerId serverId, Guid objectId, Guid kind)
	{
		throw new NotImplementedException();
	}

	public override Item GetItemConfiguration(Guid pluginDefinitionId, Guid itemKind, Guid objectId)
	{
		throw new NotImplementedException();
	}

	public override List<Item> GetItemConfigurations(Guid pluginDefinitionId, Item parentItem, Guid itemKind)
	{
		throw new NotImplementedException();
	}

	public override List<Item> GetItems()
	{
		throw new NotImplementedException();
	}

	public override List<Item> GetItemsByKind(Guid kind)
	{
		throw new NotImplementedException();
	}

	public override void SaveItemConfiguration(Guid pluginDefinitionId, Item item)
	{
		throw new NotImplementedException();
	}
}
