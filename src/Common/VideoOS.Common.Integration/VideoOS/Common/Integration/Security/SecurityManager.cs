using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using VideoOS.Common.SharedTimeProfile;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.Security;

public class SecurityManager : IDisposable
{
	public enum DevicePermissionResult
	{
		Allow,
		Deny,
		DenyTimeRestricted
	}

	private class WeakReferenceDataSetEventHandler : WeakReference
	{
		private SecurityDataSet securityDataSet;

		public WeakReferenceDataSetEventHandler(SecurityManager weakTarget, SecurityDataSet securityDataSet)
			: base(weakTarget)
		{
			this.securityDataSet = securityDataSet;
		}

		public void CallSecurityManagerOnDataChanged(object sender, DataRowChangeEventArgs e)
		{
			SecurityManager securityManager = (SecurityManager)Target;
			if (securityManager != null)
			{
				securityManager.OnDataChanged(sender, e);
			}
			else
			{
				UnregisterEventHandler();
			}
		}

		public void UnregisterEventHandler()
		{
			if (securityDataSet != null)
			{
				securityDataSet.DataChanged -= CallSecurityManagerOnDataChanged;
			}
		}
	}

	private ILegacySecurityAttributeMapper _legacySecurityAttributeMapper;

	public static Guid AdministratorRoleId = new Guid("1f30d14b-ba91-4e8a-8838-eda17931f8fc");

	private SecurityAttributes _securityAttributes;

	private Dictionary<Guid, Dictionary<Guid, Dictionary<string, SecurityPermissionDevice>>> _roleDevicePermissions;

	private Dictionary<Guid, Dictionary<string, SecurityPermission>> _roleApplicationPermissions;

	private Dictionary<Guid, string> _roleDefaultTimeProfile;

	private Dictionary<Guid, bool> _roleShowAsAnonymousPtzUser;

	private Dictionary<string, List<SecurityGroupAccessEntry>> _roleGroupAccessEntries;

	private WeakReferenceDataSetEventHandler weakReferenceDataSetEventHandler;

	private object _dataChangedEventLock = new object();

	public static readonly Type SecurityAttributesDataTable = typeof(SecurityDataSet.SecurityAttributesDataTable);

	public static readonly Type SecurityPermissionsDataTable = typeof(SecurityDataSet.SecurityPermissionsDataTable);

	public static readonly Type SecurityPermissionsDeviceDataTable = typeof(SecurityDataSet.SecurityPermissionsDeviceDataTable);

	public static readonly Type TimeProfileDefaultRoleRelationDataTable = typeof(SecurityDataSet.TimeProfileDefaultRoleRelationDataTable);

	public static readonly Type SecurityRolesDateTable = typeof(SecurityDataSet.SecurityRolesDataTable);

	private bool disposed;

	protected SecurityDataSet SecurityDataSet { get; private set; }

	public bool IsDirty { get; private set; }

	public Guid RoleId { get; private set; }

	public SecurityDataSet DataSet => SecurityDataSet;

	public bool HasChanges => SecurityDataSet.HasChanges();

	public SecurityAttributes SecurityAttributes => _securityAttributes;

	private event EventHandler<DataRowChangeEventArgs> _dataChanged;

	public event EventHandler<DataRowChangeEventArgs> DataChanged
	{
		add
		{
			lock (_dataChangedEventLock)
			{
				if (this._dataChanged == null)
				{
					SecurityDataSet.DataChanged += weakReferenceDataSetEventHandler.CallSecurityManagerOnDataChanged;
				}
				_dataChanged += value;
			}
		}
		remove
		{
			lock (_dataChangedEventLock)
			{
				if (this._dataChanged != null)
				{
					_dataChanged -= value;
					if (this._dataChanged == null)
					{
						SecurityDataSet.DataChanged -= weakReferenceDataSetEventHandler.CallSecurityManagerOnDataChanged;
					}
				}
			}
		}
	}

	public event EventHandler<EventArgs> DataSaved;

	public void OnDataChanged(object sender, DataRowChangeEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		IsDirty = e.Action != DataRowAction.Rollback;
		EventHandler<DataRowChangeEventArgs> eventHandler;
		lock (_dataChangedEventLock)
		{
			eventHandler = this._dataChanged;
		}
		eventHandler?.Invoke(sender, e);
	}

	public void OnDataSaved(object sender, EventArgs e)
	{
		IsDirty = false;
		if (this.DataSaved != null)
		{
			this.DataSaved(sender, e);
		}
	}

