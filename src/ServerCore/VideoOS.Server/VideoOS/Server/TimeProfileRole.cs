using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public class TimeProfileRole
{
	private sealed class TimeProfileRoleComponent : VmoResourceComponent
	{
		private static class RolesColumns
		{
			private static DatabaseColumnBinder RoleId = new DatabaseColumnBinder("IDRole");

			public static Guid Bind(DatabaseDataReader reader)
			{
				return RoleId.GetStringAsGuid(reader);
			}
		}

		public TimeProfileRoleComponent()
		{
		}

		public Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId)
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_roles_for_default_time_profile");
				BindGuidType("@IDTimeProfile ", timeProfileId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<Guid> list = new List<Guid>();
				while (databaseDataReader.Read())
				{
					list.Add(RolesColumns.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}

		public Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId)
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_roles_for_permissions_with_time_profile");
				BindGuidType("@IDTimeProfile ", timeProfileId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				List<Guid> list = new List<Guid>();
				while (databaseDataReader.Read())
				{
					list.Add(RolesColumns.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
	}

	public static Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId)
	{
		using TimeProfileRoleComponent timeProfileRoleComponent = new TimeProfileRoleComponent();
		return timeProfileRoleComponent.QueryRolesForDefaultTimeProfile(timeProfileId);
	}

	public static Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId)
	{
		using TimeProfileRoleComponent timeProfileRoleComponent = new TimeProfileRoleComponent();
		return timeProfileRoleComponent.QueryRolesForPermissionsWithTimeProfile(timeProfileId);
	}
}
