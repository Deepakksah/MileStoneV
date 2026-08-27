using System;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

[EntityName("UserDefinedEvent")]
public class UserDefinedEvent : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

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

	public string Subtype
	{
		get
		{
			return GetPropertyValue<string>("Subtype");
		}
		set
		{
			SetPropertyValue("Subtype", value);
		}
	}

	public UserDefinedEvent(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public UserDefinedEvent(ManagementServer managementServer, Guid customId)
		: this(managementServer.Repositories, customId)
	{
	}

	public UserDefinedEvent(IVmoRepositories repositories, Guid customId)
		: this(repositories)
	{
		SetPropertyValue("Id", customId.ToString());
	}

	public UserDefinedEvent(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public UserDefinedEvent(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
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

	public void Trigger()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.TriggerUserDefinedEvent(base.InternalId);
	}

	public void Trigger(int repetitions)
	{
		for (int i = 0; i < repetitions; i++)
		{
			Trigger();
		}
	}

	public UserDefinedEventPermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<UserDefinedEventPermissionInfo>();
	}

	public UserDefinedEventPermissionInfo[] EnumPermissions(UserDefinedEventPermissionSet permissions)
	{
		return EnumPermissions<UserDefinedEventPermissionInfo>(permissions);
	}

	public UserDefinedEventPermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions(identity, UserDefinedEventPermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public UserDefinedEventPermissionInfo[] EnumPermissions(string identity, UserDefinedEventPermissionSet permissions)
	{
		return EnumPermissions<UserDefinedEventPermissionInfo>(identity, permissions);
	}

	public UserDefinedEventPermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, UserDefinedEventPermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public UserDefinedEventPermissionSet EffectivePermissions(string identity, UserDefinedEventPermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(UserDefinedEventPermissionSet permissions, string identity)
	{
		Allow(permissions, new string[1] { identity });
	}

	public void Allow(UserDefinedEventPermissionSet permissions, string[] identities)
	{
		Allow((PermissionSetBase)permissions, identities);
	}

	public void Deny(UserDefinedEventPermissionSet permission, string identity)
	{
		Deny(permission, new string[1] { identity });
	}

	public void Deny(UserDefinedEventPermissionSet permission, string[] identities)
	{
		throw new NotImplementedException();
	}

	public void Revoke(UserDefinedEventPermissionSet permissions, string identity)
	{
		Revoke(permissions, new string[1] { identity });
	}

	public void Revoke(UserDefinedEventPermissionSet permissions, string[] identities)
	{
		Revoke((PermissionSetBase)permissions, identities);
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (UserDefinedEventPermissionSet)UserDefinedEventPermissionSet.CreateFromActionId(actionId);
	}

	public void DeleteTimebasedSecurity(string identity, UserDefinedEventPermission permission)
	{
		if (permission == UserDefinedEventPermission.Trigger)
		{
			base.Repositories.ServerCommunication.DeleteTimeBasedAccessEntry(identity, UserDefinedEventPermissionSet.PermissionEnumToActionId(UserDefinedEventPermissionSetValue.Trigger), base.InternalId);
		}
	}

	public bool SetTimebasedSecurity(string identity, UserDefinedEventPermission permission, string timeProfileId, bool isDefaultTimeProfile)
	{
		if (permission == UserDefinedEventPermission.Trigger)
		{
			AccessControlTimeProfileEntry accessControlTimeProfileEntry = base.Repositories.ServerCommunication.CreateUpdateTimeBasedAccessEntry(identity, UserDefinedEventPermissionSet.PermissionEnumToActionId(UserDefinedEventPermissionSetValue.Trigger), base.InternalId, timeProfileId, isDefaultTimeProfile);
			if (accessControlTimeProfileEntry != null && !string.IsNullOrEmpty(accessControlTimeProfileEntry.ActionId))
			{
				return true;
			}
		}
		return false;
	}

	public TimeProfilePermissionInfo QueryTimebasedSecurity(string identity, UserDefinedEventPermission permission)
	{
		TimeProfilePermissionInfo result = null;
		if (permission == UserDefinedEventPermission.Trigger)
		{
			AccessControlTimeProfileEntry accessControlTimeProfileEntry = base.Repositories.ServerCommunication.QueryTimeBasedAccessEntry(identity, UserDefinedEventPermissionSet.PermissionEnumToActionId(UserDefinedEventPermissionSetValue.Trigger), base.InternalId);
			if (accessControlTimeProfileEntry != null && !string.IsNullOrEmpty(accessControlTimeProfileEntry.ActionId))
			{
				result = new TimeProfilePermissionInfo(accessControlTimeProfileEntry);
			}
		}
		return result;
	}
}
