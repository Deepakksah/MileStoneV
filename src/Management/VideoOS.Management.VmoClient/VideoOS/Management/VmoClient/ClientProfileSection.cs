using System;

namespace VideoOS.Management.VmoClient;

public class ClientProfileSection : VmoObjectSecurity
{
	private readonly Guid _managementServerId;

	private string activeManagementClientProfileId = string.Empty;

	private string activeSmartClientProfileId = string.Empty;

	public ManagementClientProfileCollection ManagementClientProfiles { get; private set; }

	public SmartClientProfileCollection SmartClientProfiles { get; private set; }

	public ManagementClientProfile ActiveManagementClientProfile
	{
		get
		{
			if (string.IsNullOrEmpty(activeManagementClientProfileId))
			{
				activeManagementClientProfileId = base.Repositories.ServerCommunication.QueryActiveManagementClientProfileId();
			}
			return base.Repositories.GetObjectById<ManagementClientProfile>(new Guid(activeManagementClientProfileId), refreshCachedObjects: false);
		}
	}

	public SmartClientProfile ActiveSmartClientProfile
	{
		get
		{
			if (string.IsNullOrEmpty(activeSmartClientProfileId))
			{
				activeSmartClientProfileId = base.Repositories.ServerCommunication.QueryActiveSmartClientProfileId();
			}
			return base.Repositories.GetObjectById<SmartClientProfile>(new Guid(activeSmartClientProfileId), refreshCachedObjects: false);
		}
	}

	public ClientProfileSection(ManagementServer managementServer)
		: base(managementServer.Repositories, managementServer.InternalId)
	{
		_managementServerId = managementServer.Id;
		ManagementClientProfiles = new ManagementClientProfileCollection(LoadManagementClientProfiles, managementServer, isInitialized: false);
		SmartClientProfiles = new SmartClientProfileCollection(LoadSmartClientProfiles, managementServer, isInitialized: false);
	}

	public void InternalRefresh()
	{
		activeManagementClientProfileId = string.Empty;
		activeSmartClientProfileId = string.Empty;
	}

	public SmartClientProfile GetSmartClientProfileBySid(string sid)
	{
		string g = base.Repositories.ServerCommunication.QueryActiveSmartClientProfileId(sid);
		return base.Repositories.GetObjectById<SmartClientProfile>(new Guid(g), refreshCachedObjects: true);
	}

	private ManagementClientProfile[] LoadManagementClientProfiles(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, ManagementClientProfile>(_managementServerId, refreshCachedObjects);
	}

	private SmartClientProfile[] LoadSmartClientProfiles(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, SmartClientProfile>(_managementServerId, refreshCachedObjects);
	}
}
