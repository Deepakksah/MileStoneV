using System;
using System.Data;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.SharedComponent;
using VideoOS.SqlUpdate.Core.Common;
using VideoOS.SqlUpdate.Core.Providers;
using VideoOS.SqlUpdate.Core.Services;
using VideoOS.Utilities.Database;

namespace VideoOS.Corporate.Database;

public class CorporateDatabaseSkuProvider : IDatabaseSkuProvider
{
	private ILogger _logger { get; }

	public CorporateDatabaseSkuProvider(ILogger logger)
	{
		_logger = logger;
	}

	public VmsDbType GetSku(DatabaseConnection databaseConnection, string masterKey)
	{
		LoggerExtensions.LogInformation(_logger, "Loading SKU from database", Array.Empty<object>());
		try
		{
			using DatabaseCommand databaseCommand = databaseConnection.CreateCommand();
			databaseCommand.CommandType = CommandType.Text;
			databaseCommand.CommandText = "SELECT [Data] FROM [dbo].[License] WHERE IDLicense = '94F58776-84BD-44DB-B19B-851F1D53A832'";
			using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return VmsDbType.Unknown;
			}
			string text = string.Empty;
			if (databaseDataReader.GetFieldType(0) == typeof(byte[]))
			{
				if (string.IsNullOrEmpty(masterKey))
				{
					LoggerExtensions.LogInformation(_logger, "No master key in newer system, returning unknown.", Array.Empty<object>());
					return VmsDbType.Unknown;
				}
				using MemoryStream memoryStream = new MemoryStream();
				using Stream stream = databaseDataReader.GetStream(0);
				stream.CopyTo(memoryStream);
				byte[] bytes = new CorporateEncryptionSymmetric(masterKey).DecryptSequence(memoryStream.ToArray());
				text = Encoding.UTF8.GetString(bytes);
			}
			else
			{
				LoggerExtensions.LogInformation(_logger, "table has a older format.", Array.Empty<object>());
				byte[] array = Convert.FromBase64String(databaseDataReader.GetString(0));
				if (string.IsNullOrEmpty(masterKey))
				{
					LoggerExtensions.LogInformation(_logger, "Get SKU from license using legacy option.", Array.Empty<object>());
					text = new EncryptionHelper(EncryptionConfigurationHelper.PasswordEncryptionConfiguration).DecryptStringData(array);
				}
				else
				{
					byte[] bytes2 = new CorporateEncryptionSymmetric(masterKey).DecryptSequence(array);
					text = Encoding.Unicode.GetString(bytes2);
				}
			}
			if (string.IsNullOrEmpty(text))
			{
				LoggerExtensions.LogInformation(_logger, "no decryptedData returning unknown", Array.Empty<object>());
				return VmsDbType.Unknown;
			}
			return VmsDbTypeMapper.Map(GetSkuFromLicenseData(text));
		}
		catch (DatabaseException ex) when (ex.IsObjectNotFoundException)
		{
			LoggerExtensions.LogInformation(_logger, "Failed to get license information from database. Licsense table does not exist", Array.Empty<object>());
			LoggerExtensions.LogDebug(_logger, ex.ToString(), Array.Empty<object>());
		}
		return VmsDbType.Unknown;
	}

	private string GetSkuFromLicenseData(string decryptedLicenseData)
	{
		try
		{
			return LicenseFactory.CreateLicenseReader().GetSkuFromLicense(decryptedLicenseData);
		}
		catch (XmlException ex)
		{
			LoggerExtensions.LogError(_logger, "Unable to load license information {Exception}", new object[1] { ex });
			LoggerExtensions.LogDebug(_logger, ex.ToString(), Array.Empty<object>());
		}
		return string.Empty;
	}
}
