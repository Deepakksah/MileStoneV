using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class EventTypeDataComponent : GeminiResourceComponent, IEventTypeDataComponent, IDisposable
{
	private static class EventTypeColumns
	{
		private static DatabaseColumnBinder IDEventType = new DatabaseColumnBinder("IDEventType");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder GeneratorType = new DatabaseColumnBinder("GeneratorType");

		private static DatabaseColumnBinder GeneratorSubType = new DatabaseColumnBinder("GeneratorSubType");

		private static DatabaseColumnBinder BuiltIn = new DatabaseColumnBinder("Builtin");

		public static EventTypeData Bind(DatabaseDataReader reader)
		{
			return new EventTypeData
			{
				Id = IDEventType.GetGuid2(reader),
				Name = Name.GetString(reader),
				GeneratorType = GeneratorType.GetString(reader),
				GeneratorSubType = GeneratorSubType.GetString(reader),
				BuiltIn = BuiltIn.GetBoolean(reader)
			};
		}
	}

	public IEnumerable<EventTypeData> QueryEventTypes()
	{
		string sql = "SELECT CAST([IDEventType] AS UNIQUEIDENTIFIER) AS [IDEventType], [Name], [GeneratorType], [GeneratorSubType], [Builtin] FROM [EventTypes]";
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareSqlBatch(sql.Length);
			AddStatement(sql);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<EventTypeData> list = new List<EventTypeData>();
			while (databaseDataReader.Read())
			{
				list.Add(EventTypeColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}
}
