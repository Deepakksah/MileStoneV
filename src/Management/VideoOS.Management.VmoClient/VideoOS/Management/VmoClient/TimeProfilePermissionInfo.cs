using System;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public class TimeProfilePermissionInfo
{
	private string _timeProfileId;

	private bool _isDefaultTimeProfile;

	private string _sid;

	private string _actionId;

	public string ActionId => _actionId;

	public string Sid => _sid;

	public PrincipalType PrincipalType
	{
		get
		{
			if (_sid.StartsWith("S-", StringComparison.Ordinal))
			{
				return PrincipalType.WindowsUserOrGroup;
			}
			return PrincipalType.Role;
		}
	}

	public string TimeProfileId => _timeProfileId;

	public bool IsDefaultTimeProfile => _isDefaultTimeProfile;

	protected TimeProfilePermissionInfo()
	{
	}

	public TimeProfilePermissionInfo(AccessControlTimeProfileEntry entry)
	{
		_sid = entry.Sid;
		_actionId = entry.ActionId;
		if (string.IsNullOrEmpty(entry.TimeProfileId))
		{
			_timeProfileId = null;
		}
		else
		{
			_timeProfileId = entry.TimeProfileId;
		}
		_isDefaultTimeProfile = entry.IsDefaultTimeProfile;
	}
}
