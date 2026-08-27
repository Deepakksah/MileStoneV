using System;

namespace VideoOS.Management.Server;

public sealed class AccessControlMember
{
	public Guid MemberId { get; set; }

	public Guid GroupId { get; set; }

	public DateTime LastModified { get; set; }

	public static AccessControlMember[] QueryAccessControlMembers(IGeminiRequestContext requestContext, string groupId)
	{
		Validation.CheckObjectId(ref groupId, "groupId", allowNullOrEmpty: false);
		using IAccessControlMemberComponent accessControlMemberComponent = requestContext.CreateAccessControlMemberComponent();
		return accessControlMemberComponent.QueryAccessControlMembers(requestContext, groupId);
	}

	public static AccessControlMember[] QueryAccessControlAllMembers(IGeminiRequestContext requestContext)
	{
		using IAccessControlMemberComponent accessControlMemberComponent = requestContext.CreateAccessControlMemberComponent();
		return accessControlMemberComponent.QueryAccessControlAllMembers(requestContext);
	}

	public static void AddAccessControlMember(IGeminiRequestContext requestContext, string groupId, string memberId)
	{
		Validation.CheckObjectId(ref groupId, "groupId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref memberId, "memberId", allowNullOrEmpty: false);
		using IAccessControlMemberComponent accessControlMemberComponent = requestContext.CreateAccessControlMemberComponent();
		accessControlMemberComponent.AddAccessControlMember(requestContext, groupId, memberId);
	}

	public static void RemoveAccessControlMember(IGeminiRequestContext requestContext, string groupId, string memberId)
	{
		Validation.CheckObjectId(ref groupId, "groupId", allowNullOrEmpty: false);
		Validation.CheckObjectId(ref memberId, "memberId", allowNullOrEmpty: false);
		using IAccessControlMemberComponent accessControlMemberComponent = requestContext.CreateAccessControlMemberComponent();
		accessControlMemberComponent.RemoveAccessControlMember(requestContext, groupId, memberId);
	}

	public static string QueryOwningGroupId(IGeminiRequestContext requestContext, string memberId)
	{
		Validation.CheckObjectId(ref memberId, "memberId", allowNullOrEmpty: false);
		using IAccessControlMemberComponent accessControlMemberComponent = requestContext.CreateAccessControlMemberComponent();
		return accessControlMemberComponent.QueryOwningGroupId(requestContext, memberId);
	}
}
