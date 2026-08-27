using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Utilities.Database;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public sealed class SystemMonitorStateSourceComponent : GeminiResourceComponent
{
	private static class ServersColumns
	{
		private static DatabaseColumnBinder IDServer = new DatabaseColumnBinder("IDServer");

		private static DatabaseColumnBinder ServerName = new DatabaseColumnBinder("ServerName");

		private static DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("DataCollectorId");

		private static DatabaseColumnBinder ServiceName = new DatabaseColumnBinder("ServiceName");

		public static SystemMonitorStateSource Bind(DatabaseDataReader reader)
		{
			return new SystemMonitorStateSource
			{
				SourceId = IDServer.GetGuid(reader),
				DataCollectorId = DataCollectorId.GetInt32(reader, allowNulls: true, 0),
				ServiceName = ServiceName.GetString(reader, allowNulls: true),
				Name = ServerName.GetString(reader, allowNulls: true)
			};
		}
	}

	private static class ServerStateColumns
	{
		private static readonly DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("DataCollectorId");

		private static readonly DatabaseColumnBinder EventType = new DatabaseColumnBinder("EventType");

		private static readonly DatabaseColumnBinder CounterState = new DatabaseColumnBinder("CounterState");

		private static readonly DatabaseColumnBinder InstanceId = new DatabaseColumnBinder("InstanceId");

		private static readonly DatabaseColumnBinder TriggerValue = new DatabaseColumnBinder("TriggerValue");

		public static StateData Bind(DatabaseDataReader reader)
		{
			return new StateData
			{
				DataCollectorId = DataCollectorId.GetInt32(reader),
				EventType = EventType.GetInt32(reader),
				State = (CounterState)CounterState.GetInt32(reader),
				InstanceId = Guid.Parse(InstanceId.GetGuid(reader, allowNulls: true, Guid.Empty.ToString())),
				TriggerValue = TriggerValue.GetDouble(reader)
			};
		}
	}

	private static class CameraStateColumns
	{
		private static readonly DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("DataCollectorId");

		private static readonly DatabaseColumnBinder CameraId = new DatabaseColumnBinder("CameraId");

		private static readonly DatabaseColumnBinder EventType = new DatabaseColumnBinder("EventType");

		private static readonly DatabaseColumnBinder CounterState = new DatabaseColumnBinder("CounterState");

		private static readonly DatabaseColumnBinder TriggerValueColumn = new DatabaseColumnBinder("TriggerValue");

		public static StateData Bind(DatabaseDataReader reader)
		{
			return new StateData
			{
				DataCollectorId = DataCollectorId.GetInt32(reader),
				InstanceId = Guid.Parse(CameraId.GetGuid(reader)),
				EventType = EventType.GetInt32(reader),
				State = (CounterState)CounterState.GetInt32(reader),
				TriggerValue = TriggerValueColumn.GetDouble(reader)
			};
		}
	}

	public class StateData
	{
		public int DataCollectorId { get; set; }

		public Guid InstanceId { get; set; }

		public int EventType { get; set; }

		public CounterState State { get; set; }

		public double TriggerValue { get; set; }
	}

	public SystemMonitorStateSource[] QuerySystemMonitorSourcesByTypes(string[] sourceTypes)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_servers_by_service_types");
			BindIdTable("@ServiceTypes", sourceTypes);
			BindIdTable("@Ids", Enumerable.Empty<string>());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<SystemMonitorStateSource> list = new List<SystemMonitorStateSource>();
			while (databaseDataReader.Read())
			{
				list.Add(ServersColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public SystemMonitorStateSource[] QuerySystemMonitorSourcesByIds(string[] sourceIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_servers_by_service_types");
			BindIdTable("@ServiceTypes", Enumerable.Empty<string>());
			BindIdTable("@Ids", sourceIds);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<SystemMonitorStateSource> list = new List<SystemMonitorStateSource>();
			while (databaseDataReader.Read())
			{
				list.Add(ServersColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public StateData[] QuerySystemMonitorServerStates(int[] dataCollectorIds, int[] eventTypes)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_server_monitor_states_by_datacollectors");
			BindIdTable("@DataCollectors", dataCollectorIds.Select((int d) => d.ToString(CultureInfo.InvariantCulture)).ToArray());
			BindIdTable("@EventTypes", eventTypes.Select((int e) => e.ToString(CultureInfo.InvariantCulture)).ToArray());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<StateData> list = new List<StateData>();
			while (databaseDataReader.Read())
			{
				list.Add(ServerStateColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public StateData[] QuerySystemMonitorCameraStates(int[] eventTypes, Guid[] cameraIds)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_camera_monitor_states_by_ids");
			BindIdTable("@EventTypes", eventTypes.Select((int e) => e.ToString(CultureInfo.InvariantCulture)).ToArray());
			BindIdTable("@CameraIds", cameraIds.Select((Guid c) => c.ToString()).ToArray());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<StateData> list = new List<StateData>();
			while (databaseDataReader.Read())
			{
				list.Add(CameraStateColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}
}
