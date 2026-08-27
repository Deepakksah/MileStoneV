using System;
using System.Data;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Configuration;

public class RegisteredServers : CollectionBase
{
	public new RegisteredServer this[Guid id] => base[id] as RegisteredServer;

	public RegisteredServers(ConfigurationManager manager)
		: base(manager, manager.Dataset.RegisteredServers)
	{
	}

	public RegisteredServer Add(Guid id, Guid type, string name, string description, ServerAddress[] addresses, bool trusted, string customData, bool enabled, string endpoints)
	{
		return Add(_configurationManager.Dataset.RegisteredServers.AddRegisteredServersRow(DateTime.UtcNow, id.ToString(), type.ToString(), name, description, ServerAddresses.GetAsXml(addresses), trusted, customData, enabled, endpoints)) as RegisteredServer;
	}

	protected override IConfigurationItem GetInstance(DataRow row)
	{
		return new RegisteredServer(_configurationManager, row as ConfigurationDataSet.RegisteredServersRow);
	}
}
