using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RoleComponent : GeminiResourceComponent, IRoleComponent, IDisposable
{
	private static class RoleColumns
	{
		private static DatabaseColumnBinder IDRole = new DatabaseColumnBinder("IDRole");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("Type");

		private static DatabaseColumnBinder DualAuthorizationRequired = new DatabaseColumnBinder("DualRequired");

		private static DatabaseColumnBinder ShowPtzReservedNameAsAnonymous = new DatabaseColumnBinder("PtzReservedAsAnonymous");

		private static DatabaseColumnBinder AllowMobileClientLogOn = new DatabaseColumnBinder("AllowMobileClientLogOn");

		private static DatabaseColumnBinder AllowSmartClientLogOn = new DatabaseColumnBinder("AllowSmartClientLogOn");

		private static DatabaseColumnBinder AllowWebClientLogOn = new DatabaseColumnBinder("AllowWebClientLogOn");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static Role Bind(DatabaseDataReader reader)
		{
			return new Role
			{
				Id = IDRole.GetString(reader, allowNulls: false),
				Name = Name.GetString(reader, allowNulls: false),
				Description = Description.GetString(reader, allowNulls: true),
				Type = (RoleType)Type.GetByte(reader),
				DualAuthorizationRequired = DualAuthorizationRequired.GetBoolean(reader),
				ShowPtzReservedNameAsAnonymous = ShowPtzReservedNameAsAnonymous.GetBoolean(reader, allowNulls: true, defaultValue: false),
				AllowMobileClientLogOn = AllowMobileClientLogOn.GetBoolean(reader, allowNulls: false, defaultValue: false),
				AllowSmartClientLogOn = AllowSmartClientLogOn.GetBoolean(reader, allowNulls: false, defaultValue: true),
				AllowWebClientLogOn = AllowWebClientLogOn.GetBoolean(reader, allowNulls: false, defaultValue: false),
				LastModified = LastModified.GetDateTime(reader)
			};
		}
	}

	private static class RoleMemberColumns
	{
		private static DatabaseColumnBinder MemberSid = new DatabaseColumnBinder("SID");

		public static string Bind(DatabaseDataReader reader)
		{
			return MemberSid.GetString(reader, allowNulls: false);
		}
	}

	private static class RoleClaimColumns
	{
		private static DatabaseColumnBinder LoginProviderId = new DatabaseColumnBinder("LoginProviderId");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("ClaimName");

		private static DatabaseColumnBinder Value = new DatabaseColumnBinder("ClaimValue");

		public static Claim Bind(DatabaseDataReader reader)
		{
			Guid guid = LoginProviderId.GetGuid2(reader);
			string name = Name.GetString(reader);
			string value = Value.GetString(reader);
			return new Claim(guid, name, value);
		}
	}

	private static class UserUpdatedCacheColumns
	{
		private static DatabaseColumnBinder IDUser = new DatabaseColumnBinder("IDUser");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static UserUpdateInfo Bind(DatabaseDataReader reader)
		{
			Guid guid = IDUser.GetGuid2(reader);
			DateTime dateTime = LastModified.GetDateTime(reader);
			return new UserUpdateInfo(guid, dateTime);
		}
	}

	private static class ClientLoginTimeProfileColumns
	{
		private static DatabaseColumnBinder TimeProfileId = new DatabaseColumnBinder("time_profile_id");

		private static DatabaseColumnBinder IsDefault = new DatabaseColumnBinder("isDefault");

		public static Tuple<string, bool> Bind(DatabaseDataReader reader)
		{
			string item = TimeProfileId.GetString(reader, allowNulls: true);
			bool boolean = IsDefault.GetBoolean(reader);
			return new Tuple<string, bool>(item, boolean);
		}
	}

	public RoleComponent()
	{
	}

	public RoleComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	public Role[] QueryRoles()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_roles");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Role> list = new List<Role>();
			while (databaseDataReader.Read())
			{
				list.Add(RoleColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Role QueryRoleById(string roleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_role_by_id");
			BindGuidType("@IDRole", roleId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Role)null;
			}
			Role result = RoleColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Role QueryRoleByName(string name)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_role_by_name");
			BindNameType("@Name", name);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Role)null;
			}
			Role result = RoleColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string[] QueryRoleIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_role_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDRole");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid[] QueryRoleIdsByUserSidDynamic(string memberSid)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_role_ids_by_user_sid_dynamic");
			BindSIDType("@SID", memberSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Guid> list = new List<Guid>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDRole");
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseColumnBinder.GetString(databaseDataReader)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryUserSidDynamicByRoleId(string roleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_user_sid_dynamic_by_role_id");
			BindGuidType("@IDRole", roleId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				list.Add(RoleMemberColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryRoleIdsByClientProfileId(string parentId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_role_ids_by_client_profile_id");
			BindGuidType("@IDClientProfile", parentId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDRole");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Role[] QueryRolesByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_roles_by_ids");
			BindIdTable("@roleIds", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Role> list = new List<Role>();
			while (databaseDataReader.Read())
			{
				list.Add(RoleColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryRoleMembers(string roleId, bool includeDynamicallyAddedIdentities)
	{
		return CallRoleMembersQuery(roleId, includeDynamicallyAddedIdentities, "security_query_role_members");
	}

	public string[] QueryRoleMembersWithDeleted(string roleId, bool includeDynamicallyAddedIdentities)
	{
		return CallRoleMembersQuery(roleId, includeDynamicallyAddedIdentities, "security_query_role_members_with_deleted");
	}

	private string[] CallRoleMembersQuery(string roleId, bool includeDynamicallyAddedIdentities, string procName)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure(procName);
			BindGuidType("@IDRole", roleId, allowNullOrEmpty: false);
			BindBoolean("@IncludeDynamicallyAddedIdentities", includeDynamicallyAddedIdentities);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				list.Add(RoleMemberColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string CreateRole(Role newRole)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_create_role");
			BindGuidType("@IDRole", newRole.Id);
			BindNameType("@Name", newRole.Name);
			BindDescription("@Description", newRole.Description);
			BindBoolean("@DualAuthorizationRequired", newRole.DualAuthorizationRequired);
			BindBoolean("@PtzReservedAsAnonymous", newRole.ShowPtzReservedNameAsAnonymous);
			BindBoolean("@AllowMobileClientLogOn", newRole.AllowMobileClientLogOn);
			BindBoolean("@AllowSmartClientLogOn", newRole.AllowSmartClientLogOn);
			BindBoolean("@AllowWebClientLogOn", newRole.AllowWebClientLogOn);
			ExecuteNonQuery();
			return newRole.Id;
		});
	}

	public void UpdateRole(string roleId, string name, string description, bool? dualAuthorizationRequired, bool? showPtzReservedNameAsAnonymous, bool? allowMobileClientLogOn, bool? allowSmartClientLogOn, bool? allowWebClientLogOn, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_update_role");
			BindGuidType("@IDRole", roleId);
			BindNameType("@Name", name, allowNull: true);
			BindDescription("@Description", description, allowNull: true);
			BindBoolean("@DualAuthorizationRequired", dualAuthorizationRequired);
			BindBoolean("@PtzReservedAsAnonymous", showPtzReservedNameAsAnonymous);
			BindBoolean("@AllowMobileClientLogOn", allowMobileClientLogOn);
			BindBoolean("@AllowSmartClientLogOn", allowSmartClientLogOn);
			BindBoolean("@AllowWebClientLogOn", allowWebClientLogOn);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public string CopyRole(Role newRole, string sourceRoleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_copy_role");
			BindGuidType("@IDSourceRole", sourceRoleId);
			BindGuidType("@IDRole", newRole.Id);
			BindNameType("@Name", newRole.Name);
			BindDescription("@Description", newRole.Description);
			ExecuteNonQuery();
			return newRole.Id;
		});
	}

	public void DeleteRole(string roleId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_delete_role");
			BindGuidType("@IDRole", roleId);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void AddIdentityToRole(string roleId, Identity identity, bool isDynamic, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_add_role_member");
			BindGuidType("@IDRole", roleId);
			BindSIDType("@MemberSID", identity.Sid);
			BindByte("@Type", (byte)identity.Type);
			BindString("@DisplayName", identity.DisplayName, 512, allowNull: false, SqlDbType.NVarChar);
			BindString("@Description", Identity.GetFilteredDescription(identity), 2048, allowNull: true, SqlDbType.NVarChar);
			BindString("@Domain", identity.Domain, 512, allowNull: false, SqlDbType.NVarChar);
			BindString("@AccountName", identity.AccountName, 512, allowNull: false, SqlDbType.NVarChar);
			BindString("@DistinguishedName", identity.DistinguishedName, 2048, allowNull: false, SqlDbType.NVarChar);
			BindDateTime("@CurrentUtcTime", DateTime.UtcNow);
			BindBoolean("@IsDynamic", isDynamic);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void RemoveIdentityFromRole(string roleId, string memberSid, bool removeDynamic, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_remove_role_member");
			BindGuidType("@IDRole", roleId);
			BindSIDType("@MemberSID", memberSid);
			BindBoolean("@RemoveDynamic", removeDynamic);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void AddClaimToRole(string roleId, string claimProvider, string claimName, string claimValue, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_role_claim_add");
			BindGuidType("@IDRole", roleId);
			BindUniqueIdentifier("@LoginProviderId", claimProvider);
			BindString("@ClaimName", claimName, 128, allowNull: false, SqlDbType.NVarChar);
			BindString("@ClaimValue", claimValue, 512, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void RemoveClaimFromRole(string roleId, string claimProvider, string claimName, string claimValue, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_role_claim_remove");
			BindGuidType("@IDRole", roleId);
			BindUniqueIdentifier("@LoginProviderId", claimProvider);
			BindString("@ClaimName", claimName, 128, allowNull: false, SqlDbType.NVarChar);
			BindString("@ClaimValue", claimValue, 512, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public Claim[] QueryClaimsForRole(string roleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_role_claim_query_by_role");
			BindGuidType("@IDRole", roleId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Claim> list = new List<Claim>();
			while (databaseDataReader.Read())
			{
				list.Add(RoleClaimColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid[] QueryRolesByClaim(Guid claimProvider, string claimName, string claimValue)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_roles_by_claim");
			BindGuid("@LoginProviderId", claimProvider);
			BindString("@ClaimName", claimName, 128, allowNull: false, SqlDbType.NVarChar);
			BindString("@ClaimValue", claimValue, 512, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Guid> list = new List<Guid>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDRole");
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseColumnBinder.GetString(databaseDataReader)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Role[] QueryRolesByUserSid(string memberSid)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_roles_by_user_sid");
			BindSIDType("@SID", memberSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Role> list = new List<Role>();
			while (databaseDataReader.Read())
			{
				list.Add(RoleColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public UserUpdateInfo[] UserUpdatedCacheQueryWithCleanup()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_userupdatedcache_query_with_cleanup");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<UserUpdateInfo> list = new List<UserUpdateInfo>();
			while (databaseDataReader.Read())
			{
				list.Add(UserUpdatedCacheColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void UserUpdatedCacheCreateOrUpdate(Guid userId, DateTime lastModified)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_userupdatedcache_create_or_update");
			BindUniqueIdentifier("@IDUser", userId);
			BindDateTime2("@LastModified", lastModified);
			ExecuteNonQuery();
		});
	}

	public void UpdateClientLoginLegacyPermission(string roleId, string timeProfileId, bool isDefaultTimeProfile, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_update_client_login_legacy_permission");
			BindGuidType("@IDRole", roleId);
			BindString("@IDTimeProfile", timeProfileId, 36, allowNull: true, SqlDbType.Char);
			BindBoolean("@IsDefaultTimeProfile", isDefaultTimeProfile);
			BindGuidType("@Session_id", sessionId);
			ExecuteNonQuery();
		});
	}

	public string QueryClientLoginLegacyPermission(string roleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_client_login_legacy_permission");
			BindGuidType("@IDRole", roleId);
			return (string)ExecuteScalar();
		});
	}

	public Tuple<string, bool> QueryClientLoginLegacyPermissionId(string roleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_query_client_login_legacy_permission_id");
			BindGuidType("@IDRole", roleId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Tuple<string, bool> result = null;
			while (databaseDataReader.Read())
			{
				result = ClientLoginTimeProfileColumns.Bind(databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void UpdateDefaultTimeProfileId(string roleId, string timeProfileId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_default_time_profile_for_role");
			BindGuidType("@IDRole", roleId);
			BindGuidType("@IDTimeProfile", timeProfileId, allowNullOrEmpty: true);
			BindGuidType("@Session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public string QueryDefaultTimeProfileId(string roleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_default_time_profile_for_role");
			BindGuidType("@IDRole", roleId);
			return (string)ExecuteScalar();
		});
	}
}
