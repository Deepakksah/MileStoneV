using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

[DebuggerDisplay("Name = {Name}")]
public class HardwareInfo : IHardwareInfo
{
	private List<IDeviceInfo> _enabledCameras;

	private List<IDeviceInfo> _enabledDevices;

	private List<IDeviceInfo> _cameraDevices;

	private List<IDeviceInfo> _enabledNonCameraDevices;

	private int _deviceCount;

	public Guid Id { get; }

	public Guid RecorderId { get; }

	public string MacAddress { get; }

	public string MacAddressHashed { get; }

	public string SerialNumber { get; set; }

	public string DriverName { get; set; }

	public string DriverVersion { get; set; }

	public int DriverNumber { get; }

	public string DetectedModel { get; set; }

	public int LicenseCount { get; set; }

	public bool IsInterconnectedProduct { get; }

	public string LicenseMethod { get; set; }

	private ulong ConvertedMac { get; }

	public IFeedDictionary Feeds { get; private set; }

	public IPortDictionary Ports { get; private set; }

	public List<IDeviceInfo> Devices { get; private set; }

	public string VendorProductId { get; }

	public bool Enabled { get; }

	public string Name { get; }

	public HardwareLicenseState LicenseState { get; set; }

	public int ChannelLicenseCount { get; set; }

	public string DriverNameUpper { get; }

	public string DetectedModelUpper { get; }

	public List<IDeviceInfo> EnabledCameras
	{
		get
		{
			UpdateDeviceLists();
			return _enabledCameras;
		}
	}

	public List<IDeviceInfo> EnabledDevices
	{
		get
		{
			UpdateDeviceLists();
			return _enabledDevices;
		}
	}

	public List<IDeviceInfo> CameraDevices
	{
		get
		{
			UpdateDeviceLists();
			return _cameraDevices;
		}
	}

	public List<IDeviceInfo> EnabledNonCameraDevices
	{
		get
		{
			UpdateDeviceLists();
			return _enabledNonCameraDevices;
		}
	}

	public bool HasEnabledCameras => EnabledCameras.Count > 0;

	public bool HasActivatedCameras => EnabledCameras.Where((IDeviceInfo c) => c.LicenseState == DeviceInfoLicenseState.Permanent).Any();

	public bool HasCameraDevices
	{
		get
		{
			if (!Enabled)
			{
				return false;
			}
			return EnabledCameras.Where((IDeviceInfo d) => d.LicenseState == DeviceInfoLicenseState.Grace || d.LicenseState == DeviceInfoLicenseState.Permanent || d.LicenseState == DeviceInfoLicenseState.ChangeWithoutActivation || d.LicenseState == DeviceInfoLicenseState.Expired || d.LicenseState == DeviceInfoLicenseState.LicensedOnOtherRecordingServer || d.LicenseState == DeviceInfoLicenseState.Unlicensed || d.LicenseState == DeviceInfoLicenseState.UnlicensedDuplicate).Any();
		}
	}

	public bool HasLicensedCameras
	{
		get
		{
			if (!Enabled)
			{
				return false;
			}
			return EnabledCameras.Where((IDeviceInfo d) => d.LicenseState == DeviceInfoLicenseState.Grace || d.LicenseState == DeviceInfoLicenseState.Permanent || d.LicenseState == DeviceInfoLicenseState.ChangeWithoutActivation || d.LicenseState == DeviceInfoLicenseState.LicensedOnOtherRecordingServer).Any();
		}
	}

	public int[] EnabledCameraIndexes
	{
		get
		{
			int[] array = EnabledCameras.Select((IDeviceInfo c) => c.DeviceIndex).ToArray();
			Array.Sort(array);
			return array;
		}
	}

	public HardwareWhitelistStatus WhitelistStatus { get; set; }

