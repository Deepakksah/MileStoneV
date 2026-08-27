using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("TrustedIssuer")]
public class TrustedIssuer : VmoObjectIntBased, ICreatable, IUpdatable, IDeletable
{
	public string Issuer
	{
		get
		{
			return GetPropertyValue<string>("Issuer");
		}
		set
		{
			SetPropertyValue("Issuer", value);
		}
	}

	public Uri Address
	{
		get
		{
			return new Uri(GetPropertyValue<string>("Address"));
		}
		set
		{
			SetPropertyValue("Address", value?.ToString());
		}
	}

	public override bool IsCustomPropertiesSupported => false;

	public TrustedIssuer(ManagementServer managementServer, string issuer, Uri address)
		: this(managementServer?.Repositories, issuer, address)
	{
	}

	public TrustedIssuer(IVmoRepositories repositories, string issuer, Uri address)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
		Issuer = issuer ?? throw new ArgumentNullException("issuer");
		Address = address ?? throw new ArgumentNullException("address");
	}

	public TrustedIssuer(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		ThrowIfCheckVersionFail(ServerVersions.v23_3);
	}

	public void Create()
	{
		ThrowIfCheckVersionFail(ServerVersions.v23_3);
		base.Repository.CreateObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v23_3);
		base.Repository.UpdateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v23_3);
		base.Repository.DeleteObject(this);
	}

	public void Refresh()
	{
		ThrowIfNotCreatedOrDeleted();
		ThrowIfCheckVersionFail(ServerVersions.v23_3);
		base.Repository.RefreshObject(InternalId);
	}
}
