using System;

namespace VideoOS.Management.Server.ConfigurationApi.Security;

public interface ISecurityObjectMapCache
{
	string GetFullPath(IGeminiRequestContext geminiRequest, Guid securityNamespaceId, string objectId);
}
