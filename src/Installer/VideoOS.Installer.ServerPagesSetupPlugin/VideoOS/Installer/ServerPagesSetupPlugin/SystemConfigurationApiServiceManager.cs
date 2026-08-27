using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Common.Interfaces.ServiceManager;
using VideoOS.ConfigurationAPI;
using VideoOS.Installer.Common.Utilities;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public sealed class SystemConfigurationApiServiceManager : ISystemConfiguration
{
	private class Pool<T> : IDisposable where T : IDisposable
	{
		private Queue<T> _pool = new Queue<T>();

		private Func<T> _createFunc;

		public Pool(Func<T> createFunc)
		{
			_createFunc = createFunc;
		}

		public T Acquire()
		{
			lock (_pool)
			{
				if (_pool.Count == 0)
				{
					return _createFunc();
				}
				return _pool.Dequeue();
			}
		}

		public void Return(T item)
		{
			lock (_pool)
			{
				_pool.Enqueue(item);
			}
		}

		public void Dispose()
		{
			while (_pool.Any())
			{
				_pool.Dequeue().Dispose();
			}
		}
	}

	private const string RecordingServerFolderPath = "/RecordingServerFolder";

	private const string HardwareFolderPath = "/HardwareFolder";

	private const string CameraFolderPath = "/CameraFolder";

	private const string HardwareDriverFolder = "/HardwareDriverFolder";

	private const string HardwareDriverSettingsFolder = "HardwareDriverSettingsFolder";

	private const string HardwareDriverSettings = "HardwareDriverSettings";

	public int WebsitePort { get; set; } = 80;

	public IEnumerable<IHardwareInfo> FetchHardware()
	{
		Pool<ConfigurationApiServiceManager> serviceManagerPool = new Pool<ConfigurationApiServiceManager>(() => new ConfigurationApiServiceManager(WebsitePort));
		try
		{
			List<IHardwareInfo> hardwareInfos = new List<IHardwareInfo>();
			Parallel.ForEach(GetRecordingServers(serviceManagerPool), delegate(ConfigurationItem recordingServer)
			{
				ConfigurationApiServiceManager configurationApiServiceManager = serviceManagerPool.Acquire();
				try
				{
					List<IHardwareInfo> hardwareForRecordingServer = GetHardwareForRecordingServer(configurationApiServiceManager, recordingServer);
					lock (hardwareInfos)
					{
						hardwareInfos.AddRange(hardwareForRecordingServer);
					}
				}
				finally
				{
					serviceManagerPool.Return(configurationApiServiceManager);
				}
			});
			return hardwareInfos;
		}
		finally
		{
			if (serviceManagerPool != null)
			{
				((IDisposable)serviceManagerPool).Dispose();
			}
		}
	}

	private static ConfigurationItem[] GetRecordingServers(Pool<ConfigurationApiServiceManager> serviceManagerPool)
	{
		ConfigurationApiServiceManager configurationApiServiceManager = serviceManagerPool.Acquire();
		try
		{
			return configurationApiServiceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems("/RecordingServerFolder"), null);
		}
		finally
		{
			serviceManagerPool.Return(configurationApiServiceManager);
		}
	}

	private static List<IHardwareInfo> GetHardwareForRecordingServer(ConfigurationApiServiceManager serviceManager, ConfigurationItem recordingServer)
	{
		List<IHardwareInfo> list = new List<IHardwareInfo>();
		string hardwarePath = recordingServer.Path + "/HardwareFolder";
		string hardwareDriversFolderPath = recordingServer.Path + "/HardwareDriverFolder";
		ConfigurationItem[] source = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(hardwareDriversFolderPath), null);
		ConfigurationItem[] array = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(hardwarePath), null);
		foreach (ConfigurationItem configurationItem in array)
		{
			Guid configurationItemId = GetConfigurationItemId(configurationItem);
			if (!configurationItemId.Equals(Guid.Empty))
			{
				string driverPath = configurationItem.Properties.FirstOrDefault((Property prop) => prop.Key.Equals("HardwareDriverPath"))?.Value;
				if (!int.TryParse(source.FirstOrDefault((ConfigurationItem d) => d.Path.Equals(driverPath))?.Properties.FirstOrDefault((Property prop) => prop.Key.Equals("Number"))?.Value, out var result) || !DriverNumbering.IsIcpDriver(result))
				{
					IHardwareInfo hardware = GetHardware(serviceManager, configurationItem, configurationItemId, GetConfigurationItemId(recordingServer));
					list.Add(hardware);
				}
			}
		}
		return list;
	}

	private static IHardwareInfo GetHardware(ConfigurationApiServiceManager serviceManager, ConfigurationItem hardware, Guid hardwareId, Guid recordingServerId)
	{
		string name = hardware.Properties.FirstOrDefault((Property prop) => prop.Key.Equals("Name"))?.Value;
		string detectedModel = hardware.Properties.FirstOrDefault((Property prop) => prop.Key.Equals("Model"))?.Value;
		bool enabled = hardware.EnableProperty.Enabled;
		Property[] propertiesForHardware = GetPropertiesForHardware(serviceManager, hardwareId.ToString());
		Property property = propertiesForHardware?.FirstOrDefault((Property prop) => prop.Key.Contains("ProductID"));
		string driverName = property.Value;
		if (property != null && property.ValueTypeInfos != null && property.ValueTypeInfos.Length != 0)
		{
			ValueTypeInfo valueTypeInfo = property.ValueTypeInfos.Where((ValueTypeInfo vt) => vt.Value.Equals(driverName)).FirstOrDefault();
			if (valueTypeInfo != null)
			{
				driverName = valueTypeInfo.Name;
			}
		}
		string macAddress = propertiesForHardware?.FirstOrDefault((Property prop) => prop.Key.Contains("MacAddress"))?.Value;
		string serialNumber = propertiesForHardware?.FirstOrDefault((Property prop) => prop.Key.Contains("SerialNumber"))?.Value;
		IHardwareInfo hardwareInfo = LicenseFactory.CreateNewHardwareInfo(hardwareId, recordingServerId, name, enabled, macAddress, serialNumber, driverName, string.Empty, 321, detectedModel, string.Empty, isInterconnected: false, new List<IDeviceInfo>());
		GetDevicesForHardware(serviceManager, hardwareInfo);
		return hardwareInfo;
	}

	private static Property[] GetPropertiesForHardware(ConfigurationApiServiceManager serviceManager, string hardwareId)
	{
		string hardwareSettingsPath = "Hardware[" + hardwareId + "]/HardwareDriverSettingsFolder";
		return serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(hardwareSettingsPath), null).FirstOrDefault((ConfigurationItem c) => c.ItemType.Equals("HardwareDriverSettings"))?.Children.FirstOrDefault((ConfigurationItem c) => c.ItemType.Equals("HardwareDriverSettings"))?.Properties;
	}

	private static void GetDevicesForHardware(ConfigurationApiServiceManager serviceManager, IHardwareInfo hardwareInfo)
	{
		string cameraFolderPath = "Hardware[" + hardwareInfo.Id.ToString() + "]/CameraFolder";
		ConfigurationItem[] array = serviceManager.CallWithRetry((ConfigurationApiServiceClient client) => client.GetChildItems(cameraFolderPath), null);
		if (array != null)
		{
			int num = 0;
			ConfigurationItem[] array2 = array;
			foreach (ConfigurationItem obj in array2)
			{
				Guid configurationItemId = GetConfigurationItemId(obj);
				string itemType = obj.ItemType;
				bool enabled = obj.EnableProperty.Enabled;
				IDeviceInfo item = LicenseFactory.CreateNewDeviceInfo(configurationItemId, configurationItemId, itemType, num, enabled, hardwareInfo);
				hardwareInfo.Devices.Add(item);
				num++;
			}
		}
	}

	private static Guid GetConfigurationItemId(ConfigurationItem configurationItem)
	{
		string text = configurationItem.Properties.FirstOrDefault((Property prop) => prop.Key.Equals("Id"))?.Value ?? string.Empty;
		if (!string.IsNullOrEmpty(text))
		{
			return new Guid(text);
		}
		return Guid.Empty;
	}
}
