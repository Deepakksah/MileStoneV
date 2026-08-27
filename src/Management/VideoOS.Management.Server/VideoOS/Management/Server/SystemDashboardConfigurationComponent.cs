using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class SystemDashboardConfigurationComponent : GeminiResourceComponent
{
	private static class SystemDashboardConfigColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("id");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder StateSourceType = new DatabaseColumnBinder("state_source_type");

		private static DatabaseColumnBinder SingleStateSourceId = new DatabaseColumnBinder("single_state_source_id");

		private static DatabaseColumnBinder EventTypes = new DatabaseColumnBinder("event_types");

		private static DatabaseColumnBinder Position = new DatabaseColumnBinder("position");

		public static SystemDashboardConfiguration Bind(DatabaseDataReader reader)
		{
			return new SystemDashboardConfiguration
			{
				DashboardTileId = Id.GetInt32(reader),
				DashboardTileName = Name.GetString(reader, allowNulls: false),
				StateSourceType = StateSourceType.GetString(reader),
				SingleStateSourceId = SingleStateSourceId.GetGuid(reader, allowNulls: true, string.Empty),
				EventTypes = GetEventTypesFromXml(EventTypes.GetString(reader, allowNulls: true)),
				Position = Position.GetInt32(reader)
			};
		}

		private static IList<int> GetEventTypesFromXml(string types)
		{
			IList<int> list = new List<int>();
			if (string.IsNullOrEmpty(types))
			{
				return list.ToArray();
			}
			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(types);
			XmlNodeList xmlNodeList = xmlDocument.SelectNodes("Ids");
			if (xmlNodeList != null)
			{
				foreach (XmlNode item in xmlNodeList)
				{
					foreach (XmlNode childNode in item.ChildNodes)
					{
						if (!string.IsNullOrEmpty(childNode.InnerText) && int.TryParse(childNode.InnerText, out var result))
						{
							list.Add(result);
						}
					}
				}
			}
			return list.ToArray();
		}
	}

	public SystemDashboardConfiguration[] QuerySystemDashboardConfigurationByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_system_dashboard_config_by_ids");
			BindIdTable("@Ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<SystemDashboardConfiguration> list = new List<SystemDashboardConfiguration>();
			while (databaseDataReader.Read())
			{
				list.Add(SystemDashboardConfigColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QuerySystemDashboardConfigIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_system_dashboard_config_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("id");
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

	public void CreateSystemDashboardConfiguration(SystemDashboardConfiguration config, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_system_dashboard_config");
			BindInt("@Id", config.DashboardTileId);
			BindString("@Name", config.DashboardTileName, 100, allowNull: false, SqlDbType.Text);
			BindString("@StateSourceType", config.StateSourceType, 100, allowNull: false, SqlDbType.Text);
			BindString("@SingleStateSourceId", config.SingleStateSourceId, 50, allowNull: true, SqlDbType.Text);
			BindArrayAsXml("@EventTypes", config.EventTypes.Select((int i) => i.ToString(CultureInfo.InvariantCulture)).ToArray());
			BindInt("@Position", config.Position);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void UpdateSystemDashboardConfigurationData(SystemDashboardConfiguration config, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_system_dashboard_config");
			BindInt("@Id", config.DashboardTileId);
			BindString("@Name", config.DashboardTileName, 100, allowNull: false, SqlDbType.Text);
			BindString("@StateSourceType", config.StateSourceType, 100, allowNull: false, SqlDbType.Text);
			BindString("@SingleStateSourceId", config.SingleStateSourceId, 50, allowNull: true, SqlDbType.Text);
			BindArrayAsXml("@EventTypes", config.EventTypes.Select((int i) => i.ToString(CultureInfo.InvariantCulture)).ToArray());
			BindInt("@Position", config.Position);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeleteSystemDashboardConfig(int configId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_system_dashboard_config");
			BindInt("@Id", configId);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public SystemDashboardConfiguration QueryCountersStateRulesDataById(int configId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_system_dashboard_config_by_id");
			BindInt("@Id", configId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (databaseDataReader.Read())
			{
				return SystemDashboardConfigColumns.Bind(databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return (SystemDashboardConfiguration)null;
		});
	}
}
