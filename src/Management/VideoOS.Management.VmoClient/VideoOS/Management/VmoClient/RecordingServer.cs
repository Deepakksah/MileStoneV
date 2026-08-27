using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("RecordingServer")]
public class RecordingServer : VmoObjectGuidBased, ICreatable, IUpdatable
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

	public TimeSpan SynchronizationTime
	{
		get
		{
			return TimeSpan.FromMinutes(GetPropertyValue<int>("SynchronizationTime"));
		}
		set
		{
			SetPropertyValue("SynchronizationTime", Convert.ToInt32(Math.Round(value.TotalMinutes)));
		}
	}

	public bool IsSynchronizationEnabled
	{
		get
		{
			return GetPropertyValue<int>("SynchronizationTime") != -1;
		}
		set
		{
			if (!value)
			{
				SetPropertyValue("SynchronizationTime", -1);
			}
		}
	}

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public bool Enabled => GetPropertyValue<bool>("Enabled");

	public string HostName => GetPropertyValue<string>("HostName");

	public string TimeZoneName => GetPropertyValue<string>("TimeZoneName");

	public string MulticastSettings => GetPropertyValue<string>("MulticastSettings");

	public string DevicePackInfo => GetPropertyValue<string>("DevicePackInfo");

	public bool IsX64 => GetPropertyValue<bool>("X64");

	public string PrimaryFailoverGroup => GetPropertyValue<string>("PrimaryFailoverGroup");

	public string SecondaryFailoverGroup => GetPropertyValue<string>("SecondaryFailoverGroup");

	public string OriginalHostName => GetPropertyValue<string>("FailoverHostName");

	public int FailoverPort => GetPropertyValue<int>("FailoverPort");

	public Guid FailoverId => new Guid(GetPropertyValue<string>("FailoverId"));

	public bool IsFailoverRunning => FailoverId.Equals(Guid.Empty);

	public string ScanRanges => GetPropertyValue<string>("ScanRanges");

	public Uri WebserverUri => new Uri(GetPropertyValue<string>("WebServerUri"));

	public Uri WebserverUriConfiguration
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

	public RecordingStorageCollection RecordingStorages { get; private set; }

	public HardwareCollection Hardware { get; private set; }

	public RecordingServer(ManagementServer managementServer, string name, string hostName, string webserverAddress, RecordingServerCreateOptions createOptions)
		: this(managementServer.Repositories)
	{
		SetRecorderProperties(name, hostName, webserverAddress, createOptions);
	}

	public RecordingServer(ManagementServer managementServer, Guid customId, string name, string hostName, string webserverAddress, RecordingServerCreateOptions createOptions)
		: this(managementServer.Repositories)
	{
		if (Guid.Empty.Equals(customId))
		{
			throw new ArgumentException("Empty id", "customId");
		}
		SetPropertyValue("Id", customId.ToString());
		SetRecorderProperties(name, hostName, webserverAddress, createOptions);
	}

	private void SetRecorderProperties(string name, string hostName, string webserverAddress, RecordingServerCreateOptions createOptions)
	{
		if (name == null)
		{
			throw new ArgumentNullException("name");
		}
		if (string.IsNullOrWhiteSpace(name))
		{
			throw new ArgumentException("The parameter must not be an empty string.", "name");
		}
		if (hostName == null)
		{
			throw new ArgumentNullException("hostName");
		}
		if (string.IsNullOrWhiteSpace(hostName))
		{
			throw new ArgumentException("The parameter must not be an empty string.", "hostName");
		}
		if (webserverAddress == null)
		{
			throw new ArgumentNullException("webserverAddress");
		}
		if (string.IsNullOrWhiteSpace(webserverAddress))
		{
			throw new ArgumentException("The parameter must not be an empty string.", "webserverAddress");
		}
		if (createOptions == null)
		{
			throw new ArgumentNullException("createOptions");
		}
		SetPropertyValue("Name", name);
		SetPropertyValue("HostName", hostName);
		SetPropertyValue("WebServerUri", webserverAddress);
		string id = TimeZoneInfo.Local.Id;
		SetPropertyValue("TimeZoneName", id);
		SetPropertyValue("DefaultStorageName", createOptions.DefaultStorageName);
		SetPropertyValue("DefaultStoragePath", createOptions.DefaultStoragePath);
		SetPropertyValue("DefaultStorageRetainMinutes", createOptions.DefaultStorageRetainMinutes);
		SetPropertyValue("DefaultStorageMaxSizeInMB", createOptions.DefaultStorageMaxSize);
		SetPropertyValue("DefaultFailoverUdpPort", createOptions.DefaultFailoverUdpPort);
	}

	public RecordingServer(IVmoRepositories repositories)
		: this(string.Empty, new VmoPropertyCollection(), new VmoRelationCollection(), repositories, isInitialized: true)
	{
	}

	public RecordingServer(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		RecordingStorages = new RecordingStorageCollection(LoadRecordingStorage, this, isInitialized);
		Hardware = new HardwareCollection(LoadHardware, this, isInitialized);
	}

	public HardwareDriver[] GetActiveHardwareDrivers()
	{
		ThrowIfDeleted();
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.QueryActiveHardwareDrivers(base.Id.ToString());
		List<HardwareDriver> list = new List<HardwareDriver>();
		VMOWebserviceObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			HardwareDriver item = new HardwareDriver(array2[i].Properties);
			list.Add(item);
		}
		return list.ToArray();
	}

	public void Create()
	{
		base.Repository.CreateObject(this);
	}

	public HardwareDriver[] GetHardwareDrivers(bool usedOnly)
	{
		ThrowIfDeleted();
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.QueryRecordingServerHardwareDrivers(base.Id.ToString(), usedOnly);
		List<HardwareDriver> list = new List<HardwareDriver>();
		VMOWebserviceObject[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			HardwareDriver item = new HardwareDriver(array2[i].Properties);
			list.Add(item);
		}
		return list.ToArray();
	}

	public RecordingServerInformation GetRecordingServerInformation()
	{
		ThrowIfDeleted();
		VMOWebserviceObject recordingServerInformation = base.Repositories.ServerCommunication.GetRecordingServerInformation(InternalId);
		if (recordingServerInformation == null)
		{
			return null;
		}
		return new RecordingServerInformation(recordingServerInformation.Properties);
	}

	public Device[] QueryOtherDevicesWithMediaOnRecorder()
	{
		ThrowIfNotCreatedOrDeleted();
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		Guid[] id = (from o in base.Repositories.ServerCommunication.Execute("QueryOtherDevicesWithMediaOnRecorder", vmoPropertyCollection)
			select new Guid(o.Id)).ToArray();
		return base.Repositories.GetObjectsByIds<Device>(id, refreshCachedObjects: false);
	}

	public MarkedData[] QueryMarkedData()
	{
		ThrowIfNotCreatedOrDeleted();
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.Execute("QueryRecordingServerMarkedData", vmoPropertyCollection);
		if (array == null)
		{
			return null;
		}
		List<MarkedData> list = new List<MarkedData>();
		VMOWebserviceObject[] array2 = array;
		foreach (VMOWebserviceObject vMOWebserviceObject in array2)
		{
			MarkedData markedData = new MarkedData(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, base.Repositories, isCreated: true);
			if (markedData.MarkedDataType == MarkedDataDefinitions.MarkedDataTypes.MarkedData)
			{
				list.Add(markedData);
			}
		}
		return list.ToArray();
	}

	public bool HasMediaForMovedDevicesWithEvidenceLocks()
	{
		ThrowIfNotCreatedOrDeleted();
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("Id", InternalId);
		return base.Repositories.ServerCommunication.Execute("QueryRecorderHasMediaForMovedDevicesWithEvidenceLocks", vmoPropertyCollection).Any();
	}

	public AddHardwareTask AddHardware(Uri address, string userName, string password, int driverNumber)
	{
		ThrowIfNotCreatedOrDeleted();
		return AddHardwareTask.StartNew(base.Repositories, base.Id, address, userName, password, driverNumber);
	}

	public AddHardwareTask AddHardware(Uri address, string userName, string password, string definitionXml)
	{
		ThrowIfNotCreatedOrDeleted();
		return AddHardwareTask.StartNew(base.Repositories, base.Id, address, userName, password, definitionXml);
	}

	public HardwareScanTask HardwareScan(Uri address, int[] drivers, string userName, string password, bool useDefaultCredentials)
	{
		ThrowIfNotCreatedOrDeleted();
		return HardwareScanTask.StartNew(base.Repositories, base.Id, address, drivers, userName, password, useDefaultCredentials);
	}

	public HardwareScanExpressTask HardwareScanExpress(CameraCredentialCollection cameraCredentials, bool useDefaultCredentials)
	{
		ThrowIfNotCreatedOrDeleted();
		return HardwareScanExpressTask.StartNew(base.Repositories, base.Id, cameraCredentials, useDefaultCredentials, useSecurePort: false);
	}

	public HardwareScanExpressTask HardwareScanExpress(CameraCredentialCollection cameraCredentials, bool useDefaultCredentials, bool useSecurePort)
	{
		ThrowIfNotCreatedOrDeleted();
		return HardwareScanExpressTask.StartNew(base.Repositories, base.Id, cameraCredentials, useDefaultCredentials, useSecurePort);
	}

	public void Refresh()
	{
		base.Repository.RefreshObject(InternalId);
	}

	public void Update()
	{
		base.Repository.UpdateObject(this);
	}

	private RecordingStorage[] LoadRecordingStorage(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<RecordingServer, RecordingStorage>(base.Id, refreshCachedObjects);
	}

	private Hardware[] LoadHardware(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<RecordingServer, Hardware>(base.Id, refreshCachedObjects);
	}

	public AsyncTask[] LoadTasks()
	{
		return base.Repositories.GetNonVmoObjectsByParentId<RecordingServer, AsyncTask>(base.Id);
	}
}
