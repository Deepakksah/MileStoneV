using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("ManagementServer")]
public sealed class ManagementServer : VmoObjectGuidBased, IUpdatable, ISite
{
	private Version _version;

	private AddressCollection _addresses;

	private bool triedCreatePrivateViewGroup;

	private readonly MetadataCategoryCollection _metadataCategories;

	private readonly MetadataObjectDefinitionCollection _metadataObjectDefinitions;

	private readonly MetadataSearchNamespaceCollection _metadataSearchNamespaces;

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

	public string ComputerName => GetPropertyValue<string>("ComputerName");

	public string DomainName => GetPropertyValue<string>("DomainName");

	public DateTime LastModified => GetPropertyValue<DateTime>("LastModified");

	public DateTime LastStatusHandshake => GetPropertyValue<DateTime>("LastStatusHandshake");

	public string ServiceAccount => GetPropertyValue<string>("ServiceAccount");

	public int SynchronizationStatus => GetPropertyValue<int>("SynchronizationStatus");

	public string TimeZone => GetPropertyValue<string>("TimeZone");

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

	public AddressCollection Addresses
	{
		get
		{
			if (_addresses == null)
			{
				_addresses = new AddressCollection();
				if (GetPropertyValue<string>("Addresses") != null)
				{
					foreach (ServerAddress item in ServerAddresses.GetFromXml(GetPropertyValue<string>("Addresses")))
					{
						Address address = new Address(new Uri(item.URL), (AddressType)item.Type);
						_addresses.Add(address);
					}
				}
				_addresses.CollectionChangedEvent += AddressesChanged;
			}
			return _addresses;
		}
	}

	ReadOnlyAddressCollection ISite.Addresses
	{
		get
		{
			ReadOnlyAddressCollection readOnlyAddressCollection = new ReadOnlyAddressCollection();
			foreach (Address address in Addresses)
			{
				readOnlyAddressCollection.Add(new ReadOnlyAddress(address.ServerUrl, address.AddressType));
			}
			return readOnlyAddressCollection;
		}
	}

	public string MasterSiteId
	{
		get
		{
			return GetPropertyValue<string>("MasterSiteId");
		}
		set
		{
			SetPropertyValue("MasterSiteId", value);
		}
	}

	public MasterSite MasterSite
	{
		get
		{
			if (string.IsNullOrEmpty(MasterSiteId))
			{
				return null;
			}
			return GetObjectById<MasterSite>(MasterSiteId);
		}
	}

	public string PrivateUserGroupId
	{
		get
		{
			if (string.IsNullOrEmpty(GetPropertyValue<string>("PrivateUserGroupId")))
			{
				if (triedCreatePrivateViewGroup)
				{
					return string.Empty;
				}
				triedCreatePrivateViewGroup = true;
				string createPrivateViewRoot = base.Repositories.ServerCommunication.GetCreatePrivateViewRoot();
				SetPropertyValue("PrivateUserGroupId", createPrivateViewRoot);
			}
			return GetPropertyValue<string>("PrivateUserGroupId");
		}
	}

	public string LastAutomaticActivationErrorMessage
	{
		get
		{
			VMOExceptionDetail lastAutomaticActivationError = base.Repositories.ServerCommunication.GetLastAutomaticActivationError();
			if (lastAutomaticActivationError == null)
			{
				return null;
			}
			return ExceptionConverter.ConvertException(new FaultException<VMOExceptionDetail>(lastAutomaticActivationError)).Message;
		}
	}

	public RecordingServerCollection RecordingServers { get; private set; }

	public FailoverServerCollection FailoverServers { get; private set; }

	public MetadataCategoryCollection MetadataCategories => _metadataCategories;

	public MetadataObjectDefinitionCollection MetadataObjectDefinitions => _metadataObjectDefinitions;

	[Obsolete("Metadata search namespaces are not used anymore. Query methods on this class will return null or empty arrays. We are keeping the method for backwards compatibility reasons.")]
	public MetadataSearchNamespaceCollection MetadataSearchNamespaces => _metadataSearchNamespaces;

	public RuleCollection Rules { get; private set; }

	public AudioMessageCollection AudioMessages { get; private set; }

	public VideoWallCollection VideoWalls { get; private set; }

	public UserDefinedEventCollection UserDefinedEvents { get; private set; }

