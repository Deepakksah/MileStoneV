namespace VideoOS.SqlUpdate.Core.Statics;

public static class Messages
{
	public static string PrintedArg(string arg, string value = null)
	{
		return $"\t{arg,-15} {value ?? string.Empty}";
	}

	public static string ScriptsCollectorGetScriptsOutputLog(int count)
	{
		return $"{count} sql files found.";
	}

	public static string ScriptsCollectorGetAllScriptsInputLog(string scriptType)
	{
		return "Retrieving all " + scriptType + " sql files.";
	}

	public static string EmbeddedScriptNotFound(string scriptName)
	{
		return "Embedded script '" + scriptName + "' does not exist";
	}

	public static string DbNotFound(string dbName)
	{
		return "DB not found. Name: " + dbName;
	}

	public static string DbAlreadyExists(string dbName)
	{
		return "Can't create DB as it already exists. Name: " + dbName;
	}
}
