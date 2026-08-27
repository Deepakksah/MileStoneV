namespace VideoOS.Management.Server;

public interface IIdentityService
{
	bool IsMember(IGeminiRequestContext requestContext, IdentityDescriptor identityDescriptor, IdentityDescriptor descriptor);
}
