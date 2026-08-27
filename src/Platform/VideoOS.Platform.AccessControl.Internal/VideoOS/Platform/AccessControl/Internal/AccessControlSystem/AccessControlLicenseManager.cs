using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using VideoOS.Platform.AccessControl.Internal.AccessControlElements;
using VideoOS.Platform.AccessControl.Internal.Util;
using VideoOS.Platform.License;
using VideoOS.Platform.Messaging;

namespace VideoOS.Platform.AccessControl.Internal.AccessControlSystem;

public class AccessControlLicenseManager
{
	private class LicenseItemIdComparer : IEqualityComparer<LicenseItem>
	{
		public bool Equals(LicenseItem x, LicenseItem y)
		{
			if (x != null || y != null)
			{
				if (x != null && y != null)
				{
					return x.Id == y.Id;
				}
				return false;
			}
			return true;
		}

		public int GetHashCode(LicenseItem obj)
		{
			return obj?.Id.GetHashCode() ?? 0;
		}
	}

	private const string _accessControlLicenseType = "XPAC";

	private static Guid _accessControlLicenseId = new Guid("BB529DE8-608E-4EBB-B796-889504E54670");

	private const int _xpeTrialLength = 30;

	private const int _xpeTrialLicenseCount = 3;

	private LicenseInformation _licenseInfo;

	private bool _dirty;

	private object _licenseChangeSubscription;

	private ReaderWriterLockSlim _licenseLock = new ReaderWriterLockSlim();

	private LicenseItemIdComparer _licenseItemIdComparer = new LicenseItemIdComparer();

	private Dictionary<Guid, AccessControlSystemManager> _systemManagers = new Dictionary<Guid, AccessControlSystemManager>();

	private Dictionary<Guid, LicenseItem> _licenseItemCache = new Dictionary<Guid, LicenseItem>();

	private AccessControlOperationableInstance[] _configInstances = new AccessControlOperationableInstance[0];

	private AccessControlUnit[] _configDoors = new AccessControlUnit[0];

	private bool IsCorporateSku => EnvironmentManager.Instance.MasterSite.ServerId.ServerType == "XPCO";

	private int LicenseCount
	{
		get
		{
			if (_licenseInfo != null)
			{
				return _licenseInfo.Counter;
			}
			if (!IsCorporateSku)
			{
				return 3;
			}
			return 0;
		}
	}

	public LicenseInformation LicenseInfo
	{
		get
		{
			_licenseLock.EnterReadLock();
			try
			{
				LicenseInformation licenseInformation;
				Dictionary<string, LicenseItem> dictionary;
				if (_licenseInfo != null)
				{
					licenseInformation = _licenseInfo.Clone();
					dictionary = licenseInformation.ItemIdentifications.Distinct(_licenseItemIdComparer).ToDictionary((LicenseItem li) => li.Id);
					licenseInformation.ItemIdentifications.Clear();
					licenseInformation.CounterMethod = LicenseInformation.CounterMethodEnum.CountByItemIdentifications;
				}
				else
				{
					licenseInformation = new LicenseInformation
					{
						PluginId = _accessControlLicenseId,
						LicenseType = "XPAC",
						CounterMethod = LicenseInformation.CounterMethodEnum.CountByItemIdentifications,
						CustomData = ""
					};
					dictionary = new Dictionary<string, LicenseItem>();
				}
				HashSet<string> requestedLicenseIds = new HashSet<string>();
				foreach (AccessControlUnit item in _configDoors.Where((AccessControlUnit d) => d.Enabled && d.AccessControlSystemManager.Enabled && !requestedLicenseIds.Contains(d.Id.ToString())))
				{
					if (!dictionary.TryGetValue(item.Id.ToString(), out var value))
					{
						value = new LicenseItem
						{
							Id = item.Id.ToString()
						};
					}
					licenseInformation.ItemIdentifications.Add(value);
					requestedLicenseIds.Add(item.Id.ToString());
				}
				return licenseInformation;
			}
			finally
			{
				_licenseLock.ExitReadLock();
			}
		}
	}

