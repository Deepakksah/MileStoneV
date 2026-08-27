using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class ClientProfileComponent : GeminiResourceComponent, IClientProfileComponent, IDisposable
{
	private static class ClientProfileColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDClientProfile");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder Settings = new DatabaseColumnBinder("Settings");

		private static DatabaseColumnBinder DefaultProfile = new DatabaseColumnBinder("DefaultProfile");

		private static DatabaseColumnBinder ProfileType = new DatabaseColumnBinder("profile_type");

		public static ClientProfile Bind(DatabaseDataReader reader, IGeminiRequestContext requestContext)
		{
			return new ClientProfile
			{
				Id = Id.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				LastModified = LastModified.GetDateTime(reader),
				SettingsXml = ClientProfileEncryptionHelper.ClientProfileSettingsDecrypt(Settings.GetString(reader), GeminiApplicationCore.ClientProfileSpecificationCacheManager, requestContext),
				IsDefaultProfile = DefaultProfile.GetBoolean(reader),
				Profiletype = (ClientProfileType)Enum.Parse(typeof(ClientProfileType), ProfileType.GetString(reader), ignoreCase: true)
			};
		}
	}

	public string[] QueryClientProfileIds(ClientProfileType profileType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_client_profile_ids");
			BindString("@profile_type", profileType.ToString(), 50, allowNull: false, SqlDbType.VarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDClientProfile");
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

	public ClientProfile QueryClientProfileById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_client_profile_by_id");
			BindGuidType("@IDClientProfile", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ClientProfileNotExistException();
			}
			ClientProfile result = ClientProfileColumns.Bind(databaseDataReader, RequestContext);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ClientProfile[] QueryClientProfilesByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_client_profiles_by_ids");
			BindIdTable("@ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ClientProfile> list = new List<ClientProfile>();
			while (databaseDataReader.Read())
			{
				list.Add(ClientProfileColumns.Bind(databaseDataReader, RequestContext));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateClientProfile(string id, string name, string description, string settings, ClientProfileType profileType)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_client_profile");
			BindGuidType("@IDClientProfile", id);
			BindNameType("@name", name);
			BindDescription("@description", description);
			BindDescription("@settings", ClientProfileEncryptionHelper.ClientProfileSettingsEncrypt(settings, GeminiApplicationCore.ClientProfileSpecificationCacheManager, RequestContext));
			BindString("@profile_type", profileType.ToString(), 50, allowNull: false, SqlDbType.VarChar);
			ExecuteNonQuery();
		});
	}

	public void UpdateClientProfile(string id, string name, string description, string settings)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_client_profile");
			BindGuidType("@IDClientProfile", id);
			BindNameType("@name", name);
			BindDescription("@description", description);
			BindDescription("@settings", ClientProfileEncryptionHelper.ClientProfileSettingsEncrypt(settings, GeminiApplicationCore.ClientProfileSpecificationCacheManager, RequestContext));
			ExecuteNonQuery();
		});
	}

	public void DeleteClientProfile(string id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_client_profile");
			BindGuidType("@IDClientProfile", id);
			ExecuteNonQuery();
		});
	}

	public void MovePriorityUp(string id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_client_profile_priority_up");
			BindGuidType("@IDClientProfile", id);
			ExecuteNonQuery();
		});
	}

	public void MovePriorityDown(string id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_client_profile_priority_down");
			BindGuidType("@IDClientProfile", id);
			ExecuteNonQuery();
		});
	}

	public string QueryActiveProfileId(string userSid, ClientProfileType profileType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_client_profile_id_by_user_sid");
			BindSIDType("@userSID", userSid);
			BindString("@profile_type", profileType.ToString(), 50, allowNull: false, SqlDbType.VarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDClientProfile");
			if (!databaseDataReader.Read())
			{
				throw new ClientProfileNotExistException();
			}
			string result = databaseColumnBinder.GetString(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string QueryProfileIdByRoleId(string roleId, ClientProfileType profileType)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_client_profile_id_by_role_id");
			BindSIDType("@IDRole", roleId);
			BindString("@profile_type", profileType.ToString(), 50, allowNull: false, SqlDbType.VarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDClientProfile");
			if (!databaseDataReader.Read())
			{
				throw new ClientProfileNotExistException();
			}
			string result = databaseColumnBinder.GetString(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void AttachClientProfileToRole(string id, string roleId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_client_profile_attach_to_role");
			BindGuidType("@IDClientProfile", id);
			BindGuidType("@IDRole", roleId);
			BindGuidType("@Session_id", sessionId);
			ExecuteNonQuery();
		});
	}
}
