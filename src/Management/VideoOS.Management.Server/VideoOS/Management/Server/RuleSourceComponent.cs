using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RuleSourceComponent : GeminiResourceComponent, IRuleSourceComponent, IDisposable
{
	private static class RuleSourceColumns
	{
		private static DatabaseColumnBinder SourceId = new DatabaseColumnBinder("source_id");

		private static DatabaseColumnBinder CounterId = new DatabaseColumnBinder("counter_id");

		private static DatabaseColumnBinder Path = new DatabaseColumnBinder("path");

		private static DatabaseColumnBinder CounterName = new DatabaseColumnBinder("counter_name");

		private static DatabaseColumnBinder CategoryName = new DatabaseColumnBinder("category_name");

		private static DatabaseColumnBinder DataCollectorId = new DatabaseColumnBinder("data_collector_id");

		private static DatabaseColumnBinder CriticalThreshold = new DatabaseColumnBinder("critical_threshold");

		private static DatabaseColumnBinder WarningThreshold = new DatabaseColumnBinder("warning_threshold");

		private static DatabaseColumnBinder TimeWindow = new DatabaseColumnBinder("time_window");

		private static DatabaseColumnBinder CounterRuleId = new DatabaseColumnBinder("counter_state_rule_id");

		private static DatabaseColumnBinder RuleType = new DatabaseColumnBinder("rule_type");

		private static DatabaseColumnBinder Unit = new DatabaseColumnBinder("unit");

		public static RuleSource Bind(DatabaseDataReader reader)
		{
			return new RuleSource
			{
				SourceId = SourceId.GetGuid2(reader, allowNulls: true, Guid.NewGuid()).ToString(),
				CounterId = CounterId.GetInt32(reader, allowNulls: true, -1),
				CounterPath = Path.GetString(reader, allowNulls: true, string.Empty),
				DataCollectorId = DataCollectorId.GetInt32(reader, allowNulls: true, 0),
				CriticalThreshold = CriticalThreshold.GetDouble(reader, allowNulls: true, -1.0),
				WarningThreshold = WarningThreshold.GetDouble(reader, allowNulls: true, -1.0),
				TimeWindow = TimeWindow.GetInt32(reader, allowNulls: true, -1),
				CounterRuleId = CounterRuleId.GetGuid2(reader).ToString(),
				RuleType = RuleType.GetInt32(reader),
				Configurable = true,
				CategoryName = CategoryName.GetString(reader, allowNulls: true, string.Empty),
				CounterName = CounterName.GetString(reader, allowNulls: true, string.Empty),
				Unit = Unit.GetInt64(reader, allowNulls: true, 100L)
			};
		}
	}

	private string[] _ruleConfigIds = new string[7]
	{
		1.ToString(CultureInfo.InvariantCulture),
		2.ToString(CultureInfo.InvariantCulture),
		15.ToString(CultureInfo.InvariantCulture),
		16.ToString(CultureInfo.InvariantCulture),
		17.ToString(CultureInfo.InvariantCulture),
		6.ToString(CultureInfo.InvariantCulture),
		7.ToString(CultureInfo.InvariantCulture)
	};

	public List<RuleSource> QueryRuleSourcesByIds(int dataCollectorId, string[] ids, bool generateSources)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_rule_sources_by_ids");
			BindIdTable("@Ids", ids);
			BindIdTable("@RuleConfigIds", generateSources ? _ruleConfigIds : null);
			BindInt("@DataCollectorId", dataCollectorId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RuleSource> list = new List<RuleSource>();
			while (databaseDataReader.Read())
			{
				list.Add(RuleSourceColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public RuleSource QueryRuleSourceById(string sourceId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_rule_source_by_id");
			BindUniqueIdentifier("@SourceId", sourceId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (databaseDataReader.Read())
			{
				return RuleSourceColumns.Bind(databaseDataReader);
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return (RuleSource)null;
		});
	}

	public string[] QueryCountersStateRuleIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_rule_sources_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("SourceId");
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

	public List<RuleSource> QueryRuleSourcesByCounterId(string counterRuleId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_rule_sources_by_counter_rule_id");
			BindUniqueIdentifier("@CounterRuleId", counterRuleId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<RuleSource> list = new List<RuleSource>();
			while (databaseDataReader.Read())
			{
				list.Add(RuleSourceColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list;
		});
	}

	public void CreateRuleSource(RuleSource newRuleSource)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_rule_source");
			BindUniqueIdentifier("@CounterRuleId", newRuleSource.CounterRuleId);
			BindUniqueIdentifier("@SourceId", newRuleSource.SourceId);
			BindInt("@DataCollectorId", newRuleSource.DataCollectorId);
			BindInt("@CounterId", newRuleSource.CounterId);
			BindString("@DeviceId", newRuleSource.ParentId, 50, allowNull: true, SqlDbType.Text);
			BindFloat("@CriticalThreshold", newRuleSource.CriticalThreshold);
			BindFloat("@WarningThreshold", newRuleSource.WarningThreshold);
			BindInt("@TimeWindow", newRuleSource.TimeWindow);
			ExecuteNonQuery();
		});
	}

	public void UpdateRuleSource(RuleSource newRuleSource)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_rule_source");
			BindUniqueIdentifier("@SourceId", newRuleSource.SourceId);
			BindFloat("@CriticalThreshold", newRuleSource.CriticalThreshold);
			BindFloat("@WarningThreshold", newRuleSource.WarningThreshold);
			BindInt("@TimeWindow", newRuleSource.TimeWindow);
			ExecuteNonQuery();
		});
	}

	public void DeleteRuleSource(string sourceId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_rule_source");
			BindUniqueIdentifier("@SourceId", sourceId);
			ExecuteNonQuery();
		});
	}
}
