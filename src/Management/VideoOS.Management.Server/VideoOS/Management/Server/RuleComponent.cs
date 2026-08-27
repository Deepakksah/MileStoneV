using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class RuleComponent : GeminiResourceComponent, IRuleComponent, IDisposable
{
	private static class RuleColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDRule");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		private static DatabaseColumnBinder Definition = new DatabaseColumnBinder("Definition");

		private static DatabaseColumnBinder Enabled = new DatabaseColumnBinder("Enabled");

		public static Rule Bind(DatabaseDataReader reader)
		{
			return new Rule
			{
				Id = Id.GetString(reader),
				Name = Name.GetString(reader),
				Description = Description.GetString(reader),
				LastModified = LastModified.GetDateTime(reader),
				Definition = Definition.GetString(reader),
				Enabled = Enabled.GetBoolean(reader)
			};
		}
	}

	public RuleComponent()
	{
	}

	public RuleComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	public void CreateRule(string id, string name, string description, string definition, bool enabled)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("create_rule");
			BindGuidType("@Id", id);
			BindNameType("@Name", name);
			BindDescription("@Description", description);
			BindXml("@Definition", definition);
			BindBoolean("@Enabled", enabled);
			ExecuteNonQuery();
		});
	}

	public string[] QueryRuleIds()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_rule_ids");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<string> list = new List<string>();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDRule");
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

	public Rule QueryRuleById(string id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_rule_by_id");
			BindGuidType("@Id", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Rule)null;
			}
			Rule result = RuleColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public Rule[] QueryRulesByIds(string[] ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_rules_by_ids");
			BindIdTable("@ids", ids);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<Rule> list = new List<Rule>();
			while (databaseDataReader.Read())
			{
				list.Add(RuleColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void UpdateRule(string id, string name, string description, string definition, bool enabled, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("update_rule");
			BindGuidType("@Id", id);
			BindNameType("@Name", name);
			BindDescription("@Description", description);
			BindXml("@Definition", definition);
			BindBoolean("@Enabled", enabled);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}

	public void DeleteRule(string id, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("delete_rule");
			BindGuidType("@Id", id);
			BindGuidType("@session_id", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