	public SecurityManager(SecurityDataSet securityDataSet, Guid roleId)
		: this(securityDataSet, cachePermissions: false, LegacySecurityAttributeMapper.Instance)
	{
		RoleId = roleId;
	}

	public SecurityManager(SecurityDataSet securityDataSet, bool cachePermissions)
		: this(securityDataSet, cachePermissions, LegacySecurityAttributeMapper.Instance)
	{
	}

	public SecurityManager(SecurityDataSet securityDataSet, bool cachePermissions, ILegacySecurityAttributeMapper legacySecurityAttributeMapper)
	{
		if (securityDataSet == null)
		{
			throw new ArgumentNullException("securityDataSet");
		}
		if (legacySecurityAttributeMapper == null)
		{
			throw new ArgumentNullException("legacySecurityAttributeMapper");
		}
		weakReferenceDataSetEventHandler = new WeakReferenceDataSetEventHandler(this, securityDataSet);
		_legacySecurityAttributeMapper = legacySecurityAttributeMapper;
		SecurityDataSet = securityDataSet;
		_securityAttributes = new SecurityAttributes(this);
		if (!cachePermissions)
		{
			return;
		}
		_roleDefaultTimeProfile = new Dictionary<Guid, string>();
		foreach (SecurityDataSet.TimeProfileDefaultRoleRelationRow item in securityDataSet.TimeProfileDefaultRoleRelation)
		{
			_roleDefaultTimeProfile.Add(new Guid(item.IDRole), item.IDTimeProfile);
		}
		_roleShowAsAnonymousPtzUser = new Dictionary<Guid, bool>();
		foreach (SecurityDataSet.SecurityRolesRow securityRole in securityDataSet.SecurityRoles)
		{
			_roleShowAsAnonymousPtzUser.Add(new Guid(securityRole.IDRole), securityRole.PtzReservedAsAnonymous);
		}
		_roleApplicationPermissions = new Dictionary<Guid, Dictionary<string, SecurityPermission>>();
		foreach (SecurityDataSet.SecurityPermissionsRow securityPermission2 in securityDataSet.SecurityPermissions)
		{
			SecurityPermission securityPermission = new SecurityPermission(securityPermission2);
			if (!_roleApplicationPermissions.ContainsKey(securityPermission.RoleId))
			{
				_roleApplicationPermissions.Add(securityPermission.RoleId, new Dictionary<string, SecurityPermission>());
			}
			_roleApplicationPermissions[securityPermission.RoleId].Add(securityPermission.SecurityAttributeId, securityPermission);
		}
		_roleDevicePermissions = new Dictionary<Guid, Dictionary<Guid, Dictionary<string, SecurityPermissionDevice>>>();
		foreach (SecurityDataSet.SecurityPermissionsDeviceRow item2 in securityDataSet.SecurityPermissionsDevice)
		{
			SecurityPermissionDevice securityPermissionDevice = new SecurityPermissionDevice(item2);
			if (!_roleDevicePermissions.ContainsKey(securityPermissionDevice.RoleId))
			{
				_roleDevicePermissions.Add(securityPermissionDevice.RoleId, new Dictionary<Guid, Dictionary<string, SecurityPermissionDevice>>());
			}
			if (!_roleDevicePermissions[securityPermissionDevice.RoleId].ContainsKey(securityPermissionDevice.DeviceId))
			{
				_roleDevicePermissions[securityPermissionDevice.RoleId].Add(securityPermissionDevice.DeviceId, new Dictionary<string, SecurityPermissionDevice>());
			}
			_roleDevicePermissions[securityPermissionDevice.RoleId][securityPermissionDevice.DeviceId].Add(securityPermissionDevice.SecurityAttributeId, securityPermissionDevice);
		}
		_roleGroupAccessEntries = new Dictionary<string, List<SecurityGroupAccessEntry>>(StringComparer.OrdinalIgnoreCase);
		foreach (SecurityDataSet.SecurityGroupAccessEntryRow item3 in securityDataSet.SecurityGroupAccessEntry)
		{
			SecurityGroupAccessEntry securityGroupAccessEntry = new SecurityGroupAccessEntry(item3);
			if (!_roleGroupAccessEntries.TryGetValue(securityGroupAccessEntry.UserSid, out var value))
			{
				value = new List<SecurityGroupAccessEntry>();
				_roleGroupAccessEntries.Add(securityGroupAccessEntry.UserSid, value);
			}
			value.Add(securityGroupAccessEntry);
		}
	}

