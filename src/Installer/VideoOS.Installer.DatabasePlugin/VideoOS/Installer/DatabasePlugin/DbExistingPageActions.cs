using System;
using System.Collections.Generic;
using System.Globalization;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.DatabasePlugin.Properties;
using VideoOS.Installer.Logging;
using VideoOS.Installer.UI.Framework;
using VideoOS.Utilities.Database;

namespace VideoOS.Installer.DatabasePlugin;

public class DbExistingPageActions : PageSetupActionsBase
{
	public override string PageName => "DatabaseExistingType";

	public override bool BeforeActivatePage(Dictionary<string, string> parameters)
	{
		if (parameters.ContainsKey("InstallNewServerChecked") && parameters["InstallNewServerChecked"] == "1" && !DatabaseCommon.IsDefaultInstancePresent())
		{
			return false;
		}
		Dictionary<string, string> settings = GetSettings(parameters);
		IDatabaseConnectionInfo databaseConnectionInfo;
		if (settings.ContainsKey("SQL_CONNECTION_STRING_MS"))
		{
			databaseConnectionInfo = new DatabaseConnectionInfo(settings["SQL_CONNECTION_STRING_MS"]);
		}
		else
		{
			if (!settings.ContainsKey("SQL_CONNECTION_STRING_LOG"))
			{
				throw new Exception("No connection string defined for MS and Log Server");
			}
			databaseConnectionInfo = new DatabaseConnectionInfo(settings["SQL_CONNECTION_STRING_LOG"]);
		}
		bool flag = DatabaseErrorCheckHelper.DatabaseExistsWithoutException(new SqlConnectionManager(databaseConnectionInfo, null));
		parameters["InstallDatabaseExists"] = flag.ToString();
		GlobalData.SetValue("InstallDatabaseExists", flag.ToString(), "DbExistingPageActions.BeforeActivatePage()");
		return true;
	}

	private Dictionary<string, string> GetSettings(Dictionary<string, string> parameters)
	{
		Dictionary<string, string> dictionary = GlobalData.ToDict();
		if (ValidateParameters(dictionary, parameters))
		{
			dictionary = GlobalData.ToDict();
		}
		return dictionary;
	}

	public override bool BeforeLeavePage(Dictionary<string, string> parameters)
	{
		Dictionary<string, string> settings = GetSettings(parameters);
		if (settings["SQL-CREATE-DATABASE"] == "no")
		{
			if (settings.ContainsKey("SQL_CONFIRM_OVERWRITE") && settings["SQL_CONFIRM_OVERWRITE"] == bool.FalseString)
			{
				return false;
			}
			if (!CheckConnection(settings))
			{
				return false;
			}
		}
		else
		{
			using (new ShowWaitCursor())
			{
				IDatabaseConnectionInfo databaseConnectionInfo;
				if (settings.ContainsKey("SQL_CONNECTION_STRING_MS"))
				{
					databaseConnectionInfo = new DatabaseConnectionInfo(settings["SQL_CONNECTION_STRING_MS"]);
				}
				else
				{
					if (!settings.ContainsKey("SQL_CONNECTION_STRING_LOG"))
					{
						throw new Exception("No connection string defined for MS and Log Server");
					}
					databaseConnectionInfo = new DatabaseConnectionInfo(settings["SQL_CONNECTION_STRING_LOG"]);
				}
				ISqlConnectionManager sqlConnectionManager = new SqlConnectionManager(databaseConnectionInfo, null);
				if (DatabaseErrorCheckHelper.DatabaseExistsWithoutException(sqlConnectionManager))
				{
					if (!settings.ContainsKey("INSTALLATION_MODE"))
					{
						if (!settings.ContainsKey("SQL-CREATE-DATABASE"))
						{
							GlobalData.SetValue("SQL-CREATE-DATABASE", "no", "DbExistingPageActions.BeforeLeavePage()");
						}
						if (!settings.ContainsKey("SQL-KEEP-DATA"))
						{
							GlobalData.SetValue("SQL-KEEP-DATA", "yes", "DbExistingPageActions.BeforeLeavePage()");
						}
						if (!CheckConnection(settings))
						{
							return false;
						}
					}
					DataBaseErrorString dataBaseErrorString = DatabaseErrorCheckHelper.CheckSelectedDB(sqlConnectionManager);
					if (dataBaseErrorString == DataBaseErrorString.SQLServerNotSupported)
					{
						LogManager.Write(string.Format(CultureInfo.CurrentCulture, Resources.Database_NotSupportedSQLVersion, DatabaseErrorCheckHelper.MinimumSupportedSqlVersionYear));
						DatabaseErrorCheckHelper.ThrowErrorMessage(dataBaseErrorString);
					}
				}
				else if (DatabaseErrorCheckHelper.DatabaseExists(sqlConnectionManager, throwDbExist: true))
				{
					return false;
				}
			}
		}
		return true;
	}

