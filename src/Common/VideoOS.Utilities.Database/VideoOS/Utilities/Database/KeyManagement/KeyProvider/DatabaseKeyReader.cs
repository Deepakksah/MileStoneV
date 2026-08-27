using System;

namespace VideoOS.Utilities.Database.KeyManagement.KeyProvider;

public class DatabaseKeyReader : IDatabaseKeyReader
{
	public static readonly string MasterKeyId = "49c1d447-0c9f-4814-8c34-73937fe87073";

	protected readonly ISqlConnectionManager _sqlConnectionManager;

	public DatabaseKeyReader(ISqlConnectionManager sqlConnectionManager)
	{
		if (sqlConnectionManager == null)
		{
			throw new ArgumentNullException("sqlConnectionManager");
		}
		_sqlConnectionManager = sqlConnectionManager;
	}

	private static byte[] DecodeKey(string encodedKey)
	{
		return Convert.FromBase64String(encodedKey);
	}

	public byte[] ReadKey()
	{
		return TryReadKey() ?? throw new KeyStorageException(KeyStorageErrorCode.KeyIsMissingFromDatabaseOnRead, "Key is missing from the database when trying to read it.");
	}

	public bool HasKey()
	{
		return TryReadKey() != null;
	}

	public byte[] TryReadKey()
	{
		try
		{
			return _sqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
			{
				using DatabaseCommand databaseCommand = connection.CreateCommand("SELECT [Key] FROM [dbo].[EncryptionKeys] WHERE [Id] = @Id");
				databaseCommand.AddParameter(new DatabaseParameter("@Id", MasterKeyId));
				string text = (string)databaseCommand.ExecuteScalar();
				if (text == null)
				{
					return (byte[])null;
				}
				return DecodeKey(text);
			});
		}
		catch (DatabaseException ex)
		{
			if (ex.IsObjectNotFoundException)
			{
				return null;
			}
			throw;
		}
	}
}
