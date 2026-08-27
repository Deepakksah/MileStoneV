using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.HardwareSetup;
using VideoOS.Management.Common;
using VideoOS.Utilities;

namespace VideoOS.Common.Integration.Configuration;

public class Recorder : IConfigurationItem, IConfigurationItemRow
{
	public static class MulticastSettingName
	{
		public const string MulticastEnabled = "MulticastEnabled";

		public const string ClientIPRangeStart = "ClientIPRangeStart";

		public const string ClientIPRangeEnd = "ClientIPRangeEnd";

		public const string ClientPortRangeStart = "ClientPortRangeStart";

		public const string ClientPortRangeEnd = "ClientPortRangeEnd";

		public const string Mtu = "MTU";

		public const string Ttl = "TTL";
	}

	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.RecordersRow _datarow;

	private IPHardwareMembers _IPHardwareMembers;

	private Guid masterServiceId;

	private Guid activeServiceId;

	public DataRow DataRow
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow;
		}
	}

	public Guid ID
	{
		[DebuggerStepThrough]
		get
		{
			return new Guid(_datarow.IDRecorder);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.IDRecorder = value.ToString();
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Name;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Name = Character.RemoveInvalidXmlChars(value);
		}
	}

	public string Description
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Description;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Description = Character.RemoveInvalidXmlChars(value);
		}
	}

	public bool Enabled
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Enabled;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Enabled = value;
		}
	}

	public Guid IdPrimaryFailoverGroup
	{
		get
		{
			if (_datarow.IsIDFailoverPrimaryGroupNull())
			{
				return Guid.Empty;
			}
			return new Guid(_datarow.IDFailoverPrimaryGroup);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.IDFailoverPrimaryGroup = value.ToString();
		}
	}

	public Guid IdSecondaryFailoverGroup
	{
		get
		{
			if (_datarow.IsIDFailoverSecondaryGroupNull())
			{
				return Guid.Empty;
			}
			return new Guid(_datarow.IDFailoverSecondaryGroup);
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.IDFailoverSecondaryGroup = value.ToString();
		}
	}

	public DateTime LastModified
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.LastModified;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.LastModified = value;
		}
	}

	public string MulticastSettings
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.MulticastSettings;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.MulticastSettings = value;
		}
	}

	public int SynchronizationTime
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.SynchronizationTime;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.SynchronizationTime = value;
		}
	}

	public IPHardwareMembers Hardware
	{
		get
		{
			if (_IPHardwareMembers == null)
			{
				_IPHardwareMembers = new IPHardwareMembers(_configurationManager, this);
			}
			return _IPHardwareMembers;
		}
	}

	public bool DevicesLoaded { get; private set; }

	public Guid MasterServiceId
	{
		get
		{
			if (masterServiceId == Guid.Empty)
			{
				RecordingServerServiceRecorder recordingServerServiceRecorder = _configurationManager.RecordingServerServiceRecorders.Values.Cast<RecordingServerServiceRecorder>().FirstOrDefault((RecordingServerServiceRecorder r) => r.RecorderId == ID && r.ServiceType == RecordingServerServiceType.RecordingServer);
				if (recordingServerServiceRecorder == null)
				{
					throw ConfigurationItemNotFoundException.CreateWithId(typeof(RecordingServerServiceRecorder), ID);
				}
				masterServiceId = recordingServerServiceRecorder.ServiceId;
			}
			return masterServiceId;
		}
	}

	public RecordingServerService MasterService => _configurationManager.RecordingServerServices[MasterServiceId];

	public Guid ActiveServiceId
	{
		get
		{
			if (activeServiceId == Guid.Empty)
			{
				RecordingServerServiceRecorder recordingServerServiceRecorder = _configurationManager.RecordingServerServiceRecorders.Values.Cast<RecordingServerServiceRecorder>().FirstOrDefault((RecordingServerServiceRecorder r) => r.RecorderId == ID && r.Active);
				if (recordingServerServiceRecorder != null)
				{
					activeServiceId = recordingServerServiceRecorder.ServiceId;
				}
				else
				{
					activeServiceId = MasterServiceId;
				}
			}
			return activeServiceId;
		}
		set
		{
			activeServiceId = value;
		}
	}

	public RecordingServerService ActiveService => _configurationManager.RecordingServerServices[ActiveServiceId];

	public RecordingServerServiceConfigurable ServiceConfiguration => _configurationManager.RecordingServerServiceConfigurable[MasterServiceId];

	public string DisplaySortKey => Name;

	public Recorder(ConfigurationManager configurationManager, ConfigurationDataSet.RecordersRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
		DevicesLoaded = false;
	}

	public void Refresh()
	{
		using ConfigurationDataSet configurationDataSet = _configurationManager.dataLoader.GetConfigurationDataSetTables(new string[1] { _datarow.Table.TableName });
		_datarow.Table.BeginLoadData();
		_configurationManager.DisableEventHandling = true;
		foreach (ConfigurationDataSet.RecordersRow row in configurationDataSet.Recorders.Rows)
		{
			if (string.Equals(_datarow.IDRecorder, row.IDRecorder, StringComparison.OrdinalIgnoreCase))
			{
				_datarow.ItemArray = row.ItemArray;
				_datarow.AcceptChanges();
				break;
			}
		}
		_configurationManager.DisableEventHandling = false;
		_datarow.Table.EndLoadData();
	}

	public void EnsureDevicesLoaded()
	{
		if (!DevicesLoaded)
		{
			_configurationManager.DeviceItems.LoadDevicesByRecorderId(ID);
			DevicesLoaded = true;
		}
	}

	public IPHardware AddNewHardware(HardwareSetupSettings hardwareSetupSettings, Guid defaultRecordingStorageId, HardwareAccelerationMode.HardwareAccelerationModeType hardwareAccelerationMode)
	{
		if (hardwareSetupSettings == null)
		{
			throw new ArgumentNullException("hardwareSetupSettings");
		}
		string definitionXml = hardwareSetupSettings.DefinitionXml;
		string text = DefinitionUtility.InsertTypeAttributeInHardwareSettingXml(hardwareSetupSettings.SettingsXml, definitionXml, refresh: false);
		Guid id = Guid.NewGuid();
		string logOnId;
		string password;
		Uri uri;
		int? hostedHardwareId;
		string customDriverData;
		if (hardwareSetupSettings.HostedHardware != null)
		{
			logOnId = string.Empty;
			password = string.Empty;
			uri = new Uri("http://invalid");
			hostedHardwareId = hardwareSetupSettings.HostedHardware.Id;
			customDriverData = hardwareSetupSettings.CustomDriverData;
		}
		else
		{
			logOnId = hardwareSetupSettings.UserName;
			password = hardwareSetupSettings.Password;
			uri = hardwareSetupSettings.Address;
			hostedHardwareId = null;
			customDriverData = hardwareSetupSettings.CustomDriverData;
		}
		string edgeSettings = DriverSettings.EmptyXml;
		Subtypes subtype = Subtypes.Standard;
		if (hardwareSetupSettings.IsIcpOrMipDriver)
		{
			subtype = (hardwareSetupSettings.IsArculesMipDriver ? Subtypes.MipArculesDriver : ((!hardwareSetupSettings.IsMipDriver) ? Subtypes.InterconnectedProduct : Subtypes.MipDriver));
			DriverSettings settings = new DriverSettings(text);
			edgeSettings = IPHardware.DefaultEdgeSettings(Convert.ToInt32(DefinitionUtility.GetCapabilityDisplayValue(new HardwareDefinition(hardwareSetupSettings.Definition), settings, TypeReferenceIds.EdgeDefaultParallelDevices), CultureInfo.InvariantCulture));
		}
		IPHardware iPHardware = Hardware.AddNew(id, hardwareSetupSettings.Name, string.Empty, hardwareSetupSettings.Enabled, uri, logOnId, password, hardwareSetupSettings.DriverId, definitionXml, text, DriverSettings.EmptyXml, hardwareSetupSettings.ProductId, hardwareSetupSettings.DriverNumber, hostedHardwareId, customDriverData, edgeSettings, subtype);
		MainDefinition mainDefinition = MainDefinition.LoadXml(definitionXml);
		List<Device> deviceList = new List<Device>();
		foreach (DeviceSetupSettings device in hardwareSetupSettings.Devices)
		{
			AddDevice(device, iPHardware, mainDefinition, defaultRecordingStorageId, deviceList, hardwareAccelerationMode);
		}
		iPHardware.ConfigureDeviceLinks(deviceList);
		return iPHardware;
	}

	private void AddDevice(DeviceSetupSettings deviceSetupInfo, IPHardware hardware, MainDefinition mainDefinition, Guid defaultRecordingStorageId, List<Device> deviceList, HardwareAccelerationMode.HardwareAccelerationModeType hardwareAccelerationMode)
	{
		Guid recordingStorageId = Guid.Empty;
		if (_configurationManager.RegisteredDeviceTypes[deviceSetupInfo.DeviceType].IsRecordingDevice)
		{
			recordingStorageId = defaultRecordingStorageId;
		}
		string settings = DefinitionUtility.InsertTypeAttributeInHardwareSettingXml(deviceSetupInfo.SettingsXml, mainDefinition, refresh: false);
		DeviceDefinition deviceDefinition = mainDefinition.HardwareDefinition.DeviceDefinitions[deviceSetupInfo.DeviceHwid];
		string text = deviceSetupInfo.Streams;
		StreamsConfiguration streamsConfiguration = new StreamsConfiguration();
		streamsConfiguration.LoadXml(text);
		if (streamsConfiguration.Count > 0)
		{
			foreach (StreamInfo item in streamsConfiguration)
			{
				string displayNameResolved = deviceDefinition.Streams[item.Hwid].DisplayNameResolved;
				if (!string.IsNullOrEmpty(displayNameResolved))
				{
					item.Name = displayNameResolved;
				}
			}
			text = streamsConfiguration.SaveXml();
		}
		Device device = hardware.Devices.AddNew(Guid.NewGuid(), deviceSetupInfo.Name, string.Empty, string.Empty, deviceSetupInfo.DeviceType, settings, deviceSetupInfo.PresetsXml, string.Empty, multicastEnabled: false, text, deviceSetupInfo.DeviceHwid.Index, recordingStorageId, deviceSetupInfo.Enabled, failoverLiveOnly: false, disableOnFailover: false, -1, -1, -1, hardwareAccelerationMode);
		deviceList.Add(device);
		if (deviceSetupInfo.DeviceGroup != Guid.Empty)
		{
			_configurationManager.FindDeviceGroupByDeviceGroupId(deviceSetupInfo.DeviceGroup).Devices.Add(device);
		}
	}

	public static string GetDefaultMulticastSettings(IPAddress clientMulticastAddress, bool multicastEnabled)
	{
		if (clientMulticastAddress == null)
		{
			throw new ArgumentNullException("clientMulticastAddress");
		}
		Settings settings = new Settings();
		settings.Set("MulticastEnabled", Convert.ToString(multicastEnabled, CultureInfo.InvariantCulture));
		settings.Set("ClientIPRangeStart", clientMulticastAddress.ToString());
		settings.Set("ClientIPRangeEnd", clientMulticastAddress.ToString());
		settings.Set("ClientPortRangeStart", "6000");
		settings.Set("ClientPortRangeEnd", "7000");
		settings.Set("MTU", "1500");
		settings.Set("TTL", "32");
		return settings.SaveAsXml();
	}
}
