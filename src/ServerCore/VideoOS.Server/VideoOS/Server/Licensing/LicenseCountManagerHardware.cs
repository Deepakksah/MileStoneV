using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Extensions;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.Licensing;

public sealed class LicenseCountManagerHardware : ILicenseCountManager
{
	private readonly LinkedList<LicenseHardwareDevice> _licensedHardware;

	private readonly LinkedListIndex<string, LicenseHardwareDevice> _licensedHardwareByMacAddress;

	private readonly LinkedListIndex<Guid, LicenseHardwareDevice> _licensedHardwareByRecorder;

	private readonly HashSet<string> _replacementHardware;

	private readonly LinkedList<LicenseCameraFeed> _licensedFeeds;

	private readonly LinkedListIndex<string, LicenseCameraFeed> _licensedFeedsByMacAddress;

	private readonly LinkedListIndex<Guid, LicenseCameraFeed> _licensedFeedsByRecorder;

	private readonly LinkedList<RecorderHardwareRelation> _registeredRelations;

	private readonly LinkedListIndex<string, RecorderHardwareRelation> _registeredRelationsByMacAddress;

	private readonly LinkedListIndex<Guid, RecorderHardwareRelation> _registeredRelationsByRecorder;

	private readonly ILogger _licenseLogger = Telemetry.GetILogger("Licensing");

	public int UsedChangesWithoutActivation => _replacementHardware.Count;

	public Dictionary<Guid, DeviceInfoLicenseState> DeviceStatus { get; private set; }

	public LicenseCountManagerHardware()
	{
		_licensedHardware = new LinkedList<LicenseHardwareDevice>();
		_licensedHardwareByMacAddress = new LinkedListIndex<string, LicenseHardwareDevice>((LicenseHardwareDevice h) => h.Mac, StringComparer.OrdinalIgnoreCase);
		_licensedHardwareByRecorder = new LinkedListIndex<Guid, LicenseHardwareDevice>((LicenseHardwareDevice h) => h.RecorderId);
		_replacementHardware = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		_licensedFeeds = new LinkedList<LicenseCameraFeed>();
		_licensedFeedsByMacAddress = new LinkedListIndex<string, LicenseCameraFeed>((LicenseCameraFeed f) => f.HardwareInfo.MacAddress, StringComparer.OrdinalIgnoreCase);
		_licensedFeedsByRecorder = new LinkedListIndex<Guid, LicenseCameraFeed>((LicenseCameraFeed f) => f.RecorderId);
		_registeredRelations = new LinkedList<RecorderHardwareRelation>();
		_registeredRelationsByMacAddress = new LinkedListIndex<string, RecorderHardwareRelation>((RecorderHardwareRelation r) => r.Mac, StringComparer.OrdinalIgnoreCase);
		_registeredRelationsByRecorder = new LinkedListIndex<Guid, RecorderHardwareRelation>((RecorderHardwareRelation r) => r.RecorderId);
		DeviceStatus = new Dictionary<Guid, DeviceInfoLicenseState>();
		_licenseLogger = Telemetry.GetILogger("Licensing");
	}

	public int CountOfLicensesUsed(bool isInterconnected)
	{
		return _licensedHardware.Count;
	}

	public void AddLicensedHardware(IHardwareInfo hardwareInfo, Guid recorderId, bool isFreeLicense)
	{
		if (hardwareInfo == null)
		{
			throw new ArgumentNullException("hardwareInfo");
		}
		IEnumerable<LinkedListNode<LicenseHardwareDevice>> itemsByKey = _licensedHardwareByMacAddress.GetItemsByKey(hardwareInfo.MacAddress);
		if (itemsByKey.Any())
		{
			throw new ArgumentException("An entry is already registered for the mac address");
		}
		LinkedListNode<LicenseHardwareDevice> item = _licensedHardware.AddLast(new LicenseHardwareDevice(hardwareInfo.MacAddress, hardwareInfo.IsInterconnectedProduct, recorderId, isFreeLicense));
		_licensedHardwareByMacAddress.AddItem(item);
		_licensedHardwareByRecorder.AddItem(item);
	}

	public void UpdateHardwareLicenseValue(string mac, long licenseValue)
	{
		LinkedListNode<LicenseHardwareDevice> linkedListNode = _licensedHardwareByMacAddress.GetItemsByKey(mac).Single();
		linkedListNode.Value.LicensedValue = licenseValue;
	}