	public bool LicenseValid
	{
		get
		{
			_licenseLock.EnterReadLock();
			try
			{
				return _licenseInfo != null && (!_licenseInfo.TrialMode || _licenseInfo.Expire > DateTime.UtcNow);
			}
			finally
			{
				_licenseLock.ExitReadLock();
			}
		}
	}

	public DateTime LicenseExpiration
	{
		get
		{
			_licenseLock.EnterReadLock();
			try
			{
				return (_licenseInfo == null) ? DateTime.MinValue : (_licenseInfo.TrialMode ? _licenseInfo.Expire : DateTime.MaxValue);
			}
			finally
			{
				_licenseLock.ExitReadLock();
			}
		}
	}

	public event EventHandler LicenseUpdated;

	public bool ItemLicensed(Guid itemId)
	{
		_licenseLock.EnterReadLock();
		try
		{
			LicenseItem value;
			return (_licenseInfo == null || !_licenseInfo.TrialMode || _licenseInfo.Expire > DateTime.UtcNow) && _licenseItemCache.TryGetValue(itemId, out value) && (!value.ItemTrial || value.ItemTrialEnd > DateTime.UtcNow);
		}
		finally
		{
			_licenseLock.ExitReadLock();
		}
	}

	public DateTime ItemLicenseExpiration(Guid itemId)
	{
		_licenseLock.EnterReadLock();
		try
		{
			if (_licenseItemCache.TryGetValue(itemId, out var value))
			{
				return value.ItemTrial ? value.ItemTrialEnd : DateTime.MaxValue;
			}
			return DateTime.MinValue;
		}
		finally
		{
			_licenseLock.ExitReadLock();
		}
	}

	public DateTime NextLicenseExpiration(DateTime time)
	{
		_licenseLock.EnterReadLock();
		try
		{
			DateTime dateTime = ((_licenseInfo != null && _licenseInfo.TrialMode) ? _licenseInfo.Expire : DateTime.MaxValue);
			if (dateTime < time)
			{
				return DateTime.MaxValue;
			}
			DateTime dateTime2 = DateTime.MaxValue;
			IEnumerable<LicenseItem> source = _licenseItemCache.Values.Where((LicenseItem li) => li.ItemTrial && li.ItemTrialEnd >= time);
			if (source.Any())
			{
				dateTime2 = source.Min((LicenseItem li) => li.ItemTrialEnd);
			}
			return (dateTime2 < dateTime) ? dateTime2 : dateTime;
		}
		finally
		{
			_licenseLock.ExitReadLock();
		}
	}

	public void Init()
	{
		if (_licenseChangeSubscription == null)
		{
			_licenseChangeSubscription = EnvironmentManager.Instance.RegisterReceiver(LicenseChangedReceiver, new MessageIdFilter(MessageId.System.LicenseChangedIndication));
		}
		_licenseLock.EnterWriteLock();
		bool flag;
		try
		{
			LoadLicense(preserveChanges: false);
			UpdateConfigurationCache();
			flag = UpdateLicenseCache(createTempLicense: false);
		}
		finally
		{
			_licenseLock.ExitWriteLock();
		}
		if (flag)
		{
			FireLicenseUpdateEvent();
		}
	}

