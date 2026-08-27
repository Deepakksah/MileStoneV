using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("RetentionOptionRoleRelation")]
public class RetentionOptionRoleRelation : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public string IdRole
	{
		get
		{
			return GetPropertyValue<string>("IdRole");
		}
		set
		{
			SetPropertyValue("IdRole", value);
		}
	}

	public string IdRetentionOption
	{
		get
		{
			return GetPropertyValue<string>("IdRetentionOption");
		}
		set
		{
			SetPropertyValue("IdRetentionOption", value);
		}
	}

	public string RetentionName
	{
		get
		{
			return GetPropertyValue<string>("RetentionName");
		}
		set
		{
			SetPropertyValue("RetentionName", value);
		}
	}

	public override bool IsCustomPropertiesSupported => false;

	public RetentionOptionRoleRelation(ManagementServer managementServer, Guid idRole, Guid idRetentionOption)
		: this(managementServer.Repositories, idRole, idRetentionOption)
	{
	}

	public RetentionOptionRoleRelation(ManagementServer managementServer)
		: this(managementServer.Repositories)
	{
	}

	public RetentionOptionRoleRelation(IVmoRepositories repositories, Guid idRole, Guid idRetentionOption)
		: this(repositories)
	{
		if (idRole == Guid.Empty)
		{
			throw new ArgumentNullException("idRole");
		}
		if (idRetentionOption == Guid.Empty)
		{
			throw new ArgumentNullException("idRetentionOption");
		}
		SetPropertyValue("IdRetentionOption", idRetentionOption.ToString());
		SetPropertyValue("IdRole", idRole.ToString());
	}

	public RetentionOptionRoleRelation(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public RetentionOptionRoleRelation(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
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
}
