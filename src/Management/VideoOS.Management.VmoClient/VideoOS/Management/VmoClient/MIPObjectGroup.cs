using System;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MIPObjectGroup")]
public class MIPObjectGroup : ServerGroup, ICreatable, IUpdatable, IDeletable
{
	public Guid Kind => new Guid(GetPropertyValue<string>("Kind"));

	public Guid PluginId => new Guid(GetPropertyValue<string>("PluginId"));

	public string CustomData
	{
		get
		{
			return GetPropertyValue<string>("CustomData");
		}
		set
		{
			SetPropertyValue("CustomData", value);
		}
	}

	public MIPObjectGroup ParentMIPObjectGroup => GetObjectById<MIPObjectGroup>(base.ParentServerGroupId);

	public ManagementServer ManagementServer => GetObjectById<ManagementServer>("909749AE-88C4-4780-A381-383D691E98FA");

	public MIPObjectCollection MIPObjects { get; private set; }

	public MIPObjectGroupCollection MIPObjectGroups { get; private set; }

	public MIPObjectGroup(Guid id, MIPObjectGroup parentGroup, string name, Guid kind, Guid pluginId)
		: this(parentGroup.Repositories)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("ID may not be guid empty", "id");
		}
		if (parentGroup == null)
		{
			throw new ArgumentNullException("parentGroup", "cannot be null");
		}
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("cannot be null or empty", "name");
		}
		if (object.Equals(Guid.Empty, kind))
		{
			throw new ArgumentException("kind cannot be empty", "kind");
		}
		if (object.Equals(Guid.Empty, pluginId))
		{
			throw new ArgumentException("pluginId cannot be empty", "pluginId");
		}
		if (!object.Equals(parentGroup.Kind, Guid.Empty) && !object.Equals(parentGroup.Kind, kind))
		{
			throw new ArgumentException("The kind is not valid for the group", "kind");
		}
		SetPropertyValue("Id", id.ToString().ToUpper(CultureInfo.InvariantCulture));
		SetPropertyValue("ParentServerGroupId", parentGroup.InternalId);
		SetPropertyValue("Name", name);
		SetPropertyValue("Kind", kind.ToString());
		SetPropertyValue("PluginId", pluginId.ToString());
	}

	public MIPObjectGroup(Guid id, ManagementServer managementServer, string name, Guid kind, Guid pluginId)
		: this(managementServer.Repositories)
	{
		if (managementServer == null)
		{
			throw new ArgumentNullException("managementServer", "cannot be null");
		}
		if (id == Guid.Empty)
		{
			throw new ArgumentException("ID may not be guid empty", "id");
		}
		if (string.IsNullOrEmpty(name))
		{
			throw new ArgumentException("cannot be null or empty", "name");
		}
		if (object.Equals(Guid.Empty, kind))
		{
			throw new ArgumentException("kind cannot be empty", "kind");
		}
		if (object.Equals(Guid.Empty, pluginId))
		{
			throw new ArgumentException("pluginId cannot be empty", "pluginId");
		}
		SetPropertyValue("Id", id.ToString().ToUpper(CultureInfo.InvariantCulture));
		SetPropertyValue("ParentServerGroupId", managementServer.InternalId);
		SetPropertyValue("Name", name);
		SetPropertyValue("Kind", kind.ToString());
		SetPropertyValue("PluginId", pluginId.ToString());
	}

	public MIPObjectGroup(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public MIPObjectGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		MIPObjects = new MIPObjectCollection(LoadMIPObjects, this, isInitialized);
		MIPObjectGroups = new MIPObjectGroupCollection(LoadMIPObjectGroups, this, isInitialized);
	}

	private MIPObject[] LoadMIPObjects(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<MIPObjectGroup, MIPObject>(base.Id, refreshCachedObjects);
	}

	private MIPObjectGroup[] LoadMIPObjectGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<MIPObjectGroup, MIPObjectGroup>(base.Id, refreshCachedObjects);
	}
}
