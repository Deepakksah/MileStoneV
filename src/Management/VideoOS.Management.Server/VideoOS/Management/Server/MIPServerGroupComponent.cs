using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MIPServerGroupComponent : GeminiResourceComponent, IMIPServerGroupComponent, IDisposable
{
	private static class MIPServerGroupColumns
	{
		private static DatabaseColumnBinder IDServerGroup = new DatabaseColumnBinder("IDServerGroup");

		private static DatabaseColumnBinder IDParentGroup = new DatabaseColumnBinder("IDParentGroup");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Kind = new DatabaseColumnBinder("Kind");

		private static DatabaseColumnBinder PluginId = new DatabaseColumnBinder("PluginId");

		private static DatabaseColumnBinder CustomData = new DatabaseColumnBinder("CustomData");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("CreatedDate");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder RelationIds = new DatabaseColumnBinder("RelationIds");

		public static MIPServerGroup Bind(DatabaseDataReader reader)
		{
			return new MIPServerGroup
			{
				Id = IDServerGroup.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				ParentId = IDParentGroup.GetString(reader, allowNulls: true, string.Empty),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				Kind = Kind.GetString(reader, allowNulls: true, string.Empty),
				PluginId = PluginId.GetString(reader),
				CustomData = CustomData.GetString(reader, allowNulls: true),
				RelationIds = RelationIds.GetString(reader)
			};
		}
	}

	public MIPServerGroup[] QueryMIPServerGroupsByParentGroupId(string groupStructureType, string parentGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_groups_kind_by_parent_group_id");
			BindGuidType("@IDParentGroup", parentGroupId, allowNullOrEmpty: true);
			BindString("@StructureType", groupStructureType, 128, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPServerGroup> list = new List<MIPServerGroup>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPServerGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MIPServerGroup QueryMIPServerGroupByParentGroupId(string parentGroupId, string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_kind_by_parent_group_id");
			BindGuidType("@IDParentServerGroup", parentGroupId);
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			MIPServerGroup result = MIPServerGroupColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MIPServerGroup QueryMIPServerGroupById(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_group_kind_by_id");
			BindGuidType("@IDServerGroup", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ServerGroupNotFoundException();
			}
			MIPServerGroup result = MIPServerGroupColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MIPServerGroup[] QueryMIPServerGroupsByIds(string[] serverGroupIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_groups_kind_by_ids");
			BindIdTable("@serverGroupIds", serverGroupIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPServerGroup> list = new List<MIPServerGroup>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPServerGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateMIPServerGroup(MIPServerGroup serverGroup, string userSid, string structureType)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_serverkind_group");
			BindGuidType("@IDServerGroup", serverGroup.Id);
			BindNameType("@Name", serverGroup.Name);
			BindDescription("@Description", serverGroup.Description);
			BindGuidType("@Kind", serverGroup.Kind);
			BindGuidType("@PluginId", serverGroup.PluginId);
			BindString("@CustomData", serverGroup.CustomData, 8000, allowNull: true, SqlDbType.NVarChar);
			BindGuidType("@IDParentGroup", serverGroup.ParentId);
			BindString("@StructureType", structureType, 64, allowNull: false, SqlDbType.VarChar);
			BindSIDType("@UserSID", userSid);
			BindCustomProperties("@custom_properties", serverGroup.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateServerGroup(string id, string name, string description, string customData, CustomProperty[] customProperties)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_serverkind_group");
			BindGuidType("@IDServerGroup", id);
			BindNameType("@Name", name);
			BindDescription("@Description", description);
			BindString("@CustomData", customData, 8000, allowNull: true, SqlDbType.NVarChar);
			BindCustomProperties("@custom_properties", customProperties);
			ExecuteNonQuery();
		});
	}

	public void DeleteMIPServerGroup(string serverGroupId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_serverkind_group");
			BindGuidType("@IDServerGroup", serverGroupId);
			ExecuteNonQuery();
		});
	}
}
