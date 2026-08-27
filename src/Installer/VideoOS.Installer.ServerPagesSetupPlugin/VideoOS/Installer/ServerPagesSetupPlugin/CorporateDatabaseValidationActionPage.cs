using EventServerDbUpdate.UpdateActions;
using Microsoft.Extensions.Logging;
using VideoOS.Corporate.Database;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.DatabasePlugin;
using VideoOS.Installer.LogServerPlugins;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.Utilities.Database.KeyManagement;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

public class CorporateDatabaseValidationActionPage : DatabaseValidationPageAction
{
	private const string LogServerId = "L8451C4D-72B8-4D83-885D-A74B82F6C997";

	private const string ManagementServerId = "58451C4D-72B8-4D83-885D-A74B82F6C997";

	private const string EventServerId = "5F0DF842-CDD1-4382-8FE2-EF320E1C7D28";

	public override DatabaseValidationResult ValidateDatabase(string componentPackageId)
	{
		return componentPackageId switch
		{
			"58451C4D-72B8-4D83-885D-A74B82F6C997" => RunManagmentServerValidateDatabase(base.Logger), 
			"L8451C4D-72B8-4D83-885D-A74B82F6C997" => RunLogServerValidateDatabase(), 
			"5F0DF842-CDD1-4382-8FE2-EF320E1C7D28" => RunEventServerValidateDatabase(), 
			_ => null, 
		};
	}

	public override string[] GetRegisteredComponentsPackageIds()
	{
		return new string[3] { "58451C4D-72B8-4D83-885D-A74B82F6C997", "L8451C4D-72B8-4D83-885D-A74B82F6C997", "5F0DF842-CDD1-4382-8FE2-EF320E1C7D28" };
	}

	public static DatabaseValidationResult RunManagmentServerValidateDatabase(ILogger logger)
	{
		GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out var value);
		GlobalData.TryGetValue("PRODUCT-SKU", out var value2);
		IEncryptionKeyReader encryptionKeyReader = KeyManagementPasswordHelper.CreateKeyManager(logger);
		string password = null;
		if (KeyManagementPasswordHelper.GetSystemPasswordMode(encryptionKeyReader, new VideoOS.Installer.Common.GlobalDataWrapper(), logger) != SystemPasswordMode.NoPasswordRequired)
		{
			GlobalData.TryGetValue("SYSTEMPASSWORD", out var value3);
			password = EncryptHelper.DecryptPassword(value3, unicodePassword: true);
		}
		return Database.ValidateDatabase(value, value2, encryptionKeyReader, password, DatabaseValidationPageAction.ParseSqlUpdateActionFromUserParameters(), logger);
	}

	private DatabaseValidationResult RunLogServerValidateDatabase()
	{
		GlobalData.TryGetValue("SQL_CONNECTION_STRING_LOG", out var value);
		return SetupDatabaseTask.ValidateDatabase(value, DatabaseValidationPageAction.ParseSqlUpdateActionFromUserParameters(), base.Logger);
	}

	private DatabaseValidationResult RunEventServerValidateDatabase()
	{
		GlobalData.TryGetValue("SQL_CONNECTION_STRING_ES", out var value);
		if (string.IsNullOrEmpty(value))
		{
			GlobalData.TryGetValue("SQL_CONNECTION_STRING_MS", out value);
		}
		IEncryptionKeyReader encryptionKeyReader = KeyManagementPasswordHelper.CreateKeyManager(base.Logger);
		if (KeyManagementPasswordHelper.GetSystemPasswordMode(encryptionKeyReader, new VideoOS.Installer.Common.GlobalDataWrapper(), base.Logger) != SystemPasswordMode.NoPasswordRequired)
		{
			GlobalData.TryGetValue("SYSTEMPASSWORD", out var value2);
			EncryptHelper.DecryptPassword(value2, unicodePassword: true);
		}
		return EsDatabaseUpdate.ValidateDatabase(value, DatabaseValidationPageAction.ParseSqlUpdateActionFromUserParameters(), encryptionKeyReader, base.Logger);
	}
}
