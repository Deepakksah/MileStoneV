using System;
using System.Diagnostics;

namespace VideoOS.Common.Integration;

[Serializable]
public class CommunicationCallContextDto
{
	private string _userSid;

	private string _userDomainAccountName;

	private string _locationIPAddress;

	public string LocationIPAddress
	{
		[DebuggerStepThrough]
		get
		{
			return _locationIPAddress;
		}
	}

	public string UserDomainAccountName
	{
		[DebuggerStepThrough]
		get
		{
			return _userDomainAccountName;
		}
	}

	public string UserSid
	{
		[DebuggerStepThrough]
		get
		{
			return _userSid;
		}
	}

	public CommunicationCallContextDto(string userSid, string userDomainAccountName, string locationIPAddress)
	{
		_userSid = userSid;
		_userDomainAccountName = userDomainAccountName;
		_locationIPAddress = locationIPAddress;
	}
}
