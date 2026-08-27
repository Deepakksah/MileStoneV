using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MIPDeviceEvent")]
public class MIPDeviceEvent : PluginEvent, ICreatable, IUpdatable, IDeletable
{
	public override string Definition
	{
		get
		{
			return base.Definition;
		}
		set
		{
			base.Definition = value;
		}
	}

	public MIPDeviceEvent(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public MIPDeviceEvent(ManagementServer managementServer, Guid customId)
		: this(managementServer.Repositories, customId)
	{
	}

	public MIPDeviceEvent(IVmoRepositories repositories, Guid customId)
		: this(repositories)
	{
		SetPropertyValue("Id", customId.ToString());
	}

	public MIPDeviceEvent(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public MIPDeviceEvent(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}
}