	public long GetHardwareLicenseValue(string mac)
	{
		LicenseHardwareDevice licenseHardwareDevice = (from e in _licensedHardwareByMacAddress.GetItemsByKey(mac)
			select e.Value).Single();
		return licenseHardwareDevice.LicensedValue;
	}

	public bool IsHardwareLicensedOnSite(string mac)
	{
		IEnumerable<LinkedListNode<LicenseHardwareDevice>> itemsByKey = _licensedHardwareByMacAddress.GetItemsByKey(mac);
		return itemsByKey.Any();
	}

	public bool IsHardwareLicensedOnRecorder(string mac, Guid recorderId)
	{
		IEnumerable<LinkedListNode<LicenseHardwareDevice>> itemsByKey = _licensedHardwareByMacAddress.GetItemsByKey(mac);
		return itemsByKey.Any((LinkedListNode<LicenseHardwareDevice> h) => h.Value.RecorderId == recorderId);
	}

	public bool IsHardwareWithoutEnabledCamerasLicensed(IHardwareInfo hardwareInfo)
	{
		throw new InvalidOperationException();
	}

	public int GetHardwareUsedOnSite(bool icp)
	{
		return _licensedHardware.Where((LicenseHardwareDevice e) => e.Icp == icp).Count();
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
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByMacAddress.GetItemsByKey(hardwareInfo.MacAddress);
		if (itemsByKey.Any((LinkedListNode<LicenseCameraFeed> f) => f.Value.DeviceInfo.OriginalDeviceId == deviceInfo.OriginalDeviceId))
		{
			throw new ArgumentException("An entry is already registered for the mac address and device index.", "deviceInfo");
		}
		LinkedListNode<LicenseCameraFeed> item = _licensedFeeds.AddLast(new LicenseCameraFeed(hardwareInfo, deviceInfo, recorderInfo.Id, hardwareInfo.IsInterconnectedProduct, feedCount, licenseValue));
		_licensedFeedsByMacAddress.AddItem(item);
		_licensedFeedsByRecorder.AddItem(item);
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
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByMacAddress.GetItemsByKey(hardwareInfo.MacAddress);
		LicenseCameraFeed licenseCameraFeed = itemsByKey.Select((LinkedListNode<LicenseCameraFeed> e) => e.Value).Single((LicenseCameraFeed f) => f.DeviceInfo.OriginalDeviceId == deviceInfo.OriginalDeviceId);
		licenseCameraFeed.LicensedValue = licenseValue;
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
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByMacAddress.GetItemsByKey(hardwareInfo.MacAddress);
		LicenseCameraFeed licenseCameraFeed = itemsByKey.Select((LinkedListNode<LicenseCameraFeed> e) => e.Value).Single((LicenseCameraFeed f) => f.DeviceInfo.OriginalDeviceId == deviceInfo.OriginalDeviceId);
		return licenseCameraFeed.LicensedValue;
	}

	public Guid GetFirstCameraFeedDeviceId(string mac)
	{
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByMacAddress.GetItemsByKey(mac);
		LicenseCameraFeed licenseCameraFeed = itemsByKey.Select((LinkedListNode<LicenseCameraFeed> e) => e.Value).First();
		return licenseCameraFeed.DeviceInfo.OriginalDeviceId;
	}

	public bool IsAnyCameraFeedLicensedForHardware(string mac)
	{
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByMacAddress.GetItemsByKey(mac);
		return itemsByKey.Any();
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
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByMacAddress.GetItemsByKey(hardwareInfo.MacAddress);
		return itemsByKey.Any((LinkedListNode<LicenseCameraFeed> f) => f.Value.DeviceInfo.OriginalDeviceId == deviceInfo.OriginalDeviceId);
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
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByMacAddress.GetItemsByKey(hardwareInfo.MacAddress);
		return itemsByKey.Any((LinkedListNode<LicenseCameraFeed> f) => f.Value.DeviceInfo.OriginalDeviceId == deviceInfo.OriginalDeviceId && f.Value.RecorderId == recorderId);
	}

