using System;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class UserDefinedEventComponent : VmoResourceComponent
{
	private static class UserDefinedEventColumns
	{
		private static DatabaseColumnBinder IDEventType = new DatabaseColumnBinder("IDEventType");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Definition = new DatabaseColumnBinder("Definition");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static UserDefinedEvent Bind(DatabaseDataReader reader)
		{
			UserDefinedEvent userDefinedEvent = new UserDefinedEvent();
			userDefinedEvent.Id = new Guid(IDEventType.GetString(reader));
			userDefinedEvent.Name = Name.GetString(reader, allowNulls: false);
			userDefinedEvent.Definition = new EventDefinition(Definition.GetString(reader, allowNulls: false));
			userDefinedEvent.LastModified = LastModified.GetDateTime(reader);
			return userDefinedEvent;
		}
	}

	public UserDefinedEvent QueryUserDefinedEvent(Guid userDefinedEventId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_user_defined_event_by_id");
			BindGuidType("@IDEventType", userDefinedEventId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new SecurityDatabaseConfigMissingRowException();
			}
			UserDefinedEvent result = UserDefinedEventColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}
}
