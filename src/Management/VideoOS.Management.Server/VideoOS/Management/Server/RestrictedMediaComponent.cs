using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RestrictedMediaComponent : GeminiResourceComponent, IRestrictedMediaComponent, IDisposable
{
	private static class RestrictedMediaColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDRestrictedMedia");

		private static DatabaseColumnBinder DeviceIds = new DatabaseColumnBinder("IDDevices");

		private static DatabaseColumnBinder Header = new DatabaseColumnBinder("Header");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder User = new DatabaseColumnBinder("UserName");

		private static DatabaseColumnBinder StartTime = new DatabaseColumnBinder("StartTime");

		private static DatabaseColumnBinder EndTime = new DatabaseColumnBinder("EndTime");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static RestrictedMediaDataContract Bind(DatabaseDataReader reader)
		{
			return new RestrictedMediaDataContract
			{
				Id = Id.GetGuid2(reader).ToString(),
				DeviceIds = (from g in DatabaseXmlConverter.XmlIdsToGuids(DeviceIds.GetString(reader))
					select g.ToString()).ToArray(),
				Header = Header.GetString(reader),
				Description = Description.GetString(reader),
				UserName = User.GetString(reader),
				StartTime = DateTime.SpecifyKind(StartTime.GetDateTime(reader), DateTimeKind.Utc),
				EndTime = DateTime.SpecifyKind(EndTime.GetDateTime(reader), DateTimeKind.Utc),
				LastModified = DateTime.SpecifyKind(LastModified.GetDateTime(reader), DateTimeKind.Utc)
			};
		}
	}

	private static class RestrictedMediaLiveColumns
	{
		private static DatabaseColumnBinder DeviceId = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder User = new DatabaseColumnBinder("UserName");

		private static DatabaseColumnBinder StartTime = new DatabaseColumnBinder("StartTime");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static RestrictedMediaLiveDataContract Bind(DatabaseDataReader reader)
		{
			return new RestrictedMediaLiveDataContract
			{
				DeviceId = DeviceId.GetGuid(reader),
				UserName = User.GetString(reader),
				StartTime = DateTime.SpecifyKind(StartTime.GetDateTime(reader), DateTimeKind.Utc),
				LastModified = DateTime.SpecifyKind(LastModified.GetDateTime(reader), DateTimeKind.Utc)
			};
		}
	}

	public Guid[] CreatePlayback(Guid id, string[] deviceIds, string header, string description, string userName, DateTime startTime, DateTime endTime, DateTime lastModified, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_create");
			BindUniqueIdentifier("@IDRestrictedMedia", id);
			BindIdTable("@IDDevices", deviceIds);
			BindNameType("@UserName", userName);
			BindLongNameType("@Header", header);
			BindDescription2("@Description", description);
			BindDateTime2("@StartTime", startTime);
			BindDateTime2("@EndTime", endTime);
			BindDateTime2("@LastModified", lastModified);
			BindGuidType("@session_id", sessionId.ToString());
			List<Guid> list = new List<Guid>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseDataReader.GetString(0)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid[] UpdatePlayback(Guid id, string[] deviceIds, string header, string description, DateTime startTime, DateTime endTime, DateTime lastModified, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_update");
			BindUniqueIdentifier("@IDRestrictedMedia", id);
			BindIdTable("@IDDevices", deviceIds);
			BindLongNameType("@Header", header);
			BindDescription2("@Description", description);
			BindDateTime2("@StartTime", startTime);
			BindDateTime2("@EndTime", endTime);
			BindDateTime2("@LastModified", lastModified);
			BindGuidType("@session_id", sessionId.ToString());
			List<Guid> list = new List<Guid>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseDataReader.GetString(0)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid[] DeletePlayback(Guid id, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_delete");
			BindUniqueIdentifier("@IDRestrictedMedia", id);
			BindGuidType("@session_id", sessionId.ToString());
			List<Guid> list = new List<Guid>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseDataReader.GetString(0)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public RestrictedMediaDataContract QueryPlayback(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_query_by_id");
			BindUniqueIdentifier("@IDRestrictedMedia", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (RestrictedMediaDataContract)null;
			}
			RestrictedMediaDataContract result = RestrictedMediaColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public RestrictedMediaDataContract[] QueryAllPlayback()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_query_all");
			List<RestrictedMediaDataContract> list = new List<RestrictedMediaDataContract>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(RestrictedMediaColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid[] EnterLive(string[] deviceIds, string userName, DateTime startTime, DateTime lastModified, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_live_mode_enter");
			BindIdTable("@IDDevices", deviceIds);
			BindDateTime2("@StartTime", startTime);
			BindNameType("@UserName", userName);
			BindDateTime2("@LastModified", lastModified);
			BindGuidType("@session_id", sessionId);
			List<Guid> list = new List<Guid>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseDataReader.GetString(0)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid[] ExitLive(Guid id, string[] deviceIds, string header, string description, string userName, DateTime startTime, DateTime endTime, DateTime lastModified, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_live_mode_exit");
			BindUniqueIdentifier("@IDRestrictedMedia", id);
			BindIdTable("@IDDevices", deviceIds);
			BindNameType("@UserName", userName);
			BindLongNameType("@Header", header);
			BindDescription2("@Description", description);
			BindDateTime2("@StartTime", startTime);
			BindDateTime2("@EndTime", endTime);
			BindDateTime2("@LastModified", lastModified);
			BindGuidType("@session_id", sessionId.ToString());
			HashSet<Guid> hashSet = new HashSet<Guid>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				hashSet.Add(new Guid(databaseDataReader.GetString(0)));
			}
			if (databaseDataReader.NextResult())
			{
				while (databaseDataReader.Read())
				{
					hashSet.Add(new Guid(databaseDataReader.GetString(0)));
				}
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return hashSet.ToArray();
		});
	}

	public Guid[] UpdateLive(string[] deviceIds, DateTime startTime, DateTime lastModified, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_live_update");
			BindIdTable("@IDDevices", deviceIds);
			BindDateTime2("@StartTime", startTime);
			BindDateTime2("@LastModified", lastModified);
			BindGuidType("@session_id", sessionId.ToString());
			List<Guid> list = new List<Guid>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseDataReader.GetString(0)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid[] DeleteLive(string[] deviceIds, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_live_delete");
			BindIdTable("@IDDevices", deviceIds);
			BindGuidType("@session_id", sessionId);
			List<Guid> list = new List<Guid>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(new Guid(databaseDataReader.GetString(0)));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public RestrictedMediaLiveDataContract[] QueryLiveByIds(IEnumerable<string> deviceIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_live_query_by_ids");
			BindIdTable("@IDDevices", deviceIds);
			List<RestrictedMediaLiveDataContract> list = new List<RestrictedMediaLiveDataContract>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(RestrictedMediaLiveColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public RestrictedMediaLiveDataContract[] QueryAllLive()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("restricted_media_live_query_all");
			List<RestrictedMediaLiveDataContract> list = new List<RestrictedMediaLiveDataContract>();
			DatabaseDataReader databaseDataReader = ExecuteReader();
			while (databaseDataReader.Read())
			{
				list.Add(RestrictedMediaLiveColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}
}
