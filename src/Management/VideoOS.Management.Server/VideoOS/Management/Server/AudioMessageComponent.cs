using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class AudioMessageComponent<T> : GeminiResourceComponent, IAudioMessageComponent<T>, IDisposable where T : IAudioMessageItem, new()
{
	private static class AudioMessageColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("ID");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder OriginalFileName = new DatabaseColumnBinder("OriginalFileName");

		public static T Bind(DatabaseDataReader reader)
		{
			T result = new T();
			string id = Id.GetString(reader);
			result.Id = id;
			string name = Name.GetString(reader);
			result.Name = name;
			string description = Description.GetString(reader);
			result.Description = description;
			DateTime dateTime = LastModified.GetDateTime(reader);
			result.LastModified = dateTime;
			string originalFileName = OriginalFileName.GetString(reader);
			result.OriginalFileName = originalFileName;
			return result;
		}
	}

	public void CreateAudioMessage(string id, string name, string description, string originalFileName, byte[] content, int maxMessageCount)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_audio_message");
			BindGuidType("@Id", id);
			BindNameType("@Name", name);
			BindDescription("@Description", description);
			BindLongNameType("@OriginalFileName", originalFileName);
			BindVarBinaryMax("@Content", content);
			BindInt("@MaxMessageCount", maxMessageCount);
			ExecuteNonQuery();
		});
	}

	public string[] QueryAudioMessageIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_audio_message_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("ID");
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

	public T[] QueryAudioMessages()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_audio_messages");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<T> list = new List<T>();
			while (databaseDataReader.Read())
			{
				list.Add(AudioMessageColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public T QueryAudioMessageById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_audio_message_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw VmoExceptionFactory.VMO60316_ObjectNotFoundException();
			}
			T result = AudioMessageColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public byte[] QueryAudioMessageContentById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_audio_message_content_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw VmoExceptionFactory.VMO60316_ObjectNotFoundException();
			}
			byte[] binary = new DatabaseColumnBinder("Content").GetBinary(databaseDataReader, allowNulls: false);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return binary;
		});
	}

	public T[] QueryAudioMessagesByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_audio_messages_by_ids");
			BindIdTable("@ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<T> list = new List<T>();
			while (databaseDataReader.Read())
			{
				list.Add(AudioMessageColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void UpdateAudioMessage(string id, string name, string description)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_audio_message");
			BindGuidType("@Id", id);
			BindNameType("@Name", name);
			BindDescription("@Description", description);
			ExecuteNonQuery();
		});
	}

	public void UpdateAudioMessageContent(string id, string originalFileName, byte[] content, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_audio_message_content");
			BindGuidType("@Id", id);
			BindLongNameType("@OriginalFileName", originalFileName);
			BindVarBinaryMax("@Content", content);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeleteAudioMessage(string id, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_audio_message");
			BindGuidType("@Id", id);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
