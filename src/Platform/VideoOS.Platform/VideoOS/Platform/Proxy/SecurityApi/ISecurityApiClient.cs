using System;
using System.Collections.Generic;
using VideoOS.Platform.Login;

namespace VideoOS.Platform.Proxy.SecurityApi;

public interface ISecurityApiClient
{
	void Initialize(LoginSettings loginSettings, bool debugLog);

	void Close();

	bool HasPermission(Guid securityNamespaceId, Guid objectId, string permissionName);

	bool HasPermission(Guid securityNamespaceId, string permissionName);

	void PreloadCache(IEnumerable<Guid> securityNamespaceIds);

	void FlushCache();

	void FlushCache(Guid securityNamespaceId);
}
