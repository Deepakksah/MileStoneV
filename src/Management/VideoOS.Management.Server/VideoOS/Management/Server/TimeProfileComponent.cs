using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class TimeProfileComponent : GeminiResourceComponent, ITimeProfileComponent, IDisposable
{
	private static class TimeProfileColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("type");

		private static DatabaseColumnBinder XmlData = new DatabaseColumnBinder("xml_data");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		public static TimeProfile Bind(DatabaseDataReader reader)
		{
			TimeProfile obj = new TimeProfile
			{
				Id = Id.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true)
			};
			string databaseType = Type.GetString(reader);
			obj.Type = TimeProfileTypeFromDatabaseType(databaseType);
			obj.DefinitionXml = XmlData.GetString(reader);
			obj.CreatedDate = CreatedDate.GetDateTime(reader);
			obj.LastModified = LastModified.GetDateTime(reader);
			return obj;
		}
	}

	private enum DatabaseTimeProfileType
	{
		TimeProfileBase,
		SunclockProfile,
		CalendarProfile
	}

	public TimeProfile QueryTimeProfile(string timeProfileId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_time_profile_by_id");
			BindGuidType("@pkid", timeProfileId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (TimeProfile)null;
			}
			TimeProfile result = TimeProfileColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public TimeProfile[] QueryTimeProfiles()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_time_profiles");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<TimeProfile> list = new List<TimeProfile>();
			while (databaseDataReader.Read())
			{
				list.Add(TimeProfileColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryTimeProfileIds()
	{
		return QueryTimeProfileIds(DatabaseTimeProfileType.TimeProfileBase);
	}

	public string[] QueryTimeProfileIds(TimeProfileType type)
	{
		return QueryTimeProfileIds(DatabaseTypeFromTimeProfileType(type));
	}

	private string[] QueryTimeProfileIds(DatabaseTimeProfileType type)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_time_profile_ids");
			BindString("@type", type.ToString(), 15, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("pkid");
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

	public TimeProfile[] QueryTimeProfilesByIds(string[] timeProfileIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_time_profiles_by_ids");
			BindIdTable("@timeProfileIds", timeProfileIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<TimeProfile> list = new List<TimeProfile>();
			while (databaseDataReader.Read())
			{
				list.Add(TimeProfileColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateTimeProfile(TimeProfile timeProfile)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_time_profile");
			BindGuidType("@pkid", timeProfile.Id);
			BindNameType("@name", timeProfile.Name);
			BindDescription("@description", timeProfile.Description);
			BindString("@type", DatabaseTypeFromTimeProfileType(timeProfile.Type).ToString(), 15, allowNull: false, SqlDbType.NVarChar);
			BindXml("@xml_data", timeProfile.DefinitionXml);
			BindCustomProperties("@custom_properties", timeProfile.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateTimeProfile(TimeProfile timeProfile, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_time_profile");
			BindGuidType("@pkid", timeProfile.Id);
			BindNameType("@name", timeProfile.Name);
			BindDescription("@description", timeProfile.Description);
			BindXml("@xml_data", timeProfile.DefinitionXml);
			BindCustomProperties("@custom_properties", timeProfile.CustomProperties);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeleteTimeProfile(string timeProfileId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_time_profile");
			BindGuidType("@pkid", timeProfileId);
			ExecuteNonQuery();
		});
	}

	private static TimeProfileType TimeProfileTypeFromDatabaseType(string databaseType)
	{
		if (databaseType.Equals(DatabaseTimeProfileType.SunclockProfile.ToString(), StringComparison.OrdinalIgnoreCase))
		{
			return TimeProfileType.Sunclock;
		}
		return TimeProfileType.Calendar;
	}

	private static DatabaseTimeProfileType DatabaseTypeFromTimeProfileType(TimeProfileType timeProfileType)
	{
		return timeProfileType switch
		{
			TimeProfileType.Sunclock => DatabaseTimeProfileType.SunclockProfile, 
			TimeProfileType.Calendar => DatabaseTimeProfileType.CalendarProfile, 
			_ => DatabaseTimeProfileType.TimeProfileBase, 
		};
	}
}
