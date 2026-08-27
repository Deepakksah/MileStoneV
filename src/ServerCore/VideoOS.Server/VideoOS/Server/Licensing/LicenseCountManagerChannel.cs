using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Extensions;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Licensing;

public sealed class LicenseCountManagerChannel : ILicenseCountManager
{
	private readonly Dictionary<Guid, List<LicenseHardwareDevice>> _recorderHardware = new Dictionary<Guid, List<LicenseHardwareDevice>>();

	private readonly Dictionary<string, LicenseHardwareDevice> _hardwareByMacAddress = new Dictionary<string, LicenseHardwareDevice>();

	private readonly Dictionary<Guid, HashSet<string>> _macByRecorder = new Dictionary<Guid, HashSet<string>>();

	private readonly Dictionary<string, List<LicenseCameraFeed>> _feedsByMacAddress = new Dictionary<string, List<LicenseCameraFeed>>();

	private readonly Dictionary<Guid, List<LicenseCameraFeed>> _feedsByRecorder = new Dictionary<Guid, List<LicenseCameraFeed>>();

	private readonly HashSet<string> _licensedHardwareWithoutEnabledCameras = new HashSet<string>();

	private readonly HashSet<string> _changesWithoutActivation = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

	private int _licensedHardwareNonFree;

	private readonly HashSet<string> _licensedIcpHardwareWithoutDevices = new HashSet<string>();

	private int _licensedIcpFeedsSum;

	private int _licensedNonIcpFeedsSum;

	private readonly ILogger _licenseLogger;

	private readonly object _lockObject = new object();

	public int UsedChangesWithoutActivation
	{
		get
		{
			lock (_lockObject)
			{
				return _changesWithoutActivation.Count;
			}
		}
	}

	public Dictionary<Guid, DeviceInfoLicenseState> DeviceStatus { get; private set; }

	public LicenseCountManagerChannel()
	{
		DeviceStatus = new Dictionary<Guid, DeviceInfoLicenseState>();
		_licenseLogger = Telemetry.GetILogger("Licensing");
	}

	public int CountOfLicensesUsed(bool isInterconnected)
	{
		lock (_lockObject)
		{
			if (isInterconnected)
			{
				return GetCameraFeedsUsedOnSite(isInterconnected) + _licensedIcpHardwareWithoutDevices.Count();
			}
			return GetCameraFeedsUsedOnSite(isInterconnected) + _licensedHardwareNonFree;
		}
	}

