using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Installer.Common;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.UI.Framework;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class EnterpriseSlaveWarningPageActions : PageSetupActionsBase
{
	public override string PageName => "EnterpriseSlaveWarningPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		bool flag = true;
		bool flag2 = true;
		bool flag3 = false;
		if (GlobalData.TryGetValue("IS_UPGRADE", out var value))
		{
			flag = string.Equals("true", value, StringComparison.OrdinalIgnoreCase);
		}
		if (GlobalData.TryGetValue("SQL-KEEP-DATA", out value))
		{
			flag2 = string.Equals("yes", value, StringComparison.OrdinalIgnoreCase);
		}
		if (GlobalData.TryGetValue("SQL-CREATE-DATABASE", out value))
		{
			flag3 = string.Equals("yes", value, StringComparison.OrdinalIgnoreCase);
		}
		if (flag && !flag3 && flag2)
		{
			return CheckIfExistsEnterpriseSlaves();
		}
		return false;
	}

	private static bool CheckIfExistsEnterpriseSlaves()
	{
		if (GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out var value))
		{
			try
			{
				return QueryAnyEnterpriseSlaveConfigured(value);
			}
			catch (DatabaseException ex)
			{
				throw new InstallationAdapterException("Failed while checking the database for Enterprise servers.", ex);
			}
		}
		return false;
	}

	private static bool QueryAnyEnterpriseSlaveConfigured(string connectionString)
	{
		string text = "IF (EXISTS (SELECT TOP 1 * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'EnterpriseServers'))" + Environment.NewLine + "BEGIN" + Environment.NewLine + "  SELECT TOP 1 * FROM [EnterpriseServers]" + Environment.NewLine + "END";
		using (DatabaseConnection databaseConnection = new DatabaseConnection(connectionString, null))
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(text.ToString());
			databaseCommand.CommandType = CommandType.Text;
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SingleRow);
			if (databaseDataReader.HasRows)
			{
				return true;
			}
		}
		return false;
	}
}
