using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace VideoOS.Management.Server.ConfigurationApi.AccessControl;

public static class GroupMemberUtility
{
	public static void AddMember(IGeminiRequestContext requestContext, string groupId, string memberId)
	{
		AccessControlMember.AddAccessControlMember(requestContext, groupId, memberId);
		AddSecurityObject(requestContext, groupId, memberId);
	}

	public static void RemoveMember(IGeminiRequestContext requestContext, IMIPObjectProxy mipObjectProxy, string groupId, string memberId)
	{
		if (groupId == null)
		{
			groupId = LookupGroupId(requestContext, memberId);
		}
		RemoveSecurityObject(requestContext, groupId, memberId);
		AccessControlMember.RemoveAccessControlMember(requestContext, groupId, memberId);
	}

	public static void AddMembers(IGeminiRequestContext requestContext, string groupId, string selectedMember, List<Guid> memberIds)
	{
		foreach (Guid memberId in memberIds)
		{
			AccessControlMember.AddAccessControlMember(requestContext, groupId, memberId.ToString());
		}
		AddSecurityObjects(requestContext, groupId, memberIds);
	}

	public static void RemoveMembers(IGeminiRequestContext requestContext, IMIPObjectProxy mipObjectProxy, string groupId, string selectedMemberId, List<Guid> memberIds)
	{
		RemoveSecurityObjects(requestContext, groupId, memberIds);
		foreach (Guid memberId in memberIds)
		{
			AccessControlMember.RemoveAccessControlMember(requestContext, groupId, memberId.ToString());
		}
	}

	public static List<Guid> QueryMembersInGroup(IGeminiRequestContext requestContext, string groupId)
	{
		return (from m in AccessControlMember.QueryAccessControlMembers(requestContext, groupId)
			select m.MemberId).ToList();
	}

	public static List<Guid> CollectAllACUnitChildren(IGeminiRequestContext requestContext, IMIPObjectProxy mipObjectProxy, MIPObject mipObject)
	{
		if (mipObject == null || mipObject.Kind != "6d3bdb33-d913-470f-8784-3dd4ee94db8c")
		{
			throw new ArgumentException("Selected item not found, or is not an Access Control object");
		}
		List<Guid> list = new List<Guid>
		{
			new Guid(mipObject.Id)
		};
		string parentGroupId = mipObject.RelationIds.Split(';').FirstOrDefault().Trim();
		MIPServerGroup[] array = MIPServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.MIPObjectGroup, parentGroupId);
		foreach (MIPServerGroup mIPServerGroup in array)
		{
			MIPObject[] array2 = mipObjectProxy.QueryMIPObjects(requestContext, mIPServerGroup.Id);
			if (array2.Length != 0)
			{
				string id = array2.First().Id;
				if (Guid.TryParse(id.Trim(), out var result) && HasPermission(requestContext, id))
				{
					list.Add(result);
				}
			}
		}
		return list;
	}

	private static string LookupGroupId(IGeminiRequestContext requestContext, string memberId)
	{
		return AccessControlMember.QueryOwningGroupId(requestContext, memberId);
	}

	private static void AddSecurityObject(IGeminiRequestContext requestContext, string groupId, string memberId)
	{
		try
		{
			requestContext.GetSecurityManagerService().RegisterSecurityObject(requestContext, Constants.AccessControlSecurityNamespace, memberId.ToUpperInvariant(), groupId.ToUpperInvariant());
		}
		catch (Exception ex)
		{
			GeminiTrace.Error("AccessControl: Failed to register security object for group: " + groupId + ", member: " + memberId, ex);
		}
	}

	private static void RemoveSecurityObject(IGeminiRequestContext requestContext, string groupId, string memberId)
	{
		try
		{
			requestContext.GetSecurityManagerService().UnregisterSecurityObject(requestContext, Constants.AccessControlSecurityNamespace, memberId.ToUpperInvariant());
		}
		catch (Exception ex)
		{
			GeminiTrace.Error("AccessControl: Failed to unregister security object for group: " + groupId + ", member: " + memberId, ex);
		}
	}

	private static void AddSecurityObjects(IGeminiRequestContext requestContext, string groupId, List<Guid> memberIds)
	{
		try
		{
			ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
			foreach (Guid memberId in memberIds)
			{
				securityManagerService.RegisterSecurityObject(requestContext, Constants.AccessControlSecurityNamespace, memberId.ToString().ToUpperInvariant(), groupId);
			}
		}
		catch (Exception ex)
		{
			GeminiTrace.Error("AccessControl: Failed to register security object for group: " + groupId, ex);
		}
	}

	private static void RemoveSecurityObjects(IGeminiRequestContext requestContext, string groupId, List<Guid> memberIds)
	{
		try
		{
			ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
			foreach (Guid memberId in memberIds)
			{
				securityManagerService.UnregisterSecurityObject(requestContext, Constants.AccessControlSecurityNamespace, memberId.ToString("D", CultureInfo.InvariantCulture));
			}
		}
		catch (Exception ex)
		{
			GeminiTrace.Error("AccessControl: Failed to unregister security object for group: " + groupId, ex);
		}
	}

	public static bool HasPermission(IGeminiRequestContext requestContext, string memberId)
	{
		if (requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
		{
			return true;
		}
		try
		{
			string fullPath = requestContext.SecurityObjectMapCache.GetFullPath(requestContext, Constants.AccessControlSecurityNamespace, memberId.ToUpperInvariant());
			return requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, Constants.AccessControlSecurityNamespace, "GENERIC_READ", fullPath, checkAdministrator: true);
		}
		catch (Exception ex)
		{
			GeminiTrace.Error("AccessControl: could not check HasPermission for " + memberId, ex);
			return false;
		}
	}

	public static void HasManagementPermission(IGeminiRequestContext requestContext, string memberId)
	{
		if (!requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
		{
			string fullPath = requestContext.SecurityObjectMapCache.GetFullPath(requestContext, Constants.AccessControlSecurityNamespace, memberId.ToUpperInvariant());
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, Constants.AccessControlSecurityNamespace, "GENERIC_WRITE", fullPath);
		}
	}
}
