using System;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("MIPObject")]
public class MIPObject : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public string Name
	{
		get
		{
			return GetPropertyValue<string>("Name");
		}
		set
		{
			SetPropertyValue("Name", value);
		}
	}

	public Guid Kind
	{
		get
		{
			return new Guid(GetPropertyValue<string>("Kind"));
		}
		set
		{
			SetPropertyValue("Kind", value.ToString());
		}
	}

	public Guid SubKind
	{
		get
		{
			return new Guid(GetPropertyValue<string>("SubKind"));
		}
		set
		{
			SetPropertyValue("SubKind", value.ToString());
		}
	}

	public Guid PluginId
	{
		get
		{
			return new Guid(GetPropertyValue<string>("PluginId"));
		}
		set
		{
			SetPropertyValue("PluginId", value.ToString());
		}
	}

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

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public Guid ParentGroupId => new Guid(GetPropertyValue<string>("ParentServerGroupId"));

	public MIPObjectGroup ParentMIPObjectGroup
	{
		get
		{
			if (string.Equals(ParentGroupId.ToString(), "ABF0C309-E9F6-41AF-AA1A-A48B45AA0683", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return GetObjectById<MIPObjectGroup>(ParentGroupId.ToString());
		}
	}

	public MIPObject(MIPObjectGroup mipObjectGroup, Guid id, Guid kind, Guid subKind, Guid pluginId)
		: this(mipObjectGroup.Repositories)
	{
		if (mipObjectGroup == null)
		{
			throw new ArgumentNullException("mipObjectGroup");
		}
		if (id == Guid.Empty)
		{
			throw new ArgumentException("ID may not be guid empty", "id");
		}
		if (!object.Equals(mipObjectGroup.Kind, Guid.Empty) && !object.Equals(mipObjectGroup.Kind, kind))
		{
			throw new ArgumentException("The kind is not valid for the group", "kind");
		}
		SetPropertyValue("Id", id.ToString().ToUpper(CultureInfo.InvariantCulture));
		SetPropertyValue("ParentServerGroupId", mipObjectGroup.InternalId);
		SetPropertyValue("Kind", kind.ToString());
		SetPropertyValue("SubKind", subKind.ToString());
		SetPropertyValue("PluginId", pluginId.ToString());
	}

	public MIPObject(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public MIPObject(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}
}
