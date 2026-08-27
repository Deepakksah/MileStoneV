using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ClientProfile")]
public abstract class ClientProfile : VmoObjectGuidBased, IUpdatable, ICreatable, IDeletable
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

	public string Description
	{
		get
		{
			return GetPropertyValue<string>("Description");
		}
		set
		{
			SetPropertyValue("Description", value);
		}
	}

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public string SettingsXml
	{
		get
		{
			return GetPropertyValue<string>("Settings");
		}
		set
		{
			SetPropertyValue("Settings", value);
		}
	}

	public bool IsDefaultProfile => GetPropertyValue<bool>("IsDefault");

	public RoleCollection AttachedRoles { get; private set; }

	public ClientProfile(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		AttachedRoles = new RoleCollection(LoadRoles, this, isInitialized: false);
	}

	public void AttachToRole(Guid roleId)
	{
		base.Repositories.ServerCommunication.AttachClientProfileToRole(InternalId, roleId.ToString());
		AttachedRoles.Refresh(refreshChildObjects: false);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	public void MovePriorityUp()
	{
		base.Repositories.ServerCommunication.ClientProfileMoveUpPriority(base.InternalId);
	}

	public void MovePriorityDown()
	{
		base.Repositories.ServerCommunication.ClientProfileMoveDownPriority(base.InternalId);
	}

	private Role[] LoadRoles(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ClientProfile, Role>(base.Id, refreshCachedObjects);
	}
}
