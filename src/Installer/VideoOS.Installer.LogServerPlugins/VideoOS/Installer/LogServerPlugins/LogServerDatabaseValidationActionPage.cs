using VideoOS.Installer.Common;
using VideoOS.Installer.DatabasePlugin;
using VideoOS.SqlUpdate.Core.Common;

namespace VideoOS.Installer.LogServerPlugins;

public class LogServerDatabaseValidationActionPage : DatabaseValidationPageAction
{
	private const string LogServerId = "2B0B1230-724B-414C-BFE8-2CBC4143565E";

	public override DatabaseValidationResult ValidateDatabase(string componentPackageId)
	{
		if (componentPackageId == "2B0B1230-724B-414C-BFE8-2CBC4143565E")
		{
			return RunLogServerValidateDatabase();
		}
		return null;
	}

	public override string[] GetRegisteredComponentsPackageIds()
	{
		return new string[1] { "2B0B1230-724B-414C-BFE8-2CBC4143565E" };
	}

	private DatabaseValidationResult RunLogServerValidateDatabase()
	{
		GlobalData.TryGetValue("SQL_CONNECTION_STRING_LOG", out var value);
		return SetupDatabaseTask.ValidateDatabase(value, DatabaseValidationPageAction.ParseSqlUpdateActionFromUserParameters(), base.Logger);
	}
}
