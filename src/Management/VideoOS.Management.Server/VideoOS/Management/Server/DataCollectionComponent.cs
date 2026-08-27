using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;
using VideoOS.Utilities.Database;
using VideoOS.WebService.Common.DataCollectorService;

namespace VideoOS.Management.Server;

public sealed class DataCollectionComponent : GeminiResourceComponent, IDataCollectionComponent, IDisposable
{
	private static class PerformanceCounterConfigurationColumns
	{
		private static DatabaseColumnBinder CategoryName = new DatabaseColumnBinder("category_name");

		private static DatabaseColumnBinder CounterName = new DatabaseColumnBinder("counter_name");

		private static DatabaseColumnBinder InstanceName = new DatabaseColumnBinder("instance_name");

		private static DatabaseColumnBinder SampleFrequency = new DatabaseColumnBinder("sample_frequency");

		private static DatabaseColumnBinder CounterDirection = new DatabaseColumnBinder("counter_direction");

		private static DatabaseColumnBinder EnableHistory = new DatabaseColumnBinder("enable_history");

		private static DatabaseColumnBinder TriggerStatePerValue = new DatabaseColumnBinder("trigger_per_value");

		public static PerformanceCounterConfiguration Bind(DatabaseDataReader reader)
		{
			PerformanceCounterConfiguration performanceCounterConfiguration = new PerformanceCounterConfiguration();
			performanceCounterConfiguration.CounterInstance = new PerformanceCounterInstance();
			performanceCounterConfiguration.CounterInstance.CategoryName = CategoryName.GetString(reader);
			performanceCounterConfiguration.CounterInstance.CounterName = CounterName.GetString(reader);
			performanceCounterConfiguration.CounterInstance.InstanceName = InstanceName.GetString(reader);
			performanceCounterConfiguration.SampleFrequency = SampleFrequency.GetInt32(reader);
			performanceCounterConfiguration.CounterInstance.CounterDirection = CounterDirection.GetBoolean(reader);
			performanceCounterConfiguration.CounterInstance.EnableHistory = EnableHistory.GetBoolean(reader);
			performanceCounterConfiguration.CounterInstance.TriggerStatePerValue = TriggerStatePerValue.GetBoolean(reader);
			return performanceCounterConfiguration;
		}
	}

	private static class DataCollectorHistoryColumns
	{
		private static DatabaseColumnBinder Time = new DatabaseColumnBinder("time");

		private static DatabaseColumnBinder Value = new DatabaseColumnBinder("value");

		public static HistoricSample Bind(DatabaseDataReader reader)
		{
			return new HistoricSample
			{
				Time = Time.GetDateTime(reader),
				Value = (float)Value.GetDouble(reader)
			};
		}
	}

	private static class DataCollectorHostColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("pkid");

		private static DatabaseColumnBinder HostName = new DatabaseColumnBinder("host_name");

		private static DatabaseColumnBinder AppId = new DatabaseColumnBinder("application_id");

