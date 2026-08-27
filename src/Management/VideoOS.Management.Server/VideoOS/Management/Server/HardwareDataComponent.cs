using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Definition;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class HardwareDataComponent : GeminiResourceComponent, IHardwareDataComponent, IDisposable
{
	private static class HardwareColumns
	{
		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDHardware");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder IDRecorder = new DatabaseColumnBinder("IDRecorder");

		private static DatabaseColumnBinder SubType = new DatabaseColumnBinder("SubType");

		private static DatabaseColumnBinder Uri = new DatabaseColumnBinder("URI");

		public static HardwareData Bind(DatabaseDataReader reader)
		{
			return new HardwareData
			{
				LastModified = LastModified.GetDateTime(reader),
				Id = new Guid(Id.GetGuid(reader)),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader, allowNulls: true),
				RecorderId = new Guid(IDRecorder.GetGuid(reader)),
				Subtype = (Subtypes)SubType.GetInt32(reader),
				Uri = Uri.GetString(reader)
			};
		}
	}

	private static class HardwareModifiedColumns
	{
		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDHardware");

		public static HardwareModifiedData Bind(DatabaseDataReader reader)
		{
			return new HardwareModifiedData
			{
				LastModified = LastModified.GetDateTime(reader),
				Id = new Guid(Id.GetGuid(reader))
			};
		}
	}

	public IEnumerable<HardwareData> QueryHardwareByIds(Guid[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_by_ids2");
			BindIdTable("@ids", ids?.Select((Guid f) => f.ToString()).ToArray());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HardwareData> list = new List<HardwareData>();
			while (databaseDataReader.Read())
			{
				list.Add(HardwareColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public IEnumerable<HardwareModifiedData> QueryHardwareChangedAfter(DateTime changesFrom)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_hardware_ids_changed_after");
			BindDateTime("@changedAfter", changesFrom);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HardwareModifiedData> list = new List<HardwareModifiedData>();
			while (databaseDataReader.Read())
			{
				list.Add(HardwareModifiedColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list;
		});
	}
}
