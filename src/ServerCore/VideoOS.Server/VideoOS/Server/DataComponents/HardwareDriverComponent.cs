using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents;

public sealed class HardwareDriverComponent : VmoResourceComponent, IHardwareDriverComponent, IDisposable
{
	private static class HardwareDriverColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDHardwareDriver");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("DriverName");

		private static DatabaseColumnBinder Number = new DatabaseColumnBinder("DriverNumber");

		private static DatabaseColumnBinder GroupName = new DatabaseColumnBinder("GroupName");

		private static DatabaseColumnBinder UseCount = new DatabaseColumnBinder("DriverCount");

		private static DatabaseColumnBinder DriverVersion = new DatabaseColumnBinder("DriverVersion");

		public static HardwareDriverInfo Bind(DatabaseDataReader reader)
		{
			HardwareDriverInfo hardwareDriverInfo = new HardwareDriverInfo();
			hardwareDriverInfo.Id = Id.GetStringAsGuid(reader);
			hardwareDriverInfo.Name = Name.GetString(reader);
			hardwareDriverInfo.Number = Number.GetInt32(reader);
			hardwareDriverInfo.GroupName = GroupName.GetString(reader);
			hardwareDriverInfo.UseCount = UseCount.GetInt32(reader);
			hardwareDriverInfo.DriverVersion = DriverVersion.GetString(reader);
			return hardwareDriverInfo;
		}
	}

	public HardwareDriverInfo[] QueryActiveMIPDrivers()
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				List<HardwareDriverInfo> list = new List<HardwareDriverInfo>();
				PrepareStoredProcedure("query_active_mip_drivers");
				DatabaseDataReader databaseDataReader = ExecuteReader();
				while (databaseDataReader.Read())
				{
					list.Add(HardwareDriverColumns.Bind(databaseDataReader));
				}
				return list.ToArray();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}