	public int GetCameraFeedsUsedOnSite(bool icp)
	{
		return _licensedFeeds.Where((LicenseCameraFeed e) => e.Icp == icp).Sum((LicenseCameraFeed e) => e.FeedCount);
	}

	public int GetCameraFeedsUsedByRecorder(Guid recorderId, bool icp)
	{
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByRecorder.GetItemsByKey(recorderId);
		return itemsByKey.Where((LinkedListNode<LicenseCameraFeed> e) => e.Value.Icp == icp).Sum((LinkedListNode<LicenseCameraFeed> e) => e.Value.FeedCount);
	}

	public int GetCameraFeedsUsedByHardware(string mac)
	{
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey = _licensedFeedsByMacAddress.GetItemsByKey(mac);
		return itemsByKey.Sum((LinkedListNode<LicenseCameraFeed> e) => e.Value.FeedCount);
	}

	public void RegisterHardwareRecorderRelation(Guid recorderId, string mac)
	{
		IEnumerable<LinkedListNode<RecorderHardwareRelation>> itemsByKey = _registeredRelationsByMacAddress.GetItemsByKey(mac);
		if (!itemsByKey.Any((LinkedListNode<RecorderHardwareRelation> r) => r.Value.RecorderId == recorderId))
		{
			LinkedListNode<RecorderHardwareRelation> item = _registeredRelations.AddLast(new RecorderHardwareRelation(mac, recorderId));
			_registeredRelationsByMacAddress.AddItem(item);
			_registeredRelationsByRecorder.AddItem(item);
		}
	}

	public IEnumerable<Guid> GetRelatedRecordingServers(string mac)
	{
		return (from e in _registeredRelationsByMacAddress.GetItemsByKey(mac)
			select e.Value.RecorderId).ToList();
	}

	public void ClearByRecorder(Guid recorderId)
	{
		IEnumerable<LinkedListNode<LicenseHardwareDevice>> itemsByKey = _licensedHardwareByRecorder.GetItemsByKey(recorderId);
		foreach (LinkedListNode<LicenseHardwareDevice> item in itemsByKey)
		{
			_licensedHardwareByMacAddress.RemoveItem(item);
			_licensedHardware.Remove(item);
		}
		_licensedHardwareByRecorder.RemoveKey(recorderId);
		IEnumerable<LinkedListNode<LicenseCameraFeed>> itemsByKey2 = _licensedFeedsByRecorder.GetItemsByKey(recorderId);
		foreach (LinkedListNode<LicenseCameraFeed> item2 in itemsByKey2)
		{
			_licensedFeedsByMacAddress.RemoveItem(item2);
			_licensedFeeds.Remove(item2);
		}
		_licensedFeedsByRecorder.RemoveKey(recorderId);
		IEnumerable<LinkedListNode<RecorderHardwareRelation>> itemsByKey3 = _registeredRelationsByRecorder.GetItemsByKey(recorderId);
		foreach (LinkedListNode<RecorderHardwareRelation> item3 in itemsByKey3)
		{
			_registeredRelationsByMacAddress.RemoveItem(item3);
			_registeredRelations.Remove(item3);
		}
		_registeredRelationsByRecorder.RemoveKey(recorderId);
	}

	public bool IsHardwareInChangesWithoutActivation(string mac)
	{
		return _replacementHardware.Contains(mac);
	}

	public void AddHardwareToChangesWithoutActivation(string mac)
	{
		_replacementHardware.Add(mac);
	}

	public void RemoveHardwareFromChangesWithoutActivation(Collection<string> macAddresses)
	{
		if (macAddresses == null)
		{
			throw new ArgumentNullException("macAddresses");
		}
		foreach (string macAddress in macAddresses)
		{
			RemoveHardwareFromChangesWithoutActivation(macAddress);
		}
	}

	public void RemoveHardwareFromChangesWithoutActivation(string macAddress)
	{
		if (macAddress == null)
		{
			throw new ArgumentNullException("macAddress");
		}
		_replacementHardware.Remove(macAddress);
	}

	public bool IsDeviceInChangesWithoutActivation(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		throw new InvalidOperationException();
	}

	public void RemoveDeviceFromChangesWithoutActivation(IHardwareDeviceItem hardwareDeviceItem)
	{
		throw new InvalidOperationException();
	}

