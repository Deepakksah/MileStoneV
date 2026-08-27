using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class RecordingServer : IConfigurationItem
{
	public HardwareDriverFolder HardwareDriverFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "HardwareDriverFolder")
				{
					return new HardwareDriverFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public HardwareFolder HardwareFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "HardwareFolder")
				{
					return new HardwareFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public RecordingServerFailoverFolder RecordingServerFailoverFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "RecordingServerFailoverFolder")
				{
					return new RecordingServerFailoverFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public RecordingServerMulticastFolder RecordingServerMulticastFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "RecordingServerMulticastFolder")
				{
					return new RecordingServerMulticastFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public StorageFolder StorageFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "StorageFolder")
				{
					return new StorageFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
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

	public int SynchronizationTime
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("SynchronizationTime");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("SynchronizationTime", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string TimeZoneName => base.Wrapper.GetPropertyValue("TimeZoneName");

	public string HostName => base.Wrapper.GetPropertyValue("HostName");

	public int PortNumber
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PortNumber");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string WebServerUri => base.Wrapper.GetPropertyValue("WebServerUri");

	public string ActiveWebServerUri => base.Wrapper.GetPropertyValue("ActiveWebServerUri");

	public bool PublicAccessEnabled
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PublicAccessEnabled");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PublicAccessEnabled", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string PublicWebserverHostName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PublicWebserverHostName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PublicWebserverHostName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public int PublicWebserverPort
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("PublicWebserverPort");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
		set
		{
			base.Wrapper.SetPropertyValue("PublicWebserverPort", value.ToString(CultureInfo.InvariantCulture));
			base.Wrapper.IsDirty = true;
		}
	}

	public string MulticastServerAddress
	{
		get
		{
			return base.Wrapper.GetPropertyValue("MulticastServerAddress");
		}
		set
		{
			base.Wrapper.SetPropertyValue("MulticastServerAddress", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public bool ShutdownOnStorageFailure
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("ShutdownOnStorageFailure");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
		set
		{
			base.Wrapper.SetPropertyValue("ShutdownOnStorageFailure", value.ToString());
			base.Wrapper.IsDirty = true;
		}
	}

	public string Version => base.Wrapper.GetPropertyValue("Version");

	public RecordingServer(ServerId serverId, ConfigurationItem recordingServer)
		: base(serverId, recordingServer)
	{
	}

	public RecordingServer(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public RecordingServer(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask AddHardware(string hardwareAddress, string hardwareDriverPath, string userName, SecureString password, string customData)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddHardware");
			configurationApiItemWrapper.SetProperty("HardwareAddress", hardwareAddress);
			configurationApiItemWrapper.SetProperty("HardwareDriverPath", hardwareDriverPath);
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("CustomData", customData);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'AddHardware':" + ex.Message);
		}
		return null;
	}

	public AddHardwareServerTask AddHardware()
	{
		try
		{
			return new AddHardwareServerTask(base.Wrapper.ExecuteMethod("AddHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'AddHardware':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask GetOtherDevicesWithMediaOnRecorder()
	{
		try
		{
			return new ClassCollectionServerTask(base.Wrapper.ExecuteMethod("GetOtherDevicesWithMediaOnRecorder").ExecuteMethod("GetOtherDevicesWithMediaOnRecorder"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'GetOtherDevicesWithMediaOnRecorder':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask HardwareScan(string hardwareAddress, string hardwareDriverIds, string userName, SecureString password, bool useDefaultCredentials)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("HardwareScan");
			configurationApiItemWrapper.SetProperty("HardwareAddress", hardwareAddress);
			configurationApiItemWrapper.SetProperty("HardwareDriverIds", hardwareDriverIds);
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("UseDefaultCredentials", useDefaultCredentials.ToString());
			return new ClassCollectionServerTask(configurationApiItemWrapper.ExecuteMethod("HardwareScan"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'HardwareScan':" + ex.Message);
		}
		return null;
	}

	public HardwareScanServerTask HardwareScan()
	{
		try
		{
			return new HardwareScanServerTask(base.Wrapper.ExecuteMethod("HardwareScan"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'HardwareScan':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask HardwareScanExpress(string primaryUserName, string primaryPassword, string secondaryUserName, string secondaryPassword, string tertiaryUserName, string tertiaryPassword, bool useDefaultCredentials, bool useSecurePort)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("HardwareScanExpress");
			configurationApiItemWrapper.SetProperty("PrimaryUserName", primaryUserName);
			configurationApiItemWrapper.SetProperty("PrimaryPassword", primaryPassword);
			configurationApiItemWrapper.SetProperty("SecondaryUserName", secondaryUserName);
			configurationApiItemWrapper.SetProperty("SecondaryPassword", secondaryPassword);
			configurationApiItemWrapper.SetProperty("TertiaryUserName", tertiaryUserName);
			configurationApiItemWrapper.SetProperty("TertiaryPassword", tertiaryPassword);
			configurationApiItemWrapper.SetProperty("UseDefaultCredentials", useDefaultCredentials.ToString());
			configurationApiItemWrapper.SetProperty("UseSecurePort", useSecurePort.ToString());
			return new ClassCollectionServerTask(configurationApiItemWrapper.ExecuteMethod("HardwareScanExpress"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'HardwareScanExpress':" + ex.Message);
		}
		return null;
	}

	public HardwareScanExpressServerTask HardwareScanExpress()
	{
		try
		{
			return new HardwareScanExpressServerTask(base.Wrapper.ExecuteMethod("HardwareScanExpress"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'HardwareScanExpress':" + ex.Message);
		}
		return null;
	}

	public ServerTask ReadDrivesOnRecorder()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("ReadDrivesOnRecorder").ExecuteMethod("ReadDrivesOnRecorder"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'ReadDrivesOnRecorder':" + ex.Message);
		}
		return null;
	}

	public ServerTask ReadDirectoriesOnRecorder(string currentDirectory)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ReadDirectoriesOnRecorder");
			configurationApiItemWrapper.SetProperty("CurrentDirectory", currentDirectory);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ReadDirectoriesOnRecorder"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'ReadDirectoriesOnRecorder':" + ex.Message);
		}
		return null;
	}

	public ReadDirectoriesOnRecorderServerTask ReadDirectoriesOnRecorder()
	{
		try
		{
			return new ReadDirectoriesOnRecorderServerTask(base.Wrapper.ExecuteMethod("ReadDirectoriesOnRecorder"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'ReadDirectoriesOnRecorder':" + ex.Message);
		}
		return null;
	}

	public ServerTask CreateDirectoryOnRecorder(string newDirectoryFullName)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("CreateDirectoryOnRecorder");
			configurationApiItemWrapper.SetProperty("NewDirectoryFullName", newDirectoryFullName);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("CreateDirectoryOnRecorder"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'CreateDirectoryOnRecorder':" + ex.Message);
		}
		return null;
	}

	public CreateDirectoryOnRecorderServerTask CreateDirectoryOnRecorder()
	{
		try
		{
			return new CreateDirectoryOnRecorderServerTask(base.Wrapper.ExecuteMethod("CreateDirectoryOnRecorder"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'CreateDirectoryOnRecorder':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddHardware(string hardwareAddress, string hardwareDriverPath, string userName, string password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddHardware");
			configurationApiItemWrapper.SetProperty("HardwareAddress", hardwareAddress);
			configurationApiItemWrapper.SetProperty("HardwareDriverPath", hardwareDriverPath);
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'AddHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddHardware(string hardwareAddress, string hardwareDriverPath, string userName, SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddHardware");
			configurationApiItemWrapper.SetProperty("HardwareAddress", hardwareAddress);
			configurationApiItemWrapper.SetProperty("HardwareDriverPath", hardwareDriverPath);
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'AddHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask AddHardware(ChildTaskDetail hardwareScanResult, string customData = null)
	{
		if (hardwareScanResult == null)
		{
			throw new ArgumentNullMIPException("hardwareScanResult");
		}
		string property = hardwareScanResult.GetProperty("HardwareAddress");
		string property2 = hardwareScanResult.GetProperty("HardwareDriverPath");
		string property3 = hardwareScanResult.GetProperty("UserName");
		string property4 = hardwareScanResult.GetProperty("Password");
		if (property == null || property2 == null || property3 == null || property4 == null)
		{
			throw new ArgumentMIPException("One or more required properties missing in the provided hardwareScanResult. It must be the result of a hardware scan task.");
		}
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("AddHardware");
			configurationApiItemWrapper.SetProperty("HardwareAddress", property);
			configurationApiItemWrapper.SetProperty("HardwareDriverPath", property2);
			configurationApiItemWrapper.SetProperty("UserName", property3);
			configurationApiItemWrapper.SetProperty("Password", property4);
			if (!string.IsNullOrEmpty(customData))
			{
				configurationApiItemWrapper.SetProperty("CustomData", customData);
			}
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("AddHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'AddHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask HardwareScan(string hardwareAddress, string hardwareDriverIds, string userName, string password, bool useDefaultCredentials)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("HardwareScan");
			configurationApiItemWrapper.SetProperty("HardwareAddress", hardwareAddress);
			configurationApiItemWrapper.SetProperty("HardwareDriverIds", hardwareDriverIds);
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("UseDefaultCredentials", useDefaultCredentials.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("HardwareScan"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'HardwareScan':" + ex.Message);
		}
		return null;
	}

	public ServerTask HardwareScanExpress(string primaryUserName, string primaryPassword, string secondaryUserName, string secondaryPassword, string tertiaryUserName, string tertiaryPassword, bool useDefaultCredentials)
	{
		return HardwareScanExpress(primaryUserName, primaryPassword, secondaryUserName, secondaryPassword, tertiaryUserName, tertiaryPassword, useDefaultCredentials, useSecurePort: false);
	}

	public ServerTask LoadTasks(string taskType)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = new ConfigurationApiItemWrapper(base.Wrapper.ServerId, base.Wrapper.Path);
			configurationApiItemWrapper.ClearProperties();
			configurationApiItemWrapper.ConfigurationItem.EnableProperty = null;
			configurationApiItemWrapper.ConfigurationItem.ItemType = "InvokeInfo";
			configurationApiItemWrapper.SetProperty("TaskType", taskType);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("LoadTasks"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'LoadTasks':" + ex.Message);
		}
		return null;
	}

	public ClassCollectionServerTask LoadTasks()
	{
		try
		{
			return new ClassCollectionServerTask(base.Wrapper.ExecuteMethod("LoadTasks").ExecuteMethod("LoadTasks"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "RecordingServer", "Unable to call method 'LoadTasks':" + ex.Message);
		}
		return null;
	}
}
