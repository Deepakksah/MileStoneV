using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class DeviceTrackDataComponent : GeminiResourceComponent, IDeviceTrackDataComponent, IDisposable
{
	private static class DeviceTrackColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder IDTrack = new DatabaseColumnBinder("IDTrack");

		private static DatabaseColumnBinder DefaultPlayback = new DatabaseColumnBinder("DefaultPlayback");

		private static DatabaseColumnBinder MergeEdge = new DatabaseColumnBinder("MergeEdge");

		private static DatabaseColumnBinder IsPrimary = new DatabaseColumnBinder("IsPrimary");

		private static DatabaseColumnBinder IsEdge = new DatabaseColumnBinder("IsEdge");

		public static DeviceTrackData Bind(DatabaseDataReader reader)
		{
			return new DeviceTrackData
			{
				Id = new Guid(IDDevice.GetGuid(reader)),
				TrackId = IDTrack.GetGuid2(reader),
				DefaultPlayback = DefaultPlayback.GetBoolean(reader),
				MergeEdge = MergeEdge.GetBoolean(reader),
				IsPrimary = IsPrimary.GetBoolean(reader),
				IsEdge = IsEdge.GetBoolean(reader)
			};
		}
	}

	public Dictionary<Guid, IEnumerable<DeviceTrackData>> QueryDeviceTracksByDeviceIds(Guid[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_device_tracks_by_device_ids");
			BindIdTable("@ids", ids.Select((Guid i) => i.ToString()));
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<Guid, IEnumerable<DeviceTrackData>> dictionary = new Dictionary<Guid, IEnumerable<DeviceTrackData>>();
			while (databaseDataReader.Read())
			{
				DeviceTrackData deviceTrackData = DeviceTrackColumns.Bind(databaseDataReader);
				if (dictionary.TryGetValue(deviceTrackData.Id, out var value))
				{
					((List<DeviceTrackData>)value).Add(deviceTrackData);
				}
				else
				{
					List<DeviceTrackData> value2 = new List<DeviceTrackData> { deviceTrackData };
					dictionary.Add(deviceTrackData.Id, value2);
				}
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return dictionary;
		});
	}
}
