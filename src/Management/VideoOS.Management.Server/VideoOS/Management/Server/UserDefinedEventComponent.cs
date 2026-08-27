using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class UserDefinedEventComponent : GeminiResourceComponent, IUserDefinedEventComponent, IDisposable
{
	private static class UserDefinedEventColumns
	{
		private static DatabaseColumnBinder IDEventType = new DatabaseColumnBinder("IDEventType");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Definition = new DatabaseColumnBinder("Definition");

		private static DatabaseColumnBinder Subtype = new DatabaseColumnBinder("GeneratorSubType");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static UserDefinedEvent Bind(DatabaseDataReader reader)
		{
			return new UserDefinedEvent
			{
				Id = IDEventType.GetString(reader),
				Name = Name.GetString(reader, allowNulls: false),
				Definition = new EventDefinition(Definition.GetString(reader, allowNulls: false)),
				Subtype = Subtype.GetString(reader, allowNulls: false),
				LastModified = LastModified.GetDateTime(reader)
			};
		}
	}

	public UserDefinedEventComponent()
	{
	}

	public UserDefinedEventComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	public string[] QueryUserDefinedEventIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_user_defined_event_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDEventType");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public UserDefinedEvent QueryUserDefinedEvent(string userDefinedEventId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_user_defined_event_by_id");
			BindGuidType("@IDEventType", userDefinedEventId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (UserDefinedEvent)null;
			}
			UserDefinedEvent result = UserDefinedEventColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public UserDefinedEvent[] QueryUserDefinedEvents(string[] userDefinedEventIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_user_defined_events_by_ids");
			BindIdTable("@IDEventIds", userDefinedEventIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<UserDefinedEvent> list = new List<UserDefinedEvent>();
			while (databaseDataReader.Read())
			{
				list.Add(UserDefinedEventColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateUserDefinedEvent(UserDefinedEvent newUserDefinedEvent, string userSid)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_user_defined_event");
			BindSIDType("@UserSID", userSid);
			BindGuidType("@IDEventType", newUserDefinedEvent.Id);
			BindString("@GeneratorSubType", newUserDefinedEvent.Subtype, 36, allowNull: false, SqlDbType.VarChar);
			BindNameType("@Name", newUserDefinedEvent.Name);
			BindText("@Definition", newUserDefinedEvent.Definition.SaveAsXml(), allowNull: false);
			BindCustomProperties("@custom_properties", newUserDefinedEvent.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void UpdateUserDefinedEvent(UserDefinedEvent userDefinedEvent)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_user_defined_event");
			BindGuidType("@IDEventType", userDefinedEvent.Id);
			BindNameType("@Name", userDefinedEvent.Name);
			BindText("@Definition", userDefinedEvent.Definition.SaveAsXml(), allowNull: false);
			BindCustomProperties("@custom_properties", userDefinedEvent.CustomProperties);
			ExecuteNonQuery();
		});
	}

	public void DeleteUserDefinedEvent(string userDefinedEventId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_user_defined_event");
			BindGuidType("@IDEventType", userDefinedEventId);
			ExecuteNonQuery();
		});
	}
}
