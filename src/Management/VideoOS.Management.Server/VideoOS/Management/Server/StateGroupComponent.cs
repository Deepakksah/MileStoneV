using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using VideoOS.Management.Server.BusinessObjects.EventTypes;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class StateGroupComponent : GeminiResourceComponent, IStateGroupComponent, IDisposable
{
	private static class StateGroupColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("Id");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder States = new DatabaseColumnBinder("States");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("last_modified");

		public static StateGroupInfo Bind(DatabaseDataReader reader)
		{
			StateGroupInfo stateGroupInfo = new StateGroupInfo();
			stateGroupInfo.Id = Id.GetGuid2(reader);
			stateGroupInfo.Name = Name.GetString(reader);
			stateGroupInfo.States = new Collection<string>(States.GetString(reader).Split(','));
			stateGroupInfo.LastModified = LastModified.GetDateTime(reader);
			return stateGroupInfo;
		}
	}

	public StateGroupInfo QueryStateGroup(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("stategroup_query_by_id");
			BindUniqueIdentifier("@id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (StateGroupInfo)null;
			}
			StateGroupInfo result = StateGroupColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public IEnumerable<StateGroupInfo> QueryStateGroups()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("stategroup_query_all");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<StateGroupInfo> list = new List<StateGroupInfo>();
			while (databaseDataReader.Read())
			{
				list.Add(StateGroupColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public void CreateStateGroup(StateGroupInfo stateGroupInfo)
	{
		if (stateGroupInfo == null)
		{
			throw new ArgumentNullException("stateGroupInfo");
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("stategroup_create");
			BindGuidType("@Id", stateGroupInfo.Id);
			BindNameType("@Name", stateGroupInfo.Name);
			BindLongNameType("@States", string.Join(",", stateGroupInfo.States));
			ExecuteNonQuery();
		});
	}

	public void UpdateStateGroup(StateGroupInfo stateGroupInfo)
	{
		if (stateGroupInfo == null)
		{
			throw new ArgumentNullException("stateGroupInfo");
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("stategroup_update");
			BindGuidType("@Id", stateGroupInfo.Id);
			BindNameType("@Name", stateGroupInfo.Name, allowNull: true);
			BindDescription("@States", string.Join(",", stateGroupInfo.States), allowNull: true);
			ExecuteNonQuery();
		});
	}

	public void DeleteStateGroup(Guid id)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("stategroup_delete");
			BindGuidType("@Id", id);
			ExecuteNonQuery();
		});
	}
}
