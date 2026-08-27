using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration;

public class DeviceSettingUtilities
{
	private int _index = -1;

	private DeviceTypeEnum _deviceType;

	private DriverSettings _driverSettings;

	private string _deviceHwidText;

	private string _devicePtzHwidText;

	private DevicePresetConfiguration _presetConfig;

	private bool _presetConfigCached;

	public bool IsEdgeStorageSupported
	{
		get
		{
			string[] sectionNames = _driverSettings.GetSectionNames();
			for (int i = 0; i < sectionNames.Length; i++)
			{
				Hwid hwid = Hwid.ParseHwid(sectionNames[i]) as StreamHwid;
				if (hwid != null)
				{
					string valueByTypeId = _driverSettings.GetValueByTypeId(hwid.Text, TypeReferenceIds.EdgeStorageSupported.ToString());
					if (!string.IsNullOrEmpty(valueByTypeId) && bool.TryParse(valueByTypeId, out var result) && result)
					{
						return true;
					}
				}
			}
			return false;
		}
	}

	public bool IsEdgeStorageEnabled
	{
		get
		{
			bool result = false;
			string valueByTypeId = _driverSettings.GetValueByTypeId(_deviceHwidText, TypeReferenceIds.EdgeStorageEnabled.ToString());
			if (!string.IsNullOrEmpty(valueByTypeId) && !bool.TryParse(valueByTypeId, out result))
			{
				result = false;
			}
			return result;
		}
		set
		{
			_driverSettings.UpdateByTypeId(_deviceHwidText, TypeReferenceIds.EdgeStorageEnabled.ToString(), value.ToString());
		}
	}

	public string EdgeStorageIndex
	{
		get
		{
			string result = "-1";
			string valueByTypeId = _driverSettings.GetValueByTypeId(_deviceHwidText, TypeReferenceIds.EdgeStorageIndex.ToString());
			if (!string.IsNullOrEmpty(valueByTypeId))
			{
				result = valueByTypeId;
			}
			return result;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_driverSettings.UpdateByTypeId(_deviceHwidText, TypeReferenceIds.EdgeStorageIndex.ToString(), value.ToString(CultureInfo.InvariantCulture));
		}
	}

	public bool IsPtzEnabled => GetBooleanValue(_devicePtzHwidText, TypeReferenceIds.PtzEnabled);

	public bool IsPtzCenterOnPositionInViewSupported => GetBooleanValue(_devicePtzHwidText, TypeReferenceIds.PtzCenterOnPositionInView);

	public bool IsPtzCenterAndZoomToRectangleSupported => GetBooleanValue(_devicePtzHwidText, TypeReferenceIds.PtzCenterAndZoomToRectangle);

	public bool IsPtzHomeSupported => GetBooleanValue(_devicePtzHwidText, TypeReferenceIds.PtzHomeSupport);

	public bool IsPresetEnabled => GetPresetConfiguration() != null;

	public DevicePresetConfiguration PresetConfiguration => GetPresetConfiguration();

	public bool CanLoadPresetsFromDevice
	{
		get
		{
			DevicePresetConfiguration presetConfiguration = GetPresetConfiguration();
			if (presetConfiguration != null && presetConfiguration.HasInternalPresets)
			{
				return presetConfiguration.Internal.CanLoadFromDevice;
			}
			return false;
		}
	}

	public PtzSetting Pan
	{
		get
		{
			string valueByTypeId = _driverSettings.GetValueByTypeId(_devicePtzHwidText, TypeReferenceIds.PtzPan.ToString());
			if (string.IsNullOrEmpty(valueByTypeId))
			{
				return null;
			}
			return new PtzSetting("Pan", TypeReferenceIds.PtzPan.ToString(), valueByTypeId);
		}
	}

	public PtzSetting Tilt
	{
		get
		{
			string valueByTypeId = _driverSettings.GetValueByTypeId(_devicePtzHwidText, TypeReferenceIds.PtzTilt.ToString());
			if (string.IsNullOrEmpty(valueByTypeId))
			{
				return null;
			}
			return new PtzSetting("Tilt", TypeReferenceIds.PtzTilt.ToString(), valueByTypeId);
		}
	}

	public PtzSetting Zoom
	{
		get
		{
			string valueByTypeId = _driverSettings.GetValueByTypeId(_devicePtzHwidText, TypeReferenceIds.PtzZoom.ToString());
			if (string.IsNullOrEmpty(valueByTypeId))
			{
				return null;
			}
			return new PtzSetting("Zoom", TypeReferenceIds.PtzZoom.ToString(), valueByTypeId);
		}
	}

