using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class HostedHardwareComponent : GeminiResourceComponent, IHostedHardwareComponent, IDisposable
{
	private static class HostedHardwareColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("type");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder UserName = new DatabaseColumnBinder("user_name");

		private static DatabaseColumnBinder Password = new DatabaseColumnBinder("password");

		private static DatabaseColumnBinder HostingInfo = new DatabaseColumnBinder("hosting_info");

		private static DatabaseColumnBinder ManufacturerId = new DatabaseColumnBinder("manufacturer_id");

		private static DatabaseColumnBinder CustomData = new DatabaseColumnBinder("custom_data");

		private static DatabaseColumnBinder HostingServerId = new DatabaseColumnBinder("hosting_server_id");

		public static HostedHardware Bind(DatabaseDataReader reader, IGeminiRequestContext requestContext)
		{
			return new HostedHardware
			{
				Id = Id.GetInt32(reader),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				Type = Type.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				UserName = UserName.GetString(reader),
				Password = requestContext.GetEncryptionService().Decrypt(Password.GetString(reader)),
				HostingInfo = HostingInfo.GetString(reader),
				ManufacturerId = ManufacturerId.GetString(reader),
				CustomData = CustomData.GetString(reader),
				HostingServerId = HostingServerId.GetInt32(reader)
			};
		}
	}

	public HostedHardware QueryHostedHardware(int hostedHardwareId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hosted_hardware_by_id");
			BindInt("@pkid", hostedHardwareId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (HostedHardware)null;
			}
			HostedHardware result = HostedHardwareColumns.Bind(databaseDataReader, RequestContext);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public HostedHardware[] QueryHostedHardwaresByIds(int[] hostedHardwareIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hosted_hardware_by_ids");
			BindIdTable("@hostedHardwareIds", hostedHardwareIds.Select((int f) => f.ToString(CultureInfo.InvariantCulture)).ToArray());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HostedHardware> list = new List<HostedHardware>();
			while (databaseDataReader.Read())
			{
				list.Add(HostedHardwareColumns.Bind(databaseDataReader, RequestContext));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryHostedHardwareIds(int hostingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hosted_hardware_ids");
			BindInt("@hosting_server_id", hostingServerId);
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

	public int CreateHostedHardware(HostedHardware hostedHardwareToCreate, Guid sessionId, string hostedHardwareType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_hosted_hardware");
			BindString("@type", hostedHardwareType, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@name", hostedHardwareToCreate.Name, 256, allowNull: false, SqlDbType.NVarChar);
			BindDescription("@description", hostedHardwareToCreate.Description);
			BindString("@user_name", hostedHardwareToCreate.UserName, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@password", RequestContext.GetEncryptionService().Encrypt(hostedHardwareToCreate.Password), 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@hosting_info", hostedHardwareToCreate.HostingInfo, int.MaxValue, allowNull: false, SqlDbType.NVarChar);
			BindString("@manufacturer_id", hostedHardwareToCreate.ManufacturerId, int.MaxValue, allowNull: false, SqlDbType.NVarChar);
			BindString("@custom_data", hostedHardwareToCreate.CustomData, int.MaxValue, allowNull: false, SqlDbType.NVarChar);
			BindInt("@hosting_server_id", hostedHardwareToCreate.HostingServerId);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", hostedHardwareToCreate.CustomProperties);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new DatabaseConfigMissingRowException();
			}
			HostedHardware hostedHardware = HostedHardwareColumns.Bind(databaseDataReader, RequestContext);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return hostedHardware.Id;
		});
	}

	public void UpdateHostedHardware(HostedHardware hostedHardware, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_hosted_hardware");
			BindInt("@pkid", hostedHardware.Id);
			BindString("@name", hostedHardware.Name, 256, allowNull: false, SqlDbType.NVarChar);
			BindDescription("@description", hostedHardware.Description);
			BindString("@user_name", hostedHardware.UserName, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@password", RequestContext.GetEncryptionService().Encrypt(hostedHardware.Password), 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@hosting_info", hostedHardware.HostingInfo, 1073741823, allowNull: false, SqlDbType.NText);
			BindString("@manufacturer_id", hostedHardware.ManufacturerId, int.MaxValue, allowNull: false, SqlDbType.NVarChar);
			BindString("@custom_data", hostedHardware.CustomData, 1073741823, allowNull: false, SqlDbType.NText);
			BindInt("@hosting_server_id", hostedHardware.HostingServerId);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", hostedHardware.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void DeleteHostedHardware(int hostedHardwareId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_hosted_hardware");
			BindInt("@pkid", hostedHardwareId);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
