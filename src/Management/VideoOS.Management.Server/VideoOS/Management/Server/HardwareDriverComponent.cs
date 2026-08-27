using System;
using System.Collections.Generic;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class HardwareDriverComponent : GeminiResourceComponent, IHardwareDriverComponent, IDisposable
{
	private static class HardwareDriverColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDHardwareDriver");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("DriverName");

		private static DatabaseColumnBinder Number = new DatabaseColumnBinder("DriverNumber");

		private static DatabaseColumnBinder GroupName = new DatabaseColumnBinder("GroupName");

		private static DatabaseColumnBinder UseCount = new DatabaseColumnBinder("DriverCount");

		private static DatabaseColumnBinder DriverVersion = new DatabaseColumnBinder("DriverVersion");

		private static DatabaseColumnBinder XmlVersion = new DatabaseColumnBinder("XmlVersion");

		public static HardwareDriver Bind(DatabaseDataReader reader)
		{
			HardwareDriver obj = new HardwareDriver
			{
				Id = Id.GetGuid(reader),
				Name = Name.GetString(reader),
				Number = Number.GetInt32(reader),
				GroupName = GroupName.GetString(reader),
				UseCount = UseCount.GetInt32(reader)
			};
			string driverVersion = DriverVersion.GetString(reader);
			string xmlVersion = XmlVersion.GetString(reader);
			HardwareDriverVersion hardwareDriverVersion = new HardwareDriverVersion(driverVersion, xmlVersion);
			obj.DriverVersion = hardwareDriverVersion.DriverVersion;
			obj.DriverRevision = hardwareDriverVersion.Revision;
			obj.DriverType = hardwareDriverVersion.DriverType;
			return obj;
		}
	}

	public HardwareDriver[] QueryActiveHardwareDrivers(string recordingServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_active_hardware_drivers_by_recorder");
			BindGuidType("@IDRecorder", recordingServerId, allowNullOrEmpty: false);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HardwareDriver> list = new List<HardwareDriver>();
			while (databaseDataReader.Read())
			{
				list.Add(HardwareDriverColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public HardwareDriver QueryActiveHardwareDriver(string recordingServerId, string driverId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_active_hardware_driver_by_recorder_and_id");
			BindGuidType("@IDRecorder", recordingServerId, allowNullOrEmpty: false);
			BindGuidType("@IDHardwareDriver", driverId, allowNullOrEmpty: false);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new HardwareDriverNotFoundException();
			}
			HardwareDriver result = HardwareDriverColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public HardwareDriver[] QueryFailoverServerHardwareDrivers(string failoverServerId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_failover_server_hardware_drivers");
			BindGuidType("@IDFailoverServer", failoverServerId, allowNullOrEmpty: false);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HardwareDriver> list = new List<HardwareDriver>();
			while (databaseDataReader.Read())
			{
				list.Add(HardwareDriverColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public HardwareDriver[] QueryRecorderHardwareDrivers(string recordingServerId, bool usedOnly)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_recording_server_hardware_drivers");
			BindGuidType("@IDRecorder", recordingServerId, allowNullOrEmpty: false);
			BindBoolean("@UsedOnly", usedOnly);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HardwareDriver> list = new List<HardwareDriver>();
			while (databaseDataReader.Read())
			{
				list.Add(HardwareDriverColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}
}
