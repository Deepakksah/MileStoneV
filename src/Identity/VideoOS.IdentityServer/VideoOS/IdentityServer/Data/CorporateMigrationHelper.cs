using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using VideoOS.IdentityServer.Data.Migrations.XPCO;

namespace VideoOS.IdentityServer.Data;

public class CorporateMigrationHelper
{
	public class MigratedAdminIdentity
	{
		public string Identity { get; set; }
	}

	public class MigratedBasicUser
	{
		public string Id { get; set; }

		public string Sid { get; set; }

		public string UserName { get; set; }

		public byte[] PasswordHash { get; set; }

		public string PasswordHashString
		{
			get
			{
				if (PasswordHash != null)
				{
					return Convert.ToBase64String(PasswordHash);
				}
				return null;
			}
		}

		public string Salt { get; set; }

		public string EncryptionMethod { get; set; }

		public string Description { get; set; }

		public bool CanChangePassword { get; set; }

		public DateTime CreatedDate { get; set; }

		public DateTime ModifiedDate { get; set; }
	}

	private readonly DbContext _context;

	public CorporateMigrationHelper(DbContext context)
	{
		_context = context;
	}

	public async Task<bool> TableExists(string tableName)
	{
		string sql = "SET @Exists = (SELECT 1 FROM sys.tables AS T\r\n                        INNER JOIN sys.schemas AS S ON T.schema_id = S.schema_id\r\n                        WHERE S.Name = 'dbo' AND T.Name = @TableName)";
		SqlParameter exists = new SqlParameter("@Exists", SqlDbType.Bit)
		{
			Direction = ParameterDirection.Output
		};
		SqlParameter sqlParameter = new SqlParameter("@TableName", tableName);
		await _context.Database.ExecuteSqlRawAsync(sql, exists, sqlParameter);
		return exists.Value as bool? == true;
	}

	public Task DropTempCorporateBasicUsers()
	{
		return DropTable("TempCorporateBasicUsers");
	}

	public Task DropTempAdminIdentities()
	{
		return DropTable("TempCorporateAdminIdentities");
	}

	public Task DropSecurityKeys()
	{
		return DropTable(TableNames.SecurityKeys);
	}

	private async Task DropTable(string tableName)
	{
		string sql = "DROP TABLE " + tableName;
		await _context.Database.ExecuteSqlRawAsync(sql);
	}

	public async Task<List<MigratedBasicUser>> GetMissingBasicUsers()
	{
		string query = "SELECT [Id], [Sid], [Username], [PasswordHash], [SaltValue], [EncryptionMethod], [Description], [CanChangePassword], [CreatedDate], [ModifiedDate]  FROM [dbo].[TempCorporateBasicUsers] WHERE [Username] NOT IN \r\n                    (\r\n\t                    SELECT [dbo].[AspNetUsers].[UserName]\r\n\t                    FROM [dbo].[AspNetUsers]\r\n                    )";
		return await RawSqlQuery(query, ReadBasicUser);
	}

	public async Task<List<MigratedAdminIdentity>> GetMissingAdminIdentities()
	{
		string query = "SELECT [identity] FROM [dbo].[TempCorporateAdminIdentities] WHERE [identity] NOT IN \r\n                    (\r\n\t                    SELECT [dbo].[AdminIdentities].[Identity]\r\n\t                    FROM [dbo].[AdminIdentities]\r\n                    )";
		return await RawSqlQuery(query, ReadAdminIdentity);
	}

	public async Task<List<SecurityKeyDataModel>> GetMissingSecurityKeys()
	{
		string query = "SELECT * FROM [dbo].[" + TableNames.SecurityKeys + "]";
		return await RawSqlQuery(query, ReadSecurityKey);
	}

	private MigratedBasicUser ReadBasicUser(DbDataReader reader)
	{
		MigratedBasicUser migratedBasicUser = new MigratedBasicUser();
		migratedBasicUser.Id = reader.GetString("Id");
		migratedBasicUser.Sid = reader.GetString("Sid");
		migratedBasicUser.UserName = reader.GetString("Username");
		migratedBasicUser.PasswordHash = (byte[])reader["PasswordHash"];
		migratedBasicUser.Salt = reader.GetString("SaltValue");
		migratedBasicUser.EncryptionMethod = reader.GetString("EncryptionMethod");
		migratedBasicUser.Description = reader.GetString("Description");
		migratedBasicUser.CanChangePassword = reader.GetBoolean("CanChangePassword");
		migratedBasicUser.CreatedDate = reader.GetDateTime("CreatedDate");
		migratedBasicUser.ModifiedDate = reader.GetDateTime("ModifiedDate");
		return migratedBasicUser;
	}

	private MigratedAdminIdentity ReadAdminIdentity(DbDataReader reader)
	{
		MigratedAdminIdentity migratedAdminIdentity = new MigratedAdminIdentity();
		migratedAdminIdentity.Identity = reader.GetString("identity");
		return migratedAdminIdentity;
	}

	private SecurityKeyDataModel ReadSecurityKey(DbDataReader reader)
	{
		return new SecurityKeyDataModel
		{
			Id = reader.GetInt32("Id"),
			KeyId = reader.GetString("KeyId"),
			Expiration = ParseValue<DateTime?>(reader, "Expiration"),
			Created = reader.GetDateTime("Created"),
			D = (byte[])reader["D"],
			DP = (byte[])reader["DP"],
			DQ = (byte[])reader["DQ"],
			Exponent = (byte[])reader["Exponent"],
			InverseQ = (byte[])reader["InverseQ"],
			Modulus = (byte[])reader["Modulus"],
			P = (byte[])reader["P"],
			Q = (byte[])reader["Q"]
		};
	}

	private static T ParseValue<T>(IDataRecord reader, string columnName)
	{
		if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
		{
			return (T)reader.GetValue(reader.GetOrdinal(columnName));
		}
		return default(T);
	}

	public Task AddAdminIdentity(List<string> identities)
	{
		identities = identities.Select((string x) => "('" + x + "')").ToList();
		string text = string.Join(",", identities);
		string sql = "INSERT INTO [dbo].[AdminIdentities]([Identity]) Values" + text;
		return _context.Database.ExecuteSqlRawAsync(sql);
	}

	private async Task<List<T>> RawSqlQuery<T>(string query, Func<DbDataReader, T> map, params SqlParameter[] sqlParameters)
	{
		List<T> result2;
		await using (DbCommand cmd = _context.Database.GetDbConnection().CreateCommand())
		{
			cmd.CommandText = query;
			cmd.CommandType = CommandType.Text;
			cmd.Parameters.AddRange(sqlParameters);
			List<T> list2;
			try
			{
				if (cmd.Connection.State != ConnectionState.Open)
				{
					await _context.Database.OpenConnectionAsync();
				}
				List<T> list;
				await using (DbDataReader result = await cmd.ExecuteReaderAsync())
				{
					List<T> entities = new List<T>();
					while (await result.ReadAsync())
					{
						entities.Add(map(result));
					}
					list = entities;
				}
				list2 = list;
			}
			finally
			{
				if (cmd.Connection.State == ConnectionState.Open)
				{
					await _context.Database.CloseConnectionAsync();
				}
			}
			result2 = list2;
		}
		return result2;
	}
}
