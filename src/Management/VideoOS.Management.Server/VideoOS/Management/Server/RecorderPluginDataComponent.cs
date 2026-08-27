using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RecorderPluginDataComponent : GeminiResourceComponent, IRecorderPluginDataComponent, IDisposable
{
	private static class RecorderPluginColumns
	{
		private static DatabaseColumnBinder IDRecorderPlugin = new DatabaseColumnBinder("IDRecorderPlugin");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		public static RecorderPluginData Bind(DatabaseDataReader reader)
		{
			return new RecorderPluginData
			{
				RecorderPluginId = new Guid(IDRecorderPlugin.GetGuid(reader)),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader)
			};
		}
	}

	public RecorderPluginData QueryPluginById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recorder_plugin_by_id");
			BindGuidType("@IDRecorderPlugin", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (RecorderPluginData)null;
			}
			RecorderPluginData result = RecorderPluginColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public IEnumerable<RecorderPluginData> QueryPlugins()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recorder_plugins");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RecorderPluginData> list = new List<RecorderPluginData>();
			while (databaseDataReader.Read())
			{
				list.Add(RecorderPluginColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}
}
