using System;

namespace VideoOS.Utilities.Database.KeyManagement.KeyProvider;

public class DatabaseKeyProvider : DatabaseKeyReader, IDatabaseKeyProvider, IDatabaseKeyReader
{
	public DatabaseKeyProvider(ISqlConnectionManager sqlConnectionManager)
		: base(sqlConnectionManager)
	{
	}

	public void UpdateKey(byte[] key)
	{
		if (TryReadKey() == null)
		{
			throw new KeyStorageException(KeyStorageErrorCode.KeyIsMissingFromDatabaseOnUpdating, "Key is missing from the database when trying to update it.");
		}
		_sqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
		{
			using DatabaseCommand databaseCommand = connection.CreateCommand("UPDATE [dbo].[EncryptionKeys] SET [Key] = @Key WHERE [Id] = @Id");
			string value = EncodeKey(key);
			databaseCommand.AddParameter(new DatabaseParameter("@Key", value));
			databaseCommand.AddParameter(new DatabaseParameter("@Id", DatabaseKeyReader.MasterKeyId));
			databaseCommand.ExecuteNonQuery();
		});
	}

	private static string EncodeKey(byte[] key)
	{
		return Convert.ToBase64String(key);
	}

	public void InsertKey(byte[] key)
	{
		_sqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection connection)
		{
			if (!TableExists(connection))
			{
				CreateEncryptionKeysTable(connection);
			}
			InsertKey(connection, EncodeKey(key));
		});
	}

	private static bool TableExists(DatabaseConnection connection)
	{
		try
		{
			using DatabaseCommand databaseCommand = connection.CreateCommand("SELECT [Key] FROM [dbo].[EncryptionKeys] WHERE [Id] = @Id");
			databaseCommand.AddParameter(new DatabaseParameter("@Id", DatabaseKeyReader.MasterKeyId));
			databaseCommand.ExecuteScalar();
			return true;
		}
		catch (DatabaseException ex)
		{
			if (ex.IsObjectNotFoundException)
			{
				return false;
			}
			throw;
		}
	}

	public static void CreateEncryptionKeysTable(DatabaseConnection connection)
	{
		string commandText = "CREATE TABLE [dbo].[EncryptionKeys](\r\n    [Id] [char](36) NOT NULL,\r\n    [Key] [nvarchar](512) NOT NULL,\r\n    CONSTRAINT [PK_EncryptionKeys] PRIMARY KEY CLUSTERED ([Id] ASC)\r\n)";
		using DatabaseCommand databaseCommand = connection.CreateCommand();
		databaseCommand.CommandText = commandText;
		databaseCommand.ExecuteNonQuery();
	}

	public static void InsertKey(DatabaseConnection connection, string key)
	{
		using DatabaseCommand databaseCommand = connection.CreateCommand("INSERT [dbo].[EncryptionKeys] ([Id], [Key]) VALUES (@Id, @Key)");
		databaseCommand.AddParameter(new DatabaseParameter("@Id", DatabaseKeyReader.MasterKeyId));
		databaseCommand.AddParameter(new DatabaseParameter("@Key", key));
		databaseCommand.ExecuteNonQuery();
	}
}
