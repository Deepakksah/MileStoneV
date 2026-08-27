using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Server;

public static class SecurityNamespaceCache
{
	private static ConcurrentDictionary<string, string> securityDisplayNames = new ConcurrentDictionary<string, string>();

	public static void Initialize(ConnectionManager connection)
	{
		if (connection == null)
		{
			throw new ArgumentNullException("connection");
		}
		SecurityProxy clientProxy = connection.GetClientProxy<SecurityProxy>();
		SecurityNamespaceInfo[] array = clientProxy.QuerySecurityNamespaces(string.Empty);
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		SecurityNamespaceInfo[] array2 = array;
		foreach (SecurityNamespaceInfo securityNamespaceInfo in array2)
		{
			dictionary.Add(value: securityNamespaceInfo.NamespaceId.Equals(SecurityNamespaceIdentifier.SmartWall.ToString(), StringComparison.OrdinalIgnoreCase) ? OemResources.Instance.VideoWall.ApplicationNameShort : ((!securityNamespaceInfo.NamespaceId.Equals(SecurityNamespaceIdentifier.Matrix.ToString(), StringComparison.OrdinalIgnoreCase)) ? securityNamespaceInfo.DisplayName : OemResources.Instance.Manager.MatrixApplicationName), key: securityNamespaceInfo.NamespaceId);
			ActionDefinition[] actions = securityNamespaceInfo.Actions;
			foreach (ActionDefinition actionDefinition in actions)
			{
				dictionary.Add(securityNamespaceInfo.NamespaceId + actionDefinition.Name, actionDefinition.DisplayName);
			}
		}
		securityDisplayNames = new ConcurrentDictionary<string, string>(dictionary);
	}

	public static string GetSecurityNamespaceNameByLegacySecurityAction(LegacySecurityActionInformation actionInfo)
	{
		if (actionInfo == null)
		{
			throw new ArgumentNullException("actionInfo");
		}
		string key = actionInfo.NamespaceId.ToString();
		securityDisplayNames.TryGetValue(key, out var value);
		return value;
	}

	public static string GetActionNameByLegacySecurityAction(LegacySecurityActionInformation actionInfo)
	{
		if (actionInfo == null)
		{
			throw new ArgumentNullException("actionInfo");
		}
		string key = actionInfo.NamespaceId.ToString() + actionInfo.SecurityAction;
		securityDisplayNames.TryGetValue(key, out var value);
		return value;
	}
}
