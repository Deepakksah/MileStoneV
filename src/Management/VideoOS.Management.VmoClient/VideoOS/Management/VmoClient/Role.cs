using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("Role")]
public class Role : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
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

	public RoleType RoleType => GetPropertyValue<RoleType>("RoleType");

	public string DomainAccountName => string.Format(CultureInfo.InvariantCulture, "[VideoOS]\\{0}", Name);

	public bool DualAuthorizationRequired
	{
		get
		{
			return GetPropertyValue<bool>("DualAuthorizationRequired");
		}
		set
		{
			SetPropertyValue("DualAuthorizationRequired", value);
		}
	}

	public bool ShowPtzReservedNameAsAnonymous
	{
		get
		{
			return GetPropertyValue<bool>("ShowPtzReservedNameAsAnonymous");
		}
		set
		{
			SetPropertyValue("ShowPtzReservedNameAsAnonymous", value);
		}
	}

	public bool AllowMobileClientLogOn
	{
		get
		{
			return GetPropertyValue<bool>("AllowMobileClientLogOn");
		}
		set
		{
			SetPropertyValue("AllowMobileClientLogOn", value);
		}
	}

	public bool AllowSmartClientLogOn
	{
		get
		{
			return GetPropertyValue<bool>("AllowSmartClientLogOn");
		}
		set
		{
			SetPropertyValue("AllowSmartClientLogOn", value);
		}
	}

	public bool AllowWebClientLogOn
	{
		get
		{
			return GetPropertyValue<bool>("AllowWebClientLogOn");
		}
		set
		{
			SetPropertyValue("AllowWebClientLogOn", value);
		}
	}

	public ManagementClientProfile AttachedManagementClientProfile
	{
		get
		{
			ManagementClientProfile[] objectsByParentId = base.Repositories.GetObjectsByParentId<Role, ManagementClientProfile>(base.Id, refreshCachedObjects: false);
			if (objectsByParentId == null)
			{
				return null;
			}
			return objectsByParentId[0];
		}
	}

	public SmartClientProfile AttachedSmartClientProfile
	{
		get
		{
			SmartClientProfile[] objectsByParentId = base.Repositories.GetObjectsByParentId<Role, SmartClientProfile>(base.Id, refreshCachedObjects: false);
			if (objectsByParentId == null)
			{
				return null;
			}
			return objectsByParentId[0];
		}
	}

	public override bool IsCustomPropertiesSupported => false;

	public Role(ManagementServer managementServer, string name)
		: this(managementServer.Repositories, name)
	{
	}

	public Role(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public Role(IVmoRepositories repositories, string name)
		: this(repositories)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("The parameter must not be an empty string.", "name");
		}
		SetPropertyValue("Name", name);
	}

	public Role(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public Role(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
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

	public Role CopyRole(string roleName, string roleDescription)
	{
		ThrowIfNotCreatedOrDeleted();
		if (roleName == null)
		{
			throw new ArgumentNullException("roleName");
		}
		if (string.IsNullOrWhiteSpace(roleName))
		{
			throw new ArgumentException("The parameter cannot be an empty string", "roleName");
		}
		return GenericRepositoryFunctions.CopyRole(this, roleName, roleDescription);
	}

	public Identity AddMember(BasicUser basicUser)
	{
		ThrowIfNotCreatedOrDeleted();
		if (basicUser == null)
		{
			throw new ArgumentNullException("basicUser");
		}
		if (string.IsNullOrEmpty(basicUser.Sid))
		{
			throw new ObjectNotCreatedException();
		}
		return AddMember(basicUser.DomainAccountName);
	}

	public Identity AddMember(string domainAccountName)
	{
		ThrowIfNotCreatedOrDeleted();
		if (domainAccountName == null)
		{
			throw new ArgumentNullException("domainAccountName");
		}
		if (string.IsNullOrWhiteSpace(domainAccountName))
		{
			throw new ArgumentException("The parameter cannot be empty", "domainAccountName");
		}
		Identity identity = base.Repositories.ServerCommunication.ReadIdentity(SearchFactor.AccountName, domainAccountName, QueryMembership.None);
		if (identity == null)
		{
			identity = base.Repositories.ServerCommunication.ReadIdentityFromSource(SearchFactor.AccountName, domainAccountName);
		}
		if (identity == null)
		{
			throw new UserNotExistsException();
		}
		return AddMemberToRole(identity.Sid);
	}

	private Identity AddMemberToRole(string sid)
	{
		return base.Repositories.ServerCommunication.AddMemberToRole(base.InternalId, sid);
	}

	public void RemoveMember(BasicUser basicUser)
	{
		ThrowIfNotCreatedOrDeleted();
		if (basicUser == null)
		{
			throw new ArgumentNullException("basicUser");
		}
		if (string.IsNullOrEmpty(basicUser.Sid))
		{
			throw new ObjectNotCreatedException();
		}
		RemoveMember(basicUser.DomainAccountName);
	}

	public void RemoveMember(string domainAccountName)
	{
		ThrowIfNotCreatedOrDeleted();
		if (domainAccountName == null)
		{
			throw new ArgumentNullException("domainAccountName");
		}
		if (string.IsNullOrWhiteSpace(domainAccountName))
		{
			throw new ArgumentException("The parameter cannot be empty", "domainAccountName");
		}
		Identity identity = base.Repositories.ServerCommunication.ReadIdentity(SearchFactor.AccountName, domainAccountName, QueryMembership.None);
		if (identity != null)
		{
			base.Repositories.ServerCommunication.RemoveMemberFromRole(base.Id.ToString(), identity.Sid);
		}
	}

	public Identity[] EnumMembers()
	{
		ThrowIfNotCreatedOrDeleted();
		return base.Repositories.ServerCommunication.QueryRoleMembers(InternalId);
	}

	public Identity[] EnumMembers(QueryMembership queryMembership)
	{
		ThrowIfNotCreatedOrDeleted();
		return base.Repositories.ServerCommunication.QueryRoleMembersExtended(InternalId, queryMembership);
	}

	public Identity[] EnumMembers(QueryMembership queryMembership, bool includeMappedExternalUsers)
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v22_1);
		return base.Repositories.ServerCommunication.QueryRoleMembersExtended(InternalId, queryMembership, includeMappedExternalUsers);
	}

	public void AddClaim(Claim claim)
	{
		if (claim == null)
		{
			throw new ArgumentNullException("claim");
		}
		AddClaim(claim.LoginProviderId, claim.Name, claim.Value);
	}

	public void AddClaim(string loginProviderId, string claimName, string claimValue)
	{
		if (loginProviderId == null)
		{
			throw new ArgumentNullException("loginProviderId");
		}
		if (claimName == null)
		{
			throw new ArgumentNullException("claimName");
		}
		if (claimValue == null)
		{
			throw new ArgumentNullException("claimValue");
		}
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v21_2);
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		vmoPropertyCollection.Add("ClaimProvider", loginProviderId);
		vmoPropertyCollection.Add("ClaimName", claimName);
		vmoPropertyCollection.Add("ClaimValue", claimValue);
		base.Repositories.ServerCommunication.Execute("AddRoleClaim", vmoPropertyCollection);
	}

	public void RemoveClaim(string loginProviderId, string claimName, string claimValue)
	{
		if (loginProviderId == null)
		{
			throw new ArgumentNullException("loginProviderId");
		}
		if (claimName == null)
		{
			throw new ArgumentNullException("claimName");
		}
		if (claimValue == null)
		{
			throw new ArgumentNullException("claimValue");
		}
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v21_2);
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		vmoPropertyCollection.Add("ClaimProvider", loginProviderId);
		vmoPropertyCollection.Add("ClaimName", claimName);
		vmoPropertyCollection.Add("ClaimValue", claimValue);
		base.Repositories.ServerCommunication.Execute("RemoveRoleClaim", vmoPropertyCollection);
	}

	public void RemoveClaim(Claim claim)
	{
		if (claim == null)
		{
			throw new ArgumentNullException("claim");
		}
		RemoveClaim(claim.LoginProviderId, claim.Name, claim.Value);
	}

	public Claim[] EnumClaims()
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v21_2);
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.Execute("QueryRoleClaims", vmoPropertyCollection);
		List<Claim> list = new List<Claim>();
		VMOWebserviceObject[] array2 = array;
		foreach (VMOWebserviceObject vMOWebserviceObject in array2)
		{
			list.Add(new Claim(vMOWebserviceObject.Properties.GetPropertyValue<string>("ClaimProvider"), vMOWebserviceObject.Properties.GetPropertyValue<string>("ClaimName"), vMOWebserviceObject.Properties.GetPropertyValue<string>("ClaimValue")));
		}
		return list.ToArray();
	}
}
