using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("PrivateViewGroup")]
public class PrivateViewGroup : ViewGroup, ICreatable, IUpdatable, IDeletable
{
	public PrivateViewGroup Parent
	{
		get
		{
			ManagementServer objectById = GetObjectById<ManagementServer>("909749AE-88C4-4780-A381-383D691E98FA");
			if (string.Equals(base.ParentServerGroupId, objectById.PrivateUserGroupId, StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return GetObjectById<PrivateViewGroup>(base.ParentServerGroupId);
		}
	}

	public ViewCollection Views { get; private set; }

	public PrivateViewGroupCollection PrivateViewGroups { get; private set; }

	public PrivateViewGroup(PrivateViewGroup parentGroup, string name, Guid id)
		: this(parentGroup, name)
	{
		if (id.Equals(Guid.Empty))
		{
			throw new ArgumentException("cannot be Guid.Empty", "id");
		}
		SetPropertyValue("Id", id.ToString());
	}

	public PrivateViewGroup(PrivateViewGroup parentGroup, string name)
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

	public PrivateViewGroup(PrivateViewGroup parentGroup)
		: this(parentGroup.Repositories)
	{
		SetPropertyValue("ParentServerGroupId", parentGroup.InternalId);
	}

	public PrivateViewGroup(ManagementServer managementServer, string name, Guid id)
		: this(managementServer, name)
	{
		if (id.Equals(Guid.Empty))
		{
			throw new ArgumentException("cannot be Guid.Empty", "id");
		}
		SetPropertyValue("Id", id.ToString());
	}

	public PrivateViewGroup(ManagementServer managementServer, string name)
		: this(managementServer)
	{
		if (managementServer == null)
		{
			throw new ArgumentNullException("managementServer", "cannot be null");
		}
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentNullException("name", "cannot be null or empty");
		}
		SetPropertyValue("Name", name);
	}

	public PrivateViewGroup(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
		SetPropertyValue("ParentServerGroupId", managementServer.InternalId);
	}

	public PrivateViewGroup(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public PrivateViewGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		Views = new ViewCollection(LoadViews, this, isInitialized);
		PrivateViewGroups = new PrivateViewGroupCollection(LoadPrivateViewGroups, this, isInitialized);
	}

	private View[] LoadViews(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<PrivateViewGroup, View>(base.Id, refreshCachedObjects);
	}

	private PrivateViewGroup[] LoadPrivateViewGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<PrivateViewGroup, PrivateViewGroup>(base.Id, refreshCachedObjects);
	}
}
