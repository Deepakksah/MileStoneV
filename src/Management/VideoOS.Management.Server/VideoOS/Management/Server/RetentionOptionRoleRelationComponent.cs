using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RetentionOptionRoleRelationComponent : GeminiResourceComponent, IRetentionOptionRoleRelationComponent, IDisposable
{
	private static class RetentionOptionRoleRelationColumns
	{
		private static DatabaseColumnBinder IdRole = new DatabaseColumnBinder("IdRole");

		private static DatabaseColumnBinder IdRetentionOption = new DatabaseColumnBinder("IdRetentionOption");

		public static RetentionOptionRoleRelation Bind(DatabaseDataReader reader)
		{
			return new RetentionOptionRoleRelation
			{
				IdRole = IdRole.GetString(reader),
				IdRetention = IdRetentionOption.GetString(reader)
			};
		}
	}

	public RetentionOptionRoleRelation QueryRetentionOptionByRoleId(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_retention_option_relation_by_idrole");
			BindGuidType("@idRole", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new RetentionOptionRoleRelationNotExistException();
			}
			RetentionOptionRoleRelation result = RetentionOptionRoleRelationColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read() || databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void CreateRetentionRelation(string idRole, string idRetention)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_retention_option_relation");
			BindGuidType("@idRetentionOption", idRetention);
			BindGuidType("@idRole", idRole);
			ExecuteNonQuery();
		});
	}

	public void UpdateRetentionRelation(string idRole, string idRetention, string retentionName, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_retention_option_relation");
			BindGuidType("@id", idRole);
			BindGuidType("@idRetentionOption", idRetention);
			BindLongNameType("@retentionName", retentionName);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeleteRetentionRelation(string idRole)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_retention_option_relation");
			BindGuidType("@idRole", idRole);
			ExecuteNonQuery();
		});
	}

	public RetentionOptionRoleRelation[] QueryRetentionOptionRelations()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_retention_option_relations");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RetentionOptionRoleRelation> list = new List<RetentionOptionRoleRelation>();
			while (databaseDataReader.Read())
			{
				list.Add(RetentionOptionRoleRelationColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public RetentionOptionRoleRelation[] QueryRetentionOptionRoleRelationsByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_retention_option_relation_by_ids");
			BindIdTable("@ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RetentionOptionRoleRelation> list = new List<RetentionOptionRoleRelation>();
			while (databaseDataReader.Read())
			{
				list.Add(RetentionOptionRoleRelationColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryRetentionOptionRoleRelationIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_retention_option_relation_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IdRole");
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
}
