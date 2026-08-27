using System;

namespace VideoOS.Management.Server;

public class IdentityService : IIdentityService, IGeminiServerService
{
	public bool IsMember(IGeminiRequestContext requestContext, IdentityDescriptor identityDescriptor, IdentityDescriptor descriptor)
	{
		if (requestContext == null)
		{
			throw new ArgumentNullException("requestContext");
		}
		if (identityDescriptor == null)
		{
			throw new ArgumentNullException("identityDescriptor");
		}
		if (descriptor == null)
		{
			throw new ArgumentNullException("descriptor");
		}
		return IdentityCache.IsMember(requestContext, descriptor.Identifier, identityDescriptor.Identifier);
	}

	public void StartService(IGeminiRequestContext systemRequestContext)
	{
	}

	public void StopService(IGeminiRequestContext systemRequestContext)
	{
	}
}
