using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class ExternalPluginsDataComponent : GeminiResourceComponent, IExternalPluginsDataComponent, IDisposable
{
	private static class ExternalPluginColumns
	{
		private static DatabaseColumnBinder PluginID = new DatabaseColumnBinder("PluginID");

		private static DatabaseColumnBinder Data = new DatabaseColumnBinder("Data");

		public static ExternalPluginsData Bind(DatabaseDataReader reader)
		{
			return new ExternalPluginsData
			{
				PluginId = new Guid(PluginID.GetGuid(reader)),
				Data = Data.GetString(reader)
			};
		}
	}

	public IEnumerable<ExternalPluginsData> QueryExternalServerPlugins()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_external_server_plugins");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ExternalPluginsData> list = new List<ExternalPluginsData>();
			while (databaseDataReader.Read())
			{
				list.Add(ExternalPluginColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public IEnumerable<ExternalPluginsData> QueryExternalSmartClientPlugins()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_external_smart_client_plugins");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ExternalPluginsData> list = new List<ExternalPluginsData>();
			while (databaseDataReader.Read())
			{
				list.Add(ExternalPluginColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public ExternalPluginsData QueryExternalSmartClientPlugin(Guid pluginId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_external_smart_client_plugin_by_id");
			BindGuidType("@IDPlugin", pluginId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (ExternalPluginsData)null;
			}
			ExternalPluginsData result = ExternalPluginColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}
}
