using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MIPEvent")]
public class MIPEvent : PluginEvent, ICreatable, IUpdatable, IDeletable
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

	public MIPEvent(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public MIPEvent(ManagementServer managementServer, Guid customId)
		: this(managementServer.Repositories, customId)
	{
	}

	public MIPEvent(IVmoRepositories repositories, Guid customId)
		: this(repositories)
	{
		SetPropertyValue("Id", customId.ToString());
	}

	public MIPEvent(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public MIPEvent(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}
}
