using System;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Common.Integration;

public class HardwareSettingUtilities
{
	private DriverSettings _driverSettings;

	public string MacAddress
	{
		get
		{
			string valueByTypeId = _driverSettings.GetValueByTypeId("hardware", TypeReferenceIds.MacAddress.ToString());
			if (valueByTypeId == null)
			{
				return valueByTypeId;
			}
			return valueByTypeId.ToUpperInvariant();
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_driverSettings.UpdateByTypeId("hardware", TypeReferenceIds.MacAddress.ToString(), value.ToString());
		}
	}

	public string SerialNumber
	{
		get
		{
			return _driverSettings.GetValueByTypeId("hardware", TypeReferenceIds.SerialNumber.ToString());
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_driverSettings.UpdateByTypeId("hardware", TypeReferenceIds.SerialNumber.ToString(), value.ToString());
		}
	}

	public string FirmwareVersion
	{
		get
		{
			return _driverSettings.GetValueByTypeId("hardware", TypeReferenceIds.FirmwareVersion.ToString());
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_driverSettings.UpdateByTypeId("hardware", TypeReferenceIds.FirmwareVersion.ToString(), value.ToString());
		}
	}

	public bool HardwarePasswordChangeSupported => string.Equals(_driverSettings.GetValueByTypeId("hardware", TypeReferenceIds.PasswordChangeSupported.ToString()), "yes", StringComparison.OrdinalIgnoreCase);

	public bool HttpsEnabled => string.Equals(_driverSettings.GetValueByTypeId("hardware", TypeReferenceIds.HttpsEnabled.ToString()), "yes", StringComparison.OrdinalIgnoreCase);

	public int? HttpsPort
	{
		get
		{
			if (int.TryParse(_driverSettings.GetValueByTypeId("hardware", TypeReferenceIds.HttpsPort.ToString()), out var result))
			{
				return result;
			}
			return null;
		}
	}

	public HardwareSettingUtilities(string settingsXml)
	{
		_driverSettings = new DriverSettings(settingsXml);
	}

	public HardwareSettingUtilities(DriverSettings settings)
	{
		_driverSettings = settings;
	}
}
