using System;

namespace VideoOS.Management.Server;

public interface IAccessControlMemberComponent : IDisposable
{
	AccessControlMember[] QueryAccessControlMembers(IGeminiRequestContext requestContext, string groupId);

	AccessControlMember[] QueryAccessControlAllMembers(IGeminiRequestContext requestContext);

	void AddAccessControlMember(IGeminiRequestContext requestContext, string groupid, string memberId);

	void RemoveAccessControlMember(IGeminiRequestContext requestContext, string groupid, string memberId);

	string QueryOwningGroupId(IGeminiRequestContext requestContext, string memberId);
}
