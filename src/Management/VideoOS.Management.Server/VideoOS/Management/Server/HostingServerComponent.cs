using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class HostingServerComponent : GeminiResourceComponent, IHostingServerComponent, IDisposable
{
	private static class HostingServerColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("type");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder UserName = new DatabaseColumnBinder("user_name");

		private static DatabaseColumnBinder Password = new DatabaseColumnBinder("password");

		private static DatabaseColumnBinder InternalAddress = new DatabaseColumnBinder("internal_address");

		private static DatabaseColumnBinder ExternalAddress = new DatabaseColumnBinder("external_address");

		public static HostingServer Bind(DatabaseDataReader reader, IGeminiRequestContext requestContext)
		{
			return new HostingServer
			{
				Id = Id.GetInt32(reader),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				Type = Type.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				UserName = UserName.GetString(reader),
				Password = requestContext.GetEncryptionService().Decrypt(Password.GetString(reader)),
				InternalAddress = InternalAddress.GetString(reader),
				ExternalAddress = ExternalAddress.GetString(reader)
			};
		}
	}

	public HostingServer QueryHostingServer(int hostingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hosting_server_by_id");
			BindInt("@pkid", hostingServerId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (HostingServer)null;
			}
			HostingServer result = HostingServerColumns.Bind(databaseDataReader, RequestContext);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public HostingServer[] QueryHostingServersByIds(int[] hostingServerIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hosting_server_by_ids");
			BindIdTable("@hostingServerIds", hostingServerIds.Select((int f) => f.ToString(CultureInfo.InvariantCulture)).ToArray());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HostingServer> list = new List<HostingServer>();
			while (databaseDataReader.Read())
			{
				list.Add(HostingServerColumns.Bind(databaseDataReader, RequestContext));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryHostingServerIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hosting_server_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("pkid");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetInt32(databaseDataReader).ToString(CultureInfo.InvariantCulture));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public int CreateHostingServer(HostingServer hostingServerToCreate, Guid sessionId, string hostingServerType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_hosting_server");
			BindString("@type", hostingServerType, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@name", hostingServerToCreate.Name, 256, allowNull: false, SqlDbType.NVarChar);
			BindDescription("@description", hostingServerToCreate.Description);
			BindString("@user_name", hostingServerToCreate.UserName, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@password", RequestContext.GetEncryptionService().Encrypt(hostingServerToCreate.Password), 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@internal_address", hostingServerToCreate.InternalAddress, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@external_address", hostingServerToCreate.ExternalAddress, 256, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", hostingServerToCreate.CustomProperties);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new DatabaseConfigMissingRowException();
			}
			HostingServer hostingServer = HostingServerColumns.Bind(databaseDataReader, RequestContext);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return hostingServer.Id;
		});
	}

	public void UpdateHostingServer(HostingServer hostingServer, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_hosting_server");
			BindInt("@pkid", hostingServer.Id);
			BindString("@name", hostingServer.Name, 256, allowNull: false, SqlDbType.NVarChar);
			BindDescription("@description", hostingServer.Description);
			BindString("@user_name", hostingServer.UserName, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@password", RequestContext.GetEncryptionService().Encrypt(hostingServer.Password), 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@internal_address", hostingServer.InternalAddress, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@external_address", hostingServer.ExternalAddress, 256, allowNull: false, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", hostingServer.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void DeleteHostingServer(int hostingServerId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_hosting_server");
			BindInt("@pkid", hostingServerId);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
