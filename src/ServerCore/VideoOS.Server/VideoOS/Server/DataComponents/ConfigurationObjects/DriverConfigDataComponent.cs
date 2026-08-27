using System;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents.ConfigurationObjects;

public sealed class DriverConfigDataComponent : VmoResourceComponent, IDriverConfigDataComponent, IDisposable
{
	private static class DriverColumns
	{
		private static DatabaseColumnBinder IDHardwareDriver = new DatabaseColumnBinder("IDHardwareDriver");

		private static DatabaseColumnBinder IDRecordingServerService = new DatabaseColumnBinder("IDRecordingServerService");

		private static DatabaseColumnBinder DriverName = new DatabaseColumnBinder("DriverName");

		private static DatabaseColumnBinder DriverNumber = new DatabaseColumnBinder("DriverNumber");

		private static DatabaseColumnBinder DriverVersion = new DatabaseColumnBinder("DriverVersion");

		private static DatabaseColumnBinder XmlVersion = new DatabaseColumnBinder("XmlVersion");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static DriverConfigData Bind(DatabaseDataReader reader)
		{
			DriverConfigData driverConfigData = new DriverConfigData();
			driverConfigData.Id = new Guid(IDHardwareDriver.GetString(reader));
			driverConfigData.RecordingServerServiceId = new Guid(IDRecordingServerService.GetString(reader));
			driverConfigData.Name = DriverName.GetString(reader);
			driverConfigData.Number = DriverNumber.GetInt32(reader);
			driverConfigData.Version = DriverVersion.GetString(reader);
			driverConfigData.XmlVersion = XmlVersion.GetString(reader);
			driverConfigData.LastModified = LastModified.GetDateTime(reader);
			return driverConfigData;
		}
	}

	public DriverConfigDataComponent()
	{
	}

	public DriverConfigData QueryByRecordingServerServiceAndNumber(Guid recordingServerServiceId, int driverNumber)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_driver_by_service_and_number");
				BindGuidType("@recording_server_service_id", recordingServerServiceId);
				BindInt("@driver_number", driverNumber);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				return (!databaseDataReader.Read()) ? null : DriverColumns.Bind(databaseDataReader);
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}
