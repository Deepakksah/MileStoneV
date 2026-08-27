using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using Microsoft.Data.SqlClient;

namespace VideoOS.Utilities.Database;

public class DatabaseConnectionInfo : IDatabaseConnectionInfo
{
	public const string MasterDatabaseName = "master";

	public const string LocalHostDatabaseServerName = "localhost";

	public static readonly string InvalidDatabaseConnectionMessage = "Valid database connection string has not been configured";

	protected SqlConnectionStringBuilder _connectionStringBuilder;

	protected string _connectionString;

	protected string _applicationName;

	public string ServerName
	{
		get
		{
			Validate();
			return _connectionStringBuilder.DataSource;
		}
	}

	public string DatabaseName
	{
		get
		{
			Validate();
			return _connectionStringBuilder.InitialCatalog;
		}
	}

	public virtual string ConnectionString => _connectionString;

	public string ApplicationName => _applicationName;

	public DbConnectionStringBuilder ConnectionStringBuilder
	{
		get
		{
			Validate();
			return (DbConnectionStringBuilder)(object)_connectionStringBuilder;
		}
	}

	public AuthenticationType AuthenticationType
	{
		get
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Invalid comparison between Unknown and I4
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Invalid comparison between Unknown and I4
			Validate();
			if ((int)_connectionStringBuilder.Authentication == 3)
			{
				return AuthenticationType.MicrosoftEntraIntegrated;
			}
			if ((int)_connectionStringBuilder.Authentication == 7)
			{
				return AuthenticationType.MicrosoftEntraManagedIdentity;
			}
			if (_connectionStringBuilder.IntegratedSecurity)
			{
				if (!_connectionStringBuilder.TrustServerCertificate)
				{
					return AuthenticationType.IntegratedSecurity;
				}
				return AuthenticationType.IntegratedSecurityTrusted;
			}
			throw new InvalidConnectionStringException(InvalidDatabaseConnectionMessage);
		}
	}

	protected DatabaseConnectionInfo()
	{
	}

	public DatabaseConnectionInfo(string connectionString)
	{
		_connectionString = connectionString;
	}

	public DatabaseConnectionInfo(string connectionString, string applicationName)
	{
		_connectionString = connectionString;
		_applicationName = applicationName;
	}

	public DatabaseConnectionInfo(string databaseServerName, string databaseName, AuthenticationType authenticationType)
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Expected O, but got Unknown
		if (string.IsNullOrEmpty(databaseServerName))
		{
			throw new ArgumentNullException("databaseServerName");
		}
		_connectionStringBuilder = new SqlConnectionStringBuilder
		{
			DataSource = databaseServerName,
			Encrypt = SqlConnectionEncryptOption.op_Implicit(true)
		};
		switch (authenticationType)
		{
		case AuthenticationType.IntegratedSecurity:
			_connectionStringBuilder.IntegratedSecurity = true;
			_connectionStringBuilder.TrustServerCertificate = false;
			break;
		case AuthenticationType.IntegratedSecurityTrusted:
			_connectionStringBuilder.IntegratedSecurity = true;
			_connectionStringBuilder.TrustServerCertificate = true;
			break;
		case AuthenticationType.MicrosoftEntraIntegrated:
			_connectionStringBuilder.Authentication = (SqlAuthenticationMethod)3;
			_connectionStringBuilder.PersistSecurityInfo = true;
			_connectionStringBuilder.TrustServerCertificate = false;
			break;
		case AuthenticationType.MicrosoftEntraManagedIdentity:
			_connectionStringBuilder.Authentication = (SqlAuthenticationMethod)7;
			_connectionStringBuilder.PersistSecurityInfo = true;
			_connectionStringBuilder.TrustServerCertificate = false;
			break;
		default:
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "Authentication type {0} not supported", authenticationType));
		}
		if (!string.IsNullOrEmpty(databaseName))
		{
			_connectionStringBuilder.InitialCatalog = databaseName;
		}
		_connectionString = ((DbConnectionStringBuilder)(object)_connectionStringBuilder).ConnectionString;
	}

	public void Validate()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		if (string.IsNullOrEmpty(ConnectionString))
		{
			throw new InvalidConnectionStringException(InvalidDatabaseConnectionMessage);
		}
		if (_connectionStringBuilder != null)
		{
			return;
		}
		try
		{
			_connectionStringBuilder = new SqlConnectionStringBuilder(ConnectionString);
			if (!string.IsNullOrEmpty(_applicationName))
			{
				_connectionStringBuilder.ApplicationName = _applicationName;
				_connectionString = ((DbConnectionStringBuilder)(object)_connectionStringBuilder).ConnectionString;
			}
		}
		catch (Exception ex) when (ex is ArgumentException || ex is KeyNotFoundException || ex is FormatException)
		{
			throw new InvalidConnectionStringException(InvalidDatabaseConnectionMessage);
		}
	}

	public IDatabaseConnectionInfo CloneWithNewDatabaseName(string newDatabaseName)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Expected O, but got Unknown
		return new DatabaseConnectionInfo(((DbConnectionStringBuilder)new SqlConnectionStringBuilder(ConnectionString)
		{
			InitialCatalog = newDatabaseName
		}).ConnectionString);
	}

	public IDatabaseConnectionInfo CloneWithMasterDatabaseName()
	{
		return CloneWithNewDatabaseName("master");
	}
}
