using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("BasicUser")]
public class BasicUser : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	public string UserName => GetPropertyValue<string>("Name");

	public string DomainAccountName => string.Format(CultureInfo.InvariantCulture, "[BASIC]\\{0}", UserName);

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

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public string Sid => GetPropertyValue<string>("Sid");

	public bool CanChangePassword
	{
		get
		{
			return GetPropertyValue<bool>("CanChangePassword");
		}
		set
		{
			SetPropertyValue("CanChangePassword", value);
		}
	}

	public bool ForcePasswordChange
	{
		get
		{
			return GetPropertyValue<bool>("ForcePasswordChange");
		}
		set
		{
			SetPropertyValue("ForcePasswordChange", value);
		}
	}

	public BasicUserStatus Status
	{
		get
		{
			Refresh();
			return GetPropertyValue<BasicUserStatus>("Status");
		}
		set
		{
			SetPropertyValue("Status", value);
		}
	}

	public DateTime? LockoutEnd => GetPropertyValue<DateTime?>("LockoutEnd");

	public bool IsExternal => GetPropertyValue<bool>("IsExternal");

	public override bool IsCustomPropertiesSupported => false;

	public BasicUser(ManagementServer managementServer, string userName, string password)
		: this(managementServer.Repositories, userName, password)
	{
	}

	public BasicUser(ManagementServer managementServer, Guid customId, string userName, string password)
		: this(managementServer.Repositories, customId, userName, password)
	{
	}

	public BasicUser(IVmoRepositories repositories, Guid customId, string userName, string password)
		: this(repositories, userName, password)
	{
		if (customId == Guid.Empty)
		{
			throw new ArgumentException("The parameter cannot be an empty guid", "customId");
		}
		SetPropertyValue("Id", customId.ToString());
	}

	public BasicUser(IVmoRepositories repositories, string userName, string password)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		if (userName == null)
		{
			throw new ArgumentNullException("userName");
		}
		if (password == null)
		{
			throw new ArgumentNullException("password");
		}
		if (userName.Length == 0)
		{
			throw new ArgumentException("Password argument is empty", "userName");
		}
		if (password.Length == 0)
		{
			throw new ArgumentException("Password argument is empty", "password");
		}
		SetPropertyValue("Name", userName);
		SetPropertyValue("Password", password);
		SetPropertyValue("CanChangePassword", value: true);
	}

	public BasicUser(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
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

	public void ChangePassword(string newPassword)
	{
		ThrowIfNotCreatedOrDeleted();
		if (newPassword == null)
		{
			throw new ArgumentNullException("newPassword");
		}
		if (string.IsNullOrWhiteSpace(newPassword))
		{
			throw new ArgumentException("Password cannot be empty", "newPassword");
		}
		base.Repositories.ServerCommunication.BasicUserChangePassword(base.InternalId, newPassword);
	}

	public Claim[] QueryUserClaims()
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v21_2);
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.Execute("QueryUserClaims", vmoPropertyCollection);
		List<Claim> list = new List<Claim>();
		VMOWebserviceObject[] array2 = array;
		foreach (VMOWebserviceObject vMOWebserviceObject in array2)
		{
			list.Add(new Claim(vMOWebserviceObject.Properties.GetPropertyValue<string>("ClaimProvider"), vMOWebserviceObject.Properties.GetPropertyValue<string>("ClaimName"), vMOWebserviceObject.Properties.GetPropertyValue<string>("ClaimValue")));
		}
		return list.ToArray();
	}
}
