using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("Site")]
public abstract class Site : VmoObjectGuidBased, ISite
{
	private Version _version;

	private ReadOnlyAddressCollection _addresses;

	public string Name => GetPropertyValue<string>("Name");

	public string Description => GetPropertyValue<string>("Description");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public DateTime LastConfirmedHandshake => GetPropertyValue<DateTime>("LastStatusHandshake");

	public ConnectionState ConnectionState => GetPropertyValue<ConnectionState>("ConnectionState");

	public Version Version
	{
		get
		{
			if (_version == null && !Version.TryParse(GetPropertyValue<string>("Version"), out _version))
			{
				throw new InvalidOperationException("Unable to convert version information");
			}
			return _version;
		}
	}

	public ServerVersion SiteVersion
	{
		get
		{
			if (!(Version != null))
			{
				return null;
			}
			return new ServerVersion(Version);
		}
	}

	public string ServiceAccount => GetPropertyValue<string>("ServiceAccount");

	public int SynchronizationStatus => GetPropertyValue<int>("SynchronizationStatus");

	public ReadOnlyAddressCollection Addresses
	{
		get
		{
			if (_addresses == null)
			{
				_addresses = new ReadOnlyAddressCollection();
				if (GetPropertyValue<string>("Addresses") != null)
				{
					foreach (ServerAddress item in ServerAddresses.GetFromXml(GetPropertyValue<string>("Addresses")))
					{
						ReadOnlyAddress address = new ReadOnlyAddress(new Uri(item.URL), (AddressType)item.Type);
						_addresses.Add(address);
					}
				}
			}
			return _addresses;
		}
	}

	public Site(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	protected override void PropertiesLoaded()
	{
		if (_addresses != null)
		{
			_addresses = null;
		}
	}

	protected override PermissionSetBase PermissionSetFromActionId(string actionId)
	{
		return (SitePermissionSet)SitePermissionSet.CreateFromActionId(actionId);
	}

	public SitePermissionInfo[] EnumPermissions()
	{
		return EnumPermissions<SitePermissionInfo>();
	}

	public SitePermissionInfo[] EnumPermissions(SitePermissionSet permissions)
	{
		return EnumPermissions<SitePermissionInfo>(permissions);
	}

	public SitePermissionInfo[] EnumPermissions(string identity)
	{
		return EnumPermissions<SitePermissionInfo>(identity, SitePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public SitePermissionInfo[] EnumPermissions(string identity, SitePermissionSet permissions)
	{
		return EnumPermissions<SitePermissionInfo>(identity, permissions);
	}

	public SitePermissionSet EffectivePermissions(string identity)
	{
		return EffectivePermissions(identity, SitePermissionSet.SupportedPermissions(base.Repositories.ServerVersion));
	}

	public SitePermissionSet EffectivePermissions(string identity, SitePermissionSet permissions)
	{
		return base.EffectivePermissions(identity, permissions);
	}

	public void Allow(SitePermissionSet permissions, string identity)
	{
		Allow(permissions, new string[1] { identity });
	}

	public void Allow(SitePermissionSet permissions, string[] identities)
	{
		Allow((PermissionSetBase)permissions, identities);
	}

	public void Deny(SitePermissionSet permission, string identity)
	{
		Deny(permission, new string[1] { identity });
	}

	public void Deny(SitePermissionSet permission, string[] identities)
	{
	}

	public void Revoke(SitePermissionSet permissions, string identity)
	{
		Revoke(permissions, new string[1] { identity });
	}

	public void Revoke(SitePermissionSet permissions, string[] identities)
	{
		Revoke((PermissionSetBase)permissions, identities);
	}

	public void UpdateCustomProperties()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.SaveCustomProperties(base.InternalId, base.CustomProperties.DirtyCustomPropertyArray());
		CreateCustomPropertyCollection();
	}
}
