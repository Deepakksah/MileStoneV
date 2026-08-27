using System;
using System.Collections.Generic;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.UI.Framework;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public class DbInstallPageActions : PageSetupActionsBase
{
	public override string PageName => "DatabaseInstallTypePage";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (parameters.ContainsKey("SQL-SERVER-LOCAL") && string.Compare(parameters["SQL-SERVER-LOCAL"], "yes", StringComparison.OrdinalIgnoreCase) == 0 && !DatabaseCommon.IsDefaultInstancePresent())
		{
			GlobalData.SetValue("InstallNewServerChecked", "1", "dbInstActionsPlugin.BeforeActivatePage()");
		}
		return true;
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		Dictionary<string, string> dictionary = GlobalData.ToDict();
		if (dictionary.ContainsKey("IS_EXTERNALLY_MANAGED") && dictionary["IS_EXTERNALLY_MANAGED"].Equals("true", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		if (dictionary.ContainsKey("InstallNewServerChecked") && dictionary["InstallNewServerChecked"] == "0")
		{
			using (new ShowWaitCursor())
			{
				IDatabaseConnectionInfo databaseConnectionInfo;
				if (dictionary.ContainsKey("SQL_CONNECTION_STRING_MS"))
				{
					databaseConnectionInfo = new DatabaseConnectionInfo(dictionary["SQL_CONNECTION_STRING_MS"]);
				}
				else
				{
					if (!dictionary.ContainsKey("SQL_CONNECTION_STRING_LOG"))
					{
						throw new Exception("No connection string defined for MS and Log Server");
					}
					databaseConnectionInfo = new DatabaseConnectionInfo(dictionary["SQL_CONNECTION_STRING_LOG"]);
				}
				return DatabaseErrorCheckHelper.CheckServerConnection(new SqlConnectionManager(databaseConnectionInfo.CloneWithMasterDatabaseName(), null));
			}
		}
		return true;
	}
}