	public void AddLicensedHardware(IHardwareInfo hardwareInfo, Guid recorderId, bool isFreeLicense)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		lock (_lockObject)
		{
			EnsureRecordingServerRegistered(recorderId);
			if (isFreeLicense)
			{
				EnsureHardwareRemovedFromChangesWithoutActivation(hardwareInfo.MacAddress);
			}
			else if (!hardwareInfo.HasEnabledCameras)
			{
				_licensedHardwareWithoutEnabledCameras.Add(hardwareInfo.MacAddress);
			}
			if (_hardwareByMacAddress.ContainsKey(hardwareInfo.MacAddress))
			{
				throw new ArgumentException("An entry is already registered for the mac address");
			}
			LicenseHardwareDevice licenseHardwareDevice = new LicenseHardwareDevice(hardwareInfo.MacAddress, hardwareInfo.IsInterconnectedProduct, recorderId, isFreeLicense);
			_hardwareByMacAddress[hardwareInfo.MacAddress] = licenseHardwareDevice;
			_recorderHardware[recorderId].Add(licenseHardwareDevice);
			if (hardwareInfo.IsInterconnectedProduct && !isFreeLicense && !hardwareInfo.HasEnabledCameras)
			{
				_licensedIcpHardwareWithoutDevices.Add(hardwareInfo.MacAddress);
			}
			if (!isFreeLicense)
			{
				_licensedHardwareNonFree++;
			}
		}
	}

	public void UpdateHardwareLicenseValue(string mac, long licenseValue)
	{
		lock (_lockObject)
		{
			_hardwareByMacAddress[mac].LicensedValue = licenseValue;
		}
	}

	public long GetHardwareLicenseValue(string mac)
	{
		lock (_lockObject)
		{
			return _hardwareByMacAddress[mac].LicensedValue;
		}
	}

	public bool IsHardwareLicensedOnSite(string mac)
	{
		lock (_lockObject)
		{
			return _hardwareByMacAddress.ContainsKey(mac);
		}
	}

	public bool IsHardwareLicensedOnRecorder(string mac, Guid recorderId)
	{
		lock (_lockObject)
		{
			List<LicenseHardwareDevice> value;
			return _recorderHardware.TryGetValue(recorderId, out value) && value.Any((LicenseHardwareDevice h) => h.Mac.Equals(mac, StringComparison.OrdinalIgnoreCase));
		}
	}

	public bool IsHardwareWithoutEnabledCamerasLicensed(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (hardwareInfo.EnabledCameras.Count != 0)
		{
			throw new InvalidOperationException(FormattableString.Invariant($"Hardware cannot have enabled cameras - {hardwareInfo.Name}"));
		}
		lock (_lockObject)
		{
			return _licensedHardwareWithoutEnabledCameras.Contains(hardwareInfo.MacAddress);
		}
	}

	public int GetHardwareUsedOnSite(bool icp)
	{
		lock (_lockObject)
		{
			return _hardwareByMacAddress.Values.Count((LicenseHardwareDevice h) => h.Icp == icp);
		}
	}

	public void AddLicensedCameraFeed(IRecorderInfo recorderInfo, IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, long licenseValue, int feedCount)
	{
		if (recorderInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		lock (_lockObject)
		{
			EnsureRecordingServerRegistered(recorderInfo.Id);
			EnsureMacAddressFeedRegistered(hardwareInfo.MacAddress);
			EnsureHardwareRemovedFromChangesWithoutActivation(hardwareInfo.MacAddress);
			if (_feedsByMacAddress[hardwareInfo.MacAddress].Any((LicenseCameraFeed f) => f.UniqueId.Equals(deviceInfo.UniqueIdentifier, StringComparison.Ordinal)))
			{
				throw new ArgumentException("An entry is already registered for the mac address and device index.", "deviceInfo");
			}
			LicenseCameraFeed licenseCameraFeed = new LicenseCameraFeed(hardwareInfo, deviceInfo, recorderInfo.Id, hardwareInfo.IsInterconnectedProduct, feedCount, licenseValue);
			if (hardwareInfo.IsInterconnectedProduct)
			{
				_licensedIcpFeedsSum += licenseCameraFeed.FeedCount;
				_licensedIcpHardwareWithoutDevices.Remove(hardwareInfo.MacAddress);
			}
			else
			{
				_licensedNonIcpFeedsSum += licenseCameraFeed.FeedCount;
			}
			_feedsByMacAddress[hardwareInfo.MacAddress].Add(licenseCameraFeed);
			_feedsByRecorder[recorderInfo.Id].Add(licenseCameraFeed);
		}
	}

	public void UpdateCameraFeedLicensedValue(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, long licenseValue)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		lock (_lockObject)
		{
			LicenseCameraFeed licenseCameraFeed = _feedsByMacAddress[hardwareInfo.MacAddress].FirstOrDefault((LicenseCameraFeed lcf) => lcf.UniqueId.Equals(deviceInfo.UniqueIdentifier, StringComparison.Ordinal));
			if (licenseCameraFeed != null)
			{
				licenseCameraFeed.LicensedValue = licenseValue;
			}
		}
	}

	public long GetCameraFeedLicenseValue(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		lock (_lockObject)
		{
			if (_feedsByMacAddress.TryGetValue(hardwareInfo.MacAddress, out var value))
			{
				LicenseCameraFeed licenseCameraFeed = value.FirstOrDefault((LicenseCameraFeed lcf) => lcf.UniqueId.Equals(deviceInfo.UniqueIdentifier, StringComparison.Ordinal));
				if (licenseCameraFeed != null)
				{
					return licenseCameraFeed.LicensedValue;
				}
			}
		}
		throw new InvalidOperationException(FormattableString.Invariant($"Camera feed not found.  Hardware: {hardwareInfo.Name}; Device: {deviceInfo.Id}"));
	}

	public Guid GetFirstCameraFeedDeviceId(string mac)
	{
		lock (_lockObject)
		{
			if (_feedsByMacAddress.TryGetValue(mac, out var value))
			{
				return value[0].DeviceInfo.OriginalDeviceId;
			}
		}
		throw new InvalidOperationException(FormattableString.Invariant($"Camera feed not found.  Hardware: {mac}"));
	}

	public bool IsAnyCameraFeedLicensedForHardware(string mac)
	{
		lock (_lockObject)
		{
			List<LicenseCameraFeed> value;
			return _feedsByMacAddress.TryGetValue(mac, out value) && value.Any();
		}
	}

	public bool IsCameraFeedLicensedOnSite(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		lock (_lockObject)
		{
			List<LicenseCameraFeed> value;
			return _feedsByMacAddress.TryGetValue(hardwareInfo.MacAddress, out value) && value.Any((LicenseCameraFeed lcf) => lcf.UniqueId.Equals(deviceInfo.UniqueIdentifier, StringComparison.Ordinal));
		}
	}

	public bool IsCameraFeedLicensedOnRecorder(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo, Guid recorderId)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		lock (_lockObject)
		{
			List<LicenseCameraFeed> value;
			return _feedsByMacAddress.TryGetValue(hardwareInfo.MacAddress, out value) && value.Any((LicenseCameraFeed lcf) => lcf.RecorderId.Equals(recorderId) && lcf.UniqueId.Equals(deviceInfo.UniqueIdentifier, StringComparison.Ordinal));
		}
	}

	public int GetCameraFeedsUsedOnSite(bool icp)
	{
		lock (_lockObject)
		{
			return icp ? _licensedIcpFeedsSum : _licensedNonIcpFeedsSum;
		}
	}

	public int GetCameraFeedsUsedByRecorder(Guid recorderId, bool icp)
	{
		lock (_lockObject)
		{
			if (_feedsByRecorder.TryGetValue(recorderId, out var value))
			{
				return value.Where((LicenseCameraFeed rh) => rh.Icp == icp).Sum((LicenseCameraFeed rh) => rh.FeedCount);
			}
		}
		return 0;
	}

	public int GetCameraFeedsUsedByHardware(string mac)
	{
		lock (_lockObject)
		{
			if (_feedsByMacAddress.TryGetValue(mac, out var value))
			{
				return value.Sum((LicenseCameraFeed fbm) => fbm.FeedCount);
			}
		}
		return 0;
	}

	public void RegisterHardwareRecorderRelation(Guid recorderId, string mac)
	{
		if (string.IsNullOrEmpty(mac))
		{
			throw new ArgumentNullException("mac");
		}
		lock (_lockObject)
		{
			EnsureRecordingServerRegistered(recorderId);
			_macByRecorder[recorderId].Add(mac.ToUpper(CultureInfo.InvariantCulture));
		}
	}

	public IEnumerable<Guid> GetRelatedRecordingServers(string mac)
	{
		if (string.IsNullOrEmpty(mac))
		{
			throw new ArgumentNullException("mac");
		}
		List<Guid> list = new List<Guid>();
		mac = mac.ToUpper(CultureInfo.InvariantCulture);
		lock (_lockObject)
		{
			foreach (KeyValuePair<Guid, HashSet<string>> item in _macByRecorder)
			{
				if (item.Value.Contains(mac))
				{
					list.Add(item.Key);
				}
			}
			return list;
		}
	}

	public void ClearByRecorder(Guid recorderId)
	{
		lock (_lockObject)
		{
			Parallel.Invoke(delegate
			{
				if (_recorderHardware.TryGetValue(recorderId, out var value))
				{
					foreach (LicenseHardwareDevice item in value)
					{
						_hardwareByMacAddress.Remove(item.Mac);
						if (!item.IsFreeLicense)
						{
							_licensedHardwareNonFree--;
							_licensedHardwareWithoutEnabledCameras.Remove(item.Mac);
						}
						_licensedIcpHardwareWithoutDevices.Remove(item.Mac);
					}
					_recorderHardware.Remove(recorderId);
				}
			}, delegate
			{
				if (_feedsByRecorder.TryGetValue(recorderId, out var value))
				{
					foreach (LicenseCameraFeed item2 in value)
					{
						if (item2.Icp)
						{
							_licensedIcpFeedsSum -= item2.FeedCount;
						}
						else
						{
							_licensedNonIcpFeedsSum -= item2.FeedCount;
						}
					}
					_feedsByRecorder.Remove(recorderId);
				}
			}, delegate
			{
				List<string> list = new List<string>();
				foreach (KeyValuePair<string, List<LicenseCameraFeed>> item3 in _feedsByMacAddress)
				{
					for (int num = item3.Value.Count - 1; num >= 0; num--)
					{
						if (item3.Value[num].RecorderId.Equals(recorderId))
						{
							item3.Value.RemoveAt(num);
						}
						if (item3.Value.Count == 0)
						{
							list.Add(item3.Key);
						}
					}
				}
				list.ForEach(delegate(string m)
				{
					_feedsByMacAddress.Remove(m);
				});
			}, delegate
			{
				_macByRecorder.Remove(recorderId);
			});
		}
	}

	public bool IsHardwareInChangesWithoutActivation(string mac)
	{
		lock (_lockObject)
		{
			return _changesWithoutActivation.Contains(mac);
		}
	}

	public void AddHardwareToChangesWithoutActivation(string mac)
	{
		lock (_lockObject)
		{
			if (!_changesWithoutActivation.Any((string rc) => rc.StartsWith(mac + "-", StringComparison.OrdinalIgnoreCase)))
			{
				_licenseLogger.LogDebug("Adding MAC to changes without activation: {MacAddress}", mac);
				_changesWithoutActivation.Add(mac);
			}
		}
	}

	public void RemoveHardwareFromChangesWithoutActivation(Collection<string> macAddresses)
	{
		if (macAddresses == null)
		{
			throw new ArgumentNullException("macAddresses");
		}
		lock (_lockObject)
		{
			foreach (string macAddress in macAddresses)
			{
				RemoveHardwareFromChangesWithoutActivation(macAddress);
			}
		}
	}

	public void RemoveHardwareFromChangesWithoutActivation(string macAddress)
	{
		lock (_lockObject)
		{
			_licenseLogger.LogDebug("Removing MAC from changes without activation: {MacAddress}", macAddress);
			_changesWithoutActivation.Remove(macAddress);
		}
	}

	public bool IsDeviceInChangesWithoutActivation(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		lock (_lockObject)
		{
			return _changesWithoutActivation.Contains(hardwareInfo.MacAddress + "-" + deviceInfo.UniqueIdentifier);
		}
	}

	public void RemoveDevicesFromChangesWithoutActivation(Collection<IHardwareDeviceItem> hardwareDeviceItems)
	{
		if (hardwareDeviceItems == null)
		{
			throw new ArgumentNullException("hardwareDeviceItems");
		}
		lock (_lockObject)
		{
			foreach (IHardwareDeviceItem hardwareDeviceItem in hardwareDeviceItems)
			{
				RemoveDeviceFromChangesWithoutActivation(hardwareDeviceItem);
			}
		}
	}

	public void RemoveDevicesFromChangesWithoutActivation(IHardwareInfo hardwareInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		lock (_lockObject)
		{
			foreach (IDeviceInfo cameraDevice in hardwareInfo.CameraDevices)
			{
				if (DeviceStatus.Remove(cameraDevice.Id))
				{
					_changesWithoutActivation.Remove(hardwareInfo.MacAddress + "-" + cameraDevice.UniqueIdentifier);
				}
			}
		}
	}

	public void RemoveDeviceFromChangesWithoutActivation(IHardwareDeviceItem hardwareDeviceItem)
	{
		if (hardwareDeviceItem == null)
		{
			throw new ArgumentNullException("hardwareDeviceItem");
		}
		lock (_lockObject)
		{
			_changesWithoutActivation.Remove(hardwareDeviceItem.MacAddress + "-" + hardwareDeviceItem.DeviceId);
		}
	}

	public void AddDeviceToChangesWithoutActivation(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		if (deviceInfo == null)
		{
			throw new ArgumentNullException("deviceInfo");
		}
		lock (_lockObject)
		{
			EnsureHardwareRemovedFromChangesWithoutActivation(hardwareInfo.MacAddress);
			_changesWithoutActivation.Add(hardwareInfo.MacAddress + "-" + deviceInfo.UniqueIdentifier);
		}
	}

	public void ClearChangesWithoutActivation(ITemporaryChangesWithoutActivation temporaryChangesWithoutActivation, DateTime licensedActivatedDate)
	{
		if (temporaryChangesWithoutActivation == null)
		{
			throw new ArgumentNullException("temporaryChangesWithoutActivation");
		}
		lock (_lockObject)
		{
			_licenseLogger.LogDebug("Clearing all replacement limits");
			_changesWithoutActivation.Clear();
			HashSet<string> hashSet = temporaryChangesWithoutActivation.LoadTemporaryHardwareChangesWithoutActivationIdentifiers(licensedActivatedDate);
			foreach (string item in hashSet)
			{
				_changesWithoutActivation.Add(item);
			}
			hashSet = temporaryChangesWithoutActivation.LoadTemporaryDeviceChangesWithoutActivationIdentifiers(licensedActivatedDate);
			foreach (string item2 in hashSet)
			{
				_changesWithoutActivation.Add(item2);
			}
		}
	}

	public Collection<string> FindNonIcpHardwareInGrace(Guid recorderId)
	{
		throw new NotImplementedException();
	}

	public Collection<string> FindActiveHardwareInChangesWithoutActivation(Guid recorderId)
	{
		lock (_lockObject)
		{
			IEnumerable<string> source = _changesWithoutActivation.Intersect(from h in _hardwareByMacAddress.Values
				where h.RecorderId != recorderId && !h.Icp && !h.IsFreeLicense
				select h.Mac);
			return new Collection<string>(source.ToList());
		}
	}

	public void ValidateRelatedRecordingServerLicenses(ILicenseManager licenseManager, ILicenseSnapshot licenseSnapshot, Guid currentRecorderId)
	{
		if (licenseManager == null)
		{
			throw new ArgumentNullException("licenseManager");
		}
		if (licenseSnapshot == null)
		{
			throw new ArgumentNullException("licenseSnapshot");
		}
		lock (_lockObject)
		{
			int usedChangesWithoutActivation = licenseManager.CountManager.UsedChangesWithoutActivation;
			_licenseLogger.LogDebug("LicenseManager hardwareneedingreevaluation usedChangesWithoutActivation: {UsedChangesWithoutActivation}", usedChangesWithoutActivation);
			_licenseLogger.LogDebug("LicenseManager hardwareneedingreevaluation _totalAllowedChangesWithoutActivation: {TotalAllowedChangesWithoutActivation}", licenseManager.TotalAllowedChangesWithoutActivation);
			Collection<string> hardwareNeedingReevaluation = new Collection<string>();
			if (usedChangesWithoutActivation > licenseManager.TotalAllowedChangesWithoutActivation)
			{
				Collection<string> collection = licenseManager.CountManager.FindActiveHardwareInChangesWithoutActivation(currentRecorderId);
				if (collection.Count > 0)
				{
					hardwareNeedingReevaluation.AddRange(collection);
				}
			}
			else if (licenseManager.TotalAllowedChangesWithoutActivation > usedChangesWithoutActivation)
			{
				List<IHardwareInfo> list = licenseSnapshot.FindHardwareInGrace(currentRecorderId, isIcp: false).ToList();
				list.ForEach(delegate(IHardwareInfo h)
				{
					hardwareNeedingReevaluation.Add(h.MacAddress);
				});
			}
			if (hardwareNeedingReevaluation.Count <= 0)
			{
				return;
			}
			_licenseLogger.LogDebug("LicenseManager hardwareneedingreevaluation start, count: {HardwareNeedingReevaluation}", hardwareNeedingReevaluation.Count);
			foreach (string item in hardwareNeedingReevaluation)
			{
				foreach (Guid relatedRecordingServer in licenseManager.CountManager.GetRelatedRecordingServers(item))
				{
					if (!licenseManager.RecordersRequiringUpdate.ContainsKey(relatedRecordingServer))
					{
						IRecorderInfo recorder = licenseSnapshot.GetRecorder(relatedRecordingServer);
						if (recorder != null)
						{
							byte[] recorderLicense = licenseManager.GetRecorderLicense(recorder.Id, licenseSnapshot, acquireLicenseForCurrentlyUnlicensedItems: true);
							licenseManager.RecordersRequiringUpdate[relatedRecordingServer] = recorderLicense;
						}
					}
				}
			}
			_licenseLogger.LogDebug("LicenseManager hardwareneedingreevaluation stop");
		}
	}

	private void EnsureHardwareRemovedFromChangesWithoutActivation(string macAddress)
	{
		if (IsHardwareInChangesWithoutActivation(macAddress))
		{
			RemoveHardwareFromChangesWithoutActivation(macAddress);
		}
	}

	private void EnsureRecordingServerRegistered(Guid recorderId)
	{
		if (!_recorderHardware.ContainsKey(recorderId))
		{
			_recorderHardware.Add(recorderId, new List<LicenseHardwareDevice>());
		}
		if (!_feedsByRecorder.ContainsKey(recorderId))
		{
			_feedsByRecorder.Add(recorderId, new List<LicenseCameraFeed>());
		}
		if (!_macByRecorder.ContainsKey(recorderId))
		{
			_macByRecorder.Add(recorderId, new HashSet<string>());
		}
	}

	private void EnsureMacAddressFeedRegistered(string macAddress)
	{
		if (!_feedsByMacAddress.ContainsKey(macAddress))
		{
			_feedsByMacAddress.Add(macAddress, new List<LicenseCameraFeed>());
		}
	}
}
