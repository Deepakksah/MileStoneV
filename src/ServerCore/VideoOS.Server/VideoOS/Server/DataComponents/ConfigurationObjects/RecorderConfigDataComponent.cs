using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class RecorderConfigDataComponent : VmoResourceComponent, IRecorderConfigDataComponent, IDisposable
{
	private static class RecorderConfigDataColumns
	{
		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static RecorderConfigData Bind(DatabaseDataReader reader)
		{
			RecorderConfigData recorderConfigData = new RecorderConfigData();
			recorderConfigData.Id = IDRecorder.GetStringAsGuid(reader);
			recorderConfigData.Name = Name.GetString(reader);
			recorderConfigData.Enabled = Enabled.GetBoolean(reader);
			recorderConfigData.LastModified = LastModified.GetDateTime(reader);
			return recorderConfigData;
		}
	}

	public RecorderConfigDataComponent()
	{
	}

	public RecorderConfigData[] QueryRecorderData()
	{
		string sql = "SELECT [IDRecorder], [Name], [Enabled], [LastModified] FROM [Recorders]";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RecorderConfigData> list = new List<RecorderConfigData>();
			while (databaseDataReader.Read())
			{
				list.Add(RecorderConfigDataColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public RecorderConfigData QueryRecorderData(Guid recorderId)
	{
		string sql = "SELECT [IDRecorder], [Name], [Enabled], [LastModified] FROM [Recorders] WHERE [IDRecorder] = @IDRecorder";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDRecorder", recorderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			RecorderConfigData result = null;
			if (databaseDataReader.Read())
			{
				result = RecorderConfigDataColumns.Bind(databaseDataReader);
			}
			return result;
		});
	}

	public bool RecorderExists(Guid id)
	{
		string sql = "SELECT COUNT([IDRecorder]) FROM [Recorders] WHERE [IDRecorder] = @IDRecorder";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDRecorder", id);
			int num = (int)ExecuteScalar();
			return num != 0;
		});
	}

	public bool RecorderEnabled(Guid id)
	{
		string sql = "SELECT COUNT([IDRecorder]) FROM [Recorders] WHERE [IDRecorder] = @IDRecorder AND [Enabled] = 1";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			BindGuidType("@IDRecorder", id);
			int num = (int)ExecuteScalar();
			return num != 0;
		});
	}
}
