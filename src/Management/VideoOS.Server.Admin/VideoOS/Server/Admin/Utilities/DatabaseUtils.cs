using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Management.Common;
using VideoOS.Server.Admin.Properties;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.Authentication;

namespace VideoOS.Server.Admin.Utilities;

public sealed class DatabaseUtils : ServerAdminResourceComponent
{
	private static class RoleColumns
	{
		private static DatabaseColumnBinder IDRole = new DatabaseColumnBinder("IDRole");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("Name");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("Type");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static Role Bind(DatabaseDataReader reader)
		{
			return new Role
			{
				Id = IDRole.GetString(reader, allowNulls: false),
				Name = Name.GetString(reader, allowNulls: false),
				Description = Description.GetString(reader, allowNulls: true),
				Type = (RoleType)Type.GetByte(reader),
				LastModified = LastModified.GetDateTime(reader)
			};
		}
	}

	private static class RoleMemberColumns
	{
		private static DatabaseColumnBinder MemberSid = new DatabaseColumnBinder("SID");

		public static string Bind(DatabaseDataReader reader)
		{
			return MemberSid.GetString(reader, allowNulls: false);
		}
	}

	private readonly ISqlConnectionManager _connectionManager;

	private readonly AuthenticationHandler _authenticationHandler;

	private readonly ILogger _logger;

	public DatabaseUtils(ISqlConnectionManager connectionManager, ILogger logger)
		: base(connectionManager)
	{
		_connectionManager = connectionManager;
		_authenticationHandler = new AuthenticationHandler(connectionManager, logger);
		_logger = logger;
	}