	private bool CheckConnection(Dictionary<string, string> settings)
	{
		using (new ShowWaitCursor())
		{
			IDatabaseConnectionInfo databaseConnectionInfo;
			if (settings.ContainsKey("SQL_CONNECTION_STRING_MS"))
			{
				databaseConnectionInfo = new DatabaseConnectionInfo(settings["SQL_CONNECTION_STRING_MS"]);
			}
			else
			{
				if (!settings.ContainsKey("SQL_CONNECTION_STRING_LOG"))
				{
					throw new Exception("No connection string defined for MS and Log Server");
				}
				databaseConnectionInfo = new DatabaseConnectionInfo(settings["SQL_CONNECTION_STRING_LOG"]);
			}
			if (!DatabaseErrorCheckHelper.CheckConnectionToExistingDatabase(new SqlConnectionManager(databaseConnectionInfo, null)))
			{
				return false;
			}
		}
		return true;
	}

	private bool ValidateParameters(Dictionary<string, string> settings, Dictionary<string, string> parameters)
	{
		bool result = false;
		if (!settings.ContainsKey("SQL_CONNECTION_STRING_MS") && parameters.ContainsKey("SQL_CONNECTION_STRING_MS"))
		{
			GlobalData.SetValue("SQL_CONNECTION_STRING_MS", parameters["SQL_CONNECTION_STRING_MS"], "dbExistSetupActions.BeforeLeavePageValidateParams()");
			result = true;
		}
		if (!settings.ContainsKey("SQL_CONNECTION_STRING_LOG") && parameters.ContainsKey("SQL_CONNECTION_STRING_LOG"))
		{
			GlobalData.SetValue("SQL_CONNECTION_STRING_LOG", parameters["SQL_CONNECTION_STRING_LOG"], "dbExistSetupActions.BeforeLeavePageValidateParams()");
			result = true;
		}
		if (!settings.ContainsKey("IS_EXTERNALLY_MANAGED") && parameters.ContainsKey("IS_EXTERNALLY_MANAGED"))
		{
			GlobalData.SetValue("IS_EXTERNALLY_MANAGED", parameters["IS_EXTERNALLY_MANAGED"], "dbExistSetupActions.BeforeLeavePageValidateParams()");
			result = true;
		}
		if (!settings.ContainsKey("SQL-KEEP-DATA"))
		{
			GlobalData.SetValue("SQL-KEEP-DATA", parameters["SQL-KEEP-DATA"], "dbExistSetupActions.BeforeLeavePageValidateParams()");
			result = true;
		}
		if (!settings.ContainsKey("SQL-CREATE-DATABASE"))
		{
			GlobalData.SetValue("SQL-CREATE-DATABASE", parameters["SQL-CREATE-DATABASE"], "dbExistSetupActions.BeforeLeavePageValidateParams()");
			result = true;
		}
		if (parameters.ContainsKey("INSTALLATION_MODE") && !settings.ContainsKey("INSTALLATION_MODE"))
		{
			GlobalData.SetValue("INSTALLATION_MODE", parameters["INSTALLATION_MODE"], "dbExistSetupActions.BeforeLeavePageValidateParams()");
			result = true;
		}
		return result;
	}
}
