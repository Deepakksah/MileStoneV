using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class ServerGroupComponent : GeminiResourceComponent, IServerGroupComponent, IDisposable
{
	private static class ServerGroupColumns
	{
		private static DatabaseColumnBinder IDServerGroup = new DatabaseColumnBinder("IDServerGroup");

		private static DatabaseColumnBinder IDParentGroup = new DatabaseColumnBinder("IDParentGroup");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("CreatedDate");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder StructureType = new DatabaseColumnBinder("StructureType");

		private static DatabaseColumnBinder RelationIds = new DatabaseColumnBinder("RelationIds");

		public static ServerGroup Bind(DatabaseDataReader reader)
		{
			return new ServerGroup
			{
				Id = IDServerGroup.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				ParentId = IDParentGroup.GetGuid(reader, allowNulls: true, string.Empty),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				StructureType = ServerGroup.ConvertFromStringStructureTypeToEnumStructureType(StructureType.GetString(reader)),
				RelationIds = RelationIds.GetString(reader)
			};
		}
	}

	private static class IdColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		public static string Bind(DatabaseDataReader reader)
		{
			return ID.GetString(reader);
		}
	}

	private static class ParentIdColumns
	{
		private static DatabaseColumnBinder parentID = new DatabaseColumnBinder("IDParentGroup");

		public static string Bind(DatabaseDataReader reader)
		{
			return parentID.GetString(reader);
		}
	}

	private static class StructureTypeColumns
	{
		private static DatabaseColumnBinder StructureType = new DatabaseColumnBinder("StructureType");

		public static string Bind(DatabaseDataReader reader)
		{
			return StructureType.GetString(reader);
		}
	}

	private static class ServerGroupMembershipObjectColumns
	{
		private static DatabaseColumnBinder IDServerGroup = new DatabaseColumnBinder("IDServerGroup");

		public static string Bind(DatabaseDataReader reader)
		{
			return IDServerGroup.GetString(reader);
		}
	}

	private static class ServerGroupMembershipItemObjectColumns
	{
		private static DatabaseColumnBinder IDObject = new DatabaseColumnBinder("IDObject");

		public static string Bind(DatabaseDataReader reader)
		{
			return IDObject.GetString(reader);
		}
	}

	private static class CreatedColumns
	{
		private static DatabaseColumnBinder Created = new DatabaseColumnBinder("Created");

		public static bool Bind(DatabaseDataReader reader)
		{
			return Created.GetBoolean(reader);
		}
	}

	public ServerGroup[] QueryServerGroupHierarchyById(string groupStructureType, string parentGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_hierarchy_by_parent_group_id");
			BindGuidType("@IDParentGroup", parentGroupId, allowNullOrEmpty: true);
			BindString("@StructureType", groupStructureType, 128, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ServerGroup> list = new List<ServerGroup>();
			while (databaseDataReader.Read())
			{
				list.Add(ServerGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ServerGroup[] QueryServerGroupAllNoSecurity(string groupStructureType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_groups");
			BindString("@StructureType", groupStructureType, 128, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ServerGroup> list = new List<ServerGroup>();
			while (databaseDataReader.Read())
			{
				list.Add(ServerGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ServerGroup[] QueryServerGroupsByParentGroupId(string groupStructureType, string parentGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_groups_by_parent_group_id");
			BindGuidType("@IDParentGroup", parentGroupId, allowNullOrEmpty: true);
			BindString("@StructureType", groupStructureType, 128, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ServerGroup> list = new List<ServerGroup>();
			while (databaseDataReader.Read())
			{
				list.Add(ServerGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ServerGroup QueryServerGroupByParentGroupId(string parentGroupId, string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_by_parent_group_id");
			BindGuidType("@IDParentServerGroup", parentGroupId);
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			ServerGroup result = ServerGroupColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ServerGroup QueryServerGroupById(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_by_id");
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			ServerGroup result = ServerGroupColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ServerGroup[] QueryServerGroupsByIds(string[] serverGroupIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_groups_by_ids");
			BindIdTable("@serverGroupIds", serverGroupIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ServerGroup> list = new List<ServerGroup>();
			while (databaseDataReader.Read())
			{
				list.Add(ServerGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ServerGroup QueryPrivateViewGroup(string userSid, out bool created)
	{
		ServerGroup serverGroup;
		(serverGroup, created) = base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_private_view_group");
			BindUserIdType("@UserSid", userSid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			serverGroup = ServerGroupColumns.Bind(databaseDataReader);
			if (!databaseDataReader.NextResult())
			{
				throw new ServerGroupNotFoundException();
			}
			databaseDataReader.Read();
			bool item = CreatedColumns.Bind(databaseDataReader);
			return (serverGroup: serverGroup, isCreated: item);
		});
		return serverGroup;
	}

	public string QueryServerGroupRoot(string structureType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_root");
			BindGuidType("@StructureType", structureType);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			string result = databaseDataReader.GetString(databaseDataReader.GetOrdinal("IDServerGroup"));
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void CreateServerGroup(ServerGroup serverGroup, string structureType, string userSid)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_server_group");
			BindGuidType("@IDServerGroup", serverGroup.Id);
			BindNameType("@Name", serverGroup.Name);
			BindDescription("@Description", serverGroup.Description);
			BindGuidType("@IDParentGroup", serverGroup.ParentId);
			BindString("@StructureType", structureType, 64, allowNull: false, SqlDbType.VarChar);
			BindSIDType("@user_sid", userSid);
			BindCustomProperties("@custom_properties", serverGroup.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateServerGroup(string id, string name, string description, CustomProperty[] customProperties)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_server_group");
			BindGuidType("@IDServerGroup", id);
			BindNameType("@Name", name);
			BindDescription("@Description", description);
			BindCustomProperties("@custom_properties", customProperties);
			ExecuteNonQuery();
		});
	}

	public void DeleteServerGroup(string serverGroupId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_server_group");
			BindGuidType("@IDServerGroup", serverGroupId);
			ExecuteNonQuery();
		});
	}

	public string GetIdFromName(string parentId, string name)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("get_server_group_id_from_name");
			BindGuidType("@parent_id", parentId);
			BindNameType("@name", name);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			string result = IdColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string GetParentServerGroupId(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_parent_server_group_id");
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			string result = ParentIdColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string GetGroupStructure(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_structure_type");
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			string result = StructureTypeColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string GetServerGroupIdFromItem(string objectId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_id_by_member_ship_object_id");
			BindGuidType("@IDObject", objectId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			string result = ServerGroupMembershipObjectColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string GetItemFromServerGroupId(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_id_by_member_ship_server_group_id");
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			string result = ServerGroupMembershipItemObjectColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string FindRootViewGroupId(string viewGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("find_root_group_from_viewgroup");
			BindGuidType("@servergroup_id", viewGroupId);
			object obj = ExecuteScalar();
			return (obj == null) ? null : ((string)obj);
		});
	}
}
