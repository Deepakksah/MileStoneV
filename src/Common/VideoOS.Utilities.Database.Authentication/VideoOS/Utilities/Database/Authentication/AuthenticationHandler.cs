using System;
using System.Security.Principal;
using System.Text;
using Microsoft.Extensions.Logging;

namespace VideoOS.Utilities.Database.Authentication;

public class AuthenticationHandler
{
	private readonly ISqlConnectionManager _connectionManager;

	private readonly ILogger _logger;

	public AuthenticationHandler(ISqlConnectionManager connectionManager, ILogger logger)
	{
		_connectionManager = connectionManager;
		_logger = logger;
	}

	public static string GetSidFromAccountName(string accountName, ILogger logger)
	{
		LoggerExtensions.LogDebug(logger, "AuthenticationHandler.GetSidFromAccountName", Array.Empty<object>());
		try
		{
			return ((SecurityIdentifier)new NTAccount(accountName).Translate(typeof(SecurityIdentifier))).Value;
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(logger, "{ClassName}.{MethodName}: encountered an error: {Exception}", new object[3] { "AuthenticationHandler", "GetSidFromAccountName", ex });
			throw;
		}
	}

	public void CreateLogin(string accountName)
	{
		LoggerExtensions.LogDebug(_logger, "AuthenticationHandler.CreateLogin: using account name.", Array.Empty<object>());
		ExecuteCreateLogin(accountName, "[name] = '" + accountName + "'");
	}

	public void CreateLogin(string accountName, string dbSchema)
	{
		LoggerExtensions.LogDebug(_logger, "AuthenticationHandler.CreateLogin: using SID.", Array.Empty<object>());
		string sidFromAccountName = GetSidFromAccountName(accountName, _logger);
		ExecuteCreateLogin(accountName, "[" + dbSchema + "].[fn_SIDToString]([Sid]) = '" + sidFromAccountName + "'");
	}

	public void AddUserToDbOwner(string accountName, string userName)
	{
		LoggerExtensions.LogDebug(_logger, "AuthenticationHandler.AddUserToDbOwner: using account name.", Array.Empty<object>());
		ExecuteAddUserToDbOwner(accountName, userName, "[sl].[Name] = '" + accountName + "'");
	}

	public void AddUserToDbOwner(string accountName, string userName, string dbSchema)
	{
		LoggerExtensions.LogDebug(_logger, "AuthenticationHandler.AddUserToDbOwner", Array.Empty<object>());
		string sidFromAccountName = GetSidFromAccountName(accountName, _logger);
		ExecuteAddUserToDbOwner(accountName, userName, "[" + dbSchema + "].[fn_SIDToString](sl.[Sid]) = '" + sidFromAccountName + "'");
	}

	public string GetServerHostName()
	{
		using DatabaseConnection databaseConnection = _connectionManager.GetSqlConnection();
		using DatabaseCommand databaseCommand = databaseConnection.CreateCommand();
		databaseCommand.CommandText = "SELECT SERVERPROPERTY('MachineName');";
		return (databaseCommand.ExecuteScalar() as string) ?? throw new ArgumentException("Could not resolve SQL server host name");
	}

	public void SetDatabaseOwner(string serviceAccount, string computerDomain, string dbSchema, bool isNetworkService, bool canUseSid)
	{
		string serverHostName = GetServerHostName();
		bool flag = string.IsNullOrEmpty(serverHostName) || DomainHelper.IsDbServerLocal(serverHostName);
		bool flag2 = canUseSid;
		string text;
		if (isNetworkService)
		{
			text = DomainHelper.GetNetworkServiceAccountName(!flag, computerDomain);
		}
		else
		{
			text = serviceAccount;
			if (!flag)
			{
				if (DomainHelper.IsLocalUser(text))
				{
					text = DomainHelper.GetRemoteAccountNameForLocalUser(serverHostName, text);
					flag2 = false;
				}
				else
				{
					string accountName = Environment.MachineName + "\\" + text;
					if (DomainHelper.IsLocalUser(accountName))
					{
						text = DomainHelper.GetRemoteAccountNameForLocalUser(serverHostName, accountName);
						flag2 = false;
					}
				}
			}
		}
		LoggerExtensions.LogInformation(_logger, "{MethodName}(): Account name was translated to public account name: '{AccountName}'", new object[2] { "SetDatabaseOwner", text });
		if (flag2)
		{
			CreateLogin(text, dbSchema);
		}
		else
		{
			CreateLogin(text);
		}
		LoggerExtensions.LogInformation(_logger, "{MethodName}(): created login for user {AccountName}", new object[2] { "SetDatabaseOwner", text });
		if (flag2)
		{
			AddUserToDbOwner(text, text, dbSchema);
		}
		else
		{
			AddUserToDbOwner(text, text);
		}
		LoggerExtensions.LogInformation(_logger, "{MethodName}(): user {AccountName} added and given DB owner role membership", new object[2] { "SetDatabaseOwner", text });
	}

	private void ExecuteCreateLogin(string accountName, string comparisonString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("IF NOT EXISTS(SELECT [name] FROM [sys].[syslogins] WHERE " + comparisonString + ")");
		stringBuilder.AppendLine("BEGIN");
		stringBuilder.AppendLine("    CREATE LOGIN [" + accountName + "] FROM WINDOWS");
		stringBuilder.AppendLine("END ");
		try
		{
			using DatabaseConnection databaseConnection = _connectionManager.GetSqlConnection();
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(stringBuilder.ToString());
			databaseCommand.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, "{ClassName}.{MethodName}: encountered an error: {Exception}", new object[3] { "AuthenticationHandler", "CreateLogin", ex });
			throw;
		}
	}

	private void ExecuteAddUserToDbOwner(string accountName, string userName, string comparisonString)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine("USE [" + _connectionManager.DatabaseName + "]");
		stringBuilder.AppendLine("DECLARE @user nvarchar(128)");
		stringBuilder.AppendLine("SET @user = (");
		stringBuilder.AppendLine("    SELECT su.[Name]");
		stringBuilder.AppendLine("    FROM [sys].[syslogins] AS sl");
		stringBuilder.AppendLine("    JOIN [sys].[sysusers] AS su");
		stringBuilder.AppendLine("    ON sl.[Sid] = su.[Sid]");
		stringBuilder.AppendLine("    WHERE " + comparisonString + " COLLATE DATABASE_DEFAULT)");
		stringBuilder.AppendLine("IF @user is NULL");
		stringBuilder.AppendLine("    BEGIN ");
		stringBuilder.AppendLine("        CREATE USER [" + userName + "] FOR LOGIN [" + accountName + "]");
		stringBuilder.AppendLine("        SET @user = N'" + userName + "'");
		stringBuilder.AppendLine("    END");
		stringBuilder.AppendLine("IF @user != N'dbo'");
		stringBuilder.AppendLine("ALTER ROLE db_owner ADD MEMBER [" + userName + "]");
		try
		{
			using DatabaseConnection databaseConnection = _connectionManager.GetSqlConnection();
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand(stringBuilder.ToString());
			databaseCommand.ExecuteNonQuery();
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, "{ClassName}.{MethodName} encountered an error: {Exception}", new object[3] { "AuthenticationHandler", "AddUserToDbOwner", ex });
			throw;
		}
	}
}
