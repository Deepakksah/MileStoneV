using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("PublicViewGroup")]
public class PublicViewGroup : ViewGroup, ICreatable, IUpdatable, IDeletable
{
	public PublicViewGroup Parent
	{
		get
		{
			if (string.Equals(base.ParentServerGroupId, "430C2FE2-B233-42DA-B508-1A22EF03383D", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return GetObjectById<PublicViewGroup>(base.ParentServerGroupId);
		}
	}

	public ViewCollection Views { get; private set; }

	public PublicViewGroupCollection PublicViewGroups { get; private set; }

	public PublicViewGroup(PublicViewGroup parentGroup, string name, Guid id)
		: this(parentGroup, name)
	{
		if (id.Equals(Guid.Empty))
		{
			throw new ArgumentException("cannot be Guid.Empty", "id");
		}
		SetPropertyValue("Id", id.ToString());
	}

	public PublicViewGroup(PublicViewGroup parentGroup, string name)
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

	public PublicViewGroup(PublicViewGroup parentGroup)
		: this(parentGroup.Repositories)
	{
		SetPropertyValue("ParentServerGroupId", parentGroup.InternalId);
	}

	public PublicViewGroup(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
		SetPropertyValue("ParentServerGroupId", managementServer.InternalId);
	}

	public PublicViewGroup(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public PublicViewGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		Views = new ViewCollection(LoadViews, this, isInitialized);
		PublicViewGroups = new PublicViewGroupCollection(LoadPublicViewGroups, this, isInitialized);
	}

	private View[] LoadViews(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<PublicViewGroup, View>(base.Id, refreshCachedObjects);
	}

	private PublicViewGroup[] LoadPublicViewGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<PublicViewGroup, PublicViewGroup>(base.Id, refreshCachedObjects);
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (PublicViewGroupPermissionSet)PublicViewGroupPermissionSet.CreateFromActionId(actionId);
	}

	public PublicViewGroupPermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<PublicViewGroupPermissionInfo>();
	}

	public PublicViewGroupPermissionInfo[] EnumPermissions(PublicViewGroupPermissionSet permissions)
	{
		return EnumPermissions<PublicViewGroupPermissionInfo>(permissions);
	}

	public PublicViewGroupPermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<PublicViewGroupPermissionInfo>(identity, PublicViewGroupPermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public PublicViewGroupPermissionInfo[] EnumPermissions(string identity, PublicViewGroupPermissionSet permissions)
	{
		return EnumPermissions<PublicViewGroupPermissionInfo>(identity, permissions);
	}

	public PublicViewGroupPermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, PublicViewGroupPermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public PublicViewGroupPermissionSet EffectivePermissions(string identity, PublicViewGroupPermissionSet permissions)
	{
		PublicViewGroupPermissionSet supportedPermissions = PublicViewGroupPermissionSet.SupportedPermissions(base.Repositories.ServerVersion);
		if (permissions == null)
		{
			throw new ArgumentNullException("permissions");
		}
		permissions.AdjustForSupportedPermissionsDeny(supportedPermissions);
		return base.EffectivePermissions(identity, permissions).AdjustForSupportedPermissionsAllow(supportedPermissions);
	}

	public void Allow(PublicViewGroupPermissionSet permission, string identity)
	{
		Allow((PermissionSetBase)permission, new string[1] { identity });
	}

	public void Allow(PublicViewGroupPermissionSet permission, string[] identities)
	{
		Allow((PermissionSetBase)permission, identities);
	}

	public void Deny(PublicViewGroupPermissionSet permission, string identity)
	{
		Deny(permission, new string[1] { identity });
	}

	public void Deny(PublicViewGroupPermissionSet permission, string[] identities)
	{
		throw new NotImplementedException();
	}

	public void Revoke(PublicViewGroupPermissionSet permission, string identity)
	{
		Revoke((PermissionSetBase)permission, new string[1] { identity });
	}

	public void Revoke(PublicViewGroupPermissionSet permission, string[] identities)
	{
		Revoke((PermissionSetBase)permission, identities);
	}

	public void SetPermissionInheritance(bool enableInheritance)
	{
		SetPermissionInheritanceInternal(enableInheritance, base.ParentServerGroupId);
	}
}