	private bool HasGroupPermission(Guid[] memberOfRoles, string attributeName, out bool isAllowed)
	{
		LegacySecurityActionInformation legacyInfo = _legacySecurityAttributeMapper.GetSecurityActionInformation(attributeName);
		if (legacyInfo != null)
		{
			Guid[] array = memberOfRoles;
			for (int i = 0; i < array.Length; i++)
			{
				Guid guid = array[i];
				if (_roleGroupAccessEntries.TryGetValue(guid.ToString(), out var value) && value.FirstOrDefault((SecurityGroupAccessEntry a) => a.Deny && a.NamespaceId == legacyInfo.NamespaceId && string.Equals(a.ActionName, legacyInfo.SecurityAction, StringComparison.Ordinal) && string.Equals(a.ObjectId, "/", StringComparison.Ordinal)) != null)
				{
					isAllowed = false;
					return true;
				}
			}
			array = memberOfRoles;
			for (int i = 0; i < array.Length; i++)
			{
				Guid guid2 = array[i];
				if (_roleGroupAccessEntries.TryGetValue(guid2.ToString(), out var value2) && value2.FirstOrDefault((SecurityGroupAccessEntry a) => !a.Deny && a.NamespaceId == legacyInfo.NamespaceId && string.Equals(a.ActionName, legacyInfo.SecurityAction, StringComparison.Ordinal) && string.Equals(a.ObjectId, "/", StringComparison.Ordinal)) != null)
				{
					isAllowed = true;
					return true;
				}
			}
		}
		isAllowed = false;
		return false;
	}