		public static DataCollectorHost Bind(DatabaseDataReader reader)
		{
			return new DataCollectorHost
			{
				DataCollectorId = Id.GetInt32(reader),
				HostName = HostName.GetString(reader),
				ApplicationId = AppId.GetGuid2(reader).ToString()
			};
		}
	}

	private static class ServiceApplicationColumns
	{
		private static DatabaseColumnBinder ApplicationName = new DatabaseColumnBinder("name");

		private static DatabaseColumnBinder UpgradeCode = new DatabaseColumnBinder("upgrade_code");

		private static DatabaseColumnBinder IsInstalled = new DatabaseColumnBinder("is_installed");

		public static ServiceApplication Bind(DatabaseDataReader reader)
		{
			return new ServiceApplication
			{
				ApplicationName = ApplicationName.GetString(reader),
				UpgradeCode = UpgradeCode.GetString(reader, allowNulls: true, string.Empty),
				IsInstalled = IsInstalled.GetBoolean(reader)
			};
		}
	}

	public DataCollectorHost[] GetDataCollectorHosts(int dataCollectorId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_datacollectors");
			BindInt("@DatacollectorId", dataCollectorId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<DataCollectorHost> list = new List<DataCollectorHost>();
			while (databaseDataReader.Read())
			{
				list.Add(DataCollectorHostColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public int RegisterDataCollectorNewApplicationId(string applicationId, string oldApplicationId, string hostName, int version)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("register_data_collector_new_application_id");
			BindUniqueIdentifier("@application_id", new Guid(applicationId));
			BindUniqueIdentifier("@old_application_id", new Guid(oldApplicationId));
			BindString("@host_name", hostName, 200, allowNull: false, SqlDbType.NVarChar);
			BindInt("@version", version);
			return (int)ExecuteScalar();
		});
	}

	public int RegisterDataCollector(string applicationId, string hostName, int version)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("register_data_collector");
			BindUniqueIdentifier("@application_id", new Guid(applicationId));
			BindString("@host_name", hostName, 200, allowNull: false, SqlDbType.NVarChar);
			BindInt("@version", version);
			return (int)ExecuteScalar();
		});
	}

	public void MarkPerformanceCounterDataAsObsolete(string[] instances)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("mark_as_obsolete_perf_counter_data");
			BindResultTable("@Instances", instances);
			ExecuteScalar();
		});
	}

	public DataCollectorConfiguration GetDataCollectorConfiguration(int dataCollectorId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("get_data_collector_configuration");
			BindInt("@data_collector_id", dataCollectorId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			DataCollectorConfiguration dataCollectorConfiguration = new DataCollectorConfiguration();
			List<PerformanceCounterConfiguration> list = new List<PerformanceCounterConfiguration>();
			while (databaseDataReader.Read())
			{
				list.Add(PerformanceCounterConfigurationColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			dataCollectorConfiguration.ConfiguredCounters = list.ToArray();
			return dataCollectorConfiguration;
		});
	}

	public HistoricSample[] QueryHistoricSamples(HistoricSampleRequestData data)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_datacollector_history");
			BindInt("@data_collector_id", data.DataCollectorId);
			BindDateTime("@utc_start_time", data.FromTime);
			BindDateTime("@utc_end_time", DateTime.UtcNow);
			BindInt("@max_number_of_values", data.MaxSampleCount);
			BindString("@counter_path", data.CounterPath, 200, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<HistoricSample> list = new List<HistoricSample>();
			while (databaseDataReader.Read())
			{
				list.Add(DataCollectorHistoryColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void AddPerformanceCounterData(PerformanceCounterData counterData)
	{
		if (counterData == null)
		{
			throw new ArgumentNullException("counterData");
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("process_performance_counter_data");
			BindInt("@data_collector_id", counterData.DataCollectorId);
			if (counterData == null || counterData.Values == null || counterData.Values.Length == 0)
			{
				BindNullValue("@counter_values", SqlDbType.Xml);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder))
				{
					xmlWriter.WriteStartElement("cvs");
					for (int i = 0; i < counterData.Values.Length; i++)
					{
						xmlWriter.WriteStartElement("cv");
						xmlWriter.WriteAttributeString("c1", counterData.Values[i].CategoryName);
						xmlWriter.WriteAttributeString("c2", counterData.Values[i].CounterName);
						xmlWriter.WriteAttributeString("in", counterData.Values[i].InstanceName);
						xmlWriter.WriteAttributeString("ct", counterData.Values[i].CollectionTime.ToString("yyyy-MM-dd HH:mm:ss.fff", CultureInfo.InvariantCulture));
						xmlWriter.WriteAttributeString("rw", counterData.Values[i].RawValue.ToString(CultureInfo.InvariantCulture));
						xmlWriter.WriteAttributeString("cv", counterData.Values[i].CalculatedValue.ToString(CultureInfo.InvariantCulture));
						xmlWriter.WriteEndElement();
					}
					xmlWriter.WriteEndElement();
				}
				BindXml("@counter_values", stringBuilder.ToString());
			}
			ExecuteNonQuery();
		});
	}

	public ServiceApplication[] GetServiceApplicationList(int dataCollectorId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("get_service_applications");
			BindInt("@data_collector_id", dataCollectorId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<ServiceApplication> list = new List<ServiceApplication>();
			while (databaseDataReader.Read())
			{
				list.Add(ServiceApplicationColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void RegisterServiceApplications(int dataCollectorId, ServiceApplication[] serviceApplications)
	{
		if (serviceApplications == null)
		{
			throw new ArgumentNullException("serviceApplications");
		}
		StringBuilder sb = new StringBuilder();
		using (XmlWriter xmlWriter = XmlWriter.Create(sb))
		{
			xmlWriter.WriteStartElement("ServiceApplications");
			for (int i = 0; i < serviceApplications.Length; i++)
			{
				if (serviceApplications[i].IsInstalled)
				{
					xmlWriter.WriteStartElement("ServiceApplication");
					xmlWriter.WriteAttributeString("Name", serviceApplications[i].ApplicationName);
					xmlWriter.WriteEndElement();
				}
			}
			xmlWriter.WriteEndElement();
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_service_applications");
			BindInt("@data_collector_id", dataCollectorId);
			BindXml("@service_applications", sb.ToString());
			ExecuteNonQuery();
		});
	}

	public void UpdateMachineProperties(int dataCollectorId, MachineProperty[] machineProperties)
	{
		if (machineProperties == null)
		{
			throw new ArgumentNullException("machineProperties");
		}
		StringBuilder sb = new StringBuilder();
		using (XmlWriter xmlWriter = XmlWriter.Create(sb))
		{
			xmlWriter.WriteStartElement("MachineProperties");
			for (int i = 0; i < machineProperties.Length; i++)
			{
				xmlWriter.WriteStartElement("MachineProperty");
				xmlWriter.WriteAttributeString("Name", machineProperties[i].Name);
				xmlWriter.WriteAttributeString("Value", machineProperties[i].Value);
				xmlWriter.WriteEndElement();
			}
			xmlWriter.WriteEndElement();
		}
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_machine_properties");
			BindInt("@data_collector_id", dataCollectorId);
			BindXml("@machine_properties", sb.ToString());
			ExecuteNonQuery();
		});
	}
}