	public void RemoveLogin(string accountName)
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "IF EXISTS(SELECT [name] FROM sys.syslogins WHERE [name] = '{0}') ", accountName);
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "BEGIN ");
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "    DROP LOGIN [{0}]", accountName);
		stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "END ");
		PrepareSqlBatch();
		AddStatement(stringBuilder.ToString());
		BindString("@accountName", accountName, 256, allowNull: false, SqlDbType.NVarChar);
		ExecuteNonQuery();
	}

	public bool SetDatabaseOwner(string databaseName, string accountName)
	{
		PrepareSqlBatch();
		AddStatement(FormattableString.Invariant($"ALTER AUTHORIZATION ON DATABASE::[{databaseName}] TO [{accountName}]"));
		try
		{
			object obj = ExecuteScalar();
			return obj == null || (int)obj != 1;
		}
		catch (DatabaseException ex)
		{
			if (ex.Number != 15110)
			{
				throw;
			}
		}
		return true;
	}

	public string CreateUserFromLogin(string databaseName, string accountName)
	{
		string sidFromAccountName = AuthenticationHandler.GetSidFromAccountName(accountName, _logger);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendLine(FormattableString.Invariant($"USE [{databaseName}]"));
		stringBuilder.AppendLine("DECLARE @UserName sysname");
		stringBuilder.AppendLine(FormattableString.Invariant($"SELECT @UserName = [name] FROM sys.sysusers WHERE dbo.fn_SIDToString([Sid]) = '{sidFromAccountName}'"));
		stringBuilder.AppendLine("IF @UserName IS NULL");
		stringBuilder.AppendLine("BEGIN");
		stringBuilder.AppendLine(FormattableString.Invariant($"SET @UserName = '{accountName}'"));
		stringBuilder.AppendLine(FormattableString.Invariant($"CREATE USER [{accountName}] FROM LOGIN [{accountName}]"));
		stringBuilder.AppendLine("END");
		stringBuilder.AppendLine("SELECT @UserName");
		PrepareSqlBatch();
		AddStatement(stringBuilder.ToString());
		return (string)ExecuteScalar();
	}

	public string GetOriginalLogin(string databaseName)
	{
		PrepareSqlBatch();
		AddStatement(FormattableString.Invariant($"USE [{databaseName}]; SELECT ORIGINAL_LOGIN()"));
		return (string)ExecuteScalar();
	}

	public void SetDatabaseAccount(AccountInfo newAccount)
	{
		try
		{
			bool isDualServer = !DomainHelper.IsDbServerLocal(GetUnderlyingSQLAliasHostname(_connectionManager.ServerName));
			string databaseAccount = newAccount.GetDatabaseAccount(isDualServer);
			string text = null;
			try
			{
				_authenticationHandler.CreateLogin(databaseAccount, "dbo");
				text = CreateUserFromLogin(_connectionManager.DatabaseName, databaseAccount);
			}
			catch (Exception ex)
			{
				throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.FailedToCreateLogin, databaseAccount, _connectionManager.ServerName, ex.Message));
			}
			try
			{
				_authenticationHandler.AddUserToDbOwner(databaseAccount, text, "dbo");
			}
			catch (Exception)
			{
				throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.FailedToSetDatabaseOwner, databaseAccount, _connectionManager.DatabaseName));
			}
		}
		catch (DatabaseException ex3)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.FaultToConfigureDatabaseAccount, newAccount.AccountName, _connectionManager.ServerName, ex3.Message), ex3);
		}
	}

	public void TakeDatabaseOwnership()
	{
		string originalLogin = GetOriginalLogin(_connectionManager.DatabaseName);
		try
		{
			if (!SetDatabaseOwner(_connectionManager.DatabaseName, originalLogin))
			{
				throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.FailedToSetDatabaseOwner, originalLogin, _connectionManager.DatabaseName));
			}
		}
		catch (DatabaseException ex)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.FaultToConfigureDatabaseAccount, originalLogin, _connectionManager.ServerName, ex.Message), ex);
		}
	}

	public bool DatabaseExists()
	{
		try
		{
			_connectionManager.GetSqlConnection().Open();
			return true;
		}
		catch (DatabaseException)
		{
			DatabaseConnection.ClearAllPools();
			return false;
		}
	}

	public void SetAdminRoleAccount(AccountInfo serviceAccount)
	{
		try
		{
			if (!serviceAccount.IsNetworkService)
			{
				string localizedName = serviceAccount.GetLocalizedName();
				SetServiceAccount(_connectionManager.DatabaseName, localizedName);
				return;
			}
			SetServiceAccount(_connectionManager.DatabaseName, serviceAccount.AccountName);
			try
			{
				IPGlobalProperties iPGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();
				if (iPGlobalProperties != null && !string.IsNullOrEmpty(iPGlobalProperties.DomainName))
				{
					string networkServiceRemoteCredentials = AccountInfo.GetNetworkServiceRemoteCredentials();
					SetServiceAccount(_connectionManager.DatabaseName, networkServiceRemoteCredentials);
				}
			}
			catch (NetworkInformationException)
			{
			}
		}
		catch (Exception ex2)
		{
			throw new ServerAdminException(string.Format(CultureInfo.CurrentCulture, Resources.FailedToConfigureServiceAccount, serviceAccount.AccountName, _connectionManager.DatabaseName, ex2.Message));
		}
	}

	public void AddDatabaseAccount(AccountInfo newAccount)
	{
		bool flag = !DomainHelper.IsDbServerLocal(GetUnderlyingSQLAliasHostname(_connectionManager.ServerName));
		string databaseAccount = newAccount.GetDatabaseAccount(flag);
		_authenticationHandler.CreateLogin(databaseAccount, "dbo");
		AddServiceAccount(_connectionManager.DatabaseName, databaseAccount);
		SetServiceAccount(_connectionManager.DatabaseName, databaseAccount);
		if (newAccount.IsNetworkService && flag)
		{
			SetServiceAccount(_connectionManager.DatabaseName, newAccount.AccountName);
		}
	}

	public void RemoveDatabaseAccount(AccountInfo account)
	{
		bool isDualServer = !DomainHelper.IsDbServerLocal(GetUnderlyingSQLAliasHostname(_connectionManager.ServerName));
		string databaseAccount = account.GetDatabaseAccount(isDualServer);
		RemoveLogin(databaseAccount);
	}

	public void AddServiceAccount(string databaseName, string accountName)
	{
		PrepareSqlBatch();
		AddStatement(GetUseStatement(databaseName));
		ExecuteNonQuery();
		PrepareStoredProcedure("add_service_account");
		BindString("@accountName", accountName, 128, allowNull: false, SqlDbType.NVarChar);
		try
		{
			ExecuteNonQuery();
		}
		catch (DatabaseException)
		{
			throw;
		}
	}

	public void SetServiceAccount(string configDatabase, string account)
	{
		PrepareSqlBatch();
		AddStatement(GetUseStatement(configDatabase));
		ExecuteNonQuery();
		Guid parameterValue = new Guid("1f30d14b-ba91-4e8a-8838-eda17931f8fc");
		string sidFromAccountName = AuthenticationHandler.GetSidFromAccountName(account, _logger);
		Identity identity = ReadIdentityFromSource(sidFromAccountName);
		if (identity == null)
		{
			throw new ServerAdminException("Invalid sid");
		}
		PrepareStoredProcedure("security_add_role_member");
		BindGuidType("@IDRole", parameterValue);
		BindSIDType("@MemberSID", identity.Sid);
		BindByte("@Type", (byte)identity.Type);
		BindString("@DisplayName", identity.DisplayName, 512, allowNull: false, SqlDbType.NVarChar);
		BindString("@Description", identity.Description, 2048, allowNull: true, SqlDbType.NVarChar);
		BindString("@Domain", identity.Domain, 512, allowNull: false, SqlDbType.NVarChar);
		BindString("@AccountName", identity.AccountName, 512, allowNull: false, SqlDbType.NVarChar);
		BindString("@DistinguishedName", identity.DistinguishedName, 2048, allowNull: false, SqlDbType.NVarChar);
		BindDateTime("@CurrentUtcTime", DateTime.UtcNow);
		BindGuidType("@session_id", Guid.NewGuid());
		try
		{
			ExecuteNonQuery();
		}
		catch (DatabaseException)
		{
		}
	}

	private static string GetUseStatement(string databaseName)
	{
		return string.Format(CultureInfo.InvariantCulture, "USE [{0}] ", databaseName);
	}

	public Role QueryRoleByName(string name)
	{
		PrepareStoredProcedure("security_query_role_by_name");
		BindNameType("@Name", name);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		if (!databaseDataReader.Read())
		{
			return null;
		}
		return RoleColumns.Bind(databaseDataReader);
	}

	public Role QueryRoleById(string roleId)
	{
		PrepareStoredProcedure("security_query_role_by_id");
		BindGuidType("@IDRole", new Guid(roleId));
		DatabaseDataReader databaseDataReader = ExecuteReader();
		if (!databaseDataReader.Read())
		{
			return null;
		}
		return RoleColumns.Bind(databaseDataReader);
	}

	public string[] QueryRoleMembers(string roleId)
	{
		PrepareStoredProcedure("security_query_role_members");
		BindGuidType("@IDRole", new Guid(roleId), allowNullOrEmpty: false);
		BindBoolean("@IncludeDynamicallyAddedIdentities", parameterValue: false);
		DatabaseDataReader databaseDataReader = ExecuteReader();
		List<string> list = new List<string>();
		while (databaseDataReader.Read())
		{
			list.Add(RoleMemberColumns.Bind(databaseDataReader));
		}
		return list.ToArray();
	}

	public Identity ReadIdentityFromSource(string roleId)
	{
		Role role = null;
		string[] members = new string[0];
		if (SidUtility.IsRoleIdentity(roleId))
		{
			role = QueryRoleById(roleId);
			if (role == null)
			{
				return null;
			}
			members = QueryRoleMembers(roleId);
		}
		return IdentityAccessor.ReadIdentityFromSource(roleId, role, members);
	}

	private static string GetUnderlyingSQLAliasHostname(string alias)
	{
		string text = Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\MSSQLServer\\Client\\ConnectTo", alias, string.Empty) as string;
		return (string.IsNullOrEmpty(text) ? alias : text.Substring(text.IndexOf(',') + 1)).Split(new char[2] { '\\', ',' }, StringSplitOptions.RemoveEmptyEntries).First();
	}
}
