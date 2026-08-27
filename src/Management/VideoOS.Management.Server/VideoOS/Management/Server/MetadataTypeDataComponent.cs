using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class MetadataTypeDataComponent : GeminiResourceComponent, IMetadataTypeDataComponent, IDisposable
{
	private static class MetadataInfoColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder ItemIndex = new DatabaseColumnBinder("ItemIndex");

		private static DatabaseColumnBinder IDReference = new DatabaseColumnBinder("IDReference");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder IDDisplayReference = new DatabaseColumnBinder("IDDisplayReference");

		private static DatabaseColumnBinder DisplayName = new DatabaseColumnBinder("DisplayName");

		public static MetadataTypeData Bind(DatabaseDataReader reader)
		{
			return new MetadataTypeData
			{
				DeviceId = new Guid(IDDevice.GetGuid(reader)),
				ItemIndex = ItemIndex.GetInt32(reader),
				Id = IDReference.GetGuid2(reader),
				Name = Name.GetString(reader),
				DisplayId = IDDisplayReference.GetGuid2(reader),
				DisplayName = DisplayName.GetString(reader)
			};
		}
	}

	private static class RecorderColumns
	{
		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		public static RecorderData Bind(DatabaseDataReader reader)
		{
			return new RecorderData
			{
				Id = new Guid(IDRecorder.GetGuid(reader))
			};
		}
	}

	public IEnumerable<MetadataTypeData> QueryMetadataInfoForEnabledDevicesByRecorderAdmin(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadatainfo_for_devices_enabled_by_recorder");
			BindGuidType("@IDRecorder", id.ToString());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<MetadataTypeData> list = new List<MetadataTypeData>();
			while (databaseDataReader.Read())
			{
				list.Add(MetadataInfoColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public Dictionary<Guid, IEnumerable<MetadataTypeData>> QueryMetadataInfoEnabledDevicesByRoleId(string[] roleIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_metadatainfo_for_devices_enabled_by_roleids");
			BindIdTable("@RoleIds", roleIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<Guid, IEnumerable<MetadataTypeData>> dictionary = new Dictionary<Guid, IEnumerable<MetadataTypeData>>();
			while (databaseDataReader.Read())
			{
				if (dictionary.ContainsKey(RecorderColumns.Bind(databaseDataReader).Id))
				{
					((List<MetadataTypeData>)dictionary[RecorderColumns.Bind(databaseDataReader).Id]).Add(MetadataInfoColumns.Bind(databaseDataReader));
				}
				else
				{
					List<MetadataTypeData> value = new List<MetadataTypeData> { MetadataInfoColumns.Bind(databaseDataReader) };
					dictionary.Add(RecorderColumns.Bind(databaseDataReader).Id, value);
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
