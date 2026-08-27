using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class VideoWallPresetComponent : GeminiResourceComponent, IVideoWallPresetComponent, IDisposable
{
	private static class VideoWallPresetColumns
	{
		private static DatabaseColumnBinder ID = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("description");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("created_date");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		private static DatabaseColumnBinder VideoWallId = new DatabaseColumnBinder("videowall_id");

		public static VideoWallPreset Bind(DatabaseDataReader reader)
		{
			return new VideoWallPreset
			{
				Id = ID.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				CreatedDate = CreatedDate.GetDateTime(reader),
				LastModified = LastModified.GetDateTime(reader),
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

	public VideoWallPreset QueryVideoWallPreset(string videoWallPresetId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_preset_by_id");
			BindGuidType("@pkid", videoWallPresetId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new VideoWallPresetNotFoundException();
			}
			VideoWallPreset result = VideoWallPresetColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public string[] QueryVideoWallPresetIds(string videoWallId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_preset_ids");
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

	public VideoWallPreset[] QueryVideoWallPresetsByIds(string[] videoWallPresetIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_video_wall_presets_by_ids");
			BindIdTable("@videoWallPresetIds", videoWallPresetIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<VideoWallPreset> list = new List<VideoWallPreset>();
			while (databaseDataReader.Read())
			{
				list.Add(VideoWallPresetColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateVideoWallPreset(string videoWallId, VideoWallPreset videoWallPreset, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_video_wall_preset");
			BindGuidType("@pkid", videoWallPreset.Id);
			BindGuidType("@videowall_id", videoWallId);
			BindNameType("@name", videoWallPreset.Name);
			BindDescription("@description", videoWallPreset.Description);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", videoWallPreset.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateVideoWallPreset(VideoWallPreset videoWallPreset, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_video_wall_preset");
			BindGuidType("@pkid", videoWallPreset.Id);
			BindNameType("@name", videoWallPreset.Name);
			BindDescription("@description", videoWallPreset.Description);
			BindGuidType("@session_id", sessionId.ToString());
			BindCustomProperties("@custom_properties", videoWallPreset.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void DeleteVideoWallPreset(string videoWallPresetId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_video_wall_preset");
			BindGuidType("@pkid", videoWallPresetId);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public string GetVideoWallId(string videoWallPresetId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch();
			AddStatement("SELECT [videowall_id] FROM [VideoWallPreset] WHERE [pkid] = @pkid");
			BindGuidType("@pkid", videoWallPresetId);
			return (string)ExecuteScalar();
		});
	}
}
