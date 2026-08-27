using System;
using System.Data;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.Admin.Tasks;

public class CreateLicenseAsymmetricKeyTask
{
	public ILogger LogComponent { get; public set; }

	public CreateLicenseAsymmetricKeyTask(ILogger logComponent)
	{
		LogComponent = logComponent;
	}

	public void Create(IDatabaseConnectionInfo connectionInfoMs, string masterKey)
	{
		string text = GenerateRSAKey();
		if (!string.IsNullOrEmpty(text))
		{
			Create(connectionInfoMs, masterKey, text);
		}
	}

	public void Create(IDatabaseConnectionInfo connectionInfoMs, string masterKey, string rsaKey)
	{
		if (string.IsNullOrEmpty(rsaKey))
		{
			throw new ArgumentNullException("rsaKey");
		}
		InsertKeyInDatabase(connectionInfoMs, masterKey, rsaKey);
	}

	private void InsertKeyInDatabase(IDatabaseConnectionInfo connectionInfoMs, string masterKey, string rsaKey)
	{
		SqlConnectionManager sqlConnectionManager = new SqlConnectionManager(connectionInfoMs, LogComponent);
		try
		{
			sqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection sqlConnection)
			{
				bool flag = false;
				using (DatabaseCommand databaseCommand = sqlConnection.CreateCommand("SELECT 1 FROM [SystemSettings] WHERE [Name] = 'LicenseRsaKey'"))
				{
					using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader(CommandBehavior.SingleResult);
					if (databaseDataReader.HasRows)
					{
						flag = true;
						LoggerExtensions.LogDebug(LogComponent, CreateLicenseAsymmetricKeyTask_Resources.KeyAlreadyExists, Array.Empty<object>());
					}
				}
				if (!flag)
				{
					using (DatabaseCommand databaseCommand2 = sqlConnection.CreateCommand())
					{
						databaseCommand2.CommandType = CommandType.Text;
						databaseCommand2.CommandText = "INSERT INTO SystemSettings([LastModified], [Name], [Setting]) VALUES (GETUTCDATE(), 'LicenseRsaKey', @Setting)";
						databaseCommand2.AddParameter("@Setting", SqlDbType.NVarChar, 0, EncryptionFactory.CreateSymmetric(masterKey).EncryptToBase64String(rsaKey));
						databaseCommand2.ExecuteNonQuery();
						LoggerExtensions.LogDebug(LogComponent, CreateLicenseAsymmetricKeyTask_Resources.NewKeyInserted, Array.Empty<object>());
					}
				}
			});
		}
		catch (Exception)
		{
			LoggerExtensions.LogDebug(LogComponent, CreateLicenseAsymmetricKeyTask_Resources.FailedToInsertKey, Array.Empty<object>());
		}
	}

	private static string GenerateRSAKey()
	{
		using IAsymmetricEncryption asymmetricEncryption = EncryptionFactory.CreateAsymmetricForLicense();
		return asymmetricEncryption.KeyInformationAsBase64;
	}
}
