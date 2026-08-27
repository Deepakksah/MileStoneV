using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Server.Licensing.Internal;

public sealed class LicenseSnapshot : ILicenseSnapshot
{
	private readonly Dictionary<Guid, IRecorderInfo> _recorders;

	private readonly Dictionary<Guid, IRecorderLicenseInfo> _recorderLicenses;

	private readonly Dictionary<string, IHardwareLicenseInfo> _hardwareLicenses;

	private readonly Dictionary<string, IDeviceLicenseInfo> _deviceLicenses;

	private readonly ILicenseDataSetComponent _licenseDataSetComponent;

	private readonly Dictionary<string, Guid> _uniqueUnlicensed;

	private int _recorderLicenseCount;

	public IEnumerable<IRecorderLicenseInfo> RecorderLicensesWithAnyChange => RecorderLicensesByState((IRecorderLicenseInfo rl) => rl.UpdateType != LicenseUpdateAction.None);

	public IEnumerable<IHardwareLicenseInfo> HardwareLicensesWithAnyChange => HardwareLicensesByState((IHardwareLicenseInfo rl) => rl.UpdateType != LicenseUpdateAction.None);

	public IEnumerable<IDeviceLicenseInfo> DeviceLicensesWithAnyChange => DeviceLicensesByState((IDeviceLicenseInfo rl) => rl.UpdateType != LicenseUpdateAction.None);

	public IEnumerable<IRecorderInfo> Recorders
	{
		get
		{
			lock (_recorders)
			{
				return _recorders.Values;
			}
		}
	}

	public bool LicenseUpdated { get; private set; }

	public int HardwareLicenseCount => _hardwareLicenses.Count;

	public LicenseSnapshot(ILicenseDataSetComponent licenseDataSetComponent, Dictionary<Guid, IRecorderInfo> recorders, Dictionary<Guid, IRecorderLicenseInfo> recorderLicenses, Dictionary<string, IHardwareLicenseInfo> hardwareLicenses, Dictionary<string, IDeviceLicenseInfo> deviceLicenses)
	{
		_licenseDataSetComponent = licenseDataSetComponent ?? throw new ArgumentNullException("licenseDataSetComponent");
		_recorders = recorders ?? throw new ArgumentNullException("recorders");
		_recorderLicenses = recorderLicenses ?? throw new ArgumentNullException("recorderLicenses");
		_hardwareLicenses = hardwareLicenses ?? throw new ArgumentNullException("hardwareLicenses");
		_deviceLicenses = deviceLicenses ?? throw new ArgumentNullException("deviceLicenses");
		_uniqueUnlicensed = new Dictionary<string, Guid>();
		LicenseUpdated = false;
		_recorderLicenseCount = _recorderLicenses.Count;
	}

	public IEnumerable<IRecorderLicenseInfo> RecorderLicensesByState(LicenseUpdateAction state)
	{
		return RecorderLicensesByState((IRecorderLicenseInfo rl) => rl.UpdateType == state);
	}

	private IEnumerable<IRecorderLicenseInfo> RecorderLicensesByState(Func<IRecorderLicenseInfo, bool> predicate)
	{
		lock (_recorderLicenses)
		{
			return _recorderLicenses.Values.Where((IRecorderLicenseInfo rl) => predicate(rl)).ToList();
		}
	}

	public IEnumerable<IHardwareLicenseInfo> HardwareLicensesByState(LicenseUpdateAction state)
	{
		return HardwareLicensesByState((IHardwareLicenseInfo rl) => rl.UpdateType == state);
	}

	private IEnumerable<IHardwareLicenseInfo> HardwareLicensesByState(Func<IHardwareLicenseInfo, bool> predicate)
	{
		lock (_hardwareLicenses)
		{
			return _hardwareLicenses.Values.Where((IHardwareLicenseInfo hl) => predicate(hl)).ToList();
		}
	}

	public IEnumerable<IDeviceLicenseInfo> DeviceLicensesByState(LicenseUpdateAction state)
	{
		return DeviceLicensesByState((IDeviceLicenseInfo rl) => rl.UpdateType == state);
	}

	private IEnumerable<IDeviceLicenseInfo> DeviceLicensesByState(Func<IDeviceLicenseInfo, bool> predicate)
	{
		lock (_deviceLicenses)
		{
			return _deviceLicenses.Values.Where((IDeviceLicenseInfo dl) => predicate(dl)).ToList();
		}
	}

	public void SaveChanges()
	{
		UpdateLicenseDataSetFromLicenseSnapshot();
	}

