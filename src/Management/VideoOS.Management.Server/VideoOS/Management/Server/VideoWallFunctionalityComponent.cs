using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using VideoOS.Common.Integration;
using VideoOS.Common.SharedComponent;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class VideoWallFunctionalityComponent : GeminiResourceComponent, IVideoWallFunctionalityComponent, IDisposable
{
	private static class ActivatePresetColumns
	{
		private static DatabaseColumnBinder ViewId = new DatabaseColumnBinder("view_id");

		private static DatabaseColumnBinder MonitorId = new DatabaseColumnBinder("monitor_id");

		private static DatabaseColumnBinder NoLayoutMethod = new DatabaseColumnBinder("no_layout_method");

		private static DatabaseColumnBinder EmptyViewItemsMethod = new DatabaseColumnBinder("empty_viewitems_method");

		private static DatabaseColumnBinder ViewXml = new DatabaseColumnBinder("view_xml");

		private static DatabaseColumnBinder PresetXml = new DatabaseColumnBinder("preset_xml");

		public static ActivatePresetItem Bind(DatabaseDataReader reader)
		{
			return new ActivatePresetItem
			{
				ViewId = ViewId.GetString(reader),
				MonitorId = MonitorId.GetString(reader),
				NoLayout = (PresetLayoutMethod)Enum.Parse(typeof(PresetLayoutMethod), NoLayoutMethod.GetString(reader, allowNulls: false), ignoreCase: true),
				EmptyViewItems = (PresetLayoutMethod)Enum.Parse(typeof(PresetLayoutMethod), EmptyViewItemsMethod.GetString(reader, allowNulls: false), ignoreCase: true),
				ViewXml = ViewXml.GetString(reader),
				PresetXml = PresetXml.GetString(reader, allowNulls: true)
			};
		}
	}

	private class ActivatePresetItem
	{
		public string ViewId { get; set; }

		public string MonitorId { get; set; }

		public PresetLayoutMethod NoLayout { get; set; }

		public PresetLayoutMethod EmptyViewItems { get; set; }

		public string ViewXml { get; set; }

		public string PresetXml { get; set; }

		public List<MonitorViewItem> ViewItems { get; set; }
	}

	private static class MonitorViewItemsColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder MonitorViewId = new DatabaseColumnBinder("view_id");

		private static DatabaseColumnBinder Position = new DatabaseColumnBinder("position");

		private static DatabaseColumnBinder ViewItemXml = new DatabaseColumnBinder("definition_xml");

		public static MonitorViewItem Bind(DatabaseDataReader reader)
		{
			return new MonitorViewItem
			{
				Id = ID.GetString(reader),
				MonitorViewId = MonitorViewId.GetString(reader),
				Position = Position.GetInt32(reader),
				ViewItemXml = ViewItemXml.GetString(reader)
			};
		}
	}

	private class MonitorViewItem
	{
		public string Id { get; set; }

		public string MonitorViewId { get; set; }

		public int Position { get; set; }

		public string ViewItemXml { get; set; }
	}

	private static class MonitorViewItemIdsColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		public static MonitorViewItemId Bind(DatabaseDataReader reader)
		{
			return new MonitorViewItemId
			{
				Id = ID.GetString(reader)
			};
		}
	}

	private class MonitorViewItemId
	{
		public string Id { get; set; }
	}

	private static class DeviceIdNamesColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		public static DeviceIdName Bind(DatabaseDataReader reader)
		{
			return new DeviceIdName
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader)
			};
		}
	}

	private class DeviceIdName
	{
		public string Id { get; set; }

		public string Name { get; set; }
	}

	private static class LoggingInfoColumns
	{
		private static DatabaseColumnBinder RessourceID = new DatabaseColumnBinder("ressourceId");

		private static DatabaseColumnBinder RessourceName = new DatabaseColumnBinder("ressourceName");

		private static DatabaseColumnBinder RessourceHostID = new DatabaseColumnBinder("ressourceHostId");

		private static DatabaseColumnBinder RessourceHostName = new DatabaseColumnBinder("ressourceHostName");

		public static LoggingInfo Bind(DatabaseDataReader reader)
		{
			return new LoggingInfo
			{
				RessourceID = RessourceID.GetString(reader),
				RessourceName = RessourceName.GetString(reader),
				RessourceHostID = RessourceHostID.GetString(reader),
				RessourceHostName = RessourceHostName.GetString(reader)
			};
		}
	}

	private class LoggingInfo
	{
		public string RessourceID { get; set; }

		public string RessourceName { get; set; }

		public string RessourceHostID { get; set; }

		public string RessourceHostName { get; set; }
	}

	public const string EmptyLayoutid = "b7c02ce1-5f66-4886-b0c8-018224a59621";

	private const string NotSpecifiedLayoutId = "00000000-0000-0000-0000-000000000000";

	private static Lazy<XElement> _xmlTemplateTextViewItemData = new Lazy<XElement>(() => XElement.Parse("\r\n<viewitem id='' displayname='Text and image' shortcut='' type='VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application' smartClientId=''>\r\n  <iteminfo id='8de0025a-d92e-4c18-b670-9ae0625f8be1' displayname='Text and image' type='VideoOS.RemoteClient.Application.MIP.PlatformViewItem, VideoOS.RemoteClient.Application'>\r\n    <viewitem id='00000000-0000-0000-0000-000000000000' type='ImageTextViewItem.Client.ImageTextViewItemViewItemManager, VideoOS.ImageTextViewItem.Plugin'>\r\n      <properties>\r\n        <property name='DocumentBackground' value='#FF000000' />\r\n        <property name='Text' value='' />\r\n        <property name='Initialized' value='True' />\r\n        <property name='MaintainImageAspectRatio' value='True' />\r\n        <property name='WindowWidth' value='762' />\r\n      </properties>\r\n    </viewitem>\r\n  </iteminfo>\r\n  <properties />\r\n</viewitem>"));

	public static int MaxTextViewItemMessageLength = 200;

	private void LockVideoWall(string monitorId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT vw.[pkid] ");
		AddStatement("FROM [VideoWallMonitor] vwm ");
		AddStatement("INNER JOIN [VideoWall] vw WITH(UPDLOCK) ON vw.[pkid] = vwm.[videowall_id] ");
		AddStatement("WHERE vwm.[pkid] = @monitor_id ", 1);
		BindGuidType("@monitor_id", monitorId);
		ExecuteNonQuery();
	}

	private Dictionary<string, ActivatePresetItem> GetActivatePreset(string videoWallPresetId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT mview.pkid AS view_id,");
		AddStatement("monitor.pkid AS monitor_id,");
		AddStatement("monitor.no_layout_method, ");
		AddStatement("monitor.empty_viewitems_method,");
		AddStatement("mview.definition_xml AS view_xml,");
		AddStatement("mpreset.definition_xml AS preset_xml");
		AddStatement("FROM VideoWallView mview WITH(UPDLOCK)");
		AddStatement("INNER JOIN VideoWallMonitor monitor WITH(UPDLOCK) ON mview.pkid = monitor.view_id");
		AddStatement("INNER JOIN VideoWallMonitorPreset mpreset ON mpreset.monitor_id = monitor.pkid");
		AddStatement("AND mpreset.videowallpreset_id = @videoWallPresetId");
		AddStatement("WHERE videowall_id =");
		AddStatement("(SELECT DISTINCT video.pkid ");
		AddStatement("FROM VideoWall video WITH(UPDLOCK)");
		AddStatement("INNER JOIN VideoWallPreset preset WITH(UPDLOCK) ON video.pkid = preset.videowall_id ");
		AddStatement("WHERE preset.pkid = @videoWallPresetId)", 1);
		BindGuidType("@videoWallPresetId", videoWallPresetId);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		Dictionary<string, ActivatePresetItem> dictionary = new Dictionary<string, ActivatePresetItem>();
		while (databaseDataReader.Read())
		{
			ActivatePresetItem activatePresetItem = ActivatePresetColumns.Bind(databaseDataReader);
			activatePresetItem.ViewItems = new List<MonitorViewItem>();
			dictionary.Add(activatePresetItem.ViewId, activatePresetItem);
		}
		PrepareSqlBatch();
		AddStatement("SELECT vi.pkid, vi.view_id, vi.position, vi.definition_xml");
		AddStatement("FROM VideoWallViewItem vi WITH(UPDLOCK)");
		AddStatement("INNER JOIN VideoWallMonitor m ON vi.view_id = m.view_id");
		AddStatement("WHERE m.videowall_id =");
		AddStatement("(SELECT DISTINCT video.pkid");
		AddStatement("FROM VideoWall video WITH(UPDLOCK)");
		AddStatement("INNER JOIN VideoWallPreset preset WITH(UPDLOCK) ON video.pkid = preset.videowall_id");
		AddStatement("WHERE preset.pkid = @videoWallPresetId)", 1);
		AddStatement("ORDER BY view_id");
		BindGuidType("@videoWallPresetId", videoWallPresetId);
		databaseDataReader = ExecuteReader();
		List<MonitorViewItem> list = new List<MonitorViewItem>();
		while (databaseDataReader.Read())
		{
			MonitorViewItem item = MonitorViewItemsColumns.Bind(databaseDataReader);
			list.Add(item);
		}
		foreach (MonitorViewItem item2 in list)
		{
			dictionary[item2.MonitorViewId].ViewItems.Add(item2);
		}
		return dictionary;
	}

	private void UpdateViewXml(string viewId, string viewXml)
	{
		PrepareSqlBatch();
		AddStatement("UPDATE VideoWallView");
		AddStatement("SET definition_xml = @viewXml, ", 1);
		AddStatement("last_modified = GETUTCDATE()");
		AddStatement("WHERE pkid = @viewId", 1);
		BindGuidType("@viewId", viewId);
		BindXml("@viewXml", viewXml);
		ExecuteNonQuery();
	}

	private void UpdateViewClearAllItems(string viewId)
	{
		ViewItemData viewItemData = ViewHandling.CreateViewItem(ViewItemType.Empty, Guid.Empty, "Empty ViewItem", Guid.Empty, string.Empty);
		PrepareSqlBatch();
		AddStatement("UPDATE VideoWallViewItem");
		AddStatement("SET definition_xml = @viewItemXml", 1);
		AddStatement("WHERE view_id = @viewId", 1);
		BindGuidType("@viewId", viewId);
		BindXml("@viewItemXml", viewItemData.PartialXML);
		ExecuteNonQuery();
	}

	private void InsertViewItem(string viewId, int position, string viewItemXml)
	{
		PrepareStoredProcedure("create_video_wall_view_item");
		BindGuidType("@pkid", Guid.NewGuid().ToString());
		BindGuidType("@view_id", viewId);
		BindInt("@position", position);
		BindXml("@definition_xml", viewItemXml);
		ExecuteNonQuery();
	}

	private void UpdateViewItemXml(string viewId, int position, string viewItemXml)
	{
		PrepareSqlBatch();
		AddStatement("UPDATE VideoWallViewItem");
		AddStatement("SET definition_xml = @viewItemXml", 1);
		AddStatement("WHERE view_id = @viewId AND position = @position", 2);
		BindGuidType("@viewId", viewId);
		BindXml("@viewItemXml", viewItemXml);
		BindInt("@position", position);
		ExecuteNonQuery();
	}

	private void UpdateViewItemXml(string viewItemId, string viewItemXml)
	{
		PrepareSqlBatch();
		AddStatement("UPDATE VideoWallViewItem");
		AddStatement("SET definition_xml = @viewItemXml", 1);
		AddStatement("WHERE pkid = @viewItemId", 1);
		BindGuidType("@viewItemId", viewItemId);
		BindXml("@viewItemXml", viewItemXml);
		ExecuteNonQuery();
	}

	private void UpdateViewItemXmlFromViewItemXml(string sourceViewItemId, string destinationViewItemId)
	{
		PrepareSqlBatch();
		AddStatement("UPDATE VideoWallViewItem");
		AddStatement("SET definition_xml = ");
		AddStatement("(SELECT definition_xml");
		AddStatement("FROM VideoWallViewItem");
		AddStatement("WHERE pkid = @sourceViewItemId)", 1);
		AddStatement("WHERE pkid = @destinationViewItemId", 1);
		BindGuidType("@sourceViewItemId", sourceViewItemId);
		BindGuidType("@destinationViewItemId", destinationViewItemId);
		ExecuteNonQuery();
	}

	private void DeleteViewItemsFromPosition(string viewId, int fromPosition)
	{
		PrepareSqlBatch();
		AddStatement("DELETE FROM [CustomProperty]");
		AddStatement("WHERE [object_id] IN (");
		AddStatement("SELECT pkid FROM VideoWallViewItem");
		AddStatement("WHERE view_id = @viewId", 1);
		AddStatement("AND [position] >= @fromPosition", 1);
		AddStatement(")");
		BindGuidType("@viewId", viewId);
		BindInt("@fromPosition", fromPosition);
		ExecuteNonQuery();
		PrepareSqlBatch();
		AddStatement("DELETE FROM VideoWallViewItem");
		AddStatement("WHERE view_id = @viewId", 1);
		AddStatement("AND [position] >= @fromPosition", 1);
		BindGuidType("@viewId", viewId);
		BindInt("@fromPosition", fromPosition);
		ExecuteNonQuery();
	}

	private List<MonitorViewItemId> GetMonitorViewItemIds(string viewId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT [pkid]");
		AddStatement("FROM VideoWallViewItem vi WITH(UPDLOCK)");
		AddStatement("WHERE view_id = @viewId", 1);
		AddStatement("ORDER by position");
		BindGuidType("@viewId", viewId);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		List<MonitorViewItemId> list = new List<MonitorViewItemId>();
		while (databaseDataReader.Read())
		{
			MonitorViewItemId item = MonitorViewItemIdsColumns.Bind(databaseDataReader);
			list.Add(item);
		}
		return list;
	}

	private string GetPosition(string viewItemId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT [position] FROM VideoWallViewItem WITH(UPDLOCK) WHERE pkid = @viewItemId", 1);
		BindGuidType("@viewItemId", viewItemId);
		string text = ExecuteScalar().ToString();
		if (string.IsNullOrEmpty(text))
		{
			throw new ViewLayoutNotFoundException();
		}
		return text;
	}

	private List<MonitorViewItem> GetMonitorViewItems(string viewId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT [pkid],[view_id],[position],[definition_xml]");
		AddStatement("FROM VideoWallViewItem vi WITH(UPDLOCK)");
		AddStatement("WHERE view_id = @viewId", 1);
		AddStatement("ORDER by position");
		BindGuidType("@viewId", viewId);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		List<MonitorViewItem> list = new List<MonitorViewItem>();
		while (databaseDataReader.Read())
		{
			MonitorViewItem item = MonitorViewItemsColumns.Bind(databaseDataReader);
			list.Add(item);
		}
		return list;
	}

	private string GetLayoutXml(string layoutId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT definition_xml FROM VideoWallViewLayout WITH(UPDLOCK) WHERE pkid = @layoutId", 1);
		BindGuidType("@layoutId", layoutId);
		string obj = (string)ExecuteScalar();
		if (string.IsNullOrEmpty(obj))
		{
			throw new ViewLayoutNotFoundException();
		}
		return obj;
	}

	private string GetLayoutName(string layoutId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT [name] FROM VideoWallViewLayout WITH(UPDLOCK) WHERE pkid = @layoutId", 1);
		BindGuidType("@layoutId", layoutId);
		string obj = (string)ExecuteScalar();
		if (string.IsNullOrEmpty(obj))
		{
			throw new ViewLayoutNotFoundException();
		}
		return obj;
	}

	private List<DeviceIdName> GetDeviceIdNames(string[] cameras, string[] cameraGroups, int maxNumberOfItems)
	{
		List<DeviceIdName> list = new List<DeviceIdName>();
		if (cameras == null || cameraGroups == null || (cameras != null && cameras.Length == 0 && cameraGroups == null) || (cameras == null && cameraGroups != null && cameraGroups.Length == 0) || (cameras != null && cameras.Length == 0 && cameraGroups != null && cameraGroups.Length == 0))
		{
			return list;
		}
		PrepareSqlBatch();
		AddStatement(string.Format(CultureInfo.InvariantCulture, "SELECT TOP {0} [IDDevice], [Name]", maxNumberOfItems.ToString(CultureInfo.InvariantCulture)));
		AddStatement("FROM Devices");
		AddStatement("WHERE deviceType = 'CAMERA'");
		if (cameras != null && cameras.Length != 0)
		{
			AddStatement("AND (IDDevice IN (");
			bool flag = true;
			for (int i = 0; i < cameras.Length; i++)
			{
				if (flag)
				{
					flag = false;
					AddStatement(string.Format(CultureInfo.InvariantCulture, "@deviceId{0}", i.ToString(CultureInfo.InvariantCulture)), 1);
				}
				else
				{
					AddStatement(string.Format(CultureInfo.InvariantCulture, ", @deviceId{0}", i.ToString(CultureInfo.InvariantCulture)), 1);
				}
			}
			AddStatement(")");
		}
		if (cameraGroups != null && cameraGroups.Length != 0)
		{
			if (cameras != null && cameras.Length != 0)
			{
				AddStatement("OR IDDevice IN (");
			}
			else
			{
				AddStatement("AND (IDDevice IN (");
			}
			AddStatement("SELECT IDDevice FROM DeviceGroupMembers");
			AddStatement("WHERE IDDeviceGroup IN (");
			bool flag2 = true;
			for (int j = 0; j < cameraGroups.Length; j++)
			{
				if (flag2)
				{
					flag2 = false;
					AddStatement(string.Format(CultureInfo.InvariantCulture, "@deviceGroupId{0}", j.ToString(CultureInfo.InvariantCulture)), 1);
				}
				else
				{
					AddStatement(string.Format(CultureInfo.InvariantCulture, ", @deviceGroupId{0}", j.ToString(CultureInfo.InvariantCulture)), 1);
				}
			}
			AddStatement(")");
			AddStatement(")");
		}
		AddStatement(")");
		if (cameras != null && cameras.Length != 0)
		{
			for (int k = 0; k < cameras.Length; k++)
			{
				BindGuidType(string.Format(CultureInfo.InvariantCulture, "@deviceId{0}", k.ToString(CultureInfo.InvariantCulture)), cameras[k]);
			}
		}
		if (cameraGroups != null && cameraGroups.Length != 0)
		{
			for (int l = 0; l < cameraGroups.Length; l++)
			{
				BindGuidType(string.Format(CultureInfo.InvariantCulture, "@deviceGroupId{0}", l.ToString(CultureInfo.InvariantCulture)), cameraGroups[l]);
			}
		}
		DatabaseDataReader databaseDataReader = ExecuteReader();
		while (databaseDataReader.Read())
		{
			DeviceIdName item = DeviceIdNamesColumns.Bind(databaseDataReader);
			list.Add(item);
		}
		string[] array = cameras;
		foreach (string camId in array)
		{
			if (list.FirstOrDefault((DeviceIdName e) => string.Compare(e.Id, camId, StringComparison.OrdinalIgnoreCase) == 0) == null)
			{
				list.Add(new DeviceIdName
				{
					Id = camId,
					Name = "Remote Device"
				});
			}
		}
		cameras = cameras.Select((string s) => s.ToUpperInvariant()).ToArray();
		return list.OrderBy((DeviceIdName cam) => Array.IndexOf(cameras, cam.Id.ToUpperInvariant())).ToList();
	}

	private LoggingInfo GetPresetLoggingInfo(string videoWallPresetId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT vwp.[pkid] as ressourceId, vwp.[Name] as ressourceName, vw.[pkid] as ressourceHostId, vw.[name] as ressourceHostName");
		AddStatement("FROM [VideoWall] vw WITH(UPDLOCK)");
		AddStatement("INNER JOIN [VideoWallpreset] vwp ON vwp.[videowall_id] = vw.[pkid]");
		AddStatement("WHERE vwp.[pkid] = @videoWallPresetId", 1);
		BindGuidType("@videoWallPresetId", videoWallPresetId);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		if (!databaseDataReader.Read())
		{
			return null;
		}
		return LoggingInfoColumns.Bind(databaseDataReader);
	}

	private LoggingInfo GetMonitorLoggingInfo(string videoWallMonitorId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT vwm.[pkid] as ressourceId, vwm.[Name] as ressourceName, vw.[pkid] as ressourceHostId, vw.[name] as ressourceHostName");
		AddStatement("FROM [VideoWall] vw WITH(UPDLOCK)");
		AddStatement("INNER JOIN [VideoWallMonitor] vwm ON vwm.[videowall_id] = vw.[pkid]");
		AddStatement("WHERE vwm.[pkid] = @videoWallMonitorId", 1);
		BindGuidType("@videoWallMonitorId", videoWallMonitorId);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		if (!databaseDataReader.Read())
		{
			return null;
		}
		return LoggingInfoColumns.Bind(databaseDataReader);
	}

	private LoggingInfo GetViewItemLoggingInfo(string viewItemId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT vwm.[pkid] as ressourceId, vwm.[Name] as ressourceName, vw.[pkid] as ressourceHostId, vw.[name] as ressourceHostName");
		AddStatement("FROM [VideoWall] vw WITH(UPDLOCK)");
		AddStatement("INNER JOIN [VideoWallMonitor] vwm ON vwm.[videowall_id] = vw.[pkid]");
		AddStatement("INNER JOIN [VideoWallView] vwv ON vwm.[view_id] = vwv.[pkid]");
		AddStatement("INNER JOIN [VideoWallViewItem] vwvi ON vwvi.[view_id] = vwv.[pkid]");
		AddStatement("WHERE vwvi.[pkid] = @viewItemId", 1);
		BindGuidType("@viewItemId", viewItemId);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		if (!databaseDataReader.Read())
		{
			return null;
		}
		return LoggingInfoColumns.Bind(databaseDataReader);
	}

	private LoggingInfo GetViewLoggingInfo(string viewId)
	{
		PrepareSqlBatch();
		AddStatement("SELECT vwm.[pkid] as ressourceId, vwm.[Name] as ressourceName, vw.[pkid] as ressourceHostId, vw.[name] as ressourceHostName");
		AddStatement("FROM [VideoWall] vw WITH(UPDLOCK)");
		AddStatement("INNER JOIN [VideoWallMonitor] vwm ON vwm.[videowall_id] = vw.[pkid]");
		AddStatement("INNER JOIN [VideoWallView] vwv ON vwm.[view_id] = vwv.[pkid]");
		AddStatement("WHERE vwv.[pkid] = @viewId", 1);
		BindGuidType("@viewId", viewId);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		if (!databaseDataReader.Read())
		{
			return null;
		}
		return LoggingInfoColumns.Bind(databaseDataReader);
	}

	private void CreatePendingLog(Guid sessionId, LogEntryId logId, string param0, string param1, string param2, string param3, string param4, string param5)
	{
		PrepareStoredProcedure("create_pending_log");
		BindGuidType("@session_id", sessionId.ToString());
		BindLogIDType("@log_id", (int)logId);
		BindString("@param0", param0, 256, allowNull: true, SqlDbType.NVarChar);
		BindString("@param1", param1, 256, allowNull: true, SqlDbType.NVarChar);
		BindString("@param2", param2, 256, allowNull: true, SqlDbType.NVarChar);
		BindString("@param3", param3, 256, allowNull: true, SqlDbType.NVarChar);
		BindString("@param4", param4, 256, allowNull: true, SqlDbType.NVarChar);
		BindString("@param5", param5, 256, allowNull: true, SqlDbType.NVarChar);
		ExecuteNonQuery();
	}

	private void CreatePendingLog(Guid sessionId, LogEntryId logId, string param0, string param1, string param2, string param3, string param4)
	{
		CreatePendingLog(sessionId, logId, param0, param1, param2, param3, param4, null);
	}

	private void CreatePendingLog(Guid sessionId, LogEntryId logId, string param0, string param1, string param2, string param3)
	{
		CreatePendingLog(sessionId, logId, param0, param1, param2, param3, null, null);
	}

	public Tuple<string, string>[] ActivateVideoWallPreset(string videoWallPresetId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			List<Tuple<string, string>> list = new List<Tuple<string, string>>();
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				foreach (KeyValuePair<string, ActivatePresetItem> item in GetActivatePreset(videoWallPresetId))
				{
					ActivatePresetItem value = item.Value;
					list.Add(new Tuple<string, string>(value.ViewId, value.MonitorId));
					if (string.IsNullOrEmpty(value.PresetXml))
					{
						if (value.NoLayout == PresetLayoutMethod.Clear)
						{
							UpdateViewClearAllItems(value.ViewId);
						}
					}
					else
					{
						ViewData viewData = ViewData.CreateFromXML(value.PresetXml);
						string partialXML = viewData.PartialXML;
						UpdateViewXml(value.ViewId, partialXML);
						DeleteViewItemsFromPosition(value.ViewId, viewData.ViewItemsInfo.Count);
						foreach (ViewItemInfo item2 in viewData.ViewItemsInfo)
						{
							if (item2.SizeAndLocation != null)
							{
								if (item2.SizeAndLocation.PositionNumber > value.ViewItems.Count - 1)
								{
									InsertViewItem(value.ViewId, item2.SizeAndLocation.PositionNumber, item2.ViewItemData.PartialXML);
								}
								else if (item2.ViewItemData.ViewItemType != ViewItemType.Empty || value.EmptyViewItems != PresetLayoutMethod.Preserve)
								{
									UpdateViewItemXml(value.ViewId, item2.SizeAndLocation.PositionNumber, item2.ViewItemData.PartialXML);
								}
							}
						}
					}
				}
				LoggingInfo presetLoggingInfo = GetPresetLoggingInfo(videoWallPresetId);
				CommitTransaction();
				CreatePendingLog(sessionId, LogEntryId.AuditVideoWallPresetActivate, presetLoggingInfo.RessourceID, presetLoggingInfo.RessourceName, presetLoggingInfo.RessourceHostID, presetLoggingInfo.RessourceHostName, null);
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
			return list.ToArray();
		});
	}

	public string GetVideoWallId(string videoWallPresetId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT [videowall_id] FROM [VideoWallPreset] WHERE [pkid] = @pkid", 1);
			BindGuidType("@pkid", videoWallPresetId);
			return (string)ExecuteScalar();
		});
	}

	private void InternalApplyLayoutToMonitor(Monitor monitor, string layoutId, Guid sessionId)
	{
		ViewData viewData = ViewHandling.CreateViewDataFromViewLayout(GetLayoutXml(layoutId), monitor.Name);
		string partialXML = viewData.PartialXML;
		UpdateViewXml(monitor.ViewId, partialXML);
		DeleteViewItemsFromPosition(monitor.ViewId, viewData.ViewItemsInfo.Count);
		List<MonitorViewItemId> monitorViewItemIds = GetMonitorViewItemIds(monitor.ViewId);
		foreach (ViewItemInfo item in viewData.ViewItemsInfo)
		{
			if (item.SizeAndLocation.PositionNumber > monitorViewItemIds.Count - 1)
			{
				InsertViewItem(monitor.ViewId, item.SizeAndLocation.PositionNumber, item.ViewItemData.PartialXML);
			}
		}
		string layoutName = GetLayoutName(layoutId);
		LoggingInfo monitorLoggingInfo = GetMonitorLoggingInfo(monitor.Id);
		CreatePendingLog(sessionId, LogEntryId.AuditVideoWallMonitorLayoutChanged, monitorLoggingInfo.RessourceID, monitorLoggingInfo.RessourceName, monitorLoggingInfo.RessourceHostID, monitorLoggingInfo.RessourceHostName, layoutName);
	}

	private static string[] InternalCreateViewItemsXml(List<DeviceIdName> cameraIds)
	{
		List<string> list = new List<string>();
		if (cameraIds.Count == 0)
		{
			return list.ToArray();
		}
		foreach (DeviceIdName cameraId in cameraIds)
		{
			ViewItemData viewItemData = ViewHandling.CreateViewItem(ViewItemType.Camera, Guid.NewGuid(), cameraId.Name, new Guid(cameraId.Id), cameraId.Name);
			list.Add(viewItemData.PartialXML);
		}
		return list.ToArray();
	}

	private void InternalApplyViewItemDefinitionXmlToMonitorView(Monitor monitor, string[] viewItemsDefinitionXmlArray, int fromPosition, List<MonitorViewItemId> monitorViewItemIds)
	{
		if (viewItemsDefinitionXmlArray == null || viewItemsDefinitionXmlArray.Length == 0)
		{
			return;
		}
		int num = viewItemsDefinitionXmlArray.Length;
		if (monitorViewItemIds.Count < num + fromPosition)
		{
			num = monitorViewItemIds.Count - fromPosition;
		}
		int num2 = monitorViewItemIds.Count - 1;
		if (monitor.InsertionMethod == ElementInsertionMethod.Linked)
		{
			int num3 = monitorViewItemIds.Count - fromPosition - num;
			if (num3 > 0)
			{
				for (int i = 0; i < num3; i++)
				{
					int index = num2 - i;
					int index2 = num2 - num - i;
					string id = monitorViewItemIds[index2].Id;
					string id2 = monitorViewItemIds[index].Id;
					UpdateViewItemXmlFromViewItemXml(id, id2);
				}
			}
		}
		int num4 = fromPosition;
		for (int j = 0; j < viewItemsDefinitionXmlArray.Length; j++)
		{
			if (num4 > num2)
			{
				break;
			}
			UpdateViewItemXml(monitor.ViewId, num4, viewItemsDefinitionXmlArray[j]);
			num4++;
		}
	}

	public void ApplyCamerasToMonitorView(Monitor monitor, string[] cameras, string[] cameraGroups, int fromPosition, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				LockVideoWall(monitor.Id);
				List<MonitorViewItemId> monitorViewItemIds = GetMonitorViewItemIds(monitor.ViewId);
				if (fromPosition >= monitorViewItemIds.Count)
				{
					RollbackTransaction();
					return;
				}
				int maxNumberOfItems = monitorViewItemIds.Count - fromPosition;
				List<DeviceIdName> deviceIdNames = GetDeviceIdNames(cameras, cameraGroups, maxNumberOfItems);
				string[] viewItemsDefinitionXmlArray = InternalCreateViewItemsXml(deviceIdNames);
				InternalApplyViewItemDefinitionXmlToMonitorView(monitor, viewItemsDefinitionXmlArray, fromPosition, monitorViewItemIds);
				CommitTransaction();
				int num = fromPosition + 1;
				LoggingInfo monitorLoggingInfo = GetMonitorLoggingInfo(monitor.Id);
				foreach (DeviceIdName item in deviceIdNames)
				{
					CreatePendingLog(sessionId, LogEntryId.AuditVideoWallMonitorLayoutCameraChanged, monitorLoggingInfo.RessourceID, monitorLoggingInfo.RessourceName, monitorLoggingInfo.RessourceHostID, monitorLoggingInfo.RessourceHostName, num.ToString(CultureInfo.InvariantCulture), item.Name);
					num++;
				}
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		});
	}

	public void ApplyCamerasAndLayoutToMonitorView(Monitor monitor, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				LockVideoWall(monitor.Id);
				InternalApplyLayoutToMonitor(monitor, layoutId, sessionId);
				List<MonitorViewItemId> monitorViewItemIds = GetMonitorViewItemIds(monitor.ViewId);
				if (fromPosition >= monitorViewItemIds.Count)
				{
					RollbackTransaction();
					return;
				}
				int maxNumberOfItems = monitorViewItemIds.Count - fromPosition;
				List<DeviceIdName> deviceIdNames = GetDeviceIdNames(cameras, cameraGroups, maxNumberOfItems);
				string[] viewItemsDefinitionXmlArray = InternalCreateViewItemsXml(deviceIdNames);
				InternalApplyViewItemDefinitionXmlToMonitorView(monitor, viewItemsDefinitionXmlArray, fromPosition, monitorViewItemIds);
				CommitTransaction();
				int num = fromPosition + 1;
				LoggingInfo monitorLoggingInfo = GetMonitorLoggingInfo(monitor.Id);
				foreach (DeviceIdName item in deviceIdNames)
				{
					CreatePendingLog(sessionId, LogEntryId.AuditVideoWallMonitorLayoutCameraChanged, monitorLoggingInfo.RessourceID, monitorLoggingInfo.RessourceName, monitorLoggingInfo.RessourceHostID, monitorLoggingInfo.RessourceHostName, num.ToString(CultureInfo.InvariantCulture), item.Name);
					num++;
				}
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		});
	}

	public void ApplyViewItemsToMonitorView(Monitor monitor, string[] viewItemsDefinitionXml, int fromPosition, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				List<MonitorViewItemId> monitorViewItemIds = GetMonitorViewItemIds(monitor.ViewId);
				if (fromPosition >= monitorViewItemIds.Count)
				{
					RollbackTransaction();
				}
				else
				{
					InternalApplyViewItemDefinitionXmlToMonitorView(monitor, viewItemsDefinitionXml, fromPosition, monitorViewItemIds);
					CommitTransaction();
					LoggingInfo monitorLoggingInfo = GetMonitorLoggingInfo(monitor.Id);
					CreatePendingLog(sessionId, LogEntryId.AuditVideoWallMonitorEdited, monitorLoggingInfo.RessourceID, monitorLoggingInfo.RessourceName, monitorLoggingInfo.RessourceHostID, monitorLoggingInfo.RessourceHostName);
				}
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		});
	}

	public void ApplyLayoutToMonitorView(Monitor monitor, string layoutId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				InternalApplyLayoutToMonitor(monitor, layoutId, sessionId);
				CommitTransaction();
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		});
	}

	private static XElement CreateImageTextViewItem(MonitorViewItemId viewItemId, string messageToDisplay)
	{
		if (string.IsNullOrEmpty(messageToDisplay))
		{
			throw new ArgumentException("messageToDisplay must not be null or empty string.");
		}
		if (messageToDisplay.Length > MaxTextViewItemMessageLength)
		{
			messageToDisplay = messageToDisplay.Substring(0, MaxTextViewItemMessageLength);
		}
		XElement xElement = new XElement(_xmlTemplateTextViewItemData.Value);
		xElement.Attribute(XName.Get("id")).Value = viewItemId.Id;
		xElement.XPathSelectElement("//property[@name='Text']").Attribute(XName.Get("value")).Value = messageToDisplay;
		return xElement;
	}

	public void ApplyTextAndLayoutToMonitorView(Monitor monitor, string messageToDisplay, int atPosition, string layoutId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				LockVideoWall(monitor.Id);
				if (!string.IsNullOrEmpty(layoutId) && layoutId != "00000000-0000-0000-0000-000000000000")
				{
					InternalApplyLayoutToMonitor(monitor, layoutId, sessionId);
				}
				List<MonitorViewItemId> monitorViewItemIds = GetMonitorViewItemIds(monitor.ViewId);
				if (atPosition >= monitorViewItemIds.Count)
				{
					RollbackTransaction();
				}
				else
				{
					XElement xElement = CreateImageTextViewItem(monitorViewItemIds[atPosition], messageToDisplay);
					InternalApplyViewItemDefinitionXmlToMonitorView(monitor, new string[1] { xElement.ToString() }, atPosition, monitorViewItemIds);
					CommitTransaction();
					LoggingInfo monitorLoggingInfo = GetMonitorLoggingInfo(monitor.Id);
					CreatePendingLog(sessionId, LogEntryId.AuditVideoWallTextViewItemCreated, monitorLoggingInfo.RessourceID, monitorLoggingInfo.RessourceName, monitorLoggingInfo.RessourceHostID, monitorLoggingInfo.RessourceHostName, messageToDisplay);
				}
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		});
	}

	public void ClearViewItem(string viewItemId, Guid sessionId, string smartClientId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			ViewItemData viewItemData = ViewHandling.CreateViewItem(ViewItemType.Empty, new Guid(viewItemId), "Empty ViewItem", Guid.Empty, string.Empty, smartClientId);
			UpdateViewItemXml(viewItemId, viewItemData.PartialXML);
			LoggingInfo viewItemLoggingInfo = GetViewItemLoggingInfo(viewItemId);
			if (viewItemLoggingInfo != null)
			{
				string param = (int.Parse(GetPosition(viewItemId), CultureInfo.InvariantCulture) + 1).ToString(CultureInfo.InvariantCulture);
				CreatePendingLog(sessionId, LogEntryId.AuditVideoWallMonitorLayoutPositionCleared, viewItemLoggingInfo.RessourceID, viewItemLoggingInfo.RessourceName, viewItemLoggingInfo.RessourceHostID, viewItemLoggingInfo.RessourceHostName, param);
			}
		});
	}

	public void ClearView(string viewId, Guid sessionId, string viewXml, string smartClientId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			foreach (MonitorViewItem monitorViewItem in GetMonitorViewItems(viewId))
			{
				ViewItemData viewItemData = ViewHandling.CreateViewItem(ViewItemType.Empty, new Guid(monitorViewItem.Id), "Empty ViewItem", Guid.Empty, string.Empty, smartClientId);
				UpdateViewItemXml(monitorViewItem.Id, viewItemData.PartialXML);
			}
			if (!string.IsNullOrEmpty(viewXml))
			{
				UpdateViewXml(viewId, viewXml);
			}
			LoggingInfo viewLoggingInfo = GetViewLoggingInfo(viewId);
			if (viewLoggingInfo != null)
			{
				CreatePendingLog(sessionId, LogEntryId.AuditVideoWallMonitorLayoutCleared, viewLoggingInfo.RessourceID, viewLoggingInfo.RessourceName, viewLoggingInfo.RessourceHostID, viewLoggingInfo.RessourceHostName, null);
			}
		});
	}

	public void RemoveCamerasFromMonitorView(Monitor monitor, string[] cameras, string[] cameraGroups, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				LockVideoWall(monitor.Id);
				List<Tuple<int, DeviceIdName>> list = new List<Tuple<int, DeviceIdName>>();
				List<DeviceIdName> deviceIdNames = GetDeviceIdNames(cameras, cameraGroups, int.MaxValue);
				foreach (MonitorViewItem monitorViewItem in GetMonitorViewItems(monitor.ViewId))
				{
					if (ViewHandling.GetViewItemInformation(monitorViewItem.ViewItemXml, out var viewItemType, out var _, out var cameraId) && viewItemType == ViewItemType.Camera)
					{
						DeviceIdName deviceIdName = deviceIdNames.FirstOrDefault((DeviceIdName x) => x.Id == cameraId.ToString());
						if (deviceIdName != null)
						{
							ViewItemData viewItemData = ViewHandling.CreateViewItem(ViewItemType.Empty, Guid.NewGuid(), "Empty", Guid.Empty, string.Empty);
							UpdateViewItemXml(monitorViewItem.Id, viewItemData.PartialXML);
							list.Add(new Tuple<int, DeviceIdName>(monitorViewItem.Position, deviceIdName));
						}
					}
				}
				CommitTransaction();
				LoggingInfo monitorLoggingInfo = GetMonitorLoggingInfo(monitor.Id);
				foreach (Tuple<int, DeviceIdName> item in list)
				{
					int num = item.Item1 + 1;
					CreatePendingLog(sessionId, LogEntryId.AuditVideoWallMonitorCameraRemoved, monitorLoggingInfo.RessourceID, monitorLoggingInfo.RessourceName, monitorLoggingInfo.RessourceHostID, monitorLoggingInfo.RessourceHostName, num.ToString(CultureInfo.InvariantCulture), item.Item2.Name);
				}
			}
			catch
			{
				RollbackTransaction();
				throw;
			}
		});
	}
}
