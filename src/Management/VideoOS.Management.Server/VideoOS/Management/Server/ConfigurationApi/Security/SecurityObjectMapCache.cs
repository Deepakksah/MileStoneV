using System;
using System.Collections.Generic;

namespace VideoOS.Management.Server.ConfigurationApi.Security;

public class SecurityObjectMapCache : ISecurityObjectMapCache
{
	private Dictionary<Guid, Dictionary<string, string>> _namespaceMap = new Dictionary<Guid, Dictionary<string, string>>();

	private void LoadCache(IGeminiRequestContext requestContext, Guid securityNameSpaceId)
	{
		lock (_namespaceMap)
		{
			if (_namespaceMap.TryGetValue(securityNameSpaceId, out var _))
			{
				return;
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			using (ISecurityComponent securityComponent = requestContext.CreateSecurityComponent())
			{
				foreach (Tuple<string, string> item2 in securityComponent.QuerySecurityObjectMaps(securityNameSpaceId))
				{
					string key = item2.Item1.ToUpperInvariant();
					string item = item2.Item2;
					dictionary[key] = item;
				}
			}
			_namespaceMap[securityNameSpaceId] = dictionary;
		}
	}

	public string GetFullPath(IGeminiRequestContext geminiRequest, Guid securityNamespaceId, string objectId)
	{
		if (string.IsNullOrWhiteSpace(objectId))
		{
			return null;
		}
		lock (_namespaceMap)
		{
			if (!_namespaceMap.ContainsKey(securityNamespaceId))
			{
				LoadCache(geminiRequest, securityNamespaceId);
			}
			if (_namespaceMap.TryGetValue(securityNamespaceId, out var value) && value.TryGetValue(objectId.ToUpperInvariant(), out var value2))
			{
				return value2;
			}
			return null;
		}
	}
}
