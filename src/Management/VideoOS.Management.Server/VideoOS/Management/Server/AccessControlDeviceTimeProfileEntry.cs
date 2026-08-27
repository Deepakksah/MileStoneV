using System;
using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "AccessControlDeviceTimeProfileEntry", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class AccessControlDeviceTimeProfileEntry
{
	public DateTime _lastModified;

	public string _actionId;

	public Guid _roleId;

	public Guid _deviceId;

	public string _value;

	public string _timeProfileId;

	public int _timeBackward;

	public bool _isDefaultTimeProfile;

	[DataMember]
	public DateTime LastModified
	{
		get
		{
			return _lastModified;
		}
		set
		{
			_lastModified = value;
		}
	}

	[DataMember]
	public Guid RoleId
	{
		get
		{
			return _roleId;
		}
		set
		{
			_roleId = value;
		}
	}

	[DataMember]
	public string ActionId
	{
		get
		{
			return _actionId;
		}
		set
		{
			_actionId = value;
		}
	}

	[DataMember]
	public Guid DeviceId
	{
		get
		{
			return _deviceId;
		}
		set
		{
			_deviceId = value;
		}
	}

	[DataMember]
	public string Value
	{
		get
		{
			return _value;
		}
		set
		{
			_value = value;
		}
	}

	[DataMember]
	public string TimeProfileId
	{
		get
		{
			return _timeProfileId;
		}
		set
		{
			_timeProfileId = value;
		}
	}

	[DataMember]
	public bool IsDefaultTimeProfile
	{
		get
		{
			return _isDefaultTimeProfile;
		}
		set
		{
			_isDefaultTimeProfile = value;
		}
	}

	[DataMember]
	public int TimeBackward
	{
		get
		{
			return _timeBackward;
		}
		set
		{
			_timeBackward = value;
		}
	}

	public AccessControlDeviceTimeProfileEntry()
	{
	}

	public AccessControlDeviceTimeProfileEntry(string actionId, Guid roleId, Guid deviceId, string value, string timeProfileId, int timeBackward, bool isDefaultTimeProfile)
	{
		_lastModified = DateTime.UtcNow;
		_actionId = actionId;
		_roleId = roleId;
		_deviceId = deviceId;
		_value = value;
		_timeBackward = timeBackward;
		_timeProfileId = timeProfileId;
		_isDefaultTimeProfile = isDefaultTimeProfile;
	}
}
