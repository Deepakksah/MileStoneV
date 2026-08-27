using System.Runtime.Serialization;

namespace VideoOS.Management.Common;

[DataContract]
public class AccessEntry
{
	private string _actionId;

	private string _sid;

	private bool _accessDeny;

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
	public bool AccessDeny
	{
		get
		{
			return _accessDeny;
		}
		set
		{
			_accessDeny = value;
		}
	}

	public AccessEntry()
	{
	}

	public AccessEntry(string actionId, string sid, bool accessDeny)
	{
		_actionId = actionId;
		_sid = sid;
		_accessDeny = accessDeny;
	}
}