	public void RemoveDevicesFromChangesWithoutActivation(Collection<IHardwareDeviceItem> hardwareDeviceItems)
	{
		throw new InvalidOperationException();
	}

	public void RemoveDevicesFromChangesWithoutActivation(IHardwareInfo hardwareInfo)
	{
		throw new InvalidOperationException();
	}

	public void AddDeviceToChangesWithoutActivation(IHardwareInfo hardwareInfo, IDeviceInfo deviceInfo)
	{
		throw new InvalidOperationException();
	}

	public void ClearChangesWithoutActivation(ITemporaryChangesWithoutActivation temporaryChangesWithoutActivation, DateTime licensedActivatedDate)
	{
		if (temporaryChangesWithoutActivation == null)
		{
			throw new ArgumentNullException("temporaryChangesWithoutActivation");
		}
		_replacementHardware.Clear();
		HashSet<string> hashSet = temporaryChangesWithoutActivation.LoadTemporaryHardwareChangesWithoutActivationIdentifiers(licensedActivatedDate);
		foreach (string item in hashSet)
		{
			_replacementHardware.Add(item);
		}
	}

	public Collection<string> FindNonIcpHardwareInGrace(Guid recorderId)
	{
		IEnumerable<string> source = (from h in _licensedHardware
			where h.LicensedValue < DateTime.MaxValue.Ticks && h.RecorderId != recorderId && !h.Icp
			select h.Mac).Except(_replacementHardware);
		return new Collection<string>(source.ToList());
	}

	public Collection<string> FindActiveHardwareInChangesWithoutActivation(Guid recorderId)
	{
		IEnumerable<string> source = _replacementHardware.Intersect(from h in _licensedHardware
			where h.RecorderId != recorderId && !h.Icp
			select h.Mac);
		return new Collection<string>(source.ToList());
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
		bool flag = true;
		do
		{
			Collection<string> collection = new Collection<string>();
			int usedChangesWithoutActivation = licenseManager.CountManager.UsedChangesWithoutActivation;
			_licenseLogger.LogDebug("LicenseManager hardwareneedingreevaluation usedChangesWithoutActivation: {UsedChangesWithoutActivation}", usedChangesWithoutActivation);
			_licenseLogger.LogDebug("LicenseManager hardwareneedingreevaluation _totalAllowedChangesWithoutActivation: {TotalAllowedChangesWithoutActivation}", licenseManager.TotalAllowedChangesWithoutActivation);
			if (usedChangesWithoutActivation > licenseManager.TotalAllowedChangesWithoutActivation)
			{
				Collection<string> collection2 = licenseManager.CountManager.FindActiveHardwareInChangesWithoutActivation(currentRecorderId);
				if (collection2.Count > 0)
				{
					collection.AddRange(collection2);
				}
			}
			else if (licenseManager.TotalAllowedChangesWithoutActivation > usedChangesWithoutActivation)
			{
				Collection<string> collection3 = licenseManager.CountManager.FindNonIcpHardwareInGrace(currentRecorderId);
				if (collection3.Count > 0)
				{
					collection.AddRange(collection3);
				}
			}
			if (collection.Count != 0)
			{
				_licenseLogger.LogDebug("LicenseManager hardwareneedingreevaluation start, count: {HardwareNeedingReevaluation}", collection.Count);
				string mac = collection[0];
				foreach (Guid relatedRecordingServer in licenseManager.CountManager.GetRelatedRecordingServers(mac))
				{
					IRecorderInfo recorder = licenseSnapshot.GetRecorder(relatedRecordingServer);
					if (recorder != null)
					{
						byte[] recorderLicense = licenseManager.GetRecorderLicense(recorder.Id, licenseSnapshot, acquireLicenseForCurrentlyUnlicensedItems: true);
						if (licenseManager.RecordersRequiringUpdate.ContainsKey(relatedRecordingServer))
						{
							licenseManager.RecordersRequiringUpdate[relatedRecordingServer] = recorderLicense;
						}
						else
						{
							licenseManager.RecordersRequiringUpdate.Add(relatedRecordingServer, recorderLicense);
						}
					}
				}
				_licenseLogger.LogDebug("LicenseManager hardwareneedingreevaluation stop");
			}
			else
			{
				flag = false;
			}
		}
		while (flag);
	}
}
