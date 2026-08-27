using System;
using System.Collections.Generic;
using VideoOS.Management.Server.BusinessObjects.EventTypes;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class EventTypeComponent : GeminiResourceComponent, IEventTypeComponent, IDisposable
{
	private static class EventTypeColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDEventType");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Definition = new DatabaseColumnBinder("Definition");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder GeneratorType = new DatabaseColumnBinder("GeneratorType");

		private static DatabaseColumnBinder IDGenerator = new DatabaseColumnBinder("IDGenerator");

		private static DatabaseColumnBinder BuiltIn = new DatabaseColumnBinder("Builtin");

		private static DatabaseColumnBinder GeneratorSubType = new DatabaseColumnBinder("GeneratorSubType");

		public static EventTypeInfo Bind(DatabaseDataReader reader)
		{
			EventTypeInfo eventTypeInfo = new EventTypeInfo
			{
				IDEventType = new Guid(Id.GetString(reader)),
				Name = Name.GetString(reader),
				Definition = Definition.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				GeneratorType = GeneratorType.GetString(reader),
				BuiltIn = BuiltIn.GetBoolean(reader),
				GeneratorSubType = GeneratorSubType.GetString(reader)
			};
			string text = IDGenerator.GetString(reader);
			if (!string.IsNullOrWhiteSpace(text))
			{
				try
				{
					eventTypeInfo.IDGenerator = new Guid(text);
				}
				catch (FormatException)
				{
				}
			}
			return eventTypeInfo;
		}
	}

	public EventTypeInfo GetEventType(Guid eventTypeId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_eventtype_by_id");
			BindUniqueIdentifier("@idEventType", eventTypeId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (EventTypeInfo)null;
			}
			EventTypeInfo result = EventTypeColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public IEnumerable<EventTypeInfo> GetEventTypes(string[] generatorTypes, string idGenerator)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_eventtypes");
			BindIdTable("@GeneratorTypes", generatorTypes);
			if (string.IsNullOrEmpty(idGenerator))
			{
				BindUniqueIdentifier("@idGenerator", (Guid?)null);
			}
			else
			{
				BindUniqueIdentifier("@idGenerator", new Guid(idGenerator));
			}
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<EventTypeInfo> list = new List<EventTypeInfo>();
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