	public bool AddUnlicensedHardware(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (_uniqueUnlicensed.TryGetValue(hardwareInfo.MacAddress, out var value))
		{
			return hardwareInfo.Id.Equals(value);
		}
		_uniqueUnlicensed.Add(hardwareInfo.MacAddress, hardwareInfo.Id);
		return true;
	}

	public bool AddUnlicensedDevice(IDeviceInfo deviceInfo)
	{
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		if (_uniqueUnlicensed.TryGetValue(deviceInfo.UniqueIdentifier, out var value))
		{
			return deviceInfo.Id.Equals(value);
		}
		_uniqueUnlicensed.Add(deviceInfo.UniqueIdentifier, deviceInfo.Id);
		return true;
	}

	public IEnumerable<IHardwareInfo> FindHardwareInGrace(Guid excludedRecorderId, bool isIcp)
	{
		Dictionary<string, IHardwareInfo> dictionary = new Dictionary<string, IHardwareInfo>(StringComparer.OrdinalIgnoreCase);
		foreach (IRecorderInfo item in Recorders.Where((IRecorderInfo r) => !r.Id.Equals(excludedRecorderId)))
		{
			foreach (IHardwareInfo item2 in item.Hardware)
			{
				if (!dictionary.ContainsKey(item2.MacAddress) && item2.IsInterconnectedProduct == isIcp && (item2.LicenseState == HardwareLicenseState.Temporary || item2.EnabledCameras.Any((IDeviceInfo d) => d.LicenseState == DeviceInfoLicenseState.Grace || d.LicenseState == DeviceInfoLicenseState.Unknown)))
				{
					dictionary.Add(item2.MacAddress, item2);
				}
			}
		}
		return dictionary.Values;
	}

	public IRecorderInfo GetRecorder(Guid recorderId)
	{
		if (recorderId.Equals(Guid.Empty))
		{
			return null;
		}
		lock (_recorders)
		{
			IRecorderInfo value;
			return _recorders.TryGetValue(recorderId, out value) ? value : null;
		}
	}

	public void AddRecorderLicense(IRecorderLicenseInfo recorderLicenseInfo)
	{
		if (recorderLicenseInfo == null)
		{
			throw new ArgumentNullException("recorderLicenseInfo");
		}
		lock (_recorderLicenses)
		{
			if (!_recorderLicenses.ContainsKey(recorderLicenseInfo.Id))
			{
				recorderLicenseInfo.UpdateType = LicenseUpdateAction.Insert;
				_recorderLicenseCount++;
			}
			else if (recorderLicenseInfo.UpdateType != LicenseUpdateAction.Insert)
			{
				recorderLicenseInfo.UpdateType = LicenseUpdateAction.Update;
				_recorderLicenseCount++;
			}
			_recorderLicenses[recorderLicenseInfo.Id] = recorderLicenseInfo;
		}
		LicenseUpdated = true;
	}

	public IRecorderLicenseInfo GetRecorderLicense(Guid recorderId)
	{
		if (recorderId.Equals(Guid.Empty))
		{
			throw new ArgumentException("Guid can not be empty", "recorderId");
		}
		lock (_recorderLicenses)
		{
			if (_recorderLicenses.TryGetValue(recorderId, out var value) && value != null && value.UpdateType != LicenseUpdateAction.Delete)
			{
				return value;
			}
			return null;
		}
	}

	public void RemoveRecorderLicense(Guid recorderId)
	{
		if (recorderId.Equals(Guid.Empty))
		{
			throw new ArgumentException("Guid can not be empty", "recorderId");
		}
		lock (_recorderLicenses)
		{
			if (_recorderLicenses.ContainsKey(recorderId))
			{
				_recorderLicenses[recorderId].UpdateType = LicenseUpdateAction.Delete;
				_recorderLicenseCount--;
				LicenseUpdated = true;
			}
		}
	}

	public int RecorderLicenseCount()
	{
		return _recorderLicenseCount;
	}

	public void AddHardwareLicense(IHardwareLicenseInfo hardwareLicenseInfo)
	{
		if (hardwareLicenseInfo == null)
		{
			throw new ArgumentNullException("hardwareLicenseInfo");
		}
		lock (_hardwareLicenses)
		{
			if (!_hardwareLicenses.ContainsKey(hardwareLicenseInfo.UniqueIdentifier))
			{
				hardwareLicenseInfo.UpdateType = LicenseUpdateAction.Insert;
			}
			else if (hardwareLicenseInfo.UpdateType != LicenseUpdateAction.Insert)
			{
				hardwareLicenseInfo.UpdateType = LicenseUpdateAction.Update;
			}
			_hardwareLicenses[hardwareLicenseInfo.UniqueIdentifier] = hardwareLicenseInfo;
		}
		LicenseUpdated = true;
	}