	public PtzSetting[] PtzControls
	{
		get
		{
			List<PtzSetting> list = new List<PtzSetting>();
			foreach (SettingEntry sectionSetting in _driverSettings.GetSectionSettings(_devicePtzHwidText))
			{
				if (!string.IsNullOrEmpty(sectionSetting.Value) && sectionSetting.Value.Contains("<control>"))
				{
					try
					{
						list.Add(new PtzSetting(sectionSetting.Name, sectionSetting.TypeId, sectionSetting.Value));
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch
					{
					}
				}
			}
			return list.ToArray();
		}
	}

	public DevicePtzSupport PtzSupport
	{
		get
		{
			DevicePtzSupport devicePtzSupport = new DevicePtzSupport();
			devicePtzSupport.Enabled = IsPtzEnabled;
			bool.TryParse(_driverSettings.GetValueByTypeId(_devicePtzHwidText, TypeReferenceIds.PtzCenterAndZoomToRectangle.ToString()), out devicePtzSupport.CenterAndZoomToRectangle);
			bool.TryParse(_driverSettings.GetValueByTypeId(_devicePtzHwidText, TypeReferenceIds.PtzCenterOnPositionInView.ToString()), out devicePtzSupport.CenterOnPositionInView);
			bool.TryParse(_driverSettings.GetValueByTypeId(_devicePtzHwidText, TypeReferenceIds.PtzDiagonalSupport.ToString()), out devicePtzSupport.Diagonal);
			bool.TryParse(_driverSettings.GetValueByTypeId(_devicePtzHwidText, TypeReferenceIds.PtzHomeSupport.ToString()), out devicePtzSupport.Home);
			return devicePtzSupport;
		}
	}

	public string ICPDeviceName => _driverSettings.GetValueByTypeId(_deviceHwidText, TypeReferenceIds.DeviceName.ToString()) ?? string.Empty;

	public DeviceSettingUtilities(string settingsXml)
	{
		_driverSettings = new DriverSettings(settingsXml);
		GetIndexAndTypeFromSettings(_driverSettings, out _deviceType, out _index);
		_deviceHwidText = DeviceHwid.GetHwidAsText(_deviceType, _index);
		_devicePtzHwidText = DevicePtzHwid.GetHwidAsText(_deviceType, _index);
	}

	public DeviceSettingUtilities(DriverSettings settings, DeviceTypeEnum deviceType, int deviceIndex)
	{
		_index = deviceIndex;
		_driverSettings = settings;
		_deviceType = deviceType;
		_deviceHwidText = DeviceHwid.GetHwidAsText(_deviceType, _index);
		_devicePtzHwidText = DevicePtzHwid.GetHwidAsText(_deviceType, _index);
	}

	private void GetIndexAndTypeFromSettings(DriverSettings settings, out DeviceTypeEnum deviceType, out int deviceIndex)
	{
		deviceType = DeviceTypeEnum.Unknown;
		deviceIndex = -1;
		string[] sectionNames = settings.GetSectionNames();
		for (int i = 0; i < sectionNames.Length && !GetIndexAndTypeFromSectionName(sectionNames[i], out deviceIndex, out deviceType); i++)
		{
		}
	}

	private static bool GetIndexAndTypeFromSectionName(string sectionName, out int index, out DeviceTypeEnum deviceType)
	{
		index = -1;
		deviceType = DeviceTypeEnum.Unknown;
		try
		{
			string[] array = sectionName.Split(':')[1].Split('.');
			string s = array[0];
			deviceType = (DeviceTypeEnum)int.Parse(s, NumberFormatInfo.InvariantInfo);
			string s2 = array[1];
			index = int.Parse(s2, NumberFormatInfo.InvariantInfo);
			return true;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
			index = -1;
			deviceType = DeviceTypeEnum.Unknown;
			return false;
		}
	}

	private bool GetBooleanValue(string hwidText, Guid typeReferenceId)
	{
		bool result = false;
		string valueByTypeId = _driverSettings.GetValueByTypeId(hwidText, typeReferenceId.ToString());
		if (!string.IsNullOrEmpty(valueByTypeId) && !bool.TryParse(valueByTypeId, out result))
		{
			result = false;
		}
		return result;
	}

	private DevicePresetConfiguration GetPresetConfiguration()
	{
		if (!_presetConfigCached)
		{
			string valueByTypeId = _driverSettings.GetValueByTypeId(_devicePtzHwidText, TypeReferenceIds.PtzPresetConfiguration.ToString());
			try
			{
				if (!string.IsNullOrEmpty(valueByTypeId))
				{
					_presetConfig = DevicePresetConfiguration.LoadXml(valueByTypeId);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
				throw new InvalidSettingException(string.Format(CultureInfo.InvariantCulture, "PtzPresetConfiguration had invalid value: {0}", valueByTypeId));
			}
			_presetConfigCached = true;
		}
		return _presetConfig;
	}

	public int GetValidTime(int deviceIndex, int itemIndex)
	{
		int result = 3;
		string valueByTypeId = _driverSettings.GetValueByTypeId(MetadataItemHwid.GetHwidAsText(DeviceTypeEnum.Metadata, deviceIndex, itemIndex), TypeReferenceIds.ValidTime.ToString());
		if (!string.IsNullOrEmpty(valueByTypeId) && !int.TryParse(valueByTypeId, out result))
		{
			result = 3;
		}
		return result;
	}
}
