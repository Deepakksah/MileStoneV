using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class PluginEventComponent : GeminiResourceComponent, IPluginEventComponent, IDisposable
{
	private static class PluginEventColumns
	{
		private static DatabaseColumnBinder IDEventType = new DatabaseColumnBinder("IDEventType");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Definition = new DatabaseColumnBinder("Definition");

		private static DatabaseColumnBinder GeneratorType = new DatabaseColumnBinder("GeneratorType");

		private static DatabaseColumnBinder IDGenerator = new DatabaseColumnBinder("IDGenerator");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static PluginEvent Bind(DatabaseDataReader reader)
		{
			PluginEvent obj = new PluginEvent
			{
				Id = IDEventType.GetString(reader),
				Name = Name.GetString(reader, allowNulls: false),
				Definition = Definition.GetString(reader, allowNulls: false)
			};
			if (!Enum.TryParse<PluginEventType>(GeneratorType.GetString(reader, allowNulls: false), out var result))
			{
				result = PluginEventType.MIP;
			}
			obj.GeneratorType = result;
			obj.GeneratorId = IDGenerator.GetString(reader, allowNulls: false);
			obj.LastModified = LastModified.GetDateTime(reader);
			return obj;
		}
	}

	public PluginEvent[] QueryPluginEvents()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_plugin_events");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<PluginEvent> list = new List<PluginEvent>();
			while (databaseDataReader.Read())
			{
				list.Add(PluginEventColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryPluginEventIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_plugin_event_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDEventType");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public PluginEvent QueryPluginEvent(string pluginEventId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_plugin_event_by_id");
			BindGuidType("@IDEventType", pluginEventId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (PluginEvent)null;
			}
			PluginEvent result = PluginEventColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public PluginEvent[] QueryPluginEvents(string[] pluginEventIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_plugin_events_by_ids");
			BindIdTable("@IDEventIds", pluginEventIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<PluginEvent> list = new List<PluginEvent>();
			while (databaseDataReader.Read())
			{
				list.Add(PluginEventColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreatePluginEvent(PluginEvent newPluginEvent, string userSid)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_create_plugin_event");
			BindSIDType("@UserSID", userSid);
			BindGuidType("@IDEventType", newPluginEvent.Id);
			BindNameType("@Name", newPluginEvent.Name);
			BindText("@Definition", newPluginEvent.Definition, allowNull: false);
			BindText("@GeneratorType", newPluginEvent.GeneratorType.ToString(), allowNull: false);
			BindGuidType("@IDGenerator", newPluginEvent.GeneratorId);
			BindCustomProperties("@custom_properties", newPluginEvent.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdatePluginEvent(PluginEvent pluginEvent)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_update_plugin_event");
			BindGuidType("@IDEventType", pluginEvent.Id);
			BindNameType("@Name", pluginEvent.Name);
			BindText("@Definition", pluginEvent.Definition, allowNull: false);
			BindText("@GeneratorType", pluginEvent.GeneratorType.ToString(), allowNull: false);
			BindGuidType("@IDGenerator", pluginEvent.GeneratorId);
			BindCustomProperties("@custom_properties", pluginEvent.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void DeletePluginEvent(string pluginEventId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_delete_plugin_event");
			BindGuidType("@IDEventType", pluginEventId);
			ExecuteNonQuery();
		});
	}

	public PluginEvent QueryPluginEventByName(string pluginEventName)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_plugin_event_by_name");
			BindNameType("@Name", pluginEventName);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (PluginEvent)null;
			}
			PluginEvent result = PluginEventColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}
}