	public IHardwareLicenseInfo GetHardwareLicense(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		lock (_hardwareLicenses)
		{
			if (_hardwareLicenses.TryGetValue(hardwareInfo.MacAddress, out var value) && value != null && value.UpdateType != LicenseUpdateAction.Delete)
			{
				return value;
			}
			return null;
		}
	}

	public void SetHardwareLicenseExpire(IHardwareInfo hardwareInfo, DateTime expireDate)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (expireDate.Equals(DateTime.MinValue))
		{
			throw new ArgumentOutOfRangeException("expireDate");
		}
		lock (_hardwareLicenses)
		{
			if (!_hardwareLicenses.TryGetValue(hardwareInfo.MacAddress, out var value))
			{
				value = LicenseFactory.CreateHardwareLicenseInfo(hardwareInfo, DateTime.UtcNow, expireDate.Ticks);
				AddHardwareLicense(value);
			}
			else if (value.UpdateExpireDate(expireDate))
			{
				value.UpdateType = LicenseUpdateAction.Update;
				LicenseUpdated = true;
			}
		}
	}

	public void RemoveHardwareLicense(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		lock (_hardwareLicenses)
		{
			if (_hardwareLicenses.TryGetValue(hardwareInfo.MacAddress, out var value))
			{
				value.UpdateType = LicenseUpdateAction.Delete;
			}
		}
	}

	public DateTime GetHardwareLicenseExpireDate(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		lock (_hardwareLicenses)
		{
			if (_hardwareLicenses.TryGetValue(hardwareInfo.MacAddress, out var value))
			{
				return value.ExpireDate;
			}
			return new DateTime(0L, DateTimeKind.Utc);
		}
	}

	public void AddDeviceLicense(IDeviceLicenseInfo deviceLicenseInfo)
	{
		if (deviceLicenseInfo == null)
		{
			throw new ArgumentNullException("deviceLicenseInfo");
		}
		if (string.IsNullOrEmpty(deviceLicenseInfo.UniqueIdentifier))
		{
			throw new ArgumentException("UniqueIdentifier");
		}
		lock (_deviceLicenses)
		{
			if (!_deviceLicenses.ContainsKey(deviceLicenseInfo.UniqueIdentifier))
			{
				deviceLicenseInfo.UpdateType = LicenseUpdateAction.Insert;
			}
			else if (deviceLicenseInfo.UpdateType != LicenseUpdateAction.Insert)
			{
				deviceLicenseInfo.UpdateType = LicenseUpdateAction.Update;
			}
			_deviceLicenses[deviceLicenseInfo.UniqueIdentifier] = deviceLicenseInfo;
		}
		LicenseUpdated = true;
	}

	public IDeviceLicenseInfo GetDeviceLicense(IDeviceInfo deviceInfo)
	{
		if (deviceInfo == null)
		{
			throw new ArgumentException("deviceInfo can not be null", "deviceInfo");
		}
		if (string.IsNullOrEmpty(deviceInfo.UniqueIdentifier))
		{
			throw new ArgumentException("UniqueIdentifier");
		}
		lock (_deviceLicenses)
		{
			if (_deviceLicenses.TryGetValue(deviceInfo.UniqueIdentifier, out var value) && value != null && value.UpdateType != LicenseUpdateAction.Delete)
			{
				return value;
			}
			return null;
		}
	}

	public DateTime GetDeviceLicenseExpireDate(IDeviceInfo deviceInfo)
	{
		if (deviceInfo == null)
		{
			throw new ArgumentException("deviceInfo can not be null", "deviceInfo");
		}
		if (string.IsNullOrEmpty(deviceInfo.UniqueIdentifier))
		{
			throw new ArgumentException("UniqueIdentifier");
		}
		lock (_deviceLicenses)
		{
			if (_deviceLicenses.TryGetValue(deviceInfo.UniqueIdentifier, out var value) && value != null && value.UpdateType != LicenseUpdateAction.Delete)
			{
				return new DateTime(value.License, DateTimeKind.Utc);
			}
			return new DateTime(0L, DateTimeKind.Utc);
		}
	}

	public void SetDeviceLicenseExpire(IDeviceInfo deviceInfo, DateTime expireDate)
	{
		if (deviceInfo == null)
		{
			throw new ArgumentException("deviceInfo can not be null", "deviceInfo");
		}
		if (string.IsNullOrEmpty(deviceInfo.UniqueIdentifier))
		{
			throw new ArgumentException("UniqueIdentifier");
		}
		if (expireDate.Equals(DateTime.MinValue))
		{
			throw new ArgumentOutOfRangeException("expireDate");
		}
		if (expireDate.Kind != DateTimeKind.Utc)
		{
			throw new ArgumentException("Datetime must be Utc", "expireDate");
		}
		lock (_deviceLicenses)
		{
			if (_deviceLicenses.TryGetValue(deviceInfo.UniqueIdentifier, out var value))
			{
				if (value.UpdateExpireDate(expireDate))
				{
					value.UpdateType = LicenseUpdateAction.Update;
					LicenseUpdated = true;
				}
			}
			else
			{
				AddDeviceLicense(LicenseFactory.CreateDeviceLicenseInfo(deviceInfo, DateTime.UtcNow, expireDate.Ticks));
			}
		}
	}

	public void RemoveDeviceLicense(IDeviceInfo deviceInfo)
	{
		if (deviceInfo == null)
		{
			throw new ArgumentException("deviceInfo can not be null", "deviceInfo");
		}
		if (string.IsNullOrEmpty(deviceInfo.UniqueIdentifier))
		{
			throw new ArgumentException("UniqueIdentifier");
		}
		lock (_deviceLicenses)
		{
			if (_deviceLicenses.TryGetValue(deviceInfo.UniqueIdentifier, out var value))
			{
				value.UpdateType = LicenseUpdateAction.Delete;
			}
		}
	}

	public void ResetUpdateStatuses()
	{
		lock (_deviceLicenses)
		{
			foreach (KeyValuePair<string, IDeviceLicenseInfo> deviceLicense in _deviceLicenses)
			{
				deviceLicense.Value.UpdateType = LicenseUpdateAction.None;
			}
		}
		lock (_hardwareLicenses)
		{
			foreach (KeyValuePair<string, IHardwareLicenseInfo> hardwareLicense in _hardwareLicenses)
			{
				hardwareLicense.Value.UpdateType = LicenseUpdateAction.None;
			}
		}
		lock (_recorderLicenses)
		{
			foreach (KeyValuePair<Guid, IRecorderLicenseInfo> recorderLicense in _recorderLicenses)
			{
				recorderLicense.Value.UpdateType = LicenseUpdateAction.None;
			}
		}
		LicenseUpdated = false;
	}

	public IHardwareInfo FindHardwareByMacAddress(string macAddress)
	{
		if (string.IsNullOrEmpty(macAddress))
		{
			throw new ArgumentNullException("macAddress");
		}
		foreach (IRecorderInfo recorder in Recorders)
		{
			foreach (IHardwareInfo item in recorder.Hardware)
			{
				if (item.MacAddress.Equals(macAddress, StringComparison.OrdinalIgnoreCase))
				{
					return item;
				}
			}
		}
		return null;
	}

	private void UpdateLicenseDataSetFromLicenseSnapshot()
	{
		_licenseDataSetComponent.WriteLicenseDataSetChangesToDatabase(this);
		ResetUpdateStatuses();
	}

	public static ILicenseSnapshot CreateLicenseSnapshot(ILicenseDataSetComponent licenseDataSetComponent, LicenseDataSet licenseDataSet, bool enabledOnly = false)
	{
		Dictionary<Guid, IRecorderInfo> dictionary = new Dictionary<Guid, IRecorderInfo>();
		Dictionary<Guid, IHardwareInfo> dictionary2 = new Dictionary<Guid, IHardwareInfo>(licenseDataSet.Hardware.Count);
		Dictionary<string, IDeviceLicenseInfo> deviceLicenses = new Dictionary<string, IDeviceLicenseInfo>(licenseDataSet.LicenseDevice.Count);
		Dictionary<string, IHardwareLicenseInfo> hardwareLicenses = new Dictionary<string, IHardwareLicenseInfo>(licenseDataSet.LicenseHardware.Count);
		Dictionary<Guid, IRecorderLicenseInfo> recorderLicenses = new Dictionary<Guid, IRecorderLicenseInfo>(licenseDataSet.LicenseRecorder.Count);
		Dictionary<Guid, List<IDeviceInfo>> dictionary3 = new Dictionary<Guid, List<IDeviceInfo>>();
		Dictionary<Guid, List<IHardwareInfo>> dictionary4 = new Dictionary<Guid, List<IHardwareInfo>>();
		Dictionary<string, IDeviceInfo> devicesByUniqueId = new Dictionary<string, IDeviceInfo>();
		Dictionary<string, IHardwareInfo> hardwareByMacAddress = new Dictionary<string, IHardwareInfo>();
		foreach (LicenseDataSet.HardwareRow item in licenseDataSet.Hardware)
		{
			if (!enabledOnly || item.Enabled)
			{
				if (!dictionary4.TryGetValue(item.IDRecorder, out var value))
				{
					value = new List<IHardwareInfo>();
					dictionary4.Add(item.IDRecorder, value);
				}
				List<IDeviceInfo> list = new List<IDeviceInfo>();
				dictionary3[item.IDHardware] = list;
				IHardwareInfo hardwareInfo = LicenseFactory.CreateNewHardwareInfo(item.IDHardware, item.IDRecorder, item.Name, item.Enabled, item.MacAddress.ToUpperInvariant(), item.SerialNumber ?? "Unknown", item.DriverName ?? "Unknown", item.DriverVersion ?? "Unknown", item.DriverNumber, item.DetectedModel ?? "Unknown", item.VendorProductID, item.IsInterconnectedProduct, list);
				value.Add(hardwareInfo);
				hardwareByMacAddress[hardwareInfo.MacAddress] = hardwareInfo;
				dictionary2[item.IDHardware] = hardwareInfo;
			}
		}
		foreach (LicenseDataSet.DevicesRow device in licenseDataSet.Devices)
		{
			if (!enabledOnly || device.Enabled)
			{
				IHardwareInfo hardwareInfo2 = dictionary2[device.IDHardware];
				IDeviceInfo deviceInfo = LicenseFactory.CreateNewDeviceInfo(device.IDDevice, device.OriginalIdDevice, device.DeviceType, device.DeviceIndex, device.Enabled, hardwareInfo2);
				dictionary3[device.IDHardware].Add(deviceInfo);
				devicesByUniqueId[deviceInfo.UniqueIdentifier] = deviceInfo;
			}
		}
		foreach (LicenseDataSet.RecordersRow recorder in licenseDataSet.Recorders)
		{
			if ((!enabledOnly || !recorder.Enabled) && enabledOnly)
			{
				continue;
			}
			IRecorderInfo recorderInfo = LicenseFactory.CreateRecorder(recorder.IDRecorder, recorder.Enabled, recorder.DevicePackInfo);
			if (dictionary4.TryGetValue(recorder.IDRecorder, out var value2))
			{
				foreach (IHardwareInfo item2 in value2)
				{
					recorderInfo.AddHardware(item2);
				}
			}
			dictionary[recorderInfo.Id] = recorderInfo;
		}
		Parallel.Invoke(delegate
		{
			foreach (LicenseDataSet.LicenseDeviceRow item3 in licenseDataSet.LicenseDevice.Where((LicenseDataSet.LicenseDeviceRow dl) => dl.RowState != DataRowState.Deleted))
			{
				if (devicesByUniqueId.TryGetValue(item3.UniqueId, out var value3) && value3 != null)
				{
					deviceLicenses[value3.UniqueIdentifier] = LicenseFactory.CreateDeviceLicenseInfo(value3, item3.LastModified, item3.License);
				}
			}
		}, delegate
		{
			foreach (LicenseDataSet.LicenseHardwareRow item4 in licenseDataSet.LicenseHardware.Where((LicenseDataSet.LicenseHardwareRow hl) => hl.RowState != DataRowState.Deleted))
			{
				if (hardwareByMacAddress.TryGetValue(item4.UniqueId, out var value3) && value3 != null)
				{
					hardwareLicenses[value3.MacAddress] = LicenseFactory.CreateHardwareLicenseInfo(value3, item4.LastModified, item4.License);
				}
			}
		}, delegate
		{
			foreach (LicenseDataSet.LicenseRecorderRow item5 in licenseDataSet.LicenseRecorder.Where((LicenseDataSet.LicenseRecorderRow rl) => rl.RowState != DataRowState.Deleted))
			{
				recorderLicenses[item5.IDRecorder] = LicenseFactory.CreateRecorderLicenseInfo(item5.IDRecorder, item5.LastModified);
			}
		});
		return new LicenseSnapshot(licenseDataSetComponent, dictionary, recorderLicenses, hardwareLicenses, deviceLicenses);
	}
}