	public PluginEventCollection PluginEvents { get; private set; }

	public MIPActionCollection MIPActions { get; private set; }

	public TimeProfileCollection TimeProfiles { get; private set; }

	public ChildSiteCollection ChildSites { get; private set; }

	public AxisTunnelServerCollection AxisTunnelServers { get; private set; }

	public PublicViewGroupCollection PublicViewGroups { get; private set; }

	public PrivateViewGroupCollection PrivateViewGroups { get; private set; }

	public FailoverServerGroupCollection FailoverServerGroups { get; private set; }

	public MIPObjectGroupCollection MIPObjectGroups { get; private set; }

	public ViewLayoutGroupCollection ViewLayoutGroups { get; private set; }

	public BasicOwnerInfoCollection BasicOwnerInformation { get; private set; }

	public RetentionOptionRoleRelationCollection RetentionOptionRoleRelations { get; private set; }

	public PerformanceStateRuleConfigurationCollection PerformanceStateRuleConfigurations { get; private set; }

	public SystemDashboardConfigurationCollection SystemDashboardConfigurations { get; private set; }

	public DeviceGroupSection DeviceGroupSection { get; private set; }

	public SecuritySection SecuritySection { get; private set; }

	public ClientProfileSection ClientProfileSection { get; private set; }

	public ManagementServer(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		RecordingServers = new RecordingServerCollection(LoadRecorders, this, isInitialized: false);
		FailoverServers = new FailoverServerCollection(LoadFailoverServers, this, isInitialized: false);
		_metadataCategories = new MetadataCategoryCollection(LoadMetadataCategories, this, isInitialized: false);
		_metadataObjectDefinitions = new MetadataObjectDefinitionCollection(LoadMetadataObjectDefinitions, this, isInitialized: false);
		_metadataSearchNamespaces = new MetadataSearchNamespaceCollection(LoadMetadataSearchNamespaces, this, isInitialized: false);
		AudioMessages = new AudioMessageCollection(LoadAudioMessages, this, isInitialized: false);
		Rules = new RuleCollection(LoadRules, this, isInitialized: false);
		VideoWalls = new VideoWallCollection(LoadVideoWalls, this, isInitialized: false);
		UserDefinedEvents = new UserDefinedEventCollection(LoadUserDefinedEvents, this, isInitialized: false);
		PluginEvents = new PluginEventCollection(LoadPluginEvents, this, isInitialized: false);
		MIPActions = new MIPActionCollection(LoadMIPActions, this, isInitialized: false);
		TimeProfiles = new TimeProfileCollection(LoadTimeProfiles, this, isInitialized: false);
		ChildSites = new ChildSiteCollection(LoadChildSites, this, isInitialized: false);
		AxisTunnelServers = new AxisTunnelServerCollection(LoadAxisTunnelServers, this, isInitialized: false);
		PublicViewGroups = new PublicViewGroupCollection(LoadPublicViewGroups, this, isInitialized: false);
		PrivateViewGroups = new PrivateViewGroupCollection(LoadPrivateViewGroups, this, isInitialized: false);
		ViewLayoutGroups = new ViewLayoutGroupCollection(LoadViewLayoutGroups, this, isInitialized: false);
		FailoverServerGroups = new FailoverServerGroupCollection(LoadFailoverServerGroups, this, isInitialized: false);
		MIPObjectGroups = new MIPObjectGroupCollection(LoadMIPObjectGroups, this, isInitialized: false);
		DeviceGroupSection = new DeviceGroupSection(this);
		SecuritySection = new SecuritySection(this);
		ClientProfileSection = new ClientProfileSection(this);
		BasicOwnerInformation = new BasicOwnerInfoCollection(LoadBasicOwnerInformation, this, isInitialized: false);
		RetentionOptionRoleRelations = new RetentionOptionRoleRelationCollection(LoadRetentionOptionRoleRelation, this, isInitialized: false);
		PerformanceStateRuleConfigurations = new PerformanceStateRuleConfigurationCollection(LoadPerformanceStateRuleConfigurations, this, isInitialized: false);
		SystemDashboardConfigurations = new SystemDashboardConfigurationCollection(LoadSystemDashboardConfiguration, this, isInitialized: false);
	}

