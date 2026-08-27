using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server.RestApi.Utils;

public class SecurityNamespaceActionConverter
{
	private Dictionary<string, ulong> _actionNameToMask = new Dictionary<string, ulong>(StringComparer.OrdinalIgnoreCase);

	public SecurityNamespaceActionConverter(SecurityNamespaceInfo securityNamespaceInfo)
	{
		foreach (ActionDefinition action in securityNamespaceInfo.Actions)
		{
			_actionNameToMask[action.Name] = action.PermissionBit;
		}
	}

	public bool TryGetActionBit(string actionName, out ulong id)
	{
		return _actionNameToMask.TryGetValue(actionName, out id);
	}

	public string GetActionNameFromBit(ulong bit)
	{
		foreach (KeyValuePair<string, ulong> item in _actionNameToMask)
		{
			if (item.Value == bit)
			{
				return item.Key;
			}
		}
		return null;
	}
}
