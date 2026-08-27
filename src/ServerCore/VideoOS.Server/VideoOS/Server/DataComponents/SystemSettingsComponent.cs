using System;
using VideoOS.Utilities.Database;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Server.DataComponents;

public sealed class SystemSettingsComponent : VmoResourceComponent, ISystemSettingsComponent, IDisposable
{
	public SystemSettingsComponent()
	{
	}

	public string QuerySystemSettingByName(SettingList setting)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_system_setting");
			BindNameType("@settingName", setting.ToString());
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Setting");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return string.Empty;
			}
			string result = databaseColumnBinder.GetString(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}
}
