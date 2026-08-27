using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class PatrollingProfileComponent : GeminiResourceComponent, IPatrollingProfileComponent, IDisposable
{
	public static class PatrollingProfileColumns
	{
		private static DatabaseColumnBinder IdColumn = new DatabaseColumnBinder("IDPatrollingProfile");

		private static DatabaseColumnBinder NameColumn = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder DescriptionColumn = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder CameraDeviceIdColumn = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder LastModifiedColumn = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder DefinitionColumn = new DatabaseColumnBinder("Definition");

		public static PatrollingProfile Bind(DatabaseDataReader reader)
		{
			return new PatrollingProfile
			{
				Id = IdColumn.GetGuid2(reader).ToString(),
				Name = NameColumn.GetString(reader, allowNulls: false),
				CameraDeviceId = CameraDeviceIdColumn.GetGuid2(reader).ToString(),
				Description = DescriptionColumn.GetString(reader, allowNulls: true),
				LastModified = LastModifiedColumn.GetDateTime(reader),
				DefinitionXml = DefinitionColumn.GetString(reader, allowNulls: true)
			};
		}
	}

	public PatrollingProfile QueryPatrollingProfileById(string patrollingProfileId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_patrolling_profile_by_id");
			BindGuidType("@patrolling_profile_id", patrollingProfileId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (PatrollingProfile)null;
			}
			PatrollingProfile result = PatrollingProfileColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public PatrollingProfile[] QueryPatrollingProfiles(string cameraDeviceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_patrolling_profiles_by_camera");
			BindGuidType("@camera_id", cameraDeviceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<PatrollingProfile> list = new List<PatrollingProfile>();
			while (databaseDataReader.Read())
			{
				list.Add(PatrollingProfileColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public PatrollingProfile[] QueryPatrollingProfiles(string[] patrollingProfileIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_patrolling_profiles_by_ids");
			BindIdTable("@patrolling_profile_ids", patrollingProfileIds);
			List<PatrollingProfile> list = new List<PatrollingProfile>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(PatrollingProfileColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreatePatrollingProfile(string cameraDeviceId, PatrollingProfile newPatrollingProfile, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_patrolling_profile");
			BindGuidType("@patrolling_id", newPatrollingProfile.Id);
			BindGuidType("@device_id", cameraDeviceId);
			BindNameType("@name", newPatrollingProfile.Name);
			BindDescription("@description", newPatrollingProfile.Description);
			BindString("@definition", newPatrollingProfile.DefinitionXml, int.MaxValue, allowNull: true, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void UpdatePatrollingProfile(string patrollingProfileId, string name, string description, string definitionXml, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_patrolling_profile");
			BindGuidType("@patrolling_id", patrollingProfileId);
			BindNameType("@name", name);
			BindDescription("@description", description);
			BindString("@definition", definitionXml, int.MaxValue, allowNull: true, SqlDbType.NVarChar);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeletePatrollingProfile(string patrollingProfileId, string name, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_patrolling_profile");
			BindGuidType("@patrolling_id", patrollingProfileId);
			BindNameType("@name", name);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
