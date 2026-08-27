using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("FailoverServerGroup")]
public class FailoverServerGroup : ServerGroup, ICreatable, IUpdatable, IDeletable
{
	public FailoverServerGroup ParentFailoverServerGroup
	{
		get
		{
			if (string.Equals(base.ParentServerGroupId, "6A85FFC3-C134-4338-A072-024817CB5820", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return GetObjectById<FailoverServerGroup>(base.ParentServerGroupId);
		}
	}

	public ManagementServer ManagementServer => GetObjectById<ManagementServer>("909749AE-88C4-4780-A381-383D691E98FA");

	public FailoverServerCollection FailoverServers { get; private set; }

	public FailoverServerGroupCollection FailoverServerGroups { get; private set; }

	public FailoverServerGroup(FailoverServerGroup parentGroup, string name)
		: this(parentGroup)
	{
		if (parentGroup == null)
		{
			throw new ArgumentNullException("parentGroup", "cannot be null");
		}
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name", "cannot be null or empty");
		}
		SetPropertyValue("Name", name);
	}

	public FailoverServerGroup(FailoverServerGroup parentGroup)
		: this(parentGroup.Repositories)
	{
		SetPropertyValue("ParentServerGroupId", parentGroup.InternalId);
	}

	public FailoverServerGroup(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
		SetPropertyValue("ParentServerGroupId", managementServer.InternalId);
	}

	public FailoverServerGroup(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public FailoverServerGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		FailoverServers = new FailoverServerCollection(LoadFailoverServers, this, isInitialized);
		FailoverServerGroups = new FailoverServerGroupCollection(LoadFailoverServerGroups, this, isInitialized);
	}

	private FailoverServer[] LoadFailoverServers(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<FailoverServerGroup, FailoverServer>(base.Id, refreshCachedObjects);
	}

	private FailoverServerGroup[] LoadFailoverServerGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<FailoverServerGroup, FailoverServerGroup>(base.Id, refreshCachedObjects);
	}
}
