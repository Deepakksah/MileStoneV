using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("FailoverServer")]
public class FailoverServer : VmoObjectGuidBased, ICreatable, IUpdatable, IDeletable
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

	public DateTime CreatedDate => GetPropertyValue<DateTime>("CreatedDate");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public int Position
	{
		get
		{
			return GetPropertyValue<int>("Position");
		}
		set
		{
			SetPropertyValue("Position", value);
		}
	}

	public string HostName
	{
		get
		{
			return GetPropertyValue<string>("HostName");
		}
		set
		{
			SetPropertyValue("HostName", value);
		}
	}

	public int UdpPort
	{
		get
		{
			return GetPropertyValue<int>("UdpPort");
		}
		set
		{
			SetPropertyValue("UdpPort", value);
		}
	}

	public string DatabasePath
	{
		get
		{
			return GetPropertyValue<string>("DatabasePath");
		}
		set
		{
			SetPropertyValue("DatabasePath", value);
		}
	}

	public Guid RecordingServerServiceId => GetPropertyValue<Guid>("ServiceId");

	public ServiceStatus Status => GetPropertyValue<ServiceStatus>("Status");

	public bool Enabled => GetPropertyValue<bool>("Enabled");

	public Guid HotStandbyRecorderId
	{
		get
		{
			return new Guid(GetPropertyValue<string>("HotStandbyRecorderId"));
		}
		set
		{
			SetPropertyValue("HotStandbyRecorderId", value.ToString().ToUpperInvariant());
		}
	}

	public Uri WebServerUri => new Uri(GetPropertyValue<string>("WebServerUri"));

	public Uri WebServerUriConfiguration
	{
		get
		{
			string propertyValue = GetPropertyValue<string>("WebServerUriConfiguration");
			if (string.IsNullOrEmpty(propertyValue))
			{
				return null;
			}
			return new Uri(propertyValue);
		}
	}

	public bool PublicAccessEnabled
	{
		get
		{
			return GetPropertyValue<bool>("PublicAccessEnabled");
		}
		set
		{
			SetPropertyValue("PublicAccessEnabled", value);
		}
	}

	public string PublicWebServerHostName
	{
		get
		{
			return GetPropertyValue<string>("PublicWebServerHostName");
		}
		set
		{
			SetPropertyValue("PublicWebServerHostName", value);
		}
	}

	public int PublicWebServerPort
	{
		get
		{
			return GetPropertyValue<int>("PublicWebServerPort");
		}
		set
		{
			SetPropertyValue("PublicWebServerPort", value);
		}
	}

	public string ParentGroupId => GetPropertyValue<string>("ParentServerGroupId");

	public FailoverServerGroup ParentFailoverServerGroup
	{
		get
		{
			if (string.Equals(ParentGroupId, "6A85FFC3-C134-4338-A072-024817CB5820", StringComparison.OrdinalIgnoreCase))
			{
				return null;
			}
			return GetObjectById<FailoverServerGroup>(ParentGroupId);
		}
	}

	public ManagementServer ManagementServer => GetObjectById<ManagementServer>("909749AE-88C4-4780-A381-383D691E98FA");

	public FailoverServer(ManagementServer managementServer, Guid id, Guid recordingServerServiceId, bool clientEncryptionEnabled, string serverHostName, int serverPort)
		: this(managementServer.Repositories)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("ID may not be guid empty", "id");
		}
		if (recordingServerServiceId == Guid.Empty)
		{
			throw new ArgumentException("Recording Server Service ID may not be guid empty", "recordingServerServiceId");
		}
		SetPropertyValue("Id", id.ToString().ToUpper(CultureInfo.InvariantCulture));
		SetPropertyValue("ServiceId", recordingServerServiceId.ToString().ToUpper(CultureInfo.InvariantCulture));
		SetPropertyValue("ParentServerGroupId", managementServer.InternalId);
		SetPropertyValue("ClientEncryptionEnabled", clientEncryptionEnabled);
		SetPropertyValue("WebServerHostName", serverHostName);
		SetPropertyValue("WebServerPort", serverPort);
	}

	public FailoverServer(FailoverServerGroup failoverServerGroup, Guid id, Guid recordingServerServiceId, bool clientEncryptionEnabled, string serverHostName, int serverPort)
		: this(failoverServerGroup.Repositories)
	{
		if (id == Guid.Empty)
		{
			throw new ArgumentException("ID may not be guid empty", "id");
		}
		if (recordingServerServiceId == Guid.Empty)
		{
			throw new ArgumentException("Recording Server Service ID may not be guid empty", "recordingServerServiceId");
		}
		SetPropertyValue("Id", id.ToString().ToUpper(CultureInfo.InvariantCulture));
		SetPropertyValue("ServiceId", recordingServerServiceId.ToString().ToUpper(CultureInfo.InvariantCulture));
		SetPropertyValue("ParentServerGroupId", failoverServerGroup.InternalId);
		SetPropertyValue("ClientEncryptionEnabled", clientEncryptionEnabled);
		SetPropertyValue("WebServerHostName", serverHostName);
		SetPropertyValue("WebServerPort", serverPort);
	}

	public FailoverServer(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public FailoverServer(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
	}

	public void EnableFailoverServer()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.EnableFailoverServer(InternalId);
	}

	public void DisableFailoverServer()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.DisableFailoverServer(InternalId);
	}

	public void StopFailoverService()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repositories.ServerCommunication.StopFailoverService(InternalId);
	}

	public void SetRecorderInformation(bool clientEncryptionEnabled, string webserverHostName, int webserverPort, Version version, string timeZone)
	{
		ThrowIfNotCreatedOrDeleted();
		if (webserverHostName == null)
		{
			throw new ArgumentNullException("webserverHostName");
		}
		if (version == null)
		{
			throw new ArgumentNullException("version");
		}
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		vmoPropertyCollection.Add("ClientEncryptionEnabled", clientEncryptionEnabled);
		vmoPropertyCollection.Add("WebServerHostName", webserverHostName);
		vmoPropertyCollection.Add("WebServerPort", webserverPort);
		vmoPropertyCollection.Add("Version", version.ToString());
		vmoPropertyCollection.Add("TimeZone", timeZone);
		base.Repositories.ServerCommunication.Execute("SetFailoverRecorderInformation", vmoPropertyCollection);
		string scheme = ((!clientEncryptionEnabled) ? Uri.UriSchemeHttp : Uri.UriSchemeHttps);
		UriBuilder uriBuilder = new UriBuilder(scheme, webserverHostName, webserverPort);
		SetPropertyValue("WebServerUri", uriBuilder.Uri.AbsoluteUri);
	}

	public void MoveFailoverServer(FailoverServerGroup newParentGroup, int insertAtPosition)
	{
		if (newParentGroup == null)
		{
			throw new ArgumentNullException("newParentGroup");
		}
		newParentGroup.ThrowIfNotCreatedOrDeleted();
		MoveFailoverServer(newParentGroup.InternalId, insertAtPosition);
		newParentGroup.FailoverServers.Refresh(refreshChildObjects: true);
	}

	public void MoveFailoverServerToManagementServer(int insertAtPosition)
	{
		ThrowIfNotCreatedOrDeleted();
		MoveFailoverServer("6A85FFC3-C134-4338-A072-024817CB5820", insertAtPosition);
		ManagementServer.FailoverServers.Refresh(refreshChildObjects: true);
	}

	private void MoveFailoverServer(string failoverServerGroupId, int insertAtPosition)
	{
		ThrowIfNotCreatedOrDeleted();
		if (base.IsDirty)
		{
			throw new InvalidOperationException();
		}
		ManagementServer managementServer = null;
		FailoverServerGroup failoverServerGroup = null;
		if (string.Equals(ParentGroupId, "6A85FFC3-C134-4338-A072-024817CB5820", StringComparison.OrdinalIgnoreCase))
		{
			managementServer = ManagementServer;
			foreach (FailoverServer failoverServer in managementServer.FailoverServers)
			{
				if (failoverServer.IsDirty)
				{
					throw new InvalidOperationException();
				}
			}
		}
		else
		{
			failoverServerGroup = GetObjectById<FailoverServerGroup>(ParentGroupId);
			foreach (FailoverServer failoverServer2 in failoverServerGroup.FailoverServers)
			{
				if (failoverServer2.IsDirty)
				{
					throw new InvalidOperationException();
				}
			}
		}
		if (string.Equals(failoverServerGroupId, "6A85FFC3-C134-4338-A072-024817CB5820", StringComparison.OrdinalIgnoreCase))
		{
			managementServer = ManagementServer;
			foreach (FailoverServer failoverServer3 in managementServer.FailoverServers)
			{
				if (failoverServer3.IsDirty)
				{
					throw new InvalidOperationException();
				}
			}
		}
		else
		{
			foreach (FailoverServer failoverServer4 in GetObjectById<FailoverServerGroup>(failoverServerGroupId).FailoverServers)
			{
				if (failoverServer4.IsDirty)
				{
					throw new InvalidOperationException();
				}
			}
		}
		base.Repositories.ServerCommunication.MoveFailoverServer(InternalId, failoverServerGroupId, insertAtPosition);
		Refresh(updateRelations: true);
		managementServer?.FailoverServers.Refresh(refreshChildObjects: true);
		failoverServerGroup?.FailoverServers.Refresh(refreshChildObjects: true);
	}

	public HardwareDriver[] GetHardwareDrivers()
	{
		ThrowIfDeleted();
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.QueryFailoverServerHardwareDrivers(base.Id.ToString());
		List<HardwareDriver> list = new List<HardwareDriver>();
		VMOWebserviceObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			HardwareDriver item = new HardwareDriver(array2[i].Properties);
			list.Add(item);
		}
		return list.ToArray();
	}

	public void Refresh()
	{
		Refresh(updateRelations: false);
	}

	public void Refresh(bool updateRelations)
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.RefreshObject(InternalId, updateRelations, updateRelatedObjects: true);
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public void Delete()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.DeleteObject(this);
	}

	public void Update()
	{
		ThrowIfNotCreatedOrDeleted();
		base.Repository.UpdateObject(this);
	}
}