	private void AddressesChanged(object sender, EventArgs e)
	{
		SetPropertyValue("Addresses", _addresses.ToServerAddressXml());
	}

	protected override void PropertiesLoaded()
	{
		if (_addresses != null)
		{
			_addresses.CollectionChangedEvent -= AddressesChanged;
			_addresses = null;
		}
	}

	public ChildSite AttachChildSite(Guid childSiteId, AddressCollection addresses, string childSiteVersion)
	{
		if (childSiteId == Guid.Empty)
		{
			throw new ArgumentException("The parameter 'childSiteId' cannot be an empty guid", "childSiteId");
		}
		if (addresses == null)
		{
			throw new ArgumentNullException("addresses");
		}
		return GenericRepositoryFunctions.AttachChildSite(base.Repositories, childSiteId, addresses, childSiteVersion);
	}

	public void DetachChildSite(ChildSite childSite)
	{
		if (childSite == null)
		{
			throw new ArgumentNullException("childSite");
		}
		if (!string.Equals(childSite.ParentSiteId, base.Id.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			throw new ArgumentException("The childSite is not a child of this management server", "childSite");
		}
		if (!childSite.IsCreated)
		{
			throw new ArgumentException("The childSite is not created", "childSite");
		}
		GenericRepositoryFunctions.DetachChildSite(base.Repositories, childSite);
	}

	public MasterSite AttachMasterSite(Guid masterSiteId, AddressCollection addresses, string masterSiteVersion)
	{
		if (masterSiteId == Guid.Empty)
		{
			throw new ArgumentException("The parameter 'masterSiteId' cannot be an empty guid", "masterSiteId");
		}
		if (addresses == null)
		{
			throw new ArgumentNullException("addresses");
		}
		MasterSite masterSite = GenericRepositoryFunctions.AttachMasterSite(base.Repositories, masterSiteId, addresses, masterSiteVersion);
		MasterSiteId = masterSite.InternalId;
		return masterSite;
	}

	public void DetachMasterSite()
	{
		base.Repositories.ServerCommunication.DetachSite(MasterSiteId);
	}

	public void FullSiteSynchronization()
	{
		base.Repositories.ServerCommunication.TriggerFullSiteSynchronization();
	}

	public ManagementServiceStatus GetServiceStatus()
	{
		try
		{
			return (ManagementServiceStatus)base.Repositories.ServerCommunication.GetServerStatus();
		}
		catch (NotSupportedException)
		{
			throw;
		}
		catch (Exception)
		{
			throw;
		}
	}

	public SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByIds(string[] ids)
	{
		return GenericRepositoryFunctions.QuerySystemsMonitorStateSourcesByIds(base.Repositories, ids);
	}

	public SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByTypes(string[] types)
	{
		return GenericRepositoryFunctions.QuerySystemsMonitorStateSourcesByType(base.Repositories, types);
	}

	public RuleSource[] QueryAllRuleSources()
	{
		return GenericRepositoryFunctions.QueryAllRuleSources(base.Repositories);
	}

	public MarkedData[] BookmarkSearch(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optionalDeviceIds, string[] optionalUsers, string optionalSearchString, string optionalFromBookmarkId)
	{
		ValidateSearchParameters70(countLimit, deviceTypes, optionalDeviceIds, optionalUsers, optionalSearchString, optionalFromBookmarkId);
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.QueryBookmarksSearch(centerTime.ToUniversalTime(), timeLimit.ToUniversalTime(), countLimit, deviceTypes, optionalDeviceIds, optionalUsers, optionalSearchString, optionalFromBookmarkId);
		if (array == null)
		{
			return null;
		}
		List<MarkedData> list = new List<MarkedData>();
		VMOWebserviceObject[] array2 = array;
		foreach (VMOWebserviceObject vMOWebserviceObject in array2)
		{
			MarkedData item = new MarkedData(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, base.Repositories, isCreated: true);
			list.Add(item);
		}
		return list.ToArray();
	}

	public MarkedData[] BookmarkSearch(DateTime centerTime, DateTime timeLimit, int countLimit, DeviceType[] deviceTypes, string[] optionalDeviceIds, string[] optionalUsers, string optionalSearchString, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter, string optionalFromBookmarkId)
	{
		ValidateSearchParameters70(countLimit, deviceTypes, optionalDeviceIds, optionalUsers, optionalSearchString, optionalFromBookmarkId);
		ValidateSearchParameter222(optionalSearchString, optionalSearchHeader, optionalSearchDescription, optionalSearchReference, optionalSearchUser, optionalHeaderFilter, optionalDescriptionFilter);
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.QueryBookmarksSearch2(centerTime.ToUniversalTime(), timeLimit.ToUniversalTime(), countLimit, deviceTypes, optionalDeviceIds, optionalUsers, optionalSearchString, optionalSearchHeader, optionalSearchDescription, optionalSearchReference, optionalSearchUser, optionalHeaderFilter, optionalDescriptionFilter, optionalFromBookmarkId);
		if (array == null)
		{
			return null;
		}
		List<MarkedData> list = new List<MarkedData>();
		VMOWebserviceObject[] array2 = array;
		foreach (VMOWebserviceObject vMOWebserviceObject in array2)
		{
			MarkedData item = new MarkedData(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, base.Repositories, isCreated: true);
			list.Add(item);
		}
		return list.ToArray();
	}

	private static void ValidateSearchParameters70(int countLimit, DeviceType[] deviceTypes, string[] optionalDeviceIds, string[] optionalUsers, string optionalSearchString, string optionalFromBookmarkId)
	{
		if (countLimit < 1)
		{
			throw new ArgumentException("The result limit have to be atleast 1", "countLimit");
		}
		if (deviceTypes == null)
		{
			throw new ArgumentNullException("deviceTypes");
		}
		deviceTypes = GetRecordingDeviceTypes(deviceTypes);
		if (deviceTypes.Length == 0)
		{
			throw new ArgumentException("The search have to be done on atleast one device type. The search can only be made on Camera, Microphone, Metadata or Speaker", "deviceTypes");
		}
		if (optionalDeviceIds != null)
		{
			if (optionalDeviceIds.Length == 0)
			{
				throw new ArgumentException("The 'optionalDeviceIds' should contain atleast one device id or null", "optionalDeviceIds");
			}
			string[] array = optionalDeviceIds;
			for (int i = 0; i < array.Length; i++)
			{
				if (!Guid.TryParse(array[i], out var result))
				{
					throw new ArgumentException("The 'optionalDeviceIds' should contain valid guids", "optionalDeviceIds");
				}
				if (result == Guid.Empty)
				{
					throw new ArgumentException("The 'optionalDeviceIds' can not contain empty guids", "optionalDeviceIds");
				}
			}
		}
		if (optionalUsers != null)
		{
			if (optionalUsers.Length == 0)
			{
				throw new ArgumentException("The 'optionalUsers' should contain atleast one user name or null", "optionalUsers");
			}
			string[] array = optionalUsers;
			for (int i = 0; i < array.Length; i++)
			{
				if (string.IsNullOrEmpty(array[i]))
				{
					throw new ArgumentException("The 'optionalUsers' contains a empty user name", "optionalUsers");
				}
			}
		}
		if (optionalSearchString != null && optionalSearchString.Length == 0)
		{
			throw new ArgumentException("The 'optionalSearchString' should contain atleast one character or null", "optionalSearchString");
		}
		if (optionalFromBookmarkId != null)
		{
			if (!Guid.TryParse(optionalFromBookmarkId, out var result2))
			{
				throw new ArgumentException("The 'optionalFromBookmarkId' should contain valid guid or null", "optionalFromBookmarkId");
			}
			if (result2 == Guid.Empty)
			{
				throw new ArgumentException("The 'optionalFromBookmarkId' can not contain a empty guid", "optionalFromBookmarkId");
			}
		}
	}

	private static void ValidateSearchParameter222(string optionalSearchString, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter)
	{
		if (optionalSearchString == null && optionalSearchHeader)
		{
			throw new ArgumentException("The 'optionalSearchHeader' can not be chosen without a search string", "optionalSearchHeader");
		}
		if (optionalSearchString == null && optionalSearchDescription)
		{
			throw new ArgumentException("The 'optionalSearchDescription' can not be chosen without a search string", "optionalSearchDescription");
		}
		if (optionalSearchString == null && optionalSearchReference)
		{
			throw new ArgumentException("The 'optionalSearchReference' can not be chosen without a search string", "optionalSearchReference");
		}
		if (optionalSearchString == null && optionalSearchUser)
		{
			throw new ArgumentException("The 'optionalSearchUser' can not be chosen without a search string", "optionalSearchUser");
		}
		if (optionalHeaderFilter != null && optionalHeaderFilter.Length == 0)
		{
			throw new ArgumentException("The 'optionalHeaderFilter' should contain atleast one header or null", "optionalHeaderFilter");
		}
		if (optionalDescriptionFilter != null && optionalDescriptionFilter.Length == 0)
		{
			throw new ArgumentException("The 'optionalDescriptionFilter' should contain atleast one description or null", "optionalDescriptionFilter");
		}
	}

	private static DeviceType[] GetRecordingDeviceTypes(DeviceType[] deviceTypes)
	{
		List<DeviceType> list = new List<DeviceType>();
		if (deviceTypes != null)
		{
			foreach (DeviceType deviceType in deviceTypes)
			{
				if (DeviceTypeUtilities.IsRecordingDevice(deviceType))
				{
					list.Add(deviceType);
				}
			}
		}
		return list.ToArray();
	}

	public MarkedData MarkedDataSearch(Guid markedDataId)
	{
		if (markedDataId == Guid.Empty)
		{
			throw new ArgumentException("The marked data id can not be a empty guid", "markedDataId");
		}
		VMOWebserviceObject vMOWebserviceObject = base.Repositories.ServerCommunication.QueryMarkedData(markedDataId.ToString());
		if (vMOWebserviceObject == null)
		{
			return null;
		}
		return new MarkedData(vMOWebserviceObject.Id, vMOWebserviceObject.Properties, base.Repositories, isCreated: true);
	}

	public MarkedData[] MarkedDataSearch(string[] deviceIds, string searchText, bool optionalSearchHeader, bool optionalSearchDescription, bool optionalSearchReference, bool optionalSearchUser, string optionalHeaderFilter, string optionalDescriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.QueryMarkedDataSearch2(deviceIds, searchText, optionalSearchHeader, optionalSearchDescription, optionalSearchReference, optionalSearchUser, optionalHeaderFilter, optionalDescriptionFilter, users, createdFromTime.ToUniversalTime(), createdToTime.ToUniversalTime(), markedDataFromTime.ToUniversalTime(), markedDataToTime.ToUniversalTime(), tagTimeFromTime.ToUniversalTime(), tagTimeToTime.ToUniversalTime(), retentionExpireFrom.ToUniversalTime(), retentionExpireTo.ToUniversalTime(), pageIndex, pageSize, sortOrder.ToString(), sortAscending);
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

	public MarkedData[] MarkedDataSearch(string[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		VMOWebserviceObject[] array = base.Repositories.ServerCommunication.QueryMarkedDataSearch(deviceIds, searchText, users, createdFromTime.ToUniversalTime(), createdToTime.ToUniversalTime(), markedDataFromTime.ToUniversalTime(), markedDataToTime.ToUniversalTime(), tagTimeFromTime.ToUniversalTime(), tagTimeToTime.ToUniversalTime(), retentionExpireFrom.ToUniversalTime(), retentionExpireTo.ToUniversalTime(), pageIndex, pageSize, sortOrder.ToString(), sortAscending);
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

	public MarkedDataReference CreateMarkedDataReference(Device[] devices, bool triggerMarkedDataReferenceRequestedEvent, MarkedDataDefinitions.MarkedDataTypes markedDataTypes)
	{
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		foreach (Device obj in devices)
		{
			if (!obj.IsCreated)
			{
				throw new ArgumentException("One of the devices is not created", "devices");
			}
			if (obj.IsDeleted)
			{
				throw new ArgumentException("One of the devices is deleted", "devices");
			}
		}
		string[] deviceIds = devices.Select((Device d) => d.InternalId).ToArray();
		MarkedDataReference markedDataReference = base.Repositories.ServerCommunication.QueryMarkedDataReference(deviceIds, triggerMarkedDataReferenceRequestedEvent, markedDataTypes);
		markedDataReference.Repositories = base.Repositories;
		return markedDataReference;
	}

	public void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus)
	{
		base.Repositories.ServerCommunication.DeleteBookmarksByDeviceIdBeforeTime(mediaStatus);
	}

	public void DeleteServerTasksByDeviceBeforeTime(Guid deviceId, DateTime time)
	{
		base.Repositories.ServerCommunication.DeleteServerTasksByDeviceBeforeTime(deviceId.ToString(), time);
	}

	public AsyncTask[] LoadTasks()
	{
		return base.Repositories.GetNonVmoObjectsByParentId<ManagementServer, AsyncTask>(base.Id);
	}

	public void UpdateLicense(byte[] license, string ownerXml, string sitesInformation)
	{
		if (license == null)
		{
			throw new ArgumentNullException("license");
		}
		base.Repositories.ServerCommunication.UpdateLicense(license, ownerXml, sitesInformation);
	}

	public ActivateLicenseTask ActivateLicense(string userName, string password)
	{
		return ActivateLicenseTask.StartNew(base.Repositories, userName, password);
	}

	public void QueueAutomaticActivation()
	{
		base.Repositories.ServerCommunication.QueueAutomaticActivation();
	}

	public void RefreshLicenseCache()
	{
		base.Repositories.ServerCommunication.RefreshLicenseCache();
	}

	public void UpdateMipPluginsLicenseRequestPart(string mipLicenseRequestXml)
	{
		base.Repositories.ServerCommunication.UpdateMipPluginsLicenseRequestPart(mipLicenseRequestXml);
	}

	public void SetSystemSetting(string name, string value)
	{
		base.Repositories.ServerCommunication.SetSystemSetting(name, value);
	}

	public string GetSystemSetting(string name)
	{
		return base.Repositories.ServerCommunication.GetSystemSetting(name);
	}

	public bool IsFeatureLicensed(string featureName)
	{
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		if (string.IsNullOrEmpty(featureName))
		{
			throw new ArgumentException("Argument can not be empty", "featureName");
		}
		return base.Repositories.ServerCommunication.IsFeatureLicensed(featureName);
	}

	public string GetFeatureParameterValue(string featureName, string parameterName)
	{
		if (featureName == null)
		{
			throw new ArgumentNullException("featureName");
		}
		if (parameterName == null)
		{
			throw new ArgumentNullException("parameterName");
		}
		return base.Repositories.ServerCommunication.GetFeatureParameterValue(featureName, parameterName);
	}

	public DeviceSearchResult DeviceSearch(string deviceNameContains, string deviceShortNameContains, string hardwareAddressContains, IEnumerable<DeviceType> deviceTypes, string deviceIdEquals, string hardwareIdEquals, bool includeDisabledDevices)
	{
		ThrowIfCheckVersionFail(ServerVersions.v22_3);
		VmoPropertyCollection vmoPropertyCollection = new VmoPropertyCollection();
		vmoPropertyCollection.Add("DeviceNameContains", deviceNameContains);
		vmoPropertyCollection.Add("DeviceShortNameContains", deviceShortNameContains);
		vmoPropertyCollection.Add("DeviceDescriptionContains", string.Empty);
		vmoPropertyCollection.Add("HardwareAddressContains", hardwareAddressContains);
		vmoPropertyCollection.Add("DeviceIdEquals", deviceIdEquals);
		vmoPropertyCollection.Add("HardwareIdEquals", hardwareIdEquals);
		vmoPropertyCollection.Add("IncludeDisabledDevices", includeDisabledDevices);
		VmoPropertyCollection properties = CreateDeviceTypesPropertyCollection(deviceTypes);
		vmoPropertyCollection.SetNestedPropertyCollection("DeviceTypes", properties);
		return CreateDeviceSearchResult(base.Repositories.ServerCommunication.Execute("DeviceSearch", vmoPropertyCollection));
		static DeviceSearchResult CreateDeviceSearchResult(VMOWebserviceObject[] serviceObjects)
		{
			DeviceSearchResult deviceSearchResult = new DeviceSearchResult();
			foreach (VMOWebserviceObject obj in serviceObjects)
			{
				string id = obj.Id;
				string propertyValue = obj.Properties.GetPropertyValue<string>("ItemType");
				string propertyValue2 = obj.Properties.GetPropertyValue<string>("Name");
				string propertyValue3 = obj.Properties.GetPropertyValue<string>("ParentId");
				switch (propertyValue)
				{
				case "CameraDevice":
					deviceSearchResult.Devices.Add(new DeviceSearchCameraDeviceInfo(new Guid(id), propertyValue2, new Guid(propertyValue3)));
					break;
				case "MicrophoneDevice":
					deviceSearchResult.Devices.Add(new DeviceSearchMicrophoneDeviceInfo(new Guid(id), propertyValue2, new Guid(propertyValue3)));
					break;
				case "SpeakerDevice":
					deviceSearchResult.Devices.Add(new DeviceSearchSpeakerDeviceInfo(new Guid(id), propertyValue2, new Guid(propertyValue3)));
					break;
				case "MetadataDevice":
					deviceSearchResult.Devices.Add(new DeviceSearchMetadataDeviceInfo(new Guid(id), propertyValue2, new Guid(propertyValue3)));
					break;
				case "InputDevice":
					deviceSearchResult.Devices.Add(new DeviceSearchInputDeviceInfo(new Guid(id), propertyValue2, new Guid(propertyValue3)));
					break;
				case "OutputDevice":
					deviceSearchResult.Devices.Add(new DeviceSearchOutputDeviceInfo(new Guid(id), propertyValue2, new Guid(propertyValue3)));
					break;
				case "Hardware":
					deviceSearchResult.Hardware.Add(new DeviceSearchHardwareInfo(new Guid(id), propertyValue2, new Guid(propertyValue3)));
					break;
				case "RecordingServer":
					deviceSearchResult.RecordingServers.Add(new DeviceSearchRecordingServerInfo(new Guid(id), propertyValue2));
					break;
				}
			}
			return deviceSearchResult;
		}
		static VmoPropertyCollection CreateDeviceTypesPropertyCollection(IEnumerable<DeviceType> selectedDeviceTypes)
		{
			VmoPropertyCollection vmoPropertyCollection2 = new VmoPropertyCollection();
			int num = 0;
			foreach (DeviceType item in from t in (selectedDeviceTypes ?? Array.Empty<DeviceType>()).Distinct()
				where t != DeviceType.Unknown
				select t)
			{
				vmoPropertyCollection2.Add(num++.ToString(CultureInfo.InvariantCulture), item.ToString());
			}
			return vmoPropertyCollection2;
		}
	}

	public void Refresh()
	{
		ClientProfileSection.InternalRefresh();
		base.Repository.RefreshObject(base.InternalId);
	}

	public void Update()
	{
		base.Repository.UpdateObject(this);
	}

	private RecordingServer[] LoadRecorders(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, RecordingServer>(base.Id, refreshCachedObjects);
	}

	private FailoverServer[] LoadFailoverServers(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, FailoverServer>(base.Id, refreshCachedObjects);
	}

	private MetadataCategory[] LoadMetadataCategories(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, MetadataCategory>(base.Id, refreshCachedObjects);
	}

	private MetadataObjectDefinition[] LoadMetadataObjectDefinitions(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, MetadataObjectDefinition>(base.Id, refreshCachedObjects);
	}

	[Obsolete("Metadata search namespaces are not used anymore. Query methods on this class will return null or empty arrays. We are keeping the method for backwards compatibility reasons.")]
	private MetadataSearchNamespace[] LoadMetadataSearchNamespaces(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, MetadataSearchNamespace>(base.Id, refreshCachedObjects);
	}

	private AudioMessage[] LoadAudioMessages(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, AudioMessage>(base.Id, refreshCachedObjects);
	}

	private Rule[] LoadRules(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, Rule>(base.Id, refreshCachedObjects);
	}

	private VideoWall[] LoadVideoWalls(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, VideoWall>(base.Id, refreshCachedObjects);
	}

	private UserDefinedEvent[] LoadUserDefinedEvents(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, UserDefinedEvent>(base.Id, refreshCachedObjects);
	}

	private PluginEvent[] LoadPluginEvents(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, PluginEvent>(base.Id, refreshCachedObjects);
	}

	private MIPAction[] LoadMIPActions(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, MIPAction>(base.Id, refreshCachedObjects);
	}

	private TimeProfile[] LoadTimeProfiles(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, TimeProfile>(base.Id, refreshCachedObjects);
	}

	private ChildSite[] LoadChildSites(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, ChildSite>(base.Id, refreshCachedObjects);
	}

	private AxisTunnelServer[] LoadAxisTunnelServers(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, AxisTunnelServer>(base.Id, refreshCachedObjects);
	}

	private PublicViewGroup[] LoadPublicViewGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, PublicViewGroup>(base.Id, refreshCachedObjects);
	}

	private PrivateViewGroup[] LoadPrivateViewGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, PrivateViewGroup>(base.Id, refreshCachedObjects);
	}

	private ViewLayoutGroup[] LoadViewLayoutGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, ViewLayoutGroup>(base.Id, refreshCachedObjects);
	}

