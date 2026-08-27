using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "AccessControlTimeProfileEntry", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class AccessControlTimeProfileEntry
{
	public string _actionId;

	public string _sid;

	public string _timeProfileId;

	public bool _isDefaultTimeProfile;

	[DataMember]
	public string Sid
	{
		get
		{
			return _sid;
		}
		set
		{
			_sid = value;
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

	public AccessControlTimeProfileEntry()
	{
	}

	public AccessControlTimeProfileEntry(string actionId, string sid, string timeProfileId, bool isDefaultTimeProfile)
	{
		_actionId = actionId;
		_sid = sid;
		_timeProfileId = timeProfileId;
		_isDefaultTimeProfile = isDefaultTimeProfile;
	}
}
