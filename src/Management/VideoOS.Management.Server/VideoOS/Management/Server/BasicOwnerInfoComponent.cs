using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class BasicOwnerInfoComponent : GeminiResourceComponent, IBasicOwnerInfoComponent, IDisposable
{
	private static class BasicOwnerInfoColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("id");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Property = new DatabaseColumnBinder("property");

		private static DatabaseColumnBinder Order = new DatabaseColumnBinder("order");

		public static BasicOwnerInfo Bind(DatabaseDataReader reader)
		{
			return new BasicOwnerInfo
			{
				Id = Id.GetString(reader),
				Name = Name.GetString(reader),
				Property = Property.GetString(reader),
				Order = Order.GetString(reader)
			};
		}
	}

	public BasicOwnerInfoComponent()
	{
	}

	public BasicOwnerInfoComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	public BasicOwnerInfo[] QueryBasicOwnerInfo()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_basic_infos");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<BasicOwnerInfo> list = new List<BasicOwnerInfo>();
			while (databaseDataReader.Read())
			{
				list.Add(BasicOwnerInfoColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public BasicOwnerInfo QueryBasicOwnerInfoById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_basic_info_by_id");
			BindGuidType("@id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new BasicOwnerInfoNotExistException();
			}
			BasicOwnerInfo result = BasicOwnerInfoColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public BasicOwnerInfo[] QueryBasicOwnerInfosByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_basic_info_by_ids");
			BindIdTable("@ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<BasicOwnerInfo> list = new List<BasicOwnerInfo>();
			while (databaseDataReader.Read())
			{
				list.Add(BasicOwnerInfoColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryBasicOwnerInfoIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_basic_info_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("id");
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

	public void CreateBasicOwnerInfo(string id, string name, string property, string order)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_basic_info");
			BindGuidType("@id", id);
			BindString("@name", name, 256, allowNull: true, SqlDbType.NVarChar);
			BindString("@property", property, 256, allowNull: true, SqlDbType.NVarChar);
			BindString("@order", order, 50, allowNull: true, SqlDbType.NVarChar);
			ExecuteNonQuery();
		});
	}

	public void UpdateBasicOwnerInfo(string id, string name, string property, string order, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_basic_info");
			BindGuidType("@id", id);
			BindString("@name", name, 256, allowNull: true, SqlDbType.NVarChar);
			BindString("@property", property, 256, allowNull: true, SqlDbType.NVarChar);
			BindString("@order", order, 50, allowNull: true, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeleteBasicOwnerInfo(string id, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_basic_info");
			BindGuidType("@id", id);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
