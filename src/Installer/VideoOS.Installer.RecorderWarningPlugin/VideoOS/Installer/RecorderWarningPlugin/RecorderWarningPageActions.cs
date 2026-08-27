using System;
using System.Collections.Generic;
using System.Data;
using VideoOS.Installer.Common;
using VideoOS.Installer.UI.Framework;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.RecorderWarningPlugin;

public class RecorderWarningPageActions : PageSetupActionsBase
{
	public override string PageName => "RecorderWarningPage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (GlobalData.TryGetValue("SQL-KEEP-DATA", out var value) && value.Equals("no", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		if (GlobalData.TryGetValue("SQL-CREATE-DATABASE", out var value2) && value2.Equals("yes", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		string value3 = string.Empty;
		if (GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out value3))
		{
			try
			{
				using DatabaseConnection databaseConnection = new DatabaseConnection(value3, null);
				using DatabaseCommand databaseCommand = databaseConnection.CreateCommand("SELECT [version] FROM [SYSTEM]");
				databaseCommand.CommandType = CommandType.Text;
				using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SingleRow);
				if (databaseDataReader.Read())
				{
					string text = databaseDataReader.GetString(0);
					if (!string.IsNullOrEmpty(text))
					{
						Version version = new Version(text);
						if ((version.Major == 12 && version.Minor >= 2) || version.Major > 12)
						{
							return false;
						}
					}
				}
			}
			catch
			{
				return false;
			}
			try
			{
				using DatabaseConnection databaseConnection2 = new DatabaseConnection(value3, null);
				using DatabaseCommand databaseCommand2 = databaseConnection2.CreateCommand("SELECT COUNT(*) FROM [RecordingServerService]");
				databaseCommand2.CommandType = CommandType.Text;
				using DatabaseDataReader databaseDataReader2 = databaseCommand2.ExecuteReader(CommandBehavior.SingleRow);
				if (databaseDataReader2.Read())
				{
					int @int = databaseDataReader2.GetInt32(0);
					if (@int > 1)
					{
						return true;
					}
					if (@int == 0)
					{
						return false;
					}
				}
			}
			catch
			{
				return true;
			}
			try
			{
				using DatabaseConnection databaseConnection3 = new DatabaseConnection(value3, null);
				using DatabaseCommand databaseCommand3 = databaseConnection3.CreateCommand("SELECT COUNT(*) FROM RecordingServerService WHERE [host_name] IN (SELECT [SETTING] FROM SystemSettings WHERE [Name] = 'ManagementServerHostName')");
				databaseCommand3.CommandType = CommandType.Text;
				using DatabaseDataReader databaseDataReader3 = databaseCommand3.ExecuteReader(CommandBehavior.SingleRow);
				if (databaseDataReader3.Read() && databaseDataReader3.GetInt32(0) == 1)
				{
					return false;
				}
			}
			catch
			{
			}
		}
		return true;
	}
}
