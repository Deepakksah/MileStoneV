using System;
using System.Diagnostics;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[DebuggerDisplay("RegisteredClaim, {LoginProviderId} : {Name}")]
[EntityName("RegisteredClaim")]
public class RegisteredClaim : VmoObjectIntBased, ICreatable, IUpdatable, IDeletable
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

	public string DisplayName
	{
		get
		{
			return GetPropertyValue<string>("DisplayName");
		}
		set
		{
			SetPropertyValue("DisplayName", value);
		}
	}

	public bool CaseSensitive
	{
		get
		{
			return GetPropertyValue<bool>("CaseSensitive");
		}
		private set
		{
			SetPropertyValue("CaseSensitive", value);
		}
	}

	public string LoginProviderId
	{
		get
		{
			return GetPropertyValue<string>("LoginProviderId");
		}
		private set
		{
			SetPropertyValue("LoginProviderId", value);
		}
	}

	public LoginProvider LoginProvider => GetObjectById<LoginProvider>(LoginProviderId);

	public RegisteredClaim(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public RegisteredClaim(LoginProvider loginProvider, string name, string displayName, bool caseSensitive)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), loginProvider?.Repositories, isInitialized: true)
	{
		if (loginProvider == null)
		{
			throw new ArgumentNullException("loginProvider");
		}
		LoginProviderId = loginProvider.Id.ToString();
		Name = name;
		DisplayName = displayName;
		CaseSensitive = caseSensitive;
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(InternalId);
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
