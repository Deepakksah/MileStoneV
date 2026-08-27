using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using VideoOS.ConfigurationAPI;
using VideoOS.ConfigurationAPI.ConfigurationFaultException;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class AddHardwarePlugin : ProgressPlugin
{
	public AddHardwarePlugin(int serverPort)
		: base(serverPort, 16)
	{
	}

	public List<HardwareConfiguration> DoAddHardware(string configurationXml, ConfigurationApiServiceManager serviceManager)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		List<HardwareConfiguration> list = new List<HardwareConfiguration>();
		HardwareConfigurations hardwareConfigurations = new HardwareConfigurations();
		HardwareConfigurations hardwareConfigurations2 = new HardwareConfigurations();
		try
		{
			hardwareConfigurations = HardwareConfigurations.GetFromXml(configurationXml);
			int num = hardwareConfigurations.Count();
			if (num == 0)
			{
				return list;
			}
			ConfigurationItem[] array = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems("/RecordingServerFolder"), null);
			if (array.Length != 1)
			{
				return list;
			}
			ConfigurationItem recorderItem = array[0];
			int num2 = 0;
			foreach (HardwareConfiguration item2 in hardwareConfigurations)
			{
				if (Uri.TryCreate(item2.IPAddress, UriKind.Absolute, out var result))
				{
					OnProgressChanged(Convert.ToInt32((double)num2 / (double)num * 100.0), string.Format(CultureInfo.CurrentCulture, Resources.AddingCameras, string.Format(CultureInfo.CurrentCulture, ":   {0}:{1}", item2.Name, result.Port), num2 + 1, num));
				}
				else
				{
					OnProgressChanged(Convert.ToInt32((double)num2 / (double)num * 100.0), string.Format(CultureInfo.CurrentCulture, Resources.AddingCameras, ":   " + item2.Name, num2 + 1, num));
				}
				num2++;
				string text = null;
				try
				{
					text = AddHardwareToConfigApi(item2, recorderItem, serviceManager);
				}
				catch (Exception exception)
				{
					LogManager.Write(exception);
				}
				if (!string.IsNullOrEmpty(text))
				{
					hardwareConfigurations2.Add(item2);
					HardwareConfiguration item = AddDevicesToDeviceGroup(text, serviceManager, item2);
					list.Add(item);
				}
			}
		}
		catch (Exception exception2)
		{
			LogManager.Write(exception2);
		}
		list.AddRange(GetFailedHardware(hardwareConfigurations2, hardwareConfigurations));
		return list;
	}

	private static IEnumerable<HardwareConfiguration> GetFailedHardware(HardwareConfigurations addedHardware, HardwareConfigurations selectedHardware)
	{
		if (addedHardware.Count == selectedHardware.Count)
		{
			return new List<HardwareConfiguration>();
		}
		if (addedHardware.Count > selectedHardware.Count)
		{
			throw new ArgumentException("Number of added hardware is bigger that selected hardware");
		}
		return selectedHardware.Except(addedHardware).Select(delegate(HardwareConfiguration h)
		{
			h.Added = false;
			return h;
		});
	}

	public void EnableAndRenameDevices(ConfigurationApiServiceManager serviceManager, HardwareConfigurations hardwareConfigurations)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		if (hardwareConfigurations == null)
		{
			throw new ArgumentNullException("hardwareConfigurations");
		}
		try
		{
			int num = 0;
			int num2 = hardwareConfigurations.SelectMany((HardwareConfiguration f) => f.Devices).Count();
			OnProgressChanged(0, Resources.ProgressEnablingHardware);
			foreach (HardwareConfiguration hardwareConfiguration in hardwareConfigurations)
			{
				if (!hardwareConfiguration.Added)
				{
					continue;
				}
				try
				{
					string hardwarePath = "Hardware[" + hardwareConfiguration.Id.ToUpperInvariant() + "]";
					ConfigurationItem hardwareItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(hardwarePath), null);
					if (hardwareItem.Properties.TryGetPropertyValue("Name", out var value) && value != hardwareConfiguration.Name)
					{
						hardwareItem.Properties.SetValue("Name", hardwareConfiguration.Name);
						serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.SetItem(hardwareItem), null);
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Renaming hardware from: {0} to {1}", value, hardwareConfiguration.Name), TraceEventType.Information);
					}
				}
				catch (FaultException<PathNotFoundExceptionFault>)
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Path not found for hardware: {0}", hardwareConfiguration.Name), TraceEventType.Warning);
					continue;
				}
				foreach (DeviceConfiguration device in hardwareConfiguration.Devices)
				{
					try
					{
						ConfigurationItem deviceItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(device.ItemPath), null);
						deviceItem.EnableProperty.Enabled = device.Enabled;
						if (deviceItem.Properties.TryGetPropertyValue("Name", out var value2) && value2 != device.Name)
						{
							deviceItem.Properties.SetValue("Name", device.Name);
							LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Renaming device from: {0} to {1}", value2, device.Name), TraceEventType.Information);
						}
						serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.SetItem(deviceItem), null);
						num++;
						if (device.Enabled)
						{
							OnProgressChanged(Convert.ToInt32((double)num * 100.0 / (double)num2), string.Format(CultureInfo.CurrentCulture, Resources.ProgressEnablingDeviceN, device.Name));
							LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Enabling device: {0}", device.Name), TraceEventType.Information);
						}
						else
						{
							OnProgressChanged(Convert.ToInt32((double)num * 100.0 / (double)num2), string.Format(CultureInfo.CurrentCulture, Resources.ProgressDisablingDeviceN, device.Name));
							LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Disabling device: {0}", device.Name), TraceEventType.Information);
						}
					}
					catch (FaultException<PathNotFoundExceptionFault>)
					{
						num++;
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Path not found for device: {0}", device.Name), TraceEventType.Warning);
					}
				}
			}
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
	}

	private static HardwareConfiguration AddDevicesToDeviceGroup(string hardwarePath, ConfigurationApiServiceManager serviceManager, HardwareConfiguration hardwareConfiguration)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		List<DeviceConfiguration> list = new List<DeviceConfiguration>();
		ConfigurationItem hardware = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(hardwarePath), null);
		Property property = hardware.Properties.FirstOrDefault((Property p) => p.Key.Equals("Id", StringComparison.OrdinalIgnoreCase));
		Property property2 = hardware.Properties.FirstOrDefault((Property p) => p.Key.Equals("Name", StringComparison.OrdinalIgnoreCase));
		hardware.EnableProperty.Enabled = true;
		serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.SetItem(hardware), null);
		IEnumerable<DeviceConfiguration> collection = AddCameras(hardwarePath, serviceManager);
		list.AddRange(collection);
		IEnumerable<DeviceConfiguration> collection2 = AddMicrophones(hardwarePath, serviceManager);
		list.AddRange(collection2);
		IEnumerable<DeviceConfiguration> collection3 = AddSpeakers(hardwarePath, serviceManager);
		list.AddRange(collection3);
		IEnumerable<DeviceConfiguration> collection4 = AddMetaData(hardwarePath, serviceManager);
		list.AddRange(collection4);
		IEnumerable<DeviceConfiguration> collection5 = AddInput(hardwarePath, serviceManager);
		list.AddRange(collection5);
		IEnumerable<DeviceConfiguration> collection6 = AddOutput(hardwarePath, serviceManager);
		list.AddRange(collection6);
		hardwareConfiguration.Id = property.Value;
		hardwareConfiguration.Name = property2.Value;
		hardwareConfiguration.Added = true;
		hardwareConfiguration.Devices = list;
		return hardwareConfiguration;
	}

	private static IEnumerable<DeviceConfiguration> AddCameras(string hardwarePath, ConfigurationApiServiceManager serviceManager)
	{
		bool enabled = true;
		ConfigurationItem deviceGroupFolder = CreateCameraGroup(serviceManager);
		return from s in AddDevices(hardwarePath, serviceManager, deviceGroupFolder, "CameraFolder", enabled)
			select new CameraConfiguration
			{
				Id = s.Item1,
				Name = s.Item2,
				Added = true,
				Enabled = enabled
			};
	}

	private static IEnumerable<DeviceConfiguration> AddMicrophones(string hardwarePath, ConfigurationApiServiceManager serviceManager)
	{
		bool enabled = false;
		ConfigurationItem deviceGroupFolder = CreateMicrophoneGroup(serviceManager);
		return from s in AddDevices(hardwarePath, serviceManager, deviceGroupFolder, "MicrophoneFolder", enabled)
			select new MicrophoneConfiguration
			{
				Id = s.Item1,
				Name = s.Item2,
				Added = true,
				Enabled = enabled
			};
	}

	private static IEnumerable<DeviceConfiguration> AddSpeakers(string hardwarePath, ConfigurationApiServiceManager serviceManager)
	{
		bool enabled = false;
		ConfigurationItem deviceGroupFolder = CreateSpeakerGroup(serviceManager);
		return from s in AddDevices(hardwarePath, serviceManager, deviceGroupFolder, "SpeakerFolder", enabled)
			select new SpeakerConfiguration
			{
				Id = s.Item1,
				Name = s.Item2,
				Added = true,
				Enabled = enabled
			};
	}

	private static IEnumerable<DeviceConfiguration> AddMetaData(string hardwarePath, ConfigurationApiServiceManager serviceManager)
	{
		bool enabled = false;
		ConfigurationItem deviceGroupFolder = null;
		return from s in AddDevices(hardwarePath, serviceManager, deviceGroupFolder, "MetadataFolder", enabled)
			select new MetadataConfiguration
			{
				Id = s.Item1,
				Name = s.Item2,
				Added = true,
				Enabled = enabled
			};
	}

	private static IEnumerable<DeviceConfiguration> AddInput(string hardwarePath, ConfigurationApiServiceManager serviceManager)
	{
		bool enabled = false;
		ConfigurationItem deviceGroupFolder = null;
		return from s in AddDevices(hardwarePath, serviceManager, deviceGroupFolder, "InputEventFolder", enabled)
			select new InputConfiguration
			{
				Id = s.Item1,
				Name = s.Item2,
				Added = true,
				Enabled = enabled
			};
	}

	private static IEnumerable<DeviceConfiguration> AddOutput(string hardwarePath, ConfigurationApiServiceManager serviceManager)
	{
		bool enabled = false;
		ConfigurationItem deviceGroupFolder = null;
		return from s in AddDevices(hardwarePath, serviceManager, deviceGroupFolder, "OutputFolder", enabled)
			select new OutputConfiguration
			{
				Id = s.Item1,
				Name = s.Item2,
				Added = true,
				Enabled = enabled
			};
	}

	private static List<Tuple<string, string>> AddDevices(string hardwarePath, ConfigurationApiServiceManager serviceManager, ConfigurationItem deviceGroupFolder, string deviceFolderName, bool enabled)
	{
		List<Tuple<string, string>> list = new List<Tuple<string, string>>();
		string deviceFolderPath = string.Format(CultureInfo.InvariantCulture, "{0}/{1}", hardwarePath, deviceFolderName);
		foreach (ConfigurationItem device in from f in serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(deviceFolderPath), null)
			orderby f.DisplayName
			select f)
		{
			Property property = device.Properties.FirstOrDefault((Property p) => p.Key.Equals("Id", StringComparison.OrdinalIgnoreCase));
			Property property2 = device.Properties.FirstOrDefault((Property p) => p.Key.Equals("Name", StringComparison.OrdinalIgnoreCase));
			if (property != null && property2 != null)
			{
				list.Add(new Tuple<string, string>(property.Value, property2.Value));
			}
			device.EnableProperty.Enabled = enabled;
			serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.SetItem(device), null);
			if (deviceGroupFolder != null)
			{
				ConfigurationItem invokeInfoAddDeviceGroupMember = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(deviceGroupFolder, "AddDeviceGroupMember"), null);
				invokeInfoAddDeviceGroupMember.Properties.First((Property p) => p.Key.Equals("ItemSelection")).Value = device.Path;
				serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(invokeInfoAddDeviceGroupMember, "AddDeviceGroupMember"), null);
			}
		}
		return list;
	}

	private static ConfigurationItem CreateCameraGroup(ConfigurationApiServiceManager serviceManager)
	{
		return CreateDeviceGroup(serviceManager, "CameraGroupFolder", "CameraFolder", "Default camera group", "Default group created by the installer");
	}

	private static ConfigurationItem CreateMicrophoneGroup(ConfigurationApiServiceManager serviceManager)
	{
		return CreateDeviceGroup(serviceManager, "MicrophoneGroupFolder", "MicrophoneFolder", "Default microphone group", "Default speaker group created by the installer");
	}

	private static ConfigurationItem CreateSpeakerGroup(ConfigurationApiServiceManager serviceManager)
	{
		return CreateDeviceGroup(serviceManager, "SpeakerGroupFolder", "SpeakerFolder", "Default speaker group", "Default speaker group created by the installer");
	}

	private static ConfigurationItem CreateDeviceGroup(ConfigurationApiServiceManager serviceManager, string groupFolderName, string deviceFolder, string defaultDeviceGroupName, string defaultDeviceGroupDescription)
	{
		ConfigurationItem deviceGroupFolder = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(string.Format(CultureInfo.InvariantCulture, "/{0}", groupFolderName)), null);
		ConfigurationItem[] source = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(string.Format(CultureInfo.InvariantCulture, "/{0}", groupFolderName)), null);
		ConfigurationItem deviceGroup = source.FirstOrDefault((ConfigurationItem c) => c.DisplayName.Equals(defaultDeviceGroupName, StringComparison.Ordinal));
		if (deviceGroup == null)
		{
			ConfigurationItem invokeInfoAddDeviceGroup = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(deviceGroupFolder, "AddDeviceGroup"), null);
			invokeInfoAddDeviceGroup.Properties.First((Property p) => p.Key.Equals("GroupName", StringComparison.OrdinalIgnoreCase)).Value = defaultDeviceGroupName;
			invokeInfoAddDeviceGroup.Properties.First((Property p) => p.Key.Equals("GroupDescription", StringComparison.OrdinalIgnoreCase)).Value = defaultDeviceGroupDescription;
			ConfigurationItem configurationItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(invokeInfoAddDeviceGroup, "AddDeviceGroup"), null);
			Property path = configurationItem.Properties.FirstOrDefault((Property p) => string.Equals(p.Key, "Path", StringComparison.Ordinal));
			return serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", path.Value, deviceFolder)), null);
		}
		return serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(string.Format(CultureInfo.InvariantCulture, "{0}/{1}", deviceGroup.Path, deviceFolder)), null);
	}

	private static string AddHardwareToConfigApi(HardwareConfiguration cameraConfiguration, ConfigurationItem recorderItem, ConfigurationApiServiceManager serviceManager)
	{
		if (serviceManager == null)
		{
			throw new ArgumentNullException("serviceManager");
		}
		string taskPath = InvokeAddHardwareTask(cameraConfiguration, recorderItem, serviceManager);
		ConfigurationItem configurationItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(taskPath), null);
		Property property = configurationItem.Properties.FirstOrDefault((Property f) => f.Key.Equals("State", StringComparison.OrdinalIgnoreCase));
		while (property.Value.Equals("Idle", StringComparison.Ordinal) || property.Value.Equals("InProgress", StringComparison.Ordinal))
		{
			Thread.Sleep(500);
			configurationItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetItem(taskPath), null);
			property = configurationItem.Properties.FirstOrDefault((Property f) => f.Key.Equals("State", StringComparison.OrdinalIgnoreCase));
		}
		if (property.Value.Equals("Success"))
		{
			Property property2 = configurationItem.Properties.FirstOrDefault((Property p) => p.Key.Equals("Path"));
			if (property2 != null)
			{
				return property2.Value;
			}
		}
		return null;
	}

	private static string InvokeAddHardwareTask(HardwareConfiguration cameraConfiguration, ConfigurationItem recorderItem, ConfigurationApiServiceManager serviceManager)
	{
		ConfigurationItem addHardwareItem = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(recorderItem, "AddHardware"), null);
		addHardwareItem.Properties.FirstOrDefault((Property p) => p.Key.Equals("HardwareAddress", StringComparison.OrdinalIgnoreCase)).Value = cameraConfiguration.IPAddress;
		addHardwareItem.Properties.FirstOrDefault((Property p) => p.Key.Equals("HardwareDriverPath", StringComparison.OrdinalIgnoreCase)).Value = cameraConfiguration.DriverPath;
		addHardwareItem.Properties.FirstOrDefault((Property p) => p.Key.Equals("UserName", StringComparison.OrdinalIgnoreCase)).Value = cameraConfiguration.UserName;
		addHardwareItem.Properties.FirstOrDefault((Property p) => p.Key.Equals("Password", StringComparison.OrdinalIgnoreCase)).Value = cameraConfiguration.Password;
		return serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.InvokeMethod(addHardwareItem, "AddHardware"), null).Properties.FirstOrDefault((Property p) => p.Key.Equals("Path")).Value;
	}
}
