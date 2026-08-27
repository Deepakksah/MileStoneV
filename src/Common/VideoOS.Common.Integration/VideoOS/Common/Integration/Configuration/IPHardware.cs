using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.Common.Integration.Definition;
using VideoOS.Utilities;

namespace VideoOS.Common.Integration.Configuration;

public class IPHardware : IConfigurationItem, IConfigurationItemRow
{
	private ConfigurationManager _configurationManager;

	private ConfigurationDataSet.HardwareRow _datarow;

	private HardwareDeviceMembers _hardwareDeviceMember;

	private DriverSettings _settings;

	private DriverSettings _eventSettings;

	private Settings _edgeSettings;

	private MainDefinition _mainDefinition;

	private bool _reloadingSettings;

	private object _datarowLock;

	private HardwareSettingUtilities _settingUtilities;

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
			lock (_datarowLock)
			{
				return new Guid(_datarow.IDHardware);
			}
		}
	}

	public string Name
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.Name;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.Name = Character.RemoveInvalidXmlChars(value);
			}
		}
	}

	public string Description
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.Description;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.Description = Character.RemoveInvalidXmlChars(value);
			}
		}
	}

	public string SettingsAsXml
	{
		get
		{
			lock (_datarowLock)
			{
				return _datarow.IsSettingsNull() ? DriverSettings.EmptyXml : _datarow.Settings;
			}
		}
	}

	public string EventSettingsAsXml
	{
		get
		{
			lock (_datarowLock)
			{
				return _datarow.IsEventSettingsNull() ? DriverSettings.EmptyXml : _datarow.EventSettings;
			}
		}
	}

	public MainDefinition MainDefinition
	{
		get
		{
			lock (this)
			{
				if (_mainDefinition != null)
				{
					return _mainDefinition;
				}
				_mainDefinition = GetMainDefinitionWithUpdatedSettings();
			}
			return _mainDefinition;
		}
	}

	public HardwareDefinition Definition => MainDefinition.HardwareDefinition;

	public bool IsDefinitionLoaded
	{
		get
		{
			lock (this)
			{
				return _mainDefinition != null;
			}
		}
	}

	public HardwareSettingUtilities SettingUtilities
	{
		get
		{
			if (_settingUtilities == null)
			{
				_settingUtilities = new HardwareSettingUtilities(Settings);
			}
			return _settingUtilities;
		}
	}

	public DriverSettings Settings
	{
		get
		{
			if (_settings == null)
			{
				_settings = new DriverSettings();
				lock (_datarowLock)
				{
					if (!_datarow.IsSettingsNull())
					{
						_settings.LoadXml(_datarow.Settings, SettingsLoadMethod.Reload);
					}
					_settings.SettingsChanged += SettingsChanged;
				}
			}
			return _settings;
		}
	}

	public DriverSettings EventSettings
	{
		get
		{
			if (_eventSettings == null)
			{
				_eventSettings = new DriverSettings();
				lock (_datarowLock)
				{
					if (!_datarow.IsEventSettingsNull())
					{
						_eventSettings.LoadXml(_datarow.EventSettings, SettingsLoadMethod.Reload);
					}
					_eventSettings.SettingsChanged += EventSettingsChanged;
				}
			}
			return _eventSettings;
		}
	}

	public Guid IDHardwareDriver
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return new Guid(_datarow.IDHardwareDriver);
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.IDHardwareDriver = value.ToString();
			}
		}
	}

	public Guid IDRecorder
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return new Guid(_datarow.IDRecorder);
			}
		}
	}

	public string ProductId
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.VendorProductID;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.VendorProductID = value;
			}
		}
	}

	public int DriverNumber
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.DriverNumber;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.DriverNumber = value;
			}
		}
	}

	public string DriverProxyName
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.DriverProxyName;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.DriverProxyName = value;
			}
		}
	}

	public Uri Uri
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return new Uri(_datarow.URI);
			}
		}
		[DebuggerStepThrough]
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			lock (_datarowLock)
			{
				_datarow.URI = value.ToString();
			}
		}
	}

	public string DefinitionAsXml
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _configurationManager.HardwareDefinitionItems.GetHardwareDefinitionByDigest(_datarow.C18NDigest).Definition;
			}
		}
	}

	public string LoginID
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.LoginID;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				if (_datarow.LoginID != value)
				{
					_datarow.LoginID = value;
					_datarow.CredentialsLastModified = DateTime.UtcNow;
				}
			}
		}
	}

	public string Password
	{
		get
		{
			lock (_datarowLock)
			{
				return _datarow.EncryptedPassword;
			}
		}
		set
		{
			lock (_datarowLock)
			{
				string encryptedPassword = _datarow.EncryptedPassword;
				_datarow.EncryptedPassword = value;
				if (value != encryptedPassword)
				{
					_datarow.CredentialsLastModified = DateTime.UtcNow;
				}
			}
		}
	}

	public bool Enabled
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.Enabled;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.Enabled = value;
			}
		}
	}

	public DateTime LastModified
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.LastModified;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.LastModified = value;
			}
		}
	}

	public DateTime? CredentialsLastModified
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.IsCredentialsLastModifiedNull() ? ((DateTime?)null) : new DateTime?(_datarow.CredentialsLastModified);
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				if (value.HasValue)
				{
					_datarow.CredentialsLastModified = value.Value;
				}
				else
				{
					_datarow.SetCredentialsLastModifiedNull();
				}
			}
		}
	}

	public DateTime? HardwareDataLastUpdated
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.IsHardwareDataLastUpdatedNull() ? ((DateTime?)null) : new DateTime?(_datarow.HardwareDataLastUpdated);
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				if (value.HasValue)
				{
					_datarow.HardwareDataLastUpdated = value.Value;
				}
				else
				{
					_datarow.SetHardwareDataLastUpdatedNull();
				}
			}
		}
	}

	public Recorder Recorder
	{
		get
		{
			Guid id;
			lock (_datarowLock)
			{
				id = new Guid(_datarow.IDRecorder);
			}
			return _configurationManager.Recorders[id];
		}
	}

	public HardwareDeviceMembers Devices
	{
		get
		{
			if (_hardwareDeviceMember == null)
			{
				_hardwareDeviceMember = new HardwareDeviceMembers(_configurationManager, this);
			}
			return _hardwareDeviceMember;
		}
	}

	public int HostedHardwareId
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return _datarow.HostedHardware_id;
			}
		}
	}

	public bool HasHostedHardware
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return !_datarow.IsHostedHardware_idNull();
			}
		}
	}

	public HostedHardware HostedHardware
	{
		get
		{
			lock (_datarowLock)
			{
				if (!HasHostedHardware)
				{
					return null;
				}
				return _configurationManager.HostedHardwareItems.GetHostedHardwareById(HostedHardwareId);
			}
		}
	}

	public string CameraData
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return ((ConfigurationDataSet.HardwareRow)DataRow).CameraData;
			}
		}
	}

	public Settings EdgeSettings
	{
		get
		{
			if (_edgeSettings == null)
			{
				_edgeSettings = new Settings();
				lock (_datarowLock)
				{
					if (!string.IsNullOrEmpty(_datarow.EdgeSettings))
					{
						_edgeSettings.LoadXml(_datarow.EdgeSettings, SettingsLoadMethod.Reload);
					}
					_edgeSettings.SettingsChanged += EdgeSettingsChanged;
				}
			}
			return _edgeSettings;
		}
	}

	public Subtypes Subtype
	{
		[DebuggerStepThrough]
		get
		{
			lock (_datarowLock)
			{
				return (Subtypes)_datarow.SubType;
			}
		}
		[DebuggerStepThrough]
		set
		{
			lock (_datarowLock)
			{
				_datarow.SubType = (int)value;
			}
		}
	}

	public bool IsInterconnectedProduct => Subtype.HasFlag(Subtypes.InterconnectedProduct);

	public bool IsMipDriver => DriverNumbering.IsMipDriverSubtype(Subtype);

	public bool IsIcpOrMipDriver
	{
		get
		{
			if (!IsInterconnectedProduct)
			{
				return IsMipDriver;
			}
			return true;
		}
	}

	public bool IsEdgeSupported => Devices.Any((Device x) => x.SettingUtilities.IsEdgeStorageSupported);

	public bool HasConfigurationUrl => DefinitionUtility.GetCapabilityDisplayValue(Definition, Settings, TypeReferenceIds.RemoteLogOnAddress) != null;

	public bool IsVpsProduct => DriverNumbering.IsVpsDriver(DriverNumber);

	public bool IsDigitalDeviceDriver => DriverNumbering.IsDigitalDeviceDriver(DriverNumber);

	public string DisplaySortKey => Name;

	public IPHardware(ConfigurationManager configurationManager, ConfigurationDataSet.HardwareRow row)
	{
		_configurationManager = configurationManager;
		_datarow = row;
		_datarowLock = new object();
	}

	public void ReloadSettings()
	{
		_reloadingSettings = true;
		if (_datarow.IsSettingsNull())
		{
			Settings.Clear();
		}
		else
		{
			Settings.LoadXml(_datarow.Settings, SettingsLoadMethod.Update);
		}
		EventSettings.Clear();
		if (!_datarow.IsEventSettingsNull())
		{
			EventSettings.LoadXml(_datarow.EventSettings, SettingsLoadMethod.Update);
		}
		EdgeSettings.Clear();
		if (!string.IsNullOrEmpty(_datarow.EdgeSettings))
		{
			EdgeSettings.LoadXml(_datarow.EdgeSettings, SettingsLoadMethod.Update);
		}
		_reloadingSettings = false;
	}

	public void Refresh()
	{
		using ConfigurationDataSet configurationDataSet = _configurationManager.dataLoader.GetHardwareByHardwareId(new Guid(_datarow.IDHardware));
		_configurationManager.DisableEventHandling = true;
		if (configurationDataSet.Hardware.Rows.Count == 1 && configurationDataSet.Hardware.Rows[0] is ConfigurationDataSet.HardwareRow hardwareRow)
		{
			_datarow.Table.BeginLoadData();
			_datarow.ItemArray = hardwareRow.ItemArray;
			_datarow.AcceptChanges();
			_datarow.Table.EndLoadData();
		}
		_configurationManager.DisableEventHandling = false;
	}

	private MainDefinition GetMainDefinitionWithUpdatedSettings()
	{
		List<string> list = new List<string> { SettingsAsXml };
		foreach (Device device in Devices)
		{
			list.Add(device.SettingsAsXml);
		}
		string definition;
		lock (_datarowLock)
		{
			definition = _configurationManager.HardwareDefinitionItems.GetHardwareDefinitionByDigest(_datarow.C18NDigest).Definition;
		}
		return MainDefinition.LoadXml(definition, list);
	}

	public void ResetMainDefinition()
	{
		lock (this)
		{
			_mainDefinition = null;
		}
	}

	private void SettingsChanged(object sender, SettingsEventArgs eventarg)
	{
		if (!_reloadingSettings)
		{
			lock (_datarowLock)
			{
				_datarow.Settings = _settings.SaveAsXml();
			}
		}
		ResetMainDefinition();
	}

	private void EventSettingsChanged(object sender, SettingsEventArgs eventarg)
	{
		if (!_reloadingSettings)
		{
			lock (_datarowLock)
			{
				_datarow.EventSettings = _eventSettings.SaveAsXml();
			}
		}
		ResetMainDefinition();
	}

	public bool SetHardwareDefinition(string definitionXml)
	{
		definitionXml = DefinitionUtility.RemoveHardwareSpecificInfoFromHardwareDefinitionXml(definitionXml);
		string text = DefinitionUtility.CreateUniqueHardwareDefinitionKey(definitionXml);
		lock (_datarowLock)
		{
			if (text == _datarow.C18NDigest)
			{
				return false;
			}
			_datarow.C18NDigest = text;
			if (!_configurationManager.HardwareDefinitionItems.TryGetHardwareDefinitionByDigest(text, out var definition))
			{
				_configurationManager.HardwareDefinitionItems.AddNew(text, definitionXml);
			}
			else if (definition.DataRow.RowState == DataRowState.Unchanged)
			{
				definition.DataRow.SetAdded();
			}
			return true;
		}
	}

	public bool IsCameraDataNull()
	{
		return ((ConfigurationDataSet.HardwareRow)DataRow).IsCameraDataNull();
	}

	public string ResolveCameraData(string userName, string password)
	{
		return ((ConfigurationDataSet.HardwareRow)DataRow).ResolveCameraData(userName, password);
	}

	private void EdgeSettingsChanged(object sender, SettingsEventArgs eventarg)
	{
		if (!_reloadingSettings)
		{
			lock (_datarowLock)
			{
				_datarow.EdgeSettings = _edgeSettings.SaveAsXml();
			}
		}
	}

	public void ConfigureDeviceLinks(IList<Device> deviceList)
	{
		if (deviceList == null)
		{
			throw new ArgumentNullException("deviceList", "deviceList can not be null");
		}
		foreach (Device item in deviceList.Where((Device d) => RegisteredDeviceType.IsCamera(d.DeviceType)))
		{
			if (IsInterconnectedProduct)
			{
				List<string> list = new List<string>
				{
					item.Settings.GetValueByTypeId(item.Hwid.Text, TypeReferenceIds.ICPRelatedMicrophone.ToString()),
					item.Settings.GetValueByTypeId(item.Hwid.Text, TypeReferenceIds.ICPRelatedSpeaker.ToString())
				};
				string valueByTypeId = item.Settings.GetValueByTypeId(item.Hwid.Text, TypeReferenceIds.ICPRelatedMetadataIds.ToString());
				if (!string.IsNullOrEmpty(valueByTypeId))
				{
					list.AddRange(valueByTypeId.Split(';'));
				}
				foreach (Device device in Devices)
				{
					if (device.DeviceType == "AudioInput" || device.DeviceType == "AudioOutput" || device.DeviceType == "Metadata")
					{
						string icpDeviceId = device.Settings.GetValueByTypeId(device.Hwid.Text, TypeReferenceIds.DeviceId.ToString());
						if (!string.IsNullOrEmpty(icpDeviceId) && list.Any((string x) => x.Equals(icpDeviceId, StringComparison.OrdinalIgnoreCase)))
						{
							_configurationManager.DeviceLinks.Add(item.ID, device.ID);
						}
					}
				}
				continue;
			}
			foreach (Device device2 in deviceList)
			{
				if ((device2.DeviceType == "AudioInput" || device2.DeviceType == "AudioOutput" || device2.DeviceType == "Metadata") && item.Definition.Hwid.Index == device2.Definition.Hwid.Index)
				{
					_configurationManager.DeviceLinks.Add(item.ID, device2.ID);
				}
			}
		}
	}

	public static string DefaultEdgeSettings(int defaultParallelDevices)
	{
		Settings settings = new Settings();
		settings.Set("MaxTransferBitrateEnabled", Convert.ToString(value: false, CultureInfo.InvariantCulture));
		settings.Set("MaxTransferBitrate", Convert.ToString(2048, CultureInfo.InvariantCulture));
		settings.Set("RetrievalTimeSpanEnabled", Convert.ToString(value: false, CultureInfo.InvariantCulture));
		settings.Set("RetrievalTimeSpanStart", DateTime.Parse("18:00:00", CultureInfo.InvariantCulture).ToString("HH:mm", CultureInfo.InvariantCulture));
		settings.Set("RetrievalTimeSpanEnd", DateTime.Parse("06:00:00", CultureInfo.InvariantCulture).ToString("HH:mm", CultureInfo.InvariantCulture));
		settings.Set("MaxParallelDevices", Convert.ToString(defaultParallelDevices, CultureInfo.InvariantCulture));
		return settings.SaveAsXml();
	}
}
