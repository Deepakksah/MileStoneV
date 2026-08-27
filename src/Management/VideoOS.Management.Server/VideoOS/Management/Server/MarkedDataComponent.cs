using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient.Server;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MarkedDataComponent : GeminiResourceComponent, IMarkedDataComponent, IDisposable
{
	private static class BookmarkDeviceDataColumns
	{
		private static DatabaseColumnBinder IDMarkedData = new DatabaseColumnBinder("IDMarkedData");

		private static DatabaseColumnBinder DeviceId = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder Size = new DatabaseColumnBinder("Size");

		public static BookmarkedDevice Bind(DatabaseDataReader reader)
		{
			return new BookmarkedDevice
			{
				MarkedDataId = IDMarkedData.GetGuid2(reader).ToString(),
				DeviceId = DeviceId.GetGuid(reader),
				Size = Size.GetInt64(reader)
			};
		}
	}

	private class BookmarkedDevice
	{
		public string MarkedDataId { get; set; }

		public string DeviceId { get; set; }

		public long Size { get; set; }
	}

	private static class BookmarkDataColumns
	{
		private static DatabaseColumnBinder IDMarkedData = new DatabaseColumnBinder("IDMarkedData");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("CreatedDate");

		private static DatabaseColumnBinder Reference = new DatabaseColumnBinder("Reference");

		private static DatabaseColumnBinder User = new DatabaseColumnBinder("UserName");

		private static DatabaseColumnBinder Header = new DatabaseColumnBinder("Header");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder MarkedDataType = new DatabaseColumnBinder("MarkDataType");

		private static DatabaseColumnBinder StartTime = new DatabaseColumnBinder("StartTime");

		private static DatabaseColumnBinder TagTime = new DatabaseColumnBinder("TagTime");

		private static DatabaseColumnBinder EndTime = new DatabaseColumnBinder("EndTime");

		private static DatabaseColumnBinder RetentionType = new DatabaseColumnBinder("RetentionType");

		private static DatabaseColumnBinder RetentionUnits = new DatabaseColumnBinder("RetentionUnits");

		private static DatabaseColumnBinder RetentionExpire = new DatabaseColumnBinder("RetentionExpire");

		private static DatabaseColumnBinder ProtectionSize = new DatabaseColumnBinder("ProtectionSize");

		public static MarkedDataDataContract Bind(DatabaseDataReader reader)
		{
			MarkedDataDataContract obj = new MarkedDataDataContract
			{
				Id = IDMarkedData.GetGuid2(reader).ToString(),
				LastModified = DateTime.SpecifyKind(LastModified.GetDateTime(reader), DateTimeKind.Utc),
				Created = DateTime.SpecifyKind(CreatedDate.GetDateTime(reader), DateTimeKind.Utc),
				Reference = Reference.GetString(reader),
				UserName = User.GetString(reader),
				Header = Header.GetString(reader),
				Description = Description.GetString(reader),
				MarkedDataType = MarkedDataType.GetInt32(reader),
				StartTime = new DateTime(StartTime.GetInt64(reader), DateTimeKind.Utc),
				TagTime = new DateTime(TagTime.GetInt64(reader), DateTimeKind.Utc),
				EndTime = new DateTime(EndTime.GetInt64(reader), DateTimeKind.Utc)
			};
			obj.UseRetention = obj.MarkedDataType == 2;
			obj.RetentionType = RetentionType.GetInt32(reader);
			obj.RetentionUnits = RetentionUnits.GetInt32(reader);
			obj.RetentionExpire = new DateTime(RetentionExpire.GetInt64(reader), DateTimeKind.Utc);
			obj.RetentionSize = ProtectionSize.GetInt64(reader);
			return obj;
		}
	}

	private static class MarkedDataColumns
	{
		private static DatabaseColumnBinder IDMarkedData = new DatabaseColumnBinder("IDMarkedData");

		private static DatabaseColumnBinder DeviceIds = new DatabaseColumnBinder("IDDevices");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder CreatedDate = new DatabaseColumnBinder("CreatedDate");

		private static DatabaseColumnBinder Reference = new DatabaseColumnBinder("Reference");

		private static DatabaseColumnBinder User = new DatabaseColumnBinder("UserName");

		private static DatabaseColumnBinder Header = new DatabaseColumnBinder("Header");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder MarkedDataType = new DatabaseColumnBinder("MarkDataType");

		private static DatabaseColumnBinder StartTime = new DatabaseColumnBinder("StartTime");

		private static DatabaseColumnBinder TagTime = new DatabaseColumnBinder("TagTime");

		private static DatabaseColumnBinder EndTime = new DatabaseColumnBinder("EndTime");

		private static DatabaseColumnBinder RetentionType = new DatabaseColumnBinder("RetentionType");

		private static DatabaseColumnBinder RetentionUnits = new DatabaseColumnBinder("RetentionUnits");

		private static DatabaseColumnBinder RetentionExpire = new DatabaseColumnBinder("RetentionExpire");

		private static DatabaseColumnBinder ProtectionSize = new DatabaseColumnBinder("ProtectionSize");

		public static MarkedDataDataContract Bind(DatabaseDataReader reader)
		{
			MarkedDataDataContract obj = new MarkedDataDataContract
			{
				Id = IDMarkedData.GetGuid2(reader).ToString(),
				StickerSizes = DatabaseXmlConverter.DeviceXmlToStickerSize(DeviceIds.GetString(reader))
			};
			obj.DeviceIds = obj.StickerSizes.Select((StickerSize2 d) => d.DeviceId.ToString()).ToArray();
			obj.LastModified = DateTime.SpecifyKind(LastModified.GetDateTime(reader), DateTimeKind.Utc);
			obj.Created = DateTime.SpecifyKind(CreatedDate.GetDateTime(reader), DateTimeKind.Utc);
			obj.Reference = Reference.GetString(reader);
			obj.UserName = User.GetString(reader);
			obj.Header = Header.GetString(reader);
			obj.Description = Description.GetString(reader);
			obj.MarkedDataType = MarkedDataType.GetInt32(reader);
			obj.StartTime = new DateTime(StartTime.GetInt64(reader), DateTimeKind.Utc);
			obj.TagTime = new DateTime(TagTime.GetInt64(reader), DateTimeKind.Utc);
			obj.EndTime = new DateTime(EndTime.GetInt64(reader), DateTimeKind.Utc);
			obj.UseRetention = obj.MarkedDataType == 2;
			obj.RetentionType = RetentionType.GetInt32(reader);
			obj.RetentionUnits = RetentionUnits.GetInt32(reader);
			obj.RetentionExpire = new DateTime(RetentionExpire.GetInt64(reader), DateTimeKind.Utc);
			obj.RetentionSize = ProtectionSize.GetInt64(reader);
			return obj;
		}
	}

	private static readonly SqlMetaData[] _metadataStickerSize = new SqlMetaData[2]
	{
		new SqlMetaData("IDDevice", SqlDbType.UniqueIdentifier),
		new SqlMetaData("Size", SqlDbType.BigInt)
	};

	public MarkedDataDataContract QueryMarkedDataItemNoSecurity(string markedDataId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("markeddata_query_item_nosecurity");
			BindUniqueIdentifier("@IDMarkedData", new Guid(markedDataId));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (MarkedDataDataContract)null;
			}
			MarkedDataDataContract result = MarkedDataColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public MarkedDataDataContract[] QueryBookmarksSearchDeviceFilter(BookmarkSearchParametersDeviceFilter searchParameters)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("bookmarks_query_search_device_filter");
			BindIdTable("@IDDevices", searchParameters.DeviceIds);
			BindResultTable("@Users", searchParameters.Users);
			BindString("@SearchStr", searchParameters.SearchString, 512, allowNull: true, SqlDbType.NVarChar);
			BindBoolean("@SearchHeader", searchParameters.SearchHeader);
			BindBoolean("@SearchDescription", searchParameters.SearchDescription);
			BindBoolean("@SearchReference", searchParameters.SearchReference);
			BindBoolean("@SearchUser", searchParameters.SearchUser);
			BindString("@Header", searchParameters.HeaderFilter, 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Description", searchParameters.DescriptionFilter, 512, allowNull: true, SqlDbType.NVarChar);
			BindLong("@CenterTime", searchParameters.CenterTime.Ticks);
			BindLong("@TimeLimit", searchParameters.TimeLimit.Ticks);
			BindInt("@CountLimit", searchParameters.CountLimit);
			BindGuidType("@FromBookmarkId", searchParameters.FromBookmarkId, allowNullOrEmpty: true);
			return QueryBookmarkSearch(ExecuteReader());
		});
	}

	public MarkedDataDataContract[] QueryBookmarksSearchNoDeviceFilter(string[] roleIds, BookmarkSearchParametersNoDeviceFilter searchParameters)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("bookmarks_query_search_no_device_filter");
			BindIdTable("@RoleIds", roleIds);
			BindResultTable("@DeviceTypes", DatabaseDeviceTypeStrings.GetDeviceTypeStrings(searchParameters.DeviceTypes));
			BindResultTable("@Users", searchParameters.Users);
			BindString("@SearchStr", searchParameters.SearchString, 512, allowNull: true, SqlDbType.NVarChar);
			BindBoolean("@SearchHeader", searchParameters.SearchHeader);
			BindBoolean("@SearchDescription", searchParameters.SearchDescription);
			BindBoolean("@SearchReference", searchParameters.SearchReference);
			BindBoolean("@SearchUser", searchParameters.SearchUser);
			BindString("@Header", searchParameters.HeaderFilter, 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Description", searchParameters.DescriptionFilter, 512, allowNull: true, SqlDbType.NVarChar);
			BindLong("@CenterTime", searchParameters.CenterTime.Ticks);
			BindLong("@TimeLimit", searchParameters.TimeLimit.Ticks);
			BindInt("@CountLimit", searchParameters.CountLimit);
			BindGuidType("@FromBookmarkId", searchParameters.FromBookmarkId, allowNullOrEmpty: true);
			return QueryBookmarkSearch(ExecuteReader());
		});
	}

	private static MarkedDataDataContract[] QueryBookmarkSearch(DatabaseDataReader reader)
	{
		Dictionary<string, List<string>> dictionary = new Dictionary<string, List<string>>();
		Dictionary<string, List<StickerSize2>> dictionary2 = new Dictionary<string, List<StickerSize2>>();
		Dictionary<string, MarkedDataDataContract> dictionary3 = new Dictionary<string, MarkedDataDataContract>();
		while (reader.Read())
		{
			MarkedDataDataContract markedDataDataContract = BookmarkDataColumns.Bind(reader);
			dictionary3.Add(markedDataDataContract.Id, markedDataDataContract);
		}
		if (reader.NextResult())
		{
			while (reader.Read())
			{
				BookmarkedDevice bookmarkedDevice = BookmarkDeviceDataColumns.Bind(reader);
				StickerSize2 item = new StickerSize2(new Guid(bookmarkedDevice.DeviceId), bookmarkedDevice.Size);
				if (dictionary2.TryGetValue(bookmarkedDevice.MarkedDataId, out var value))
				{
					value.Add(item);
					dictionary2[bookmarkedDevice.MarkedDataId] = value;
					dictionary[bookmarkedDevice.MarkedDataId].Add(bookmarkedDevice.DeviceId);
				}
				else
				{
					dictionary2.Add(bookmarkedDevice.MarkedDataId, new List<StickerSize2> { item });
					dictionary.Add(bookmarkedDevice.MarkedDataId, new List<string> { bookmarkedDevice.DeviceId });
				}
			}
		}
		else if (dictionary3.Count > 0)
		{
			throw new DatabaseConfigMissingRowException();
		}
		if (reader.NextResult())
		{
			throw new DatabaseConfigExtraRowsException();
		}
		foreach (KeyValuePair<string, MarkedDataDataContract> item2 in dictionary3)
		{
			item2.Value.StickerSizes = dictionary2[item2.Key];
			item2.Value.DeviceIds = dictionary[item2.Key].ToArray();
		}
		return dictionary3.Values.ToArray();
	}

	public MarkedDataDataContract[] QueryMarkedDataSearch(string[] roleIds, MarkedDataSearchParams searchParameters)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("markeddata_query_search");
			BindIdTable("@RoleIds", roleIds);
			BindIdTable("@IDDevices", searchParameters.DeviceIds);
			BindResultTable("@Users", searchParameters.Users);
			BindString("@SearchText", searchParameters.SearchString, 512, allowNull: true, SqlDbType.NVarChar);
			BindBoolean("@SearchHeader", searchParameters.SearchHeader);
			BindBoolean("@SearchDescription", searchParameters.SearchDescription);
			BindBoolean("@SearchReference", searchParameters.SearchReference);
			BindBoolean("@SearchUser", searchParameters.SearchUser);
			BindString("@Header", searchParameters.HeaderFilter, 512, allowNull: true, SqlDbType.NVarChar);
			BindString("@Description", searchParameters.DescriptionFilter, 512, allowNull: true, SqlDbType.NVarChar);
			if (searchParameters.CreatedFromTime > new DateTime(1900, 1, 1))
			{
				BindDateTime("@CreatedFromTime", searchParameters.CreatedFromTime);
			}
			if (searchParameters.CreatedToTime > new DateTime(1900, 1, 1))
			{
				BindDateTime("@CreatedToTime", searchParameters.CreatedToTime);
			}
			BindLong("@MarkedDataFromTime", searchParameters.MarkedDataFromTime.Ticks);
			BindLong("@MarkedDataToTime", searchParameters.MarkedDataToTime.Ticks);
			BindLong("@TagTimeFromTime", searchParameters.TagTimeFromTime.Ticks);
			BindLong("@TagTimeToTime", searchParameters.TagTimeToTime.Ticks);
			BindLong("@RetentionExpireFromTime", searchParameters.RetentionExpireFromTime.Ticks);
			BindLong("@RetentionExpireToTime", searchParameters.RetentionExpireToTime.Ticks);
			BindLong("@PageIndex", searchParameters.PageIndex);
			BindLong("@PageSize", searchParameters.PageSize);
			BindString("@SortOrder", searchParameters.SortOrder, 50, allowNull: true, SqlDbType.NVarChar);
			BindBoolean("@SortAscending", searchParameters.SortAscending);
			BindInt("@MarkedDataType", (int)searchParameters.MarkedDataType);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MarkedDataDataContract> list = new List<MarkedDataDataContract>();
			while (databaseDataReader.Read())
			{
				list.Add(MarkedDataColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Guid[] CreateMarkedData(Guid id, string[] deviceIds, string reference, string userName, string header, string description, MarkedDataDefinitions.MarkedDataTypes markedDataType, DateTime startTime, DateTime tagTime, DateTime endTime, OptionType retentionType, int retentionUnits, DateTime retentionExpire, long retentionSize, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("markeddata_create");
			BindUniqueIdentifier("@IDMarkedData", id);
			BindIdTable("@IDDevices", deviceIds);
			BindNameType("@Reference", reference);
			BindNameType("@UserName", userName);
			BindLongNameType("@Header", header);
			BindDescription("@Description", description);
			BindInt("@MarkDataType", (int)markedDataType);
			BindLong("@StartTime", startTime.Ticks);
			BindLong("@TagTime", tagTime.Ticks);
			BindLong("@EndTime", endTime.Ticks);
			BindInt("@RetentionType", (int)retentionType);
			BindInt("@RetentionUnits", retentionUnits);
			BindLong("@RetentionExpire", retentionExpire.Ticks);
			BindLong("@ProtectionSize", retentionSize);
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

	private DatabaseParameter BindStickerSizeTable(string parameterName, IEnumerable<StickerSize2> stickerSizes)
	{
		stickerSizes = stickerSizes ?? Enumerable.Empty<StickerSize2>();
		Func<StickerSize2, SqlDataRecord> selector = (StickerSize2 stickers) => BindStickerSizeRow(stickers, _metadataStickerSize);
		return BindTable(parameterName, "tvp_StickerSizeTable", stickerSizes.Select(selector).ToList(), convertEmptyToNull: true);
	}

	private static SqlDataRecord BindStickerSizeRow(StickerSize2 value, SqlMetaData[] type)
	{
		SqlDataRecord sqlDataRecord = new SqlDataRecord(type);
		sqlDataRecord.SetGuid(0, value.DeviceId);
		sqlDataRecord.SetInt64(1, value.Size);
		return sqlDataRecord;
	}

	public Guid[] UpdateMarkedData(Guid markedDataId, string[] deviceIds, string reference, string header, string description, DateTime startTime, DateTime tagTime, DateTime endTime, OptionType retentionType, int retentionUnits, DateTime retentionExpire, long retentionSize, IEnumerable<StickerSize2> stickerSizes, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("markeddata_update");
			BindUniqueIdentifier("@IDMarkedData", markedDataId);
			BindIdTable("@IDDevices", deviceIds);
			BindNameType("@Reference", reference);
			BindLongNameType("@Header", header);
			BindDescription("@Description", description);
			BindLong("@StartTime", startTime.Ticks);
			BindLong("@TagTime", tagTime.Ticks);
			BindLong("@EndTime", endTime.Ticks);
			BindInt("@RetentionType", (int)retentionType);
			BindInt("@RetentionUnits", retentionUnits);
			BindLong("@RetentionExpire", retentionExpire.Ticks);
			BindLong("@ProtectionSize", retentionSize);
			BindStickerSizeTable("@StickerSizes", stickerSizes);
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

	public void UpdateMarkedDataSizes(Guid markedDataId, IEnumerable<StickerSize2> stickerSizes)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("markeddata_update_sizes");
			BindUniqueIdentifier("@IDMarkedData", markedDataId);
			BindStickerSizeTable("@StickerSizes", stickerSizes);
			ExecuteNonQuery();
		});
	}

	public Guid[] DeleteMarkedData(Guid markedDataId, Guid sessionId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("markeddata_delete");
			BindUniqueIdentifier("@IDMarkedData", markedDataId);
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

	public void DeleteBookmarksByDeviceIdBeforeTime(MediaStatusEntry mediaStatus)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("bookmarks_delete_by_device_id_before_time");
			BindGuidType("@IDDevice", mediaStatus.DeviceId.ToString());
			BindLong("@BeforeTime", mediaStatus.MediaRetentionTime.Ticks);
			ExecuteNonQuery();
		});
	}

	public MarkedDataDataContract[] QueryRecordingServerMarkedData(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("markeddata_query_by_recording_server");
			BindUniqueIdentifier("@recording_server_id", recordingServerId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MarkedDataDataContract> list = new List<MarkedDataDataContract>();
			while (databaseDataReader.Read())
			{
				list.Add(MarkedDataColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public int NextSequenceCounterValue(string counterName)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("NextSequenceCounterValue");
			BindString("@CounterName", counterName, 30, allowNull: false, SqlDbType.NVarChar);
			return (int)ExecuteScalar();
		});
	}

	public MarkedDataDataContract[] QueryExpiredMarkedData()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("markeddata_query_items_expired");
			BindLong("@RetentionExpireFromTime", 0L);
			BindLong("@RetentionExpireToTime", DateTime.UtcNow.Ticks);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MarkedDataDataContract> list = new List<MarkedDataDataContract>();
			while (databaseDataReader.Read())
			{
				list.Add(MarkedDataColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public bool RecorderHasLockedDeviceDataDistributions(string recorderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("recording_server_has_locked_device_data_distribution");
			BindGuid("@recorder_id", new Guid(recorderId));
			return (bool)ExecuteScalar();
		});
	}

	public string[] QueryOldSchemeRecordingServers()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_old_scheme_recorders");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDRecorder");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetString(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}
}
