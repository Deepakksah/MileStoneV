using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;
using VideoOS.Vms.Utilities.SystemSettings;

namespace VideoOS.Management.Server;

public class ConfigurationDataComponent : GeminiResourceComponent, IConfigurationDataComponent, IDisposable
{
	private static class SingleStringColumn
	{
		private static DatabaseColumnBinder StringResult = new DatabaseColumnBinder("StringResult");

		public static string Bind(DatabaseDataReader reader)
		{
			return StringResult.GetString(reader);
		}
	}

	private static class SingleIDColumn
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("ID");

		public static Guid Bind(DatabaseDataReader reader)
		{
			return new Guid(ID.GetGuid(reader));
		}
	}

	private static class IdNameDescriptionDataColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("ID");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		public static IdNameDescription Bind(DatabaseDataReader reader)
		{
			return new IdNameDescription
			{
				Id = new Guid(ID.GetGuid(reader)),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader)
			};
		}
	}

	private static class AudioMessageIdNameDescriptionDataColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("ID");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		public static AudioMessageIdNameDescription Bind(DatabaseDataReader reader)
		{
			return new AudioMessageIdNameDescription
			{
				Id = new Guid(ID.GetGuid(reader)),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader)
			};
		}
	}

	public string GetClientSettingsXml()
	{
		string sql = string.Format(CultureInfo.InvariantCulture, "SELECT [Setting] AS [StringResult] FROM [SystemSettings] WHERE [Name] = '{0}'", SettingList.SmartClientSettings);
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(VmoResources.VMO61005);
			}
			string result = SingleStringColumn.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(VmoResources.VMO61004);
			}
			return result;
		});
	}

	public IdNameDescription FindHomeSiteInfo()
	{
		string sql = "SELECT [IDSite] AS [ID], [Name], [Description] FROM [Sites] WHERE [IDSiteType] = '1'";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(VmoResources.VMO61005);
			}
			IdNameDescription result = IdNameDescriptionDataColumns.Bind(databaseDataReader);
			if (databaseDataReader.Read())
			{
				throw new ConfigurationDataSubsystemException(VmoResources.VMO61004);
			}
			return result;
		});
	}

	public AudioMessageIdNameDescription[] FindAudioMessages()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			List<AudioMessageIdNameDescription> list = new List<AudioMessageIdNameDescription>();
			string text = "SELECT [ID], [Name], [Description] FROM [AudioMessage]";
			PrepareSqlBatch(text.Length);
			AddStatement(text);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(AudioMessageIdNameDescriptionDataColumns.Bind(databaseDataReader));
			}
			return list.ToArray();
		});
	}

	public bool FindPrivacyMaskUsed()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			string text = "SELECT TOP 1 [Enabled] from [DevicePrivacyMask] WHERE [Enabled] = 1";
			PrepareSqlBatch(text.Length);
			AddStatement(text);
			return ExecuteReader().Read() ? true : false;
		});
	}
}
