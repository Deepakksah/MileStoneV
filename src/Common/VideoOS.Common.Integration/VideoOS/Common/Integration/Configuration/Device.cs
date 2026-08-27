using System;
using System.Data;
using System.Diagnostics;
using VideoOS.Common.Integration.Definition;
using VideoOS.Utilities;

namespace VideoOS.Common.Integration.Configuration;

public class Device : IConfigurationItem, IConfigurationItemRow, IDeviceHwid
{
	private readonly ConfigurationManager _configurationManager;

	private readonly ConfigurationDataSet.DevicesRow _datarow;

	private DeviceRecorderPluginMembers _deviceRecorderPlugins;

	private DriverSettings _settings;

	private PtzPresetCollection _presets;

	private bool _reloadingSettings;

	private Guid _deviceId;

	private Guid _hardwareId;

	private readonly string _deviceType;

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
			return _deviceId;
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

	public string GisPoint
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.GisPointString;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.GisPointString = value;
		}
	}

	public string ShortName
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.ShortName;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.ShortName = value;
		}
	}

	public int Icon
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Icon;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Icon = value;
		}
	}

	public double CoverageDirection
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.CoverageDirection;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.CoverageDirection = value;
		}
	}

	public double CoverageDepth
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.CoverageDepth;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.CoverageDepth = value;
		}
	}

	public double CoverageFieldOfView
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.CoverageFieldOfView;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.CoverageFieldOfView = value;
		}
	}

	public int Index
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.DeviceIndex;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.DeviceIndex = value;
		}
	}

	public DeviceHwid Hwid => new DeviceHwid(DeviceType, Index);

	public Guid IPHardwareID
	{
		[DebuggerStepThrough]
		get
		{
			return _hardwareId;
		}
	}

	public string Location
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Location;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Location = value;
		}
	}

	public PtzPresetCollection Presets
	{
		get
		{
			if (_presets == null)
			{
				_presets = new PtzPresetCollection();
				if (!_datarow.IsPresetsNull())
				{
					_presets.LoadXml(_datarow.Presets);
				}
				_presets.Changed += Presets_Changed;
			}
			else if (!_datarow.IsPresetsNull())
			{
				_presets.Changed -= Presets_Changed;
				_presets.LoadXml(_datarow.Presets);
				_presets.Changed += Presets_Changed;
			}
			return _presets;
		}
	}

	public string PresetsAsXml
	{
		get
		{
			return Presets.SaveAsXml();
		}
		set
		{
			_datarow.Presets = value;
			if (_presets != null)
			{
				_presets.Changed -= Presets_Changed;
				_presets.LoadXml(value);
				_presets.Changed += Presets_Changed;
			}
		}
	}

	public DriverSettings Settings
	{
		get
		{
			if (_settings == null)
			{
				_settings = new DriverSettings();
				if (!_datarow.IsSettingsNull())
				{
					_settings.LoadXml(_datarow.Settings, SettingsLoadMethod.Reload);
				}
				_settings.SettingsChanged += SettingsChanged;
			}
			return _settings;
		}
	}

	public string SettingsAsXml
	{
		get
		{
			if (_datarow.IsSettingsNull())
			{
				return DriverSettings.EmptyXml;
			}
			return _datarow.Settings;
		}
	}

	public string DeviceType
	{
		[DebuggerStepThrough]
		get
		{
			return _deviceType;
		}
	}

	public DeviceTrackCollection Tracks { get; }

	public IPHardware IPHardware => _configurationManager.HardwareItems.GetHardwareByHardwareId(_hardwareId);

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

	public string Shortcut
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Shortcut;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Shortcut = value;
		}
	}

	public int PtzManualSessionTimeout
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.PtzManualSessionTimeout;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.PtzManualSessionTimeout = value;
		}
	}

	public int PtzReservedSessionTimeout
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.PtzReservedSessionTimeout;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.PtzReservedSessionTimeout = value;
		}
	}

	public int PtzExtendedManualSessionTimeout
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.PtzExtendedManualSessionTimeout;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.PtzExtendedManualSessionTimeout = value;
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

	public bool MulticastEnabled
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.MulticastEnabled;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.MulticastEnabled = value;
		}
	}

	public string Streams
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.Streams;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.Streams = value;
		}
	}

	public Subtypes Subtype
	{
		[DebuggerStepThrough]
		get
		{
			return (Subtypes)_datarow.SubType;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.SubType = (int)value;
		}
	}

	public bool FailoverLiveOnly
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.FailoverLiveOnly;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.FailoverLiveOnly = value;
		}
	}

	public bool DisableOnFailover
	{
		[DebuggerStepThrough]
		get
		{
			return _datarow.DisableOnFailover;
		}
		[DebuggerStepThrough]
		set
		{
			_datarow.DisableOnFailover = value;
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

	public DeviceDefinition Definition => IPHardware.Definition.GetDeviceDefinition(Hwid);

	public DeviceRecorderPluginMembers DeviceRecorderPlugins
	{
		get
		{
			if (_deviceRecorderPlugins == null)
			{
				_deviceRecorderPlugins = new DeviceRecorderPluginMembers(_configurationManager, this);
			}
			return _deviceRecorderPlugins;
		}
	}

	public bool IsRecordingDevice => RegisteredDeviceType.IsRecordingDeviceType(DeviceType);

	public DeviceSettingUtilities SettingUtilities => new DeviceSettingUtilities(Settings, (DeviceTypeEnum)Enum.Parse(typeof(DeviceTypeEnum), DeviceType), Index);

	public string DisplaySortKey
	{
		[DebuggerStepThrough]
		get
		{
			return Name;
		}
	}

	public Device(ConfigurationManager configurationManager, ConfigurationDataSet.DevicesRow row)
	{
		if (row == null)
		{
			throw new ArgumentNullException("row");
		}
		_configurationManager = configurationManager;
		_datarow = row;
		_deviceId = new Guid(_datarow.IDDevice);
		_hardwareId = new Guid(_datarow.IDHardware);
		_deviceType = _datarow.DeviceType.Trim();
		Tracks = new DeviceTrackCollection(_configurationManager, this);
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
		_reloadingSettings = false;
	}

	public void Presets_Changed(object sender, EventArgs e)
	{
		_datarow.Presets = _presets.SaveAsXml();
	}

	private void SettingsChanged(object sender, SettingsEventArgs eventarg)
	{
		if (!_reloadingSettings)
		{
			_datarow.Settings = _settings.SaveAsXml();
		}
		IPHardware.ResetMainDefinition();
	}

	public void ClearSettings()
	{
		_settings = null;
	}

	public static string GetDefaultStreamXml4NewDevice(DeviceDefinition deviceDefinition, string settingsXml)
	{
		if (deviceDefinition == null)
		{
			throw new ArgumentNullException("deviceDefinition");
		}
		string result = string.Empty;
		if (deviceDefinition.StreamCombinations.Supported)
		{
			result = GetDefaultStreamXml(deviceDefinition, settingsXml);
		}
		return result;
	}

	public static string GetDefaultStreamXml(DeviceDefinition deviceDefinition, string settingsXml)
	{
		string result = string.Empty;
		DriverSettings settings = new DriverSettings(settingsXml);
		StreamDefinition defaultStreamDefinition = DefinitionUtility.GetDefaultStreamDefinition(deviceDefinition, settings);
		if (defaultStreamDefinition != null)
		{
			result = new StreamsConfiguration
			{
				new StreamInfo(defaultStreamDefinition.ReferenceId, defaultStreamDefinition.Hwid, defaultStreamDefinition.DisplayNameResolved, liveFeed: true, liveDefault: true, deviceDefinition.Base.GetDeviceType() == 4, WellKnownDeviceTrackDefinition.Primary)
			}.SaveXml();
		}
		return result;
	}
}
