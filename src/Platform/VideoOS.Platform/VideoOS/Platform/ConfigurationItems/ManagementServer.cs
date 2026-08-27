using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ManagementServer : IConfigurationItem
{
	public bool IsSmartClientAllowed => IsClientAllowed("SmartClient");

	public bool IsWebClientAllowed => IsClientAllowed("WebClient");

	public bool IsMobileClientAllowed => IsClientAllowed("MobileClient");

	public AccessControlSystemFolder AccessControlSystemFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "AccessControlSystemFolder")
				{
					return new AccessControlSystemFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public AlarmDefinitionFolder AlarmDefinitionFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "AlarmDefinitionFolder")
				{
					return new AlarmDefinitionFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public AnalyticsEventFolder AnalyticsEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "AnalyticsEventFolder")
				{
					return new AnalyticsEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public BasicOwnerInformationFolder BasicOwnerInformationFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "BasicOwnerInformationFolder")
				{
					return new BasicOwnerInformationFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public BasicUserFolder BasicUserFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "BasicUserFolder")
				{
					return new BasicUserFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public CameraGroupFolder CameraGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CameraGroupFolder")
				{
					return new CameraGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public ClientProfileFolder ClientProfileFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ClientProfileFolder")
				{
					return new ClientProfileFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public CompressorFolder CompressorFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CompressorFolder")
				{
					return new CompressorFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public EventTypeGroupFolder EventTypeGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "EventTypeGroupFolder")
				{
					return new EventTypeGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public EvidenceLockProfileFolder EvidenceLockProfileFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "EvidenceLockProfileFolder")
				{
					return new EvidenceLockProfileFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public FailoverGroupFolder FailoverGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "FailoverGroupFolder")
				{
					return new FailoverGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public GenericEventDataSourceFolder GenericEventDataSourceFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "GenericEventDataSourceFolder")
				{
					return new GenericEventDataSourceFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public GenericEventFolder GenericEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "GenericEventFolder")
				{
					return new GenericEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public GisMapLocationFolder GisMapLocationFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "GisMapLocationFolder")
				{
					return new GisMapLocationFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public InputEventGroupFolder InputEventGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "InputEventGroupFolder")
				{
					return new InputEventGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public LayoutGroupFolder LayoutGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "LayoutGroupFolder")
				{
					return new LayoutGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public LicenseInformationFolder LicenseInformationFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "LicenseInformationFolder")
				{
					return new LicenseInformationFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public LoginProviderFolder LoginProviderFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "LoginProviderFolder")
				{
					return new LoginProviderFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public LprMatchListFolder LprMatchListFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "LprMatchListFolder")
				{
					return new LprMatchListFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MailNotificationProfileFolder MailNotificationProfileFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MailNotificationProfileFolder")
				{
					return new MailNotificationProfileFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MatrixFolder MatrixFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MatrixFolder")
				{
					return new MatrixFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MetadataGroupFolder MetadataGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MetadataGroupFolder")
				{
					return new MetadataGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MicrophoneGroupFolder MicrophoneGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MicrophoneGroupFolder")
				{
					return new MicrophoneGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public OutputGroupFolder OutputGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "OutputGroupFolder")
				{
					return new OutputGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public RecordingServerFolder RecordingServerFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "RecordingServerFolder")
				{
					return new RecordingServerFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public RoleFolder RoleFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "RoleFolder")
				{
					return new RoleFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public SaveSearchesFolder SaveSearchesFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "SaveSearchesFolder")
				{
					return new SaveSearchesFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public ServerFolder ServerFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ServerFolder")
				{
					return new ServerFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public SiteFolder SiteFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "SiteFolder")
				{
					return new SiteFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public SpeakerGroupFolder SpeakerGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "SpeakerGroupFolder")
				{
					return new SpeakerGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public StateGroupFolder StateGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "StateGroupFolder")
				{
					return new StateGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public SystemAddressFolder SystemAddressFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "SystemAddressFolder")
				{
					return new SystemAddressFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public TimeProfileFolder TimeProfileFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "TimeProfileFolder")
				{
					return new TimeProfileFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public ToolOptionFolder ToolOptionFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ToolOptionFolder")
				{
					return new ToolOptionFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public UserDefinedEventFolder UserDefinedEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "UserDefinedEventFolder")
				{
					return new UserDefinedEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public VideoWallFolder VideoWallFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "VideoWallFolder")
				{
					return new VideoWallFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public ViewGroupFolder ViewGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "ViewGroupFolder")
				{
					return new ViewGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public string TimeZone => base.Wrapper.GetPropertyValue("TimeZone");

	public string ComputerName => base.Wrapper.GetPropertyValue("ComputerName");

	public string DomainName => base.Wrapper.GetPropertyValue("DomainName");

	public DateTime LastStatusHandshake
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastStatusHandshake"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public int PhysicalMemory
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PhysicalMemory");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string Platform => base.Wrapper.GetPropertyValue("Platform");

	public int Processors
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("Processors");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string ServiceAccount => base.Wrapper.GetPropertyValue("ServiceAccount");

	public int SynchronizationStatus
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("SynchronizationStatus");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string MasterSiteAddress => base.Wrapper.GetPropertyValue("MasterSiteAddress");

	public string Version => base.Wrapper.GetPropertyValue("Version");

	public ManagementServer(ServerId serverId, ConfigurationItem managementServer)
		: base(serverId, managementServer)
	{
	}

	public ManagementServer(FQID fqid)
		: base(fqid)
	{
	}

	public ManagementServer(ServerId serverId)
		: base(serverId, "/")
	{
	}

	private bool IsClientAllowed(string clientType)
	{
		if (!base.Wrapper.IsValid)
		{
			return true;
		}
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ClientLogOnSupported");
			configurationApiItemWrapper.SetProperty("ClientApplicationType", clientType);
			return configurationApiItemWrapper.ExecuteMethod("ClientLogOnSupported").GetPropertyValueBool("ClientLogOnAllowed");
		}
		catch (ArgumentException)
		{
			return true;
		}
		catch (Exception)
		{
			return false;
		}
	}

	public ServerTask ClientLogOnSupported(string clientApplicationType, string userName)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ClientLogOnSupported");
			configurationApiItemWrapper.SetProperty("ClientApplicationType", clientApplicationType);
			configurationApiItemWrapper.SetProperty("UserName", userName);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ClientLogOnSupported"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ManagementServer", "Unable to call method 'ClientLogOnSupported':" + ex.Message);
		}
		return null;
	}

	public ClientLogOnSupportedServerTask ClientLogOnSupported()
	{
		try
		{
			return new ClientLogOnSupportedServerTask(base.Wrapper.ExecuteMethod("ClientLogOnSupported"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ManagementServer", "Unable to call method 'ClientLogOnSupported':" + ex.Message);
		}
		return null;
	}

	public ServerTask UploadFileChunk(string transferId, string chunkData, int offset, int size, string checksum)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("UploadFileChunk");
			configurationApiItemWrapper.SetProperty("TransferId", transferId);
			configurationApiItemWrapper.SetProperty("ChunkData", chunkData);
			configurationApiItemWrapper.SetProperty("Offset", offset.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("Size", size.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("Checksum", checksum);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("UploadFileChunk"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ManagementServer", "Unable to call method 'UploadFileChunk':" + ex.Message);
		}
		return null;
	}

	public UploadFileChunkServerTask UploadFileChunk()
	{
		try
		{
			return new UploadFileChunkServerTask(base.Wrapper.ExecuteMethod("UploadFileChunk"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ManagementServer", "Unable to call method 'UploadFileChunk':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask LoadTasks(bool includeRecordingServerTasks)
	{
		return LoadTasks(includeRecordingServerTasks, "");
	}

	public ClassCollectionServerTask LoadTasks(bool includeRecordingServerTasks, string taskType)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(base.Wrapper.ServerId, base.Wrapper.Path);
			configurationApiItemWrapper.ClearProperties();
			configurationApiItemWrapper.ConfigurationItem.EnableProperty = null;
			configurationApiItemWrapper.ConfigurationItem.ItemType = "InvokeInfo";
			configurationApiItemWrapper.SetProperty("IncludeRecordingServerTasks", includeRecordingServerTasks.ToString());
			configurationApiItemWrapper.SetProperty("TaskType", taskType);
			return new ClassCollectionServerTask(configurationApiItemWrapper.ExecuteMethod("LoadTasks"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ManagementServer", "Unable to call method 'LoadTasks':" + ex.Message);
		}
		return null;
	}

	public LoadTasksServerTask LoadTasks()
	{
		try
		{
			return new LoadTasksServerTask(base.Wrapper.ExecuteMethod("LoadTasks"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "ManagementServer", "Unable to call method 'LoadTasks':" + ex.Message);
		}
		return null;
	}
}
