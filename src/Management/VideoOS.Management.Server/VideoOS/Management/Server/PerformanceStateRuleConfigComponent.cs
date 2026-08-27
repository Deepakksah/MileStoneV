using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VideoOS.Utilities.Database;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public sealed class PerformanceStateRuleConfigComponent : GeminiResourceComponent, IPerformanceStateRuleConfigComponent, IDisposable
{
	public class IsStateChangedBinder : ObjectBinder<bool>
	{
		private DatabaseColumnBinder isStateChangedColumn = new DatabaseColumnBinder("IsStateChanged");

		protected override bool Bind()
		{
			return isStateChangedColumn.GetBoolean(base.Reader, allowNulls: false, defaultValue: false);
		}
	}

	public class RuleSourcesBinder : ObjectBinder<SystemMonitorManager.EventSource>
	{
		private static DatabaseColumnBinder IDServerColumn = new DatabaseColumnBinder("IDServer");

		private static DatabaseColumnBinder DataCollectorIdColumn = new DatabaseColumnBinder("DataCollectorId");

		private static DatabaseColumnBinder ServerTypeColumn = new DatabaseColumnBinder("ServerType");

		private static DatabaseColumnBinder ServerNameColumn = new DatabaseColumnBinder("ServerName");

		protected override SystemMonitorManager.EventSource Bind()
		{
			return new SystemMonitorManager.EventSource(IDServerColumn.GetString(base.Reader), ServerTypeColumn.GetGuid(base.Reader), DataCollectorIdColumn.GetInt32(base.Reader), ServerNameColumn.GetString(base.Reader));
		}
	}

	private static class CounterStateChangeDataColumns
	{
		private static DatabaseColumnBinder CounterPath = new DatabaseColumnBinder("CounterPath");

		private static DatabaseColumnBinder CounterRuleId = new DatabaseColumnBinder("CounterRuleId");

		private static DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("DataCollectorId");

		private static DatabaseColumnBinder State = new DatabaseColumnBinder("State");

		private static DatabaseColumnBinder TriggerValue = new DatabaseColumnBinder("TriggerValue");

		public static void Bind(ref List<CounterStateChangeData> states, DatabaseDataReader reader)
		{
			states.Add(new CounterStateChangeData
			{
				CounterPath = CounterPath.GetString(reader),
				CounterRuleId = CounterRuleId.GetGuid(reader),
				DataCollectorId = DataCollectorId.GetInt32(reader),
				State = State.GetInt32(reader),
				TriggerValue = TriggerValue.GetDouble(reader)
			});
		}
	}

	private static class ServersColumns
	{
		private static DatabaseColumnBinder IDDevice = new DatabaseColumnBinder("IDDevice");

		private static DatabaseColumnBinder IDServer = new DatabaseColumnBinder("IDServer");

		private static DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("DataCollectorId");

		private static DatabaseColumnBinder ServerType = new DatabaseColumnBinder("ServerType");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("ServerName");

		public static void Bind(ref Dictionary<int, List<SystemMonitorManager.EventSource>> servers, DatabaseDataReader reader)
		{
			string guid = IDServer.GetGuid(reader);
			string guid2 = ServerType.GetGuid(reader);
			int @int = DataCollectorId.GetInt32(reader);
			string name = Name.GetString(reader, allowNulls: true, string.Empty);
			if (servers.ContainsKey(@int))
			{
				servers[@int].Add(new SystemMonitorManager.EventSource(guid, guid2, @int, name));
				return;
			}
			servers.Add(@int, new List<SystemMonitorManager.EventSource>
			{
				new SystemMonitorManager.EventSource(guid, guid2, @int, name)
			});
		}

		public static void BindDevices(ref Dictionary<string, int> devices, DatabaseDataReader reader)
		{
			string guid = IDDevice.GetGuid(reader);
			int @int = DataCollectorId.GetInt32(reader, allowNulls: true, 0);
			if (@int != 0)
			{
				devices.Add(guid, @int);
			}
		}
	}

	private static class DataCollectorMachinesColumns
	{
		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("HostName");

		private static DatabaseColumnBinder CollectorId = new DatabaseColumnBinder("CollectorId");

		private static DatabaseColumnBinder ChildName = new DatabaseColumnBinder("ServiceName");

		private static DatabaseColumnBinder ChildType = new DatabaseColumnBinder("ServiceType");

		public static void Bind(ref List<DataCollectorParent> parents, DatabaseDataReader reader)
		{
			string parentName = HostName.GetString(reader);
			string parentId = HostName.GetString(reader);
			int @int = CollectorId.GetInt32(reader);
			string text = ChildName.GetString(reader, allowNulls: true, "");
			string serviceType = ChildType.GetString(reader, allowNulls: true, "");
			if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(text))
			{
				DataCollectorParent dataCollectorParent = parents.FirstOrDefault((DataCollectorParent i) => i.Id == parentId);
				if (dataCollectorParent != null)
				{
					dataCollectorParent.Children.Add(new DataCollectorChild
					{
						Id = Guid.NewGuid().ToString(),
						ServiceName = text,
						ServiceType = serviceType
					});
					return;
				}
				DataCollectorParent dataCollectorParent2 = new DataCollectorParent();
				dataCollectorParent2.ParentName = parentName;
				dataCollectorParent2.Id = parentId;
				dataCollectorParent2.DataCollectorId = @int;
				dataCollectorParent2.Children = new List<DataCollectorChild>();
				dataCollectorParent2.Children.Add(new DataCollectorChild
				{
					Id = Guid.NewGuid().ToString(),
					ServiceName = text,
					ServiceType = serviceType
				});
				parents.Add(dataCollectorParent2);
			}
		}
	}

	private static class PerformanceStorageInstanceColumns
	{
		private static DatabaseColumnBinder CounterId = new DatabaseColumnBinder("CounterId");

		private static DatabaseColumnBinder StorageId = new DatabaseColumnBinder("StorageId");

		private static DatabaseColumnBinder DatacollectorId = new DatabaseColumnBinder("DatacollectorId");

		private static DatabaseColumnBinder Path = new DatabaseColumnBinder("Path");

		private static DatabaseColumnBinder StorageSizeMb = new DatabaseColumnBinder("StorageSizeMb");

		public static RetentionTimeRuleConfig Bind(DatabaseDataReader reader)
		{
			return new RetentionTimeRuleConfig
			{
				CounterId = CounterId.GetInt32(reader),
				StorageId = Guid.Parse(StorageId.GetGuid(reader)),
				DatacollectorId = DatacollectorId.GetInt32(reader),
				Path = Path.GetString(reader),
				StorageSizeMb = StorageSizeMb.GetInt64(reader)
			};
		}
	}

	private static class PerformanceStateRuleConfigColumns
	{
		private static DatabaseColumnBinder RuleCounterId = new DatabaseColumnBinder("counter_state_rule_id");

		private static DatabaseColumnBinder CriticalThreshold = new DatabaseColumnBinder("critical_threshold");

		private static DatabaseColumnBinder WarningThreshold = new DatabaseColumnBinder("warning_threshold");

		private static DatabaseColumnBinder TimeWindow = new DatabaseColumnBinder("time_window");

		private static DatabaseColumnBinder RuleType = new DatabaseColumnBinder("rule_type");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("rule_name");

		private static DatabaseColumnBinder Disabled = new DatabaseColumnBinder("disabled");

		private static DatabaseColumnBinder Unit = new DatabaseColumnBinder("unit");

		private static DatabaseColumnBinder Direction = new DatabaseColumnBinder("direction");

		private static DatabaseColumnBinder CategoryName = new DatabaseColumnBinder("category_name");

		private static DatabaseColumnBinder CounterName = new DatabaseColumnBinder("counter_name");

		private static DatabaseColumnBinder CriticalValue = new DatabaseColumnBinder("CriticalValue");

		private static DatabaseColumnBinder WarningValue = new DatabaseColumnBinder("WarningValue");

		public static PerformanceStateRuleConfig Bind(DatabaseDataReader reader)
		{
			return new PerformanceStateRuleConfig
			{
				CounterRuleId = RuleCounterId.GetGuid2(reader).ToString(),
				CriticalThreshold = CriticalThreshold.GetDouble(reader),
				WarningThreshold = WarningThreshold.GetDouble(reader),
				TimeWindow = TimeWindow.GetInt32(reader),
				RuleName = Name.GetString(reader),
				RuleType = RuleType.GetInt32(reader),
				Disabled = Disabled.GetBoolean(reader),
				Direction = Direction.GetBoolean(reader),
				Unit = Unit.GetInt64(reader, allowNulls: true, 100L),
				CategoryName = CategoryName.GetString(reader, allowNulls: true, string.Empty),
				CounterName = CounterName.GetString(reader, allowNulls: true, string.Empty)
			};
		}

		public static void BindInitValues(DatabaseDataReader reader, out double criticalValue, out double warningValue)
		{
			criticalValue = CriticalValue.GetDouble(reader);
			warningValue = WarningValue.GetDouble(reader);
		}
	}

	public PerformanceStateRuleConfig[] QueryCountersStateRulesDataByIds(int dataCollector, string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_perfcounter_state_rule_items");
			BindIdTable("@Ids", ids);
			BindInt("@DataCollectorId", dataCollector);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<PerformanceStateRuleConfig> list = new List<PerformanceStateRuleConfig>();
			while (databaseDataReader.Read())
			{
				list.Add(PerformanceStateRuleConfigColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public string[] QueryCountersStateRuleIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_counters_state_rule_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IdConfig");
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

	public RetentionTimeRuleConfig[] QueryStorageCounters()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_performanceCounter_storage_instances");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RetentionTimeRuleConfig> list = new List<RetentionTimeRuleConfig>();
			while (databaseDataReader.Read())
			{
				list.Add(PerformanceStorageInstanceColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public RetentionTimeHistoricalData[] QueryHistoricalStorageData(RetentionTimeQueryData data)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_performanceCounter_storage_data");
			BindInt("@instance_id", data.CounterInstanceId);
			BindDateTime("@time", data.StartDate);
			BindInt("@days", data.Days);
			BindInt("@samples", data.SampleCount);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RetentionTimeHistoricalData> list = new List<RetentionTimeHistoricalData>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("calculated_value");
			DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("collection_time");
			while (databaseDataReader.Read())
			{
				list.Add(new RetentionTimeHistoricalData
				{
					Value = databaseColumnBinder.GetDouble(databaseDataReader, allowNulls: true, 0.0),
					Time = databaseColumnBinder2.GetDateTime(databaseDataReader)
				});
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void CreateCountersStateRulesData(PerformanceStateRuleConfig config, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_perfcounter_state_rule");
			BindUniqueIdentifier("@CounterStateRuleId", config.CounterRuleId);
			BindFloat("@CriticalThreshold", config.CriticalThreshold);
			BindFloat("@WarningThreshold", config.WarningThreshold);
			BindInt("@TimeWindow", config.TimeWindow);
			BindString("@Name", config.RuleName, 50, allowNull: true, SqlDbType.Text);
			BindInt("@RuleType", config.RuleType);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void UpdateCountersStateRulesData(PerformanceStateRuleConfig config, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_perfcounter_state_rule");
			BindUniqueIdentifier("@CounterStateRuleId", config.CounterRuleId);
			BindFloat("@CriticalThreshold", config.CriticalThreshold);
			BindFloat("@WarningThreshold", config.WarningThreshold);
			BindInt("@TimeWindow", config.TimeWindow);
			BindString("@Name", config.RuleName, 50, allowNull: true, SqlDbType.Text);
			BindBoolean("@Disabled", config.Disabled);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeleteCountersStateRulesData(string counterRuleId, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_perfcounter_state_rule");
			BindUniqueIdentifier("@CounterStateRuleId", counterRuleId);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public PerformanceStateRuleConfig QueryCountersStateRulesDataById(string counterRuleId)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("query_perfcounter_state_rule_item");
				BindUniqueIdentifier("@CounterStateRuleId", counterRuleId);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				if (databaseDataReader.Read())
				{
					return PerformanceStateRuleConfigColumns.Bind(databaseDataReader);
				}
				if (databaseDataReader.NextResult())
				{
					throw new SecurityDatabaseConfigExtraRowsException();
				}
				return (PerformanceStateRuleConfig)null;
			});
		}
		catch (DatabaseException)
		{
		}
		return null;
	}

	public List<DataCollectorParent> GetDataCollectorMachines()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_dc_machines");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DataCollectorParent> parents = new List<DataCollectorParent>();
			while (databaseDataReader.Read())
			{
				DataCollectorMachinesColumns.Bind(ref parents, databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return parents;
		});
	}

	public CounterStateChangeData[] SetStatesByRule(string counterRuleId, int newState)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("persist_states_by_perfrule");
			BindInt("@NewState", newState);
			BindUniqueIdentifier("@CounterRuleId", counterRuleId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<CounterStateChangeData> states = new List<CounterStateChangeData>();
			while (databaseDataReader.Read())
			{
				CounterStateChangeDataColumns.Bind(ref states, databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return states.ToArray();
		});
	}

	public Dictionary<string, int> QueryDataCollectorIdsByDeviceIds(string[] list)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_datacollector_by_device_ids");
			BindIdTable("@Ids", list);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<string, int> devices = new Dictionary<string, int>();
			while (databaseDataReader.Read())
			{
				ServersColumns.BindDevices(ref devices, databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return devices;
		});
	}

	public Dictionary<int, List<SystemMonitorManager.EventSource>> QueryAllMachinesByServerType(IList<Guid> list)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_servers_by_service_types");
			BindIdTable("@ServiceTypes", list.Select((Guid i) => i.ToString()).ToArray());
			BindIdTable("@Ids", Enumerable.Empty<string>());
			DatabaseDataReader databaseDataReader = ExecuteReader();
			Dictionary<int, List<SystemMonitorManager.EventSource>> servers = new Dictionary<int, List<SystemMonitorManager.EventSource>>();
			while (databaseDataReader.Read())
			{
				ServersColumns.Bind(ref servers, databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return servers;
		});
	}

	public ResultCollection RegisterStateChange(CounterStateChangeData stateChangeData, bool lookupServerSourcesRequired)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("register_state_change");
			BindString("@RuleId", stateChangeData.CounterRuleId, 100, allowNull: false, SqlDbType.NVarChar);
			BindString("@CounterPath", stateChangeData.CounterPath, 364, allowNull: false, SqlDbType.NVarChar);
			BindInt("@DataCollectorId", stateChangeData.DataCollectorId);
			BindInt("@State", stateChangeData.State);
			BindFloat("@TriggerValue", stateChangeData.TriggerValue);
			BindBoolean("@LookupSources", lookupServerSourcesRequired);
			ResultCollection resultCollection = new ResultCollection(ExecuteReader(), base.StoredProcedureName, RequestContext);
			resultCollection.AddBinder(new IsStateChangedBinder());
			if (lookupServerSourcesRequired)
			{
				resultCollection.AddBinder(new RuleSourcesBinder());
			}
			return resultCollection;
		});
	}

	public void DeleteState(CounterStateChangeData stateChangeData)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_state");
			BindString("@RuleId", stateChangeData.CounterRuleId, 100, allowNull: false, SqlDbType.NVarChar);
			BindString("@CounterPath", stateChangeData.CounterPath, 364, allowNull: false, SqlDbType.NVarChar);
			BindInt("@DataCollectorId", stateChangeData.DataCollectorId);
			ExecuteNonQuery();
		});
	}

	public void DeleteStatesFromDatacollector(int datacollectorId, string[] activePaths)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_states_by_datacollector");
			BindInt("@DataCollectorId", datacollectorId);
			BindResultTable("@paths", activePaths);
			ExecuteNonQuery();
		});
	}

	public void GetPerformanceCounterDefaultValues(string id, out double criticalValue, out double warningValue)
	{
		(criticalValue, warningValue) = base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_perfcounter_initvalues");
			BindUniqueIdentifier("@CounterStateRuleId", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			double criticalValue2 = 0.0;
			double warningValue2 = 0.0;
			while (databaseDataReader.Read())
			{
				PerformanceStateRuleConfigColumns.BindInitValues(databaseDataReader, out criticalValue2, out warningValue2);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return (criticalValueResult: criticalValue2, warningValueResult: warningValue2);
		});
	}

	public List<int> QueryActiveRecorders()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_active_recorders_datacollector_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<int> list = new List<int>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Id");
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetInt32(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToList();
		});
	}
}
