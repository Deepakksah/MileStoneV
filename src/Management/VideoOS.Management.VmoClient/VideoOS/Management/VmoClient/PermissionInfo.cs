using System;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public class PermissionInfo
{
	private PermissionSetBase _permissionSet;

	private PermissionState _permissionState;

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

	public PermissionSetBase PermissionTypeInternal => _permissionSet;

	public PermissionState PermissionState => _permissionState;

	protected PermissionInfo()
	{
		_permissionState = PermissionState.Revoke;
	}

	public void SetPermissionInfo(AccessControlEntry entry, PermissionSetBase permissionSet)
	{
		_sid = entry.Sid;
		_actionId = entry.ActionId;
		_permissionSet = permissionSet;
		_permissionState = PermissionState.Allow;
	}

	public override string ToString()
	{
		return _actionId;
	}
}
