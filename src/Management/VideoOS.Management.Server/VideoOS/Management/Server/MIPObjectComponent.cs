using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MIPObjectComponent : GeminiResourceComponent, IMIPObjectComponent, IDisposable
{
	private static class MIPObjectColumns
	{
		private static DatabaseColumnBinder IdMIPObject = new DatabaseColumnBinder("IDMIPObject");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Kind = new DatabaseColumnBinder("Kind");

		private static DatabaseColumnBinder SubKind = new DatabaseColumnBinder("SubKind");

		private static DatabaseColumnBinder PluginId = new DatabaseColumnBinder("PluginId");

		private static DatabaseColumnBinder CustomData = new DatabaseColumnBinder("CustomData");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("CreatedDate");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder RelationIds = new DatabaseColumnBinder("RelationIds");

		public static MIPObject Bind(DatabaseDataReader reader)
		{
			return new MIPObject
			{
				Id = IdMIPObject.GetGuid2(reader).ToString(),
				Name = Name.GetString(reader),
				Kind = Kind.GetGuid2(reader).ToString(),
				SubKind = SubKind.GetGuid2(reader).ToString(),
				PluginId = PluginId.GetGuid2(reader).ToString(),
				CustomData = CustomData.GetString(reader, allowNulls: true),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				RelationIds = RelationIds.GetString(reader)
			};
		}

		public static MIPObject BindNoRelations(DatabaseDataReader reader)
		{
			return new MIPObject
			{
				Id = IdMIPObject.GetGuid2(reader).ToString(),
				Name = Name.GetString(reader),
				Kind = Kind.GetGuid2(reader).ToString(),
				SubKind = SubKind.GetGuid2(reader).ToString(),
				PluginId = PluginId.GetGuid2(reader).ToString(),
				CustomData = CustomData.GetString(reader, allowNulls: true),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				RelationIds = string.Empty
			};
		}
	}

	public MIPObject[] QueryMIPObjects(string parentId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_mip_objects");
			BindBoolean("@FilterOnServerGroup", !string.IsNullOrWhiteSpace(parentId));
			BindGuidType("@IDServerGroup", parentId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPObject> list = new List<MIPObject>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPObjectColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public MIPObject QueryMIPObject(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_mip_object_by_id");
			BindUniqueIdentifier("@IDMIPObject", new Guid(id));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new MIPObjectNotFoundException();
			}
			MIPObject result = MIPObjectColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MIPObject[] QueryMIPObjectsByIds(string[] mipObjectIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_mip_object_by_ids");
			BindIdTable("@MIPObjectIds", mipObjectIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPObject> list = new List<MIPObject>();
			int num = 0;
			while (databaseDataReader.Read())
			{
				list.Add(MIPObjectColumns.Bind(databaseDataReader));
				num++;
			}
			if (num != mipObjectIds.Length)
			{
				throw new MIPObjectNotFoundException();
			}
			return list.ToArray();
		});
	}

	public MIPObject[] QueryMIPObjectsByKind(string kindId, string subKind)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_mip_objects_by_kind");
			BindUniqueIdentifier("@Kind", new Guid(kindId));
			BindString("@SubKind", subKind, 36, allowNull: true, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MIPObject> list = new List<MIPObject>();
			while (databaseDataReader.Read())
			{
				list.Add(MIPObjectColumns.BindNoRelations(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public string CreateMIPObject(string id, string parentId, string name, string kind, string subKind, string pluginId, string customData)
	{
		if (string.IsNullOrEmpty(id))
		{
			id = Guid.NewGuid().ToString();
		}
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_create_mip_object");
			BindUniqueIdentifier("@pkid", id);
			BindGuidType("@parent_id", parentId);
			BindNameType("@Name", name);
			BindUniqueIdentifier("@Kind", kind);
			BindUniqueIdentifier("@SubKind", subKind);
			BindUniqueIdentifier("@PluginId", pluginId);
			BindString("@CustomData", customData, 8000, allowNull: true, SqlDbType.NVarChar);
			ExecuteNonQuery();
			return id;
		});
	}

	public void UpdateMIPObject(string id, string name, string customData)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_mip_object");
			BindUniqueIdentifier("@pkid", new Guid(id));
			BindNameType("@Name", name);
			BindString("@CustomData", customData, 8000, allowNull: true, SqlDbType.NVarChar);
			ExecuteNonQuery();
		});
	}

	public void DeleteMIPObject(string mipObjectId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_delete_mip_object");
			BindUniqueIdentifier("@IDMIPObject", new Guid(mipObjectId));
			ExecuteNonQuery();
		});
	}
}