	public bool HasApplicationPermission(Identity identity, string attributeName, DateTime time, ITimeProfileEvaluation timeProfileEvaluator)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		if (timeProfileEvaluator == null)
		{
			throw new ArgumentNullException("timeProfileEvaluator");
		}
		Guid[] memberOfRoles = identity.MemberOfRoles;
		return HasApplicationPermissionInternal(memberOfRoles, attributeName, time, timeProfileEvaluator);
	}

	public bool HasApplicationPermissionInternal(Guid[] memberOfRoles, string attributeName, DateTime time, ITimeProfileEvaluation timeProfileEvaluator)
	{
		if (memberOfRoles.Any((Guid id) => id == AdministratorRoleId))
		{
			return true;
		}
		if (HasGroupPermission(memberOfRoles, attributeName, out var isAllowed))
		{
			return isAllowed;
		}
		HashSet<string> hashSet = new HashSet<string>();
		foreach (Guid key in memberOfRoles)
		{
			SecurityPermission value = null;
			if (_roleApplicationPermissions.TryGetValue(key, out var value2))
			{
				value2.TryGetValue(attributeName, out value);
			}
			if (value != null)
			{
				string value3 = null;
				if (value.IsDefaultTimeProfile)
				{
					_roleDefaultTimeProfile.TryGetValue(key, out value3);
				}
				else
				{
					value3 = value.TimeProfileId;
				}
				if (string.IsNullOrEmpty(value3))
				{
					return true;
				}
				hashSet.Add(value3);
			}
		}
		foreach (string item in hashSet)
		{
			if (timeProfileEvaluator.EvaluateTimeProfile(new Guid(item), time))
			{
				return true;
			}
		}
		return false;
	}

	public DevicePermissionResult HasDevicePermission(Identity identity, string attributeName, Guid deviceId, DateTime dataTime, DateTime currentTime, ITimeProfileEvaluation timeProfileEvaluator)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		Guid[] memberOfRoles = identity.MemberOfRoles;
		return HasDevicePermissionInternal(memberOfRoles, attributeName, deviceId, dataTime, currentTime, timeProfileEvaluator);
	}

	public DevicePermissionResult HasDevicePermissionInternal(Guid[] memberOfRoles, string attributeName, Guid deviceId, DateTime dataTime, DateTime currentTime, ITimeProfileEvaluation timeProfileEvaluator)
	{
		DevicePermissionResult result = DevicePermissionResult.Deny;
		if (memberOfRoles.Any((Guid id) => id == AdministratorRoleId))
		{
			return DevicePermissionResult.Allow;
		}
		if (HasGroupPermission(memberOfRoles, attributeName, out var isAllowed))
		{
			if (isAllowed)
			{
				return DevicePermissionResult.Allow;
			}
			return DevicePermissionResult.Deny;
		}
		bool flag = timeProfileEvaluator != null;
		HashSet<string> hashSet = new HashSet<string>();
		foreach (Guid guid in memberOfRoles)
		{
			SecurityPermissionDevice securityPermissionDevice = FindCachedSecurityPermissionDevice(attributeName, deviceId, guid);
			if (securityPermissionDevice == null)
			{
				continue;
			}
			if (!flag)
			{
				return DevicePermissionResult.Allow;
			}
			result = DevicePermissionResult.DenyTimeRestricted;
			if (securityPermissionDevice.TimeBackward == 0 || dataTime >= currentTime.AddMinutes(-securityPermissionDevice.TimeBackward))
			{
				string value = null;
				if (securityPermissionDevice.IsDefaultTimeProfile)
				{
					_roleDefaultTimeProfile.TryGetValue(guid, out value);
				}
				else
				{
					value = securityPermissionDevice.TimeProfileId;
				}
				if (string.IsNullOrEmpty(value))
				{
					return DevicePermissionResult.Allow;
				}
				hashSet.Add(value);
			}
		}
		foreach (string item in hashSet)
		{
			if (timeProfileEvaluator.EvaluateTimeProfile(new Guid(item), dataTime))
			{
				return DevicePermissionResult.Allow;
			}
		}
		return result;
	}

	public DevicePermissionResult HasGroupPermission(Identity identity, string attributeName)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		Guid[] memberOfRoles = identity.MemberOfRoles;
		return HasGroupPermissionInternal(memberOfRoles, attributeName);
	}

	public DevicePermissionResult HasGroupPermissionInternal(Guid[] memberOfRoles, string attributeName)
	{
		if (memberOfRoles.Any((Guid id) => id == AdministratorRoleId))
		{
			return DevicePermissionResult.Allow;
		}
		if (HasGroupPermission(memberOfRoles, attributeName, out var isAllowed))
		{
			if (isAllowed)
			{
				return DevicePermissionResult.Allow;
			}
			return DevicePermissionResult.Deny;
		}
		return DevicePermissionResult.Deny;
	}

	public object GetDeviceAttributeValue(Identity identity, string attributeName, Guid deviceId)
	{
		if (identity == null)
		{
			throw new ArgumentNullException("identity");
		}
		SecurityAttribute securityAttribute = _securityAttributes[attributeName];
		object obj = null;
		Guid[] memberOfRoles = identity.MemberOfRoles;
		foreach (Guid guid in memberOfRoles)
		{
			if (guid == AdministratorRoleId)
			{
				object administratorAttributeValue = securityAttribute.Definition.GetAdministratorAttributeValue();
				if (administratorAttributeValue != null)
				{
					return administratorAttributeValue;
				}
			}
			SecurityPermissionDevice securityPermissionDevice = FindCachedSecurityPermissionDevice(attributeName, deviceId, guid);
			object obj2 = null;
			obj2 = ((securityPermissionDevice == null) ? securityAttribute.Definition.GetDefaultAttributeValue() : ((object)int.Parse(securityPermissionDevice.Value, CultureInfo.InvariantCulture)));
			if (obj2 != null)
			{
				obj = ((obj != null) ? ((object)Math.Max((int)obj, (int)obj2)) : obj2);
			}
		}
		return obj;
	}

	private SecurityPermissionDevice FindCachedSecurityPermissionDevice(string attributeName, Guid deviceId, Guid roleId)
	{
		SecurityPermissionDevice value = null;
		if (_roleDevicePermissions.TryGetValue(roleId, out var value2) && value2.TryGetValue(deviceId, out var value3))
		{
			value3.TryGetValue(attributeName, out value);
		}
		return value;
	}

	public bool IsAnonymousPtzUser(Identity identity)
	{
		if (identity == null)
		{
			return false;
		}
		Guid[] memberOfRoles = identity.MemberOfRoles;
		foreach (Guid key in memberOfRoles)
		{
			if (_roleShowAsAnonymousPtzUser.ContainsKey(key) && _roleShowAsAnonymousPtzUser[key])
			{
				return true;
			}
		}
		return false;
	}

	~SecurityManager()
	{
		Dispose(disposing: false);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!disposed)
		{
			if (disposing && weakReferenceDataSetEventHandler != null)
			{
				weakReferenceDataSetEventHandler.UnregisterEventHandler();
			}
			disposed = true;
		}
	}
}