	public HardwareInfo(Guid id, Guid recorderId, string name, bool enabled, string macAddress, string serialNumber, string driverName, string driverVersion, int driverNumber, string detectedModel, string vendorProductId, bool isInterconnected, List<IDeviceInfo> devices)
	{
		Devices = devices ?? throw new ArgumentNullException("devices");
		Id = id;
		RecorderId = recorderId;
		Name = name;
		Enabled = enabled;
		MacAddress = macAddress?.ToUpperInvariant();
		SerialNumber = serialNumber;
		DriverName = driverName;
		DriverVersion = driverVersion;
		DetectedModel = detectedModel;
		DriverNumber = driverNumber;
		VendorProductId = vendorProductId;
		IsInterconnectedProduct = isInterconnected;
		LicenseMethod = "Normal";
		LicenseCount = 1;
		Feeds = LicenseFactory.CreateNewFeedDictionary();
		Ports = new PortDictionary();
		LicenseState = HardwareLicenseState.Unknown;
		DriverNameUpper = DecodeValue(DriverName?.ToUpperInvariant());
		DetectedModelUpper = DecodeValue(DetectedModel?.ToUpperInvariant());
		WhitelistStatus = HardwareWhitelistStatus.NotSet;
		if (string.IsNullOrEmpty(macAddress))
		{
			macAddress = MacAddressUtilities.EmptyMacAddress;
		}
		MacAddressHashed = MacAddressUtilities.CreateMacAddressHashString(macAddress);
		ConvertedMac = MacToUlong(macAddress);
	}

	public override bool Equals(object obj)
	{
		HardwareInfo hardwareInfo = obj as HardwareInfo;
		if (obj == null || hardwareInfo == null)
		{
			return false;
		}
		if (ConvertedMac == 0L || hardwareInfo.ConvertedMac == 0L)
		{
			return MacAddress.Equals(hardwareInfo.MacAddress, StringComparison.OrdinalIgnoreCase);
		}
		return ConvertedMac == hardwareInfo.ConvertedMac;
	}

	public override int GetHashCode()
	{
		return MacAddress.GetHashCode();
	}

	private static ulong MacToUlong(string macAddress)
	{
		try
		{
			return Convert.ToUInt64(macAddress.Replace(":", ""), 16);
		}
		catch
		{
			return 0uL;
		}
	}

	private void UpdateDeviceLists()
	{
		bool flag = false;
		if (_deviceCount != Devices.Count)
		{
			_deviceCount = Devices.Count;
			flag = true;
		}
		if (flag || _enabledCameras == null)
		{
			_enabledCameras = (_enabledCameras = Devices.Where((IDeviceInfo d) => d.Enabled && d.DeviceType.Equals("Camera", StringComparison.OrdinalIgnoreCase)).ToList());
		}
		if (flag || _enabledDevices == null)
		{
			_enabledDevices = Devices.Where((IDeviceInfo d) => d.Enabled).ToList();
		}
		if (flag || _cameraDevices == null)
		{
			_cameraDevices = Devices.Where((IDeviceInfo d) => d.DeviceType.Equals("Camera", StringComparison.OrdinalIgnoreCase)).ToList();
		}
		if (flag || _enabledNonCameraDevices == null)
		{
			_enabledNonCameraDevices = Devices.Where((IDeviceInfo d) => d.Enabled && !d.DeviceType.Equals("Camera", StringComparison.OrdinalIgnoreCase)).ToList();
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string DecodeValue(string value)
	{
		if (value == null)
		{
			return value;
		}
		StringBuilder stringBuilder = new StringBuilder(value.Length);
		StringBuilder stringBuilder2 = new StringBuilder(value.Length);
		bool flag = false;
		foreach (char c in value)
		{
			switch (c)
			{
			case '&':
				flag = true;
				stringBuilder2.Append(c);
				break;
			case ';':
				if (flag)
				{
					stringBuilder2.Append(c);
					stringBuilder.Append(ConvertAndClearWordValue(stringBuilder2));
					flag = false;
				}
				else
				{
					stringBuilder.Append(c);
				}
				break;
			default:
				if (flag)
				{
					stringBuilder2.Append(c);
				}
				else
				{
					stringBuilder.Append(c);
				}
				break;
			}
		}
		stringBuilder.Append(ConvertAndClearWordValue(stringBuilder2));
		return stringBuilder.ToString();
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static string ConvertAndClearWordValue(StringBuilder currentWord)
	{
		string empty = string.Empty;
		switch (currentWord.ToString())
		{
		case "&AMP;":
		case "&amp;":
			empty = "&";
			break;
		case "&lt;":
		case "&LT;":
			empty = "<";
			break;
		case "&gt;":
		case "&GT;":
			empty = ">";
			break;
		case "&quot;":
		case "&QUOT;":
			empty = "\"";
			break;
		case "&apos;":
		case "&APOS;":
			empty = "'";
			break;
		default:
			empty = currentWord.ToString();
			break;
		}
		currentWord.Clear();
		return empty;
	}
}
