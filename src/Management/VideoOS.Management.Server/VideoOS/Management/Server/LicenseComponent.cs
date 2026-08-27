using System;
using VideoOS.Common.Integration.LicenseInfo;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class LicenseComponent : GeminiResourceComponent, ILicenseComponent, IDisposable
{
	private static class LicenseModifiedColumns
	{
		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static DateTime Bind(DatabaseDataReader reader)
		{
			return LastModified.GetDateTime(reader);
		}
	}

	private static class LicenseColumns
	{
		private static DatabaseColumnBinder Data = new DatabaseColumnBinder("Data");

		public static ILicenseDocument Bind(DatabaseDataReader reader, IGeminiRequestContext requestContext)
		{
			byte[] result = Data.GetBytesAsync(reader).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
				.GetResult();
			byte[] utf8LicenseBytes = requestContext.GetEncryptionService().Decrypt(result);
			return LicenseFactory.CreateLicenseLoader().CreateLicenseDocument(utf8LicenseBytes);
		}
	}

	public DateTime QueryLicenseModifiedTime()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_license_modified");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			DateTime result = DateTime.MinValue;
			while (databaseDataReader.Read())
			{
				result = LicenseModifiedColumns.Bind(databaseDataReader);
			}
			return result;
		});
	}

	public ILicenseDocument QueryLicenseDocument()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("admin_query_license_document");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			ILicenseDocument result = null;
			while (databaseDataReader.Read())
			{
				result = LicenseColumns.Bind(databaseDataReader, RequestContext);
			}
			return result;
		});
	}

	public ILicenseInfo QueryLicenseItemById(Guid id)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_license_by_id");
			BindUniqueIdentifier("@IDLicense", id);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			LicenseRow licenseRow = null;
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("Data");
			DatabaseColumnBinder databaseColumnBinder2 = new DatabaseColumnBinder("LastModified");
			if (databaseDataReader.Read())
			{
				licenseRow = new LicenseRow();
				byte[] result = databaseColumnBinder.GetBytesAsync(databaseDataReader).ConfigureAwait(continueOnCapturedContext: false).GetAwaiter()
					.GetResult();
				licenseRow.Data = RequestContext.GetEncryptionService().Decrypt(result);
				licenseRow.LastModified = databaseColumnBinder2.GetDateTime(databaseDataReader);
				licenseRow.Id = id;
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return licenseRow;
		});
	}

	public void SafelyAddOrUpdateLicenseItem(Guid id, DateTime previousLastModified, byte[] data)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("add_or_update_lic_safely");
			BindUniqueIdentifier("@IDLicense", id);
			BindDateTime("@PreviousLastModified", previousLastModified);
			byte[] parameterValue = RequestContext.GetEncryptionService().Encrypt(data);
			BindByteArray("@Data", parameterValue, allowNull: false);
			ExecuteNonQuery();
		});
	}
}
