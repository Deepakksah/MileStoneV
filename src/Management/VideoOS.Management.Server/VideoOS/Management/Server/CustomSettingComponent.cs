using System;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class CustomSettingComponent : GeminiResourceComponent, ICustomSettingComponent, IDisposable
{
	private static class CustomSettingColumns
	{
		private static DatabaseColumnBinder IDCustomSetting = new DatabaseColumnBinder("IDCustomSetting");

		private static DatabaseColumnBinder ModifiedUser = new DatabaseColumnBinder("ModifiedUser");

		private static DatabaseColumnBinder ModifiedDate = new DatabaseColumnBinder("ModifiedDate");

		private static DatabaseColumnBinder DataVersion = new DatabaseColumnBinder("DataVersion");

		private static DatabaseColumnBinder SettingDataXml = new DatabaseColumnBinder("SettingDataXml");

		public static CustomSetting Bind(DatabaseDataReader reader, IGeminiRequestContext requestContext)
		{
			return new CustomSetting
			{
				Id = new Guid(IDCustomSetting.GetGuid(reader)),
				LastModifiedUser = ModifiedUser.GetString(reader),
				LastModifiedTime = ModifiedDate.GetDateTime(reader),
				DataVersion = DataVersion.GetInt32(reader),
				CustomSettingDataXml = requestContext.GetEncryptionService().Decrypt(SettingDataXml.GetString(reader))
			};
		}
	}

	public CustomSetting GetCustomSetting(string userSid, Guid customSettingId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("get_custom_setting");
			BindSIDType("@user_sid", userSid);
			BindGuidType("@setting_id", customSettingId.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (CustomSetting)null;
			}
			CustomSetting result = CustomSettingColumns.Bind(databaseDataReader, RequestContext);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void UpdateCustomSetting(string userSid, Guid customSettingId, int dataVersion, string settingDataXml, string modifiedUser, bool forceOverride)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_custom_setting");
			BindSIDType("@user_sid", userSid);
			BindGuidType("@setting_id", customSettingId.ToString());
			BindInt("@data_version", dataVersion);
			BindStringMax("@setting_data_xml", RequestContext.GetEncryptionService().Encrypt(settingDataXml));
			BindString("@modified_user", modifiedUser, 256, allowNull: false, SqlDbType.VarChar);
			BindBoolean("@force_override", forceOverride);
			ExecuteNonQuery();
		});
	}
}