	private FailoverServerGroup[] LoadFailoverServerGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, FailoverServerGroup>(base.Id, refreshCachedObjects);
	}

	private MIPObjectGroup[] LoadMIPObjectGroups(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, MIPObjectGroup>(base.Id, refreshCachedObjects);
	}

	private BasicOwnerInfo[] LoadBasicOwnerInformation(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, BasicOwnerInfo>(base.Id, refreshCachedObjects);
	}

	private RetentionOptionRoleRelation[] LoadRetentionOptionRoleRelation(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, RetentionOptionRoleRelation>(base.Id, refreshCachedObjects);
	}

	private PerformanceStateRuleConfiguration[] LoadPerformanceStateRuleConfigurations(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, PerformanceStateRuleConfiguration>(base.Id, refreshCachedObjects);
	}

	private SystemDashboardConfiguration[] LoadSystemDashboardConfiguration(bool refreshCachedObjects)
	{
		return base.Repositories.GetObjectsByParentId<ManagementServer, SystemDashboardConfiguration>(base.Id, refreshCachedObjects);
	}

	public override void AddNotification(VmoObject obj)
	{
		Type type = obj.GetType();
		if (type == typeof(PublicViewGroup))
		{
			if (string.Equals(((PublicViewGroup)obj).ParentServerGroupId, "430C2FE2-B233-42DA-B508-1A22EF03383D", StringComparison.OrdinalIgnoreCase))
			{
				PublicViewGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(PrivateViewGroup))
		{
			if (string.Equals(((PrivateViewGroup)obj).ParentServerGroupId, PrivateUserGroupId, StringComparison.OrdinalIgnoreCase))
			{
				PrivateViewGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(ViewLayoutGroup))
		{
			if (string.Equals(((ViewLayoutGroup)obj).ParentServerGroupId, "430C2FE2-B233-42DA-B508-1A22EF03383D", StringComparison.OrdinalIgnoreCase))
			{
				ViewLayoutGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(FailoverServerGroup))
		{
			if (obj.Relations.Count == 0)
			{
				FailoverServerGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(FailoverServer))
		{
			if (string.Equals(((FailoverServer)obj).ParentGroupId, "6A85FFC3-C134-4338-A072-024817CB5820", StringComparison.OrdinalIgnoreCase))
			{
				FailoverServers.AddVmoObject(obj);
			}
		}
		else if (type == typeof(MIPObjectGroup))
		{
			if (obj.Relations.Count == 0)
			{
				MIPObjectGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(ChildSite))
		{
			if (obj.Relations.Count == 0)
			{
				ChildSites.AddVmoObject(obj);
			}
		}
		else if (type == typeof(CameraDeviceGroup))
		{
			if (obj.Relations.Count == 0)
			{
				DeviceGroupSection.CameraDeviceGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(MicrophoneDeviceGroup))
		{
			if (obj.Relations.Count == 0)
			{
				DeviceGroupSection.MicrophoneDeviceGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(MetadataDeviceGroup))
		{
			if (obj.Relations.Count == 0)
			{
				DeviceGroupSection.MetadataDeviceGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(SpeakerDeviceGroup))
		{
			if (obj.Relations.Count == 0)
			{
				DeviceGroupSection.SpeakerDeviceGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(InputDeviceGroup))
		{
			if (obj.Relations.Count == 0)
			{
				DeviceGroupSection.InputDeviceGroups.AddVmoObject(obj);
			}
		}
		else if (type == typeof(OutputDeviceGroup))
		{
			if (obj.Relations.Count == 0)
			{
				DeviceGroupSection.OutputDeviceGroups.AddVmoObject(obj);
			}
		}
		else
		{
			base.AddNotification(obj);
		}
	}
}
