using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class ViewComponent : GeminiResourceComponent, IViewComponent, IDisposable
{
	private static class ViewColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder DefinitionXml = new DatabaseColumnBinder("definition_xml");

		private static DatabaseColumnBinder ViewGroupId = new DatabaseColumnBinder("view_group_id");

		private static DatabaseColumnBinder ViewGroupRelationIds = new DatabaseColumnBinder("view_group_relation_ids");

		private static DatabaseColumnBinder MonitorId = new DatabaseColumnBinder("monitor_id");

		private static DatabaseColumnBinder VideoWallId = new DatabaseColumnBinder("video_wall_id");

		public static View Bind(DatabaseDataReader reader)
		{
			View view = new View
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				DefinitionXml = DefinitionXml.GetString(reader),
				ViewGroupId = ViewGroupId.GetString(reader),
				MonitorId = MonitorId.GetString(reader),
				VideoWallId = VideoWallId.GetString(reader)
			};
			string text = ViewGroupRelationIds.GetString(reader);
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(text))
			{
				string[] array = text.Split(';');
				foreach (string text2 in array)
				{
					if (!string.IsNullOrEmpty(text2))
					{
						list.Add(text2);
					}
				}
			}
			view.RelatedViewGroupIds = list.ToArray();
			return view;
		}
	}

	private static class ViewItemColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Position = new DatabaseColumnBinder("position");

		private static DatabaseColumnBinder DefinitionXml = new DatabaseColumnBinder("definition_xml");

		private static DatabaseColumnBinder ViewId = new DatabaseColumnBinder("view_id");

		private static DatabaseColumnBinder ViewGroupId = new DatabaseColumnBinder("view_group_id");

		private static DatabaseColumnBinder MonitorId = new DatabaseColumnBinder("monitor_id");

		private static DatabaseColumnBinder VideoWallId = new DatabaseColumnBinder("video_wall_id");

		public static ViewItem Bind(DatabaseDataReader reader)
		{
			return new ViewItem
			{
				Id = ID.GetString(reader),
				Position = Position.GetInt32(reader),
				DefinitionXml = DefinitionXml.GetString(reader),
				ViewId = ViewId.GetString(reader),
				ViewGroupId = ViewGroupId.GetString(reader),
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

	public View[] QueryViewHierarchyByServerGroupId(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_hierarchy_by_server_group_id");
			BindGuidType("@serverGroupId", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<View> list = new List<View>();
			while (databaseDataReader.Read())
			{
				list.Add(ViewColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public View[] QueryViewAllNoSecurity()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_views_all");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<View> list = new List<View>();
			while (databaseDataReader.Read())
			{
				list.Add(ViewColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public View[] QueryViews(string parentId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_views");
			BindGuidType("@parentId", parentId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<View> list = new List<View>();
			while (databaseDataReader.Read())
			{
				list.Add(ViewColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public View QueryView(string viewId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_by_id");
			BindGuidType("@pkid", viewId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (View)null;
			}
			View result = ViewColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public View[] QueryViewsByIds(string[] viewIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_views_by_ids");
			BindIdTable("@viewIds", viewIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<View> list = new List<View>();
			while (databaseDataReader.Read())
			{
				list.Add(ViewColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryViewIds(string viewGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_ids");
			BindGuidType("@parentId", viewGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				list.Add(IdColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateView(string parentId, View view)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			try
			{
				BeginTransaction(IsolationLevel.ReadCommitted);
				PrepareStoredProcedure("create_video_wall_view");
				BindGuidType("@pkid", view.Id);
				BindGuidType("@parent_id", parentId);
				BindNameType("@name", view.Name);
				BindDescription("@description", view.Description);
				BindXml("@definition_xml", view.DefinitionXml);
				BindCustomProperties("@custom_properties", view.CustomProperties);
				ExecuteNonQuery();
				if (view != null && view.ViewItems != null)
				{
					for (int i = 0; i < view.ViewItems.Length; i++)
					{
						view.ViewItems[i].Position = i;
						CreateViewItem(view.Id, view.ViewItems[i]);
					}
				}
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

	public void UpdateView(View view)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_view");
			BindGuidType("@pkid", view.Id);
			BindNameType("@name", view.Name);
			BindDescription("@description", view.Description);
			BindXml("@definition_xml", view.DefinitionXml);
			BindCustomProperties("@custom_properties", view.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void DeleteView(string viewId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_video_wall_view");
			BindGuidType("@pkid", viewId);
			ExecuteNonQuery();
		});
	}

	public string GetIdFromItem(string viewItemId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("SELECT [view_id] ");
			stringBuilder.Append("FROM [VideoWallViewItem] ");
			stringBuilder.Append("WHERE [pkid] = @pkid ");
			PrepareSqlBatch();
			AddStatement(stringBuilder.ToString());
			BindGuidType("@pkid", viewItemId);
			return (string)ExecuteScalar();
		});
	}

	public ViewItem[] QueryViewItemHierarchyByServerGroupId(string serverGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_item_hierarchy_by_server_group_id");
			BindGuidType("@serverGroupId", serverGroupId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ViewItem> list = new List<ViewItem>();
			while (databaseDataReader.Read())
			{
				list.Add(ViewItemColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ViewItem[] QueryViewItemsAll()
	{
		PrepareStoredProcedure("query_view_items_all");
		DatabaseDataReader databaseDataReader = ExecuteReader();
		List<ViewItem> list = new List<ViewItem>();
		while (databaseDataReader.Read())
		{
			list.Add(ViewItemColumns.Bind(databaseDataReader));
		}
		if (databaseDataReader.NextResult())
		{
			throw new DatabaseConfigExtraRowsException();
		}
		return list.ToArray();
	}

	public ViewItem QueryViewItem(string viewItemId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_viewitem_by_id");
			BindGuidType("@pkid", viewItemId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ViewNotFoundException();
			}
			ViewItem result = ViewItemColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string[] QueryViewItemIds(string viewId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_item_ids");
			BindGuidType("@view_id", viewId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			while (databaseDataReader.Read())
			{
				list.Add(IdColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public ViewItem[] QueryViewItemsByIds(string[] viewItemIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_items_by_ids");
			BindIdTable("@viewItemIds", viewItemIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ViewItem> list = new List<ViewItem>();
			while (databaseDataReader.Read())
			{
				list.Add(ViewItemColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateViewItem(string viewId, ViewItem viewItem)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_video_wall_view_item");
			BindGuidType("@pkid", viewItem.Id);
			BindGuidType("@view_id", viewId);
			BindInt("@position", viewItem.Position);
			BindXml("@definition_xml", viewItem.DefinitionXml);
			BindCustomProperties("@custom_properties", viewItem.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateViewItem(ViewItem viewItem)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_viewitem");
			BindGuidType("@pkid", viewItem.Id);
			BindXml("@definition_xml", viewItem.DefinitionXml);
			BindCustomProperties("@custom_properties", viewItem.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void ApplyViewToView(string sourceViewId, string destinationViewId, Guid sessionId, string clientId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_view_apply_view");
			BindGuidType("@source_view_id", sourceViewId);
			BindGuidType("@destination_view_id", destinationViewId);
			BindGuidType("@session_id", sessionId.ToString());
			BindGuidType("@client_id", clientId, allowNullOrEmpty: true);
			ExecuteNonQuery();
		});
	}

	public bool IsMonitorView(string viewId, out string videoWallId)
	{
		bool result;
		(result, videoWallId) = base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT [videowall_id] AS [pkid] FROM [VideoWallMonitor] WHERE [view_id] = @view_id");
			BindGuidType("@view_id", viewId);
			string text = (string)ExecuteScalar();
			return (isMonitorViewResult: text.IsGuid(), videoWallIdResult: text);
		});
		return result;
	}

	public void ChangeViewItemId(string existingId, string newId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_viewitem_id");
			BindGuidType("@pkid", existingId);
			BindGuidType("@newId", newId);
			ExecuteNonQuery();
		});
	}
}
