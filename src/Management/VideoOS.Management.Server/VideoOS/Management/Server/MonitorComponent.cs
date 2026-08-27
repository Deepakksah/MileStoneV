using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VideoOS.Common.SharedComponent;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MonitorComponent : GeminiResourceComponent, IMonitorComponent, IDisposable
{
	public static class MonitorColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder AspectRatio = new DatabaseColumnBinder("aspect_ratio");

		private static DatabaseColumnBinder InsertionMethod = new DatabaseColumnBinder("insertion_method");

		private static DatabaseColumnBinder NoLayoutMethod = new DatabaseColumnBinder("no_layout_method");

		private static DatabaseColumnBinder EmptyViewItemsMethod = new DatabaseColumnBinder("empty_viewitems_method");

		private static DatabaseColumnBinder MonitorSize = new DatabaseColumnBinder("monitor_size");

		private static DatabaseColumnBinder LocationX = new DatabaseColumnBinder("location_x");

		private static DatabaseColumnBinder LocationY = new DatabaseColumnBinder("location_y");

		private static DatabaseColumnBinder ViewId = new DatabaseColumnBinder("view_id");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder VideoWallId = new DatabaseColumnBinder("videowall_id");

		private static DatabaseColumnBinder MonitorState = new DatabaseColumnBinder("monitor_state");

		public static Monitor Bind(DatabaseDataReader reader)
		{
			return new Monitor
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				ViewId = ViewId.GetString(reader),
				AspectRatio = (AspectRatio)Enum.Parse(typeof(AspectRatio), AspectRatio.GetString(reader, allowNulls: false), ignoreCase: true),
				InsertionMethod = (ElementInsertionMethod)Enum.Parse(typeof(ElementInsertionMethod), InsertionMethod.GetString(reader, allowNulls: false), ignoreCase: true),
				NoLayout = (PresetLayoutMethod)Enum.Parse(typeof(PresetLayoutMethod), NoLayoutMethod.GetString(reader, allowNulls: false), ignoreCase: true),
				EmptyViewItems = (PresetLayoutMethod)Enum.Parse(typeof(PresetLayoutMethod), EmptyViewItemsMethod.GetString(reader, allowNulls: false), ignoreCase: true),
				MonitorSize = MonitorSize.GetFloat(reader),
				LocationX = LocationX.GetDouble(reader),
				LocationY = LocationY.GetDouble(reader),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				VideoWallId = VideoWallId.GetString(reader),
				MonitorState = MonitorState.GetString(reader)
			};
		}
	}

	private static class MonitorPresetColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder DefinitionXml = new DatabaseColumnBinder("definition_xml");

		private static DatabaseColumnBinder VideoWallPresetId = new DatabaseColumnBinder("videowall_preset_id");

		private static DatabaseColumnBinder MonitorId = new DatabaseColumnBinder("monitor_id");

		private static DatabaseColumnBinder VideoWallId = new DatabaseColumnBinder("videowall_id");

		public static MonitorPreset Bind(DatabaseDataReader reader)
		{
			return new MonitorPreset
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				DefinitionXml = DefinitionXml.GetString(reader, allowNulls: true),
				VideoWallPresetId = VideoWallPresetId.GetString(reader),
				MonitorId = MonitorId.GetString(reader),
				VideoWallId = VideoWallId.GetString(reader)
			};
		}
	}

	private static class IdColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		public static string Bind(DatabaseDataReader reader)
		{
			return ID.GetString(reader);
		}
	}

	public const int MonitorStateMaxLength = 2000;

	public Monitor QueryMonitor(string monitorId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_monitor_by_id");
			BindGuidType("@pkid", monitorId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new MonitorNotFoundException();
			}
			Monitor result = MonitorColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Monitor[] QueryMonitors(string videoWallId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_monitors");
			BindGuidType("@videoWallId", videoWallId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Monitor> list = new List<Monitor>();
			while (databaseDataReader.Read())
			{
				list.Add(MonitorColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryMonitorIds(string videoWallId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_monitor_ids");
			BindGuidType("@videoWallId", videoWallId);
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

	public Monitor[] QueryMonitorsByIds(string[] monitorIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_monitors_by_ids");
			BindIdTable("@monitorIds", monitorIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Monitor> list = new List<Monitor>();
			while (databaseDataReader.Read())
			{
				list.Add(MonitorColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateMonitor(string videoWallId, Monitor monitor, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				PrepareSqlBatch();
				AddStatement("SELECT definition_xml FROM VideoWallViewLayout WITH(UPDLOCK) WHERE pkid = @layoutId", 1);
				BindGuidType("@layoutId", "b7c02ce1-5f66-4886-b0c8-018224a59621");
				ViewData viewData = ViewHandling.CreateViewDataFromViewLayout((string)ExecuteScalar(), monitor.Name);
				string partialXML = viewData.PartialXML;
				string partialXML2 = viewData.ViewItemsInfo[0].ViewItemData.PartialXML;
				PrepareStoredProcedure("create_video_wall_monitor");
				BindGuidType("@pkid", monitor.Id);
				BindGuidType("@videowall_id", videoWallId);
				BindNameType("@name", monitor.Name);
				BindDescription("@description", monitor.Description);
				BindString("@aspect_ratio", Enum.GetName(typeof(AspectRatio), monitor.AspectRatio), 50, allowNull: false, SqlDbType.VarChar);
				BindString("@insertion_method", Enum.GetName(typeof(ElementInsertionMethod), monitor.InsertionMethod), 50, allowNull: false, SqlDbType.VarChar);
				BindString("@no_layout_method", Enum.GetName(typeof(PresetLayoutMethod), monitor.NoLayout), 50, allowNull: false, SqlDbType.VarChar);
				BindString("@empty_viewitems_method", Enum.GetName(typeof(PresetLayoutMethod), monitor.EmptyViewItems), 50, allowNull: false, SqlDbType.VarChar);
				BindFloat("@monitor_size", monitor.MonitorSize);
				BindFloat("@location_x", monitor.LocationX);
				BindFloat("@location_y", monitor.LocationY);
				BindXml("@default_view_definition_xml", partialXML);
				BindXml("@default_view_item_definition_xml", partialXML2);
				BindGuidType("@session_id", sessionId.ToString());
				BindCustomProperties("@custom_properties", monitor.CustomProperties);
				ExecuteNonQuery();
				CommitTransaction();
			}
			catch (DatabaseException)
			{
				RollbackTransaction();
				throw;
			}
			catch (VmoServerException)
			{
				RollbackTransaction();
				throw;
			}
		});
	}

	public void UpdateMonitor(Monitor monitor, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_monitor");
			BindGuidType("@pkid", monitor.Id);
			BindNameType("@name", monitor.Name);
			BindDescription("@description", monitor.Description);
			BindString("@aspect_ratio", Enum.GetName(typeof(AspectRatio), monitor.AspectRatio), 50, allowNull: false, SqlDbType.VarChar);
			BindString("@insertion_method", Enum.GetName(typeof(ElementInsertionMethod), monitor.InsertionMethod), 50, allowNull: false, SqlDbType.VarChar);
			BindString("@no_layout_method", Enum.GetName(typeof(PresetLayoutMethod), monitor.NoLayout), 50, allowNull: false, SqlDbType.VarChar);
			BindString("@empty_viewitems_method", Enum.GetName(typeof(PresetLayoutMethod), monitor.EmptyViewItems), 50, allowNull: false, SqlDbType.VarChar);
			BindFloat("@monitor_size", monitor.MonitorSize);
			BindFloat("@location_x", monitor.LocationX);
			BindFloat("@location_y", monitor.LocationY);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", monitor.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateMonitorState(string monitorId, string xmlState)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_monitor_state");
			BindGuidType("@pkid", monitorId);
			BindString("@monitor_state", xmlState, 2000, allowNull: false, SqlDbType.VarChar);
			ExecuteNonQuery();
		});
	}

	public void DeleteMonitor(string monitorId, string sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_video_wall_monitor");
			BindGuidType("@pkid", monitorId);
			BindGuidType("@session_id", sessionId);
			ExecuteNonQuery();
		});
	}

	public string QueryMonitorIdFromPreset(string monitorPresetId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT [monitor_id] ");
			stringBuilder.Append("FROM [VideoWallMonitorPreset] vmp ");
			stringBuilder.Append("WHERE vmp.[pkid] = @pkid ");
			PrepareSqlBatch();
			AddStatement(stringBuilder.ToString());
			BindGuidType("@pkid", monitorPresetId);
			return (string)ExecuteScalar();
		});
	}

	public string QueryVideoWallId(string monitorId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT [videowall_id] FROM [VideoWallMonitor] WHERE [pkid] = @pkid");
			BindGuidType("@pkid", monitorId);
			return (string)ExecuteScalar();
		});
	}

	public string[] QueryMonitorPresetIdsByMonitorId(string monitorId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_monitor_preset_ids_by_monitor_id");
			BindGuidType("@monitor_id", monitorId);
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

	public string[] QueryMonitorPresetIdsByVideoWallPresetId(string videoWallPresetId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_monitor_preset_ids_by_video_wall_preset_id");
			BindGuidType("@video_wall_preset_id", videoWallPresetId);
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

	public MonitorPreset[] QueryMonitorPresetsByIds(string[] monitorPresetIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_monitor_presets_by_ids");
			BindIdTable("@monitorPresetIds", monitorPresetIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MonitorPreset> list = new List<MonitorPreset>();
			while (databaseDataReader.Read())
			{
				list.Add(MonitorPresetColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public MonitorPreset QueryMonitorPreset(string presetId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_monitor_preset_by_id");
			BindGuidType("@pkid", presetId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new MonitorNotFoundException();
			}
			MonitorPreset result = MonitorPresetColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void UpdateMonitorPreset(string presetId, string definitionXml, Guid sessionId, CustomProperty[] customProperties)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_monitor_preset");
			BindGuidType("@pkid", presetId);
			BindXml("@definition_xml", definitionXml);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", customProperties);
			ExecuteNonQuery();
		});
	}
}
