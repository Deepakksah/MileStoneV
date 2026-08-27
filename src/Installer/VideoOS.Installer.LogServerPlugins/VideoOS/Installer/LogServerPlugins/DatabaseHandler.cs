using System;
using System.Data;
using System.Globalization;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Database.Authentication;

namespace VideoOS.Installer.LogServerPlugins;

public sealed class DatabaseHandler : IDatabaseHandler
{
	private readonly ISqlConnectionManager _sqlConnectionManager;

	private readonly AuthenticationHandler _authenticationHandler;

	public const string MinimumDatabaseVersion = "1.0.0";

	private const string AnonymousLoggingName = "AnonymousLogging";

	public DatabaseHandler(ISqlConnectionManager sqlConnectionManager, AuthenticationHandler authenticationHandler)
	{
		if (sqlConnectionManager == null)
		{
			throw new ArgumentNullException("sqlConnectionManager");
		}
		if (authenticationHandler == null)
		{
			throw new ArgumentNullException("authenticationHandler");
		}
		_sqlConnectionManager = sqlConnectionManager;
		_authenticationHandler = authenticationHandler;
	}

	public void SetDatabaseOwner(string databaseName, string accountName)
	{
		_authenticationHandler.SetDatabaseOwner(accountName, GetComputerDomain(), null, DomainHelper.IsNetworkService(accountName), canUseSid: false);
	}

	public void SetAnonymousLogging(bool enabled)
	{
		_sqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
		{
			using DatabaseCommand databaseCommand = connection.CreateCommand();
			databaseCommand.CommandText = "LogServer.WriteConfiguration";
			databaseCommand.CommandType = CommandType.StoredProcedure;
			using DataTable dataTable = new DataTable("LogServer.ConfigurationTable")
			{
				Locale = CultureInfo.InvariantCulture,
				Columns = { "Name", "Value" }
			};
			dataTable.Rows.Add("AnonymousLogging", enabled.ToString());
			databaseCommand.AddParameter(new DatabaseParameter
			{
				ParameterName = "@Configurations",
				Value = dataTable,
				SqlDbType = SqlDbType.Structured,
				TypeName = dataTable.TableName
			});
			databaseCommand.ExecuteNonQuery();
		});
	}

	private static string GetComputerDomain()
	{
		string text = null;
		string computerNameEx = NativeMethods.GetComputerNameEx(NativeMethods.ComputerNameFormat.ComputerNameDnsDomain);
		if (computerNameEx != null)
		{
			text = NativeMethods.DnsHostnameToComputerName(computerNameEx);
		}
		return text ?? Environment.MachineName;
	}
}
