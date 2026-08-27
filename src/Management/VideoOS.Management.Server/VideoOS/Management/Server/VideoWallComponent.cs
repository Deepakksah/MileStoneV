using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class VideoWallComponent : GeminiResourceComponent, IVideoWallComponent, IDisposable
{
	private static class VideoWallColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder Width = new DatabaseColumnBinder("width");

		private static DatabaseColumnBinder Height = new DatabaseColumnBinder("height");

		private static DatabaseColumnBinder StatusText = new DatabaseColumnBinder("statustext");

		private static DatabaseColumnBinder TitleBar = new DatabaseColumnBinder("titlebar");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		public static VideoWall Bind(DatabaseDataReader reader)
		{
			return new VideoWall
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				Width = Width.GetDouble(reader),
				Height = Height.GetDouble(reader),
				StatusText = StatusText.GetBoolean(reader),
				TitleBar = (TitleBar)Enum.Parse(typeof(TitleBar), TitleBar.GetString(reader, allowNulls: false), ignoreCase: true),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader)
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

	public VideoWallComponent()
	{
	}

	public VideoWallComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	public VideoWall[] QueryVideoWallsByIds(string[] videoWallIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_walls_by_ids");
			BindIdTable("@VideoWallIds", videoWallIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<VideoWall> list = new List<VideoWall>();
			while (databaseDataReader.Read())
			{
				list.Add(VideoWallColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryVideoWallIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_ids");
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

	public VideoWall QueryVideoWall(string videoWallId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_by_id");
			BindGuidType("@pkid", videoWallId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new VideoWallNotFoundException();
			}
			VideoWall result = VideoWallColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public void CreateVideoWall(VideoWall videoWall, string userSid)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_video_wall");
			BindGuidType("@pkid", videoWall.Id);
			BindNameType("@name", videoWall.Name);
			BindDescription("@description", videoWall.Description);
			BindFloat("@width", videoWall.Width);
			BindFloat("@height", videoWall.Height);
			BindBoolean("@statusText", videoWall.StatusText);
			BindEnumType("@titlebar", typeof(TitleBar), videoWall.TitleBar);
			BindSIDType("@user_sid", userSid);
			BindCustomProperties("@custom_properties", videoWall.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateVideoWall(VideoWall videoWall, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall");
			BindGuidType("@pkid", videoWall.Id);
			BindNameType("@name", videoWall.Name);
			BindDescription("@description", videoWall.Description);
			BindFloat("@width", videoWall.Width);
			BindFloat("@height", videoWall.Height);
			BindBoolean("@statusText", videoWall.StatusText);
			BindEnumType("@titlebar", typeof(TitleBar), videoWall.TitleBar);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", videoWall.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void DeleteVideoWall(string videoWallId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_video_wall");
			BindGuidType("@pkid", videoWallId);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
