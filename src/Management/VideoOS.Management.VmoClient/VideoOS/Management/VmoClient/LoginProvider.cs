using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("LoginProvider")]
public class LoginProvider : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
{
	private ScopesCollection _scopes;

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

	public string ClientId
	{
		get
		{
			return GetPropertyValue<string>("ClientId");
		}
		set
		{
			SetPropertyValue("ClientId", value);
		}
	}

	public string ClientSecret
	{
		set
		{
			SetPropertyValue("ClientSecret", value);
		}
	}

	public string ClientSecretType
	{
		get
		{
			return GetPropertyValue<string>("ClientSecretType");
		}
		set
		{
			SetPropertyValue("ClientSecretType", value);
		}
	}

	public bool ClientSecretHasValue => GetPropertyValue<bool>("ClientSecretHasValue");

	public string CallbackPath
	{
		get
		{
			return GetPropertyValue<string>("CallbackPath");
		}
		set
		{
			SetPropertyValue("CallbackPath", value);
		}
	}

	public string Authority
	{
		get
		{
			return GetPropertyValue<string>("Authority");
		}
		set
		{
			SetPropertyValue("Authority", value);
		}
	}

	public string UserNameClaimType
	{
		get
		{
			return GetPropertyValue<string>("UserNameClaimType");
		}
		set
		{
			SetPropertyValue("UserNameClaimType", value);
		}
	}

	public ScopesCollection Scopes
	{
		get
		{
			if (_scopes == null)
			{
				List<string> scopes = (from p in base.ObjectProperties.GetNestedPropertyCollection("Scopes")
					select p.Value as string).ToList();
				_scopes = new ScopesCollection(scopes);
				_scopes.CollectionChanged += ScopesCollectionChanged;
			}
			return _scopes;
		}
	}

	public RegisteredClaimCollection Claims { get; }

	public bool PromptForLogin
	{
		get
		{
			return GetPropertyValue<bool>("PromptForLogin");
		}
		set
		{
			SetPropertyValue("PromptForLogin", value);
		}
	}

	public bool Enabled
	{
		get
		{
			return GetPropertyValue<bool>("Enabled");
		}
		set
		{
			SetPropertyValue("Enabled", value);
		}
	}

	public override bool IsCustomPropertiesSupported => false;

	public LoginProvider(ManagementServer managementServer, string name, string clientId, string clientSecret, string callbackPath, string authority, string userNameClaimType, string clientSecretType = "SharedSecret")
		: this(managementServer?.Repositories, name, clientId, clientSecret, callbackPath, authority, userNameClaimType, clientSecretType)
	{
	}

	public LoginProvider(IVmoRepositories repositories, string name, string clientId, string clientSecret, string callbackPath, string authority, string userNameClaimType, string clientSecretType = "SharedSecret")
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		ThrowIfCheckVersionFail(ServerVersions.v22_1);
		Name = name ?? throw new ArgumentNullException("name");
		ClientId = clientId ?? throw new ArgumentNullException("clientId");
		ClientSecret = clientSecret ?? throw new ArgumentNullException("clientSecret");
		ClientSecretType = clientSecretType ?? throw new ArgumentNullException("clientSecret");
		CallbackPath = callbackPath ?? throw new ArgumentNullException("callbackPath");
		Authority = authority ?? throw new ArgumentNullException("authority");
		UserNameClaimType = userNameClaimType ?? throw new ArgumentNullException("userNameClaimType");
		PromptForLogin = true;
		Enabled = true;
		base.ObjectProperties.SetNestedPropertyCollection("Scopes", new VmoPropertyCollection());
	}

	public LoginProvider(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		Claims = new RegisteredClaimCollection(LoadClaims, this, isInitialized: false);
	}

	private RegisteredClaim[] LoadClaims(bool refreshCachedObjects)
	{
		ThrowIfCheckVersionFail(ServerVersions.v21_2);
		return base.Repositories.GetObjectsByParentId<LoginProvider, RegisteredClaim>(base.Id, refreshCachedObjects);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v22_1);
		base.Repository.RefreshObject(InternalId);
	}

	public void Create()
	{
		ThrowIfCheckVersionFail(ServerVersions.v22_1);
		base.Repository.CreateObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v22_1);
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v22_1);
		base.Repository.DeleteObject(this);
	}

	protected override void PropertiesLoaded()
	{
		base.PropertiesLoaded();
		if (_scopes != null)
		{
			_scopes.CollectionChanged -= ScopesCollectionChanged;
			_scopes = null;
		}
	}

	private void ScopesCollectionChanged(object sender, EventArgs e)
	{
		VmoPropertyCollection nestedPropertyCollection = base.ObjectProperties.GetNestedPropertyCollection("Scopes");
		nestedPropertyCollection.Clear();
		int num = 0;
		foreach (string scope in _scopes)
		{
			nestedPropertyCollection.Add(num++.ToString(CultureInfo.InvariantCulture), scope);
		}
	}
}
