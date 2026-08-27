using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[DataContract(Name = "RoleInfo")]
public sealed class RoleOld
{
	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public RoleType Type { get; set; }

	[DataMember]
	public DateTime LastModified { get; set; }

	public static RoleOld[] QueryRoles(GeminiRequestContext requestContext)
	{
		return ConvertToRoleOlds(Role.QueryRoles(requestContext));
	}

	public static RoleOld QueryRoleById(IGeminiRequestContext requestContext, string roleId)
	{
		return ConvertToRoleOld(Role.QueryRoleById(requestContext, roleId));
	}

	public static RoleOld QueryRoleByName(IGeminiRequestContext requestContext, string name)
	{
		return ConvertToRoleOld(Role.QueryRoleByName(requestContext, name));
	}

	public static RoleOld CreateRole(IGeminiRequestContext requestContext, string name, string description)
	{
		return ConvertToRoleOld(Role.CreateRole(requestContext, name, description, dualAuthorizationRequired: false, showPtzReservedNameAsAnonymous: false, allowMobileClientLogOn: true, allowSmartClientLogOn: true, allowWebClientLogOn: true));
	}

	public static RoleOld UpdateRole(IGeminiRequestContext requestContext, string roleId, string name, string description)
	{
		return ConvertToRoleOld(Role.UpdateRole(requestContext, roleId, name, description));
	}

	public static RoleOld CopyRole(IGeminiRequestContext requestContext, string sourceRoleId, string name, string description)
	{
		return ConvertToRoleOld(Role.CopyRole(requestContext, sourceRoleId, name, description));
	}

	public static void DeleteRole(IGeminiRequestContext requestContext, string roleId)
	{
		Role.DeleteRole(requestContext, roleId);
	}

	public static Identity AddMemberToRole(IGeminiRequestContext requestContext, string roleId, string memberSid)
	{
		return Role.AddMemberToRole(requestContext, roleId, memberSid);
	}

	public static void RemoveMemberFromRole(IGeminiRequestContext requestContext, string roleId, string memberSid)
	{
		Role.RemoveMemberFromRole(requestContext, roleId, memberSid);
	}

	private static RoleOld ConvertToRoleOld(Role role)
	{
		if (role == null)
		{
			return null;
		}
		return new RoleOld
		{
			Id = role.Id,
			Name = role.Name,
			Description = role.Description,
			LastModified = role.LastModified,
			Type = role.Type
		};
	}

	private static RoleOld[] ConvertToRoleOlds(Role[] roles)
	{
		if (roles == null)
		{
			return null;
		}
		List<RoleOld> list = new List<RoleOld>();
		foreach (Role role in roles)
		{
			list.Add(ConvertToRoleOld(role));
		}
		return list.ToArray();
	}
}
