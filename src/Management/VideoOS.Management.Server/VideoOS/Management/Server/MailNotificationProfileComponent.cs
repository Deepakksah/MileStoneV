using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MailNotificationProfileComponent : GeminiResourceComponent, IMailNotificationProfileComponent, IDisposable
{
	private static class EmailNotificationProfileColumns
	{
		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Settings = new DatabaseColumnBinder("Settings");

		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDNotificationProfile");

		public static MailNotificationProfile Bind(DatabaseDataReader reader)
		{
			return new MailNotificationProfile
			{
				LastModified = LastModified.GetDateTime(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				SettingsXml = Settings.GetString(reader),
				ProfileId = Id.GetStringAsGuid(reader)
			};
		}
	}

	public MailNotificationProfile[] QueryProfiles()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_mailnotificationprofiles");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MailNotificationProfile> list = new List<MailNotificationProfile>();
			while (databaseDataReader.Read())
			{
				list.Add(EmailNotificationProfileColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public MailNotificationProfile QueryProfile(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_mailnotificationprofile");
			BindUniqueIdentifier("@IDNotificationProfile", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MailNotificationProfile)null;
			}
			MailNotificationProfile result = EmailNotificationProfileColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MailNotificationProfile UpdateProfile(Guid id, string name, string description, string settingsXML)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_mailnotificationprofile");
			BindGuid("@IDNotificationProfile", id);
			BindString("@Name", name, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@Description", description, 9999, allowNull: false, SqlDbType.NVarChar);
			BindString("@SettingsXML", settingsXML, 9999, allowNull: false, SqlDbType.NVarChar);
			ExecuteNonQuery();
		});
		return QueryProfile(id);
	}

	public MailNotificationProfile CreateProfile(Guid id, string name, string description, string settingsXML, Guid settingsId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_mailnotificationprofile");
			BindGuid("@IDNotificationProfile", id);
			BindString("@Name", name, 256, allowNull: false, SqlDbType.NVarChar);
			BindString("@Description", description, 9999, allowNull: false, SqlDbType.NVarChar);
			BindString("@SettingsXML", settingsXML, 9999, allowNull: false, SqlDbType.NVarChar);
			BindGuid("@IDNotificationSetting", settingsId);
			ExecuteNonQuery();
		});
		return QueryProfile(id);
	}

	public void DeleteProfile(Guid id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_mailnotificationprofile");
			BindGuid("@IDNotificationProfile", id);
			ExecuteNonQuery();
		});
	}
}
