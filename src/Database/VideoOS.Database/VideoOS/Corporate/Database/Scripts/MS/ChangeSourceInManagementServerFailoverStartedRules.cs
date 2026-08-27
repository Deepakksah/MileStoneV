using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Utilities.Database;

namespace VideoOS.Corporate.Database.Scripts.MS;

public class ChangeSourceInManagementServerFailoverStartedRules
{
	public static void Execute(DatabaseConnection connection)
	{
		Dictionary<string, string> rules = SubstituteSourceIds(GetAllRulesXMLs(connection));
		SaveUpdatedRules(connection, rules);
	}

	private static Dictionary<string, string> GetAllRulesXMLs(DatabaseConnection connection)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		using DatabaseCommand databaseCommand = connection.CreateCommand("SELECT * FROM [dbo].[Rules]");
		databaseCommand.CommandType = CommandType.Text;
		using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
		while (databaseDataReader.Read())
		{
			if (!databaseDataReader.IsDBNull(databaseDataReader.GetOrdinal("IDRule")))
			{
				string key = databaseDataReader.GetString(databaseDataReader.GetOrdinal("IDRule"));
				if (!databaseDataReader.IsDBNull(databaseDataReader.GetOrdinal("Definition")))
				{
					string text = databaseDataReader.GetString(databaseDataReader.GetOrdinal("Definition"));
					if (RuleSourceIdSubstitutor25R1.AnySourceIdsToSubstitute(text))
					{
						dictionary[key] = text;
					}
					continue;
				}
				throw new KeyNotFoundException("Definition is not present");
			}
			throw new KeyNotFoundException("IDRule is not present");
		}
		return dictionary;
	}

	private static Dictionary<string, string> SubstituteSourceIds(Dictionary<string, string> rules)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>();
		foreach (KeyValuePair<string, string> rule in rules)
		{
			dictionary[rule.Key] = RuleSourceIdSubstitutor25R1.SubstituteSourceIds(rule.Value);
		}
		return dictionary;
	}

	private static void SaveUpdatedRules(DatabaseConnection connection, Dictionary<string, string> rules)
	{
		string commandText = "UPDATE [dbo].[Rules] SET [Definition] = @Definition, [LastModified] = GETUTCDATE() WHERE [IDRule] = @Id";
		using DatabaseCommand databaseCommand = connection.CreateCommand(commandText);
		databaseCommand.CommandType = CommandType.Text;
		databaseCommand.CommandTimeout = 0;
		DatabaseParameter[] array = new DatabaseParameter[2]
		{
			new DatabaseParameter("@Id", SqlDbType.UniqueIdentifier),
			new DatabaseParameter("@Definition", SqlDbType.NVarChar)
		};
		databaseCommand.AddParameters(array);
		foreach (KeyValuePair<string, string> rule in rules)
		{
			array[0].Value = new Guid(rule.Key);
			array[1].Value = rule.Value;
			if (databaseCommand.ExecuteNonQuery() == -1)
			{
				throw new ArgumentException("Failed to update Item with ID " + rule.Key + " and value " + rule.Value);
			}
		}
	}
}