	public void Close()
	{
		if (_licenseChangeSubscription != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_licenseChangeSubscription);
			_licenseChangeSubscription = null;
		}
	}

	public void RegisterSystemManager(AccessControlSystemManager systemManager)
	{
		_licenseLock.EnterWriteLock();
		bool flag;
		try
		{
			_systemManagers[systemManager.Id] = systemManager;
			systemManager.AccessControlLicenseManager = this;
			UpdateConfigurationCache();
			flag = UpdateLicenseCache(createTempLicense: false);
		}
		finally
		{
			_licenseLock.ExitWriteLock();
		}
		if (flag)
		{
			FireLicenseUpdateEvent();
		}
	}

	public void UnregisterSystemManager(Guid systemManagerId)
	{
		_licenseLock.EnterWriteLock();
		bool flag;
		try
		{
			_systemManagers.Remove(systemManagerId);
			UpdateConfigurationCache();
			FreeLicenses();
			flag = UpdateLicenseCache(createTempLicense: false);
		}
		finally
		{
			_licenseLock.ExitWriteLock();
		}
		if (flag)
		{
			FireLicenseUpdateEvent();
		}
	}

	private void LoadLicense(bool preserveChanges)
	{
		if (EnvironmentManager.Instance.Mode == Mode.ServerSetup)
		{
			LicenseInformation license = EnvironmentManager.Instance.LicenseManager.GetLicense(_accessControlLicenseId, "XPAC");
			LicenseInformation licenseInformation = (preserveChanges ? _licenseInfo : EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.GetLicenseInformation(_accessControlLicenseId, "XPAC"));
			if (license != null)
			{
				_licenseInfo = license.Clone();
				if (IsCorporateSku)
				{
					_licenseInfo.ItemIdentifications = new Collection<LicenseItem>(_licenseInfo.ItemIdentifications.Distinct(_licenseItemIdComparer).ToList());
					if (licenseInformation != null)
					{
						foreach (LicenseItem item in licenseInformation.Clone().ItemIdentifications.Distinct(_licenseItemIdComparer))
						{
							if (item.ItemTrial && !_licenseInfo.ItemIdentifications.Any((LicenseItem li) => li.Id == item.Id))
							{
								_licenseInfo.ItemIdentifications.Add(item);
							}
						}
					}
				}
				else
				{
					_licenseInfo.ItemIdentifications.Clear();
					if (licenseInformation != null)
					{
						foreach (LicenseItem item2 in licenseInformation.Clone().ItemIdentifications.Distinct(_licenseItemIdComparer))
						{
							_licenseInfo.ItemIdentifications.Add(item2);
						}
					}
				}
				_dirty &= preserveChanges;
				if (!_dirty && LicenseInfoChanged(_licenseInfo, licenseInformation))
				{
					_dirty = true;
				}
			}
			else if (licenseInformation != null)
			{
				_licenseInfo = licenseInformation.Clone();
				_licenseInfo.ItemIdentifications = new Collection<LicenseItem>(_licenseInfo.ItemIdentifications.Distinct(_licenseItemIdComparer).ToList());
				if (!_licenseInfo.TrialMode && _licenseInfo.Counter > 0)
				{
					_licenseInfo.Counter = 0;
					_dirty = true;
				}
				else
				{
					_dirty &= preserveChanges;
				}
			}
		}
		else
		{
			LicenseInformation licenseInformation2 = EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.GetLicenseInformation(_accessControlLicenseId, "XPAC");
			if (licenseInformation2 != null)
			{
				_licenseInfo = licenseInformation2.Clone();
				_licenseInfo.ItemIdentifications = new Collection<LicenseItem>(_licenseInfo.ItemIdentifications.Distinct(_licenseItemIdComparer).ToList());
				_dirty = false;
			}
		}
	}

	private static bool LicenseInfoChanged(LicenseInformation lic1, LicenseInformation lic2)
	{
		if (lic1 != lic2)
		{
			if (lic1 != null && lic2 != null && !(lic1.PluginId != lic2.PluginId) && !(lic1.LicenseType != lic2.LicenseType) && lic1.Counter == lic2.Counter && lic1.ItemIdentifications.Count == lic2.ItemIdentifications.Count && lic1.TrialMode == lic2.TrialMode && !(lic1.Expire != lic2.Expire))
			{
				return lic1.ItemIdentifications.Any(delegate(LicenseItem item1)
				{
					LicenseItem licenseItem = lic2.ItemIdentifications.FirstOrDefault((LicenseItem i) => i.Id == item1.Id);
					return licenseItem == null || item1.ItemTrial != licenseItem.ItemTrial || item1.ItemTrialEnd.Date != licenseItem.ItemTrialEnd.Date;
				});
			}
			return true;
		}
		return false;
	}

	public int AvailableLicenses(AccessControlSystemManager systemManager)
	{
		_licenseLock.EnterReadLock();
		try
		{
			if (systemManager == null)
			{
				return 0;
			}
			IEnumerable<AccessControlSystemManager> accessControlSystemManagers = _systemManagers.Values.Where((AccessControlSystemManager s) => s.Enabled && s.Id != systemManager.Id);
			int num = DoorHelper.GetDoorUnits(systemManager).Concat(DoorHelper.GetDoorUnits(accessControlSystemManagers)).Count((AccessControlUnit d) => d.Enabled);
			return LicenseCount - num;
		}
		finally
		{
			_licenseLock.ExitReadLock();
		}
	}

	public void SaveLicense()
	{
		if (EnvironmentManager.Instance.Mode != Mode.ServerSetup)
		{
			throw new ImplementationMIPException("Only supported in ServerSetup mode");
		}
		_licenseLock.EnterReadLock();
		try
		{
			SaveLicenseInternal();
		}
		finally
		{
			_licenseLock.ExitReadLock();
		}
	}

	private void SaveLicenseInternal()
	{
		if (EnvironmentManager.Instance.Mode == Mode.ServerSetup && _dirty && _licenseInfo != null)
		{
			VideoOS.Platform.Configuration.Instance.BeginMassUpdate();
			EnvironmentManager.Instance.LicenseManager.ReservedLicenseManager.SaveLicenseInformation(_licenseInfo);
			VideoOS.Platform.Configuration.Instance.EndMassUpdate();
			_dirty = false;
		}
	}

	public void UpdateLicense(AccessControlSystemManager systemManager)
	{
		_licenseLock.EnterWriteLock();
		bool flag;
		try
		{
			AutoLicenseConfiguration(systemManager);
			flag = UpdateLicenseCache(createTempLicense: false);
		}
		finally
		{
			_licenseLock.ExitWriteLock();
		}
		if (flag)
		{
			FireLicenseUpdateEvent();
		}
	}

	private void AutoLicenseConfiguration(AccessControlSystemManager systemManager)
	{
		if (EnvironmentManager.Instance.Mode != Mode.ServerSetup)
		{
			return;
		}
		IEnumerable<AccessControlSystemManager> accessControlSystemManagers = ((systemManager == null) ? _systemManagers.Values : _systemManagers.Values.Where((AccessControlSystemManager s) => s.Id != systemManager.Id).Concat(new AccessControlSystemManager[1] { systemManager }));
		IEnumerable<AccessControlUnit> doorUnits = DoorHelper.GetDoorUnits(accessControlSystemManagers);
		if (_licenseInfo == null && doorUnits.Any() && !IsCorporateSku)
		{
			_licenseInfo = new LicenseInformation
			{
				PluginId = _accessControlLicenseId,
				LicenseType = "XPAC",
				Counter = 3,
				CounterMethod = LicenseInformation.CounterMethodEnum.CountByItemIdentifications,
				CustomData = "",
				TrialMode = true,
				Expire = DateTime.UtcNow + TimeSpan.FromDays(30.0)
			};
		}
		if (_licenseInfo == null)
		{
			return;
		}
		Dictionary<string, LicenseItem> doorLicenses = _licenseInfo.ItemIdentifications.Distinct(_licenseItemIdComparer).ToDictionary((LicenseItem li) => li.Id);
		if (!IsCorporateSku)
		{
			foreach (AccessControlUnit item in doorUnits)
			{
				if ((!item.Enabled || !item.AccessControlSystemManager.Enabled) && doorLicenses.TryGetValue(item.Id.ToString(), out var value))
				{
					_licenseInfo.ItemIdentifications.Remove(value);
					doorLicenses.Remove(item.Id.ToString());
					_dirty = true;
				}
			}
		}
		int num = _licenseInfo.Counter - doorUnits.Count((AccessControlUnit d) => d.Enabled && d.AccessControlSystemManager.Enabled && doorLicenses.ContainsKey(d.Id.ToString()));
		foreach (AccessControlUnit item2 in doorUnits.Where((AccessControlUnit d) => d.Enabled && d.AccessControlSystemManager.Enabled && !doorLicenses.ContainsKey(d.Id.ToString())))
		{
			if (num-- > 0)
			{
				_licenseInfo.ItemIdentifications.Add(CreateItemLicense(item2.Id.ToString(), item2.Name));
				_dirty = true;
			}
		}
	}

	private LicenseItem CreateItemLicense(string id, string name)
	{
		if (IsCorporateSku)
		{
			TimeSpan timeSpan = TimeSpan.FromDays(EnvironmentManager.Instance.SystemLicense.GracePeriod);
			return new LicenseItem
			{
				Id = id,
				Name = name,
				ItemTrial = true,
				ItemTrialEnd = DateTime.UtcNow + timeSpan
			};
		}
		return new LicenseItem
		{
			Id = id,
			Name = name
		};
	}

	private void FreeLicenses()
	{
		if (EnvironmentManager.Instance.Mode != Mode.ServerSetup || IsCorporateSku || _licenseInfo == null)
		{
			return;
		}
		Dictionary<string, LicenseItem> dictionary = _licenseInfo.ItemIdentifications.Distinct(_licenseItemIdComparer).ToDictionary((LicenseItem li) => li.Id);
		AccessControlUnit[] configDoors = _configDoors;
		foreach (AccessControlUnit accessControlUnit in configDoors)
		{
			if ((!accessControlUnit.Enabled || !accessControlUnit.AccessControlSystemManager.Enabled) && dictionary.TryGetValue(accessControlUnit.Id.ToString(), out var value))
			{
				_licenseInfo.ItemIdentifications.Remove(value);
				_dirty = true;
			}
		}
	}

	public void UpdateConfiguration()
	{
		_licenseLock.EnterWriteLock();
		bool flag;
		try
		{
			UpdateConfigurationCache();
			flag = UpdateLicenseCache(createTempLicense: true);
			if (flag && EnvironmentManager.Instance.Mode == Mode.ServerSetup)
			{
				EnvironmentManager.Instance.PostMessage(new VideoOS.Platform.Messaging.Message(MessageId.System.LicenseAutomaticUpdateRequest));
			}
		}
		finally
		{
			_licenseLock.ExitWriteLock();
		}
		if (flag)
		{
			FireLicenseUpdateEvent();
		}
	}

	private void UpdateConfigurationCache()
	{
		_configInstances = _systemManagers.Values.SelectMany((AccessControlSystemManager manager) => manager.GetAccessControlElements(typeof(AccessControlOperationableInstance))).Cast<AccessControlOperationableInstance>().ToArray();
		_configDoors = (from unit in _configInstances.OfType<AccessControlUnit>()
			where unit.AccessControlType.Categories.Intersect(CategoryHandler.CategoryUnitTypesDoor).Any()
			select unit).ToArray();
	}

	private bool UpdateLicenseCache(bool createTempLicense)
	{
		List<KeyValuePair<Guid, LicenseItem>> list = _licenseItemCache.ToList();
		_licenseItemCache.Clear();
		if (_licenseInfo == null)
		{
			return list.Count != 0;
		}
		createTempLicense &= EnvironmentManager.Instance.Mode == Mode.ServerService;
		Dictionary<string, LicenseItem> dictionary = _licenseInfo.ItemIdentifications.Distinct(_licenseItemIdComparer).ToDictionary((LicenseItem li) => li.Id);
		List<AccessControlUnit> list2 = new List<AccessControlUnit>();
		Dictionary<AccessControlUnit, HashSet<AccessControlUnit>> dictionary2 = new Dictionary<AccessControlUnit, HashSet<AccessControlUnit>>();
		foreach (AccessControlUnit item in _configInstances.OfType<AccessControlUnit>())
		{
			AccessControlUnit parent = item.Parent;
			if (parent != null)
			{
				if (dictionary2.TryGetValue(parent, out var value))
				{
					value.Add(item);
					continue;
				}
				value = new HashSet<AccessControlUnit> { item };
				dictionary2.Add(parent, value);
			}
		}
		int num = _licenseInfo.Counter;
		AccessControlUnit[] configDoors = _configDoors;
		foreach (AccessControlUnit accessControlUnit in configDoors)
		{
			LicenseItem value2 = null;
			if (accessControlUnit.Enabled && accessControlUnit.AccessControlSystemManager.Enabled)
			{
				if (num > 0 && dictionary.TryGetValue(accessControlUnit.Id.ToString(), out value2))
				{
					value2.Name = accessControlUnit.Name;
					AddLicenseToCache(accessControlUnit, value2);
					num--;
				}
				else if (createTempLicense)
				{
					value2 = new LicenseItem
					{
						Id = accessControlUnit.Id.ToString(),
						Name = accessControlUnit.Name + " [TEMP]",
						ItemTrial = true,
						ItemTrialEnd = DateTime.MaxValue
					};
					AddLicenseToCache(accessControlUnit, value2);
				}
			}
			HashSet<AccessControlUnit> hashSet = new HashSet<AccessControlUnit> { accessControlUnit };
			while (hashSet.Any())
			{
				list2.AddRange(hashSet);
				HashSet<AccessControlUnit> hashSet2 = new HashSet<AccessControlUnit>();
				foreach (AccessControlUnit item2 in hashSet)
				{
					if (dictionary2.TryGetValue(item2, out var value3))
					{
						hashSet2.UnionWith(value3);
					}
				}
				hashSet = hashSet2;
				if (value2 == null)
				{
					continue;
				}
				foreach (AccessControlUnit item3 in hashSet)
				{
					AddLicenseToCache(item3, value2);
				}
			}
		}
		LicenseItem licenseItem = new LicenseItem
		{
			Id = string.Empty,
			Name = "Non-door"
		};
		foreach (AccessControlOperationableInstance item4 in _configInstances.Except(list2))
		{
			AddLicenseToCache(item4, licenseItem);
		}
		if (list.Count != _licenseItemCache.Count)
		{
			return true;
		}
		foreach (KeyValuePair<Guid, LicenseItem> item5 in list)
		{
			if (!_licenseItemCache.TryGetValue(item5.Key, out var value4) || item5.Value.ItemTrial != value4.ItemTrial || (item5.Value.ItemTrial && item5.Value.ItemTrialEnd != value4.ItemTrialEnd))
			{
				return true;
			}
		}
		return false;
	}

	private void AddLicenseToCache(AccessControlOperationableInstance instance, LicenseItem licenseItem)
	{
		if (!_licenseItemCache.ContainsKey(instance.Id))
		{
			_licenseItemCache.Add(instance.Id, licenseItem);
		}
	}

	private object LicenseChangedReceiver(VideoOS.Platform.Messaging.Message message, FQID destination, FQID sender)
	{
		_licenseLock.EnterWriteLock();
		bool flag;
		try
		{
			LoadLicense(preserveChanges: true);
			AutoLicenseConfiguration(null);
			flag = UpdateLicenseCache(createTempLicense: false);
			SaveLicenseInternal();
		}
		finally
		{
			_licenseLock.ExitWriteLock();
		}
		if (flag)
		{
			FireLicenseUpdateEvent();
		}
		return null;
	}

	private void FireLicenseUpdateEvent()
	{
		this.LicenseUpdated?.Invoke(this, EventArgs.Empty);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Close();
			_licenseLock.Dispose();
		}
	}
}
