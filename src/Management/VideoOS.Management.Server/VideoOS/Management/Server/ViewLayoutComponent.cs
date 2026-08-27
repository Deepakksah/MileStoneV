using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class ViewLayoutComponent : GeminiResourceComponent, IViewLayoutComponent, IDisposable
{
	private static class ViewLayoutColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder DefinitionXml = new DatabaseColumnBinder("definition_xml");

		private static DatabaseColumnBinder ViewLayoutGroupId = new DatabaseColumnBinder("view_layout_group_id");

		public static ViewLayout Bind(DatabaseDataReader reader)
		{
			return new ViewLayout
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
				DefinitionXml = DefinitionXml.GetString(reader),
				ViewLayoutGroupId = ViewLayoutGroupId.GetString(reader)
			};
		}
	}

	public ViewLayout QueryViewLayout(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_layout_by_id");
			BindGuidType("@pkid", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new ViewLayoutNotFoundException();
			}
			ViewLayout result = ViewLayoutColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public ViewLayout[] QueryViewLayoutsByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_layouts_by_ids");
			BindIdTable("@viewLayoutIds", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ViewLayout> list = new List<ViewLayout>();
			while (databaseDataReader.Read())
			{
				list.Add(ViewLayoutColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryViewLayoutIds(string viewLayoutGroupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_view_layouts_ids");
			BindGuidType("@parentId", viewLayoutGroupId);
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

	public void CreateViewLayout(string id, string viewLayoutGroupId, string name, string description, string definitionXml)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_video_wall_view_layout");
			BindGuidType("@pkid", id);
			BindGuidType("@parent_id", viewLayoutGroupId);
			BindNameType("@name", name);
			BindDescription("@description", description);
			BindXml("@definition_xml", definitionXml);
			ExecuteNonQuery();
		});
	}

	public void UpdateViewLayout(string id, string name, string description, string definitionXml)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_view_layout");
			BindGuidType("@pkid", id);
			BindNameType("@name", name);
			BindDescription("@description", description);
			BindXml("@definition_xml", definitionXml);
			ExecuteNonQuery();
		});
	}

	public void DeleteViewLayout(string id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_video_wall_view_layout");
			BindGuidType("@pkid", id);
			ExecuteNonQuery();
		});
	}
}
