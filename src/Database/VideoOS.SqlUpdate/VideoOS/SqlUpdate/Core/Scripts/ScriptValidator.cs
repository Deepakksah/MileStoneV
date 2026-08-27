using System.Collections.Generic;
using System.Linq;
using VideoOS.SqlUpdate.Core.Common;

namespace VideoOS.SqlUpdate.Core.Scripts;

public class ScriptValidator : IScriptValidator
{
	public static IEnumerable<string> ClrStatements = new string[16]
	{
		"CREATE ASSEMBLY", "EXTERNAL NAME", "dbo.[C18NDigest]", "[dbo].[EncryptData]", "[UpdateEncryption]", "[dbo].[Encrypt]", "CreateTLSData", "UpdateClientProfileXMLEncryption", "UpdateTaskXMLDatabaseSetEncryptionPassword", "UpdateTaskXMLSystemChangeHardwarePassword",
		"UpdateCustomSettingXMLEncryption", "UpdateHardwarePasswordEncryption", "UpdateEncryptionActivationCredentials", "UpdateEncryptionSystemSettings", "UpdateMIPLicensesXMLEncryption", "UpdateMIPRegistrationsXMLEncryption"
	};

	public static IEnumerable<string> ConfigureStatements = new string[2] { "sp_configure", "RECONFIGURE" };

	public void IsValidScript(SqlScript script)
	{
		foreach (string statement in script.Statements)
		{
			IsValidSqlStatement(statement);
		}
	}

	private void IsValidSqlStatement(string statement)
	{
		if (string.IsNullOrEmpty(statement))
		{
			throw new SqlUpdateException("Script contains null or empty statements");
		}
		if (ClrStatements.Any((string s) => statement.Contains(s)))
		{
			throw new SqlUpdateException("Script contains clr statements");
		}
		if (ConfigureStatements.Any((string s) => statement.Contains(s)))
		{
			throw new SqlUpdateException("Script contains configure statements");
		}
		if (statement.Contains("TRUSTWORTHY"))
		{
			throw new SqlUpdateException("Script contains invalid keyword 'TRUSTWORTHY'");
		}
		if (statement.Contains("ALTER DATABASE"))
		{
			throw new SqlUpdateException("Script contains invalid keyword 'ALTER DATABASE'");
		}
	}
}
