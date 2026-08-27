namespace VideoOS.SqlUpdate.Core.Statics;

public static class ScriptContentReplacementKeys
{
	public const string DbName = "{DatabaseName}";

	public const string IdpDbName = "{IdpDatabaseName}";

	public const string MasterKey = "{MasterEncryptionKey}";

	public const string MasterKeyEncrypted = "{EncryptedMasterEncryptionKey}";

	public const string NewDatabaseHomeSiteId = "{NewDatabaseHomeSiteId}";

	public const string NewDatabaseLicenseTlsData = "{NewDatabaseLicenseTlsData}";

	public const string Target = "{target}";

	public const string Source = "{source}";
}
