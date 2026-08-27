using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Hardware : IConfigurationItem
{
	public CameraFolder CameraFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CameraFolder")
				{
					return new CameraFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public CustomPropertiesFolder CustomPropertiesFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "CustomPropertiesFolder")
				{
					return new CustomPropertiesFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public HardwareDeviceEventFolder HardwareDeviceEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "HardwareDeviceEventFolder")
				{
					return new HardwareDeviceEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public HardwareDriverSettingsFolder HardwareDriverSettingsFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "HardwareDriverSettingsFolder")
				{
					return new HardwareDriverSettingsFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public HardwarePtzSettingsFolder HardwarePtzSettingsFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "HardwarePtzSettingsFolder")
				{
					return new HardwarePtzSettingsFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public InputEventFolder InputEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "InputEventFolder")
				{
					return new InputEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MetadataFolder MetadataFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MetadataFolder")
				{
					return new MetadataFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MicrophoneFolder MicrophoneFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MicrophoneFolder")
				{
					return new MicrophoneFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public OutputFolder OutputFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "OutputFolder")
				{
					return new OutputFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public SpeakerFolder SpeakerFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "SpeakerFolder")
				{
					return new SpeakerFolder(base.ServerId, configurationItem);
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

	public string Address
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Address");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Address", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string UserName
	{
		get
		{
			return base.Wrapper.GetPropertyValue("UserName");
		}
		set
		{
			base.Wrapper.SetPropertyValue("UserName", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string Model => base.Wrapper.GetPropertyValue("Model");

	public DateTime PasswordLastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("PasswordLastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

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

	public string HardwareDriverPath => base.Wrapper.GetPropertyValue("HardwareDriverPath");

	public Hardware(ServerId serverId, ConfigurationItem hardware)
		: base(serverId, hardware)
	{
	}

	public Hardware(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Hardware(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask ReadPasswordHardware()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("ReadPasswordHardware").ExecuteMethod("ReadPasswordHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'ReadPasswordHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangePasswordHardware(SecureString password, bool updatePasswordOnDevice)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangePasswordHardware");
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("UpdatePasswordOnDevice", updatePasswordOnDevice.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangePasswordHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'ChangePasswordHardware':" + ex.Message);
		}
		return null;
	}

	public ChangePasswordHardwareServerTask ChangePasswordHardware()
	{
		try
		{
			return new ChangePasswordHardwareServerTask(base.Wrapper.ExecuteMethod("ChangePasswordHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'ChangePasswordHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask MoveHardware(string destinationRecordingServer)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("MoveHardware");
			configurationApiItemWrapper.SetProperty("DestinationRecordingServer", destinationRecordingServer);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("MoveHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'MoveHardware':" + ex.Message);
		}
		return null;
	}

	public MoveHardwareServerTask MoveHardware()
	{
		try
		{
			return new MoveHardwareServerTask(base.Wrapper.ExecuteMethod("MoveHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'MoveHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask ReplaceHardware(string address, int port, bool useHttps, int httpsPort, string userName, SecureString password, string driver, string customDriverData, bool allowDeletingDisabledDevices)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ReplaceHardware");
			configurationApiItemWrapper.SetProperty("Address", address);
			configurationApiItemWrapper.SetProperty("Port", port.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("UseHttps", useHttps.ToString());
			configurationApiItemWrapper.SetProperty("HttpsPort", httpsPort.ToString(CultureInfo.InvariantCulture));
			configurationApiItemWrapper.SetProperty("UserName", userName);
			configurationApiItemWrapper.SetProperty("Password", password);
			configurationApiItemWrapper.SetProperty("Driver", driver);
			configurationApiItemWrapper.SetProperty("CustomDriverData", customDriverData);
			configurationApiItemWrapper.SetProperty("AllowDeletingDisabledDevices", allowDeletingDisabledDevices.ToString());
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ReplaceHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'ReplaceHardware':" + ex.Message);
		}
		return null;
	}

	public ReplaceHardwareServerTask ReplaceHardware()
	{
		try
		{
			return new ReplaceHardwareServerTask(base.Wrapper.ExecuteMethod("ReplaceHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'ReplaceHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask UpdateFirmwareHardware(string storageId)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("UpdateFirmwareHardware");
			configurationApiItemWrapper.SetProperty("StorageId", storageId);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("UpdateFirmwareHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'UpdateFirmwareHardware':" + ex.Message);
		}
		return null;
	}

	public UpdateFirmwareHardwareServerTask UpdateFirmwareHardware()
	{
		try
		{
			return new UpdateFirmwareHardwareServerTask(base.Wrapper.ExecuteMethod("UpdateFirmwareHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'UpdateFirmwareHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangePasswordHardware(string password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangePasswordHardware");
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangePasswordHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'ChangePasswordHardware':" + ex.Message);
		}
		return null;
	}

	public ServerTask ChangePasswordHardware(SecureString password)
	{
		try
		{
			ConfigurationApiItemWrapper configurationApiItemWrapper = base.Wrapper.ExecuteMethod("ChangePasswordHardware");
			configurationApiItemWrapper.SetProperty("Password", password);
			return new ServerTask(configurationApiItemWrapper.ExecuteMethod("ChangePasswordHardware"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'ChangePasswordHardware':" + ex.Message);
		}
		return null;
	}

	public UpdateHardwareServerTask UpdateHardware()
	{
		try
		{
			return new UpdateHardwareServerTask(base.Wrapper.ExecuteMethod("UpdateHardware", new Property[1]
			{
				new Property
				{
					Key = "SessionDataId",
					Value = 0.ToString(CultureInfo.InvariantCulture)
				}
			}));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'UpdateHardware':" + ex.Message);
		}
		return null;
	}

	public UpdateHardwareServerTask UpdateHardware(long sessionDataId)
	{
		try
		{
			return new UpdateHardwareServerTask(base.Wrapper.ExecuteMethod("UpdateHardware", new Property[1]
			{
				new Property
				{
					Key = "SessionDataId",
					Value = sessionDataId.ToString(CultureInfo.InvariantCulture)
				}
			}));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Hardware", "Unable to call method 'UpdateHardware':" + ex.Message);
		}
		return null;
	}
}
