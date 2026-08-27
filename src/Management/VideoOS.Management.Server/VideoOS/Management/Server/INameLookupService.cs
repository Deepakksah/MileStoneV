using System;

namespace VideoOS.Management.Server;

public interface INameLookupService
{
	string GetObjectName(IGeminiRequestContext requestContext, string securityNamespaceId, string objectId);

	void InitializeMipNamespaces(IGeminiRequestContext requestContext);

	string GetRoleName(IGeminiRequestContext requestContext, Guid roleId);
}
