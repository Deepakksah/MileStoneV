using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class RegisteredClaimComponent : GeminiResourceComponent, IRegisteredClaimComponent, IDisposable
{
	private static class RegisteredClaimDataColumns
	{
		private static DatabaseColumnBinder Id = new DatabaseColumnBinder("IDClaim");

		private static DatabaseColumnBinder LoginProviderId = new DatabaseColumnBinder("LoginProviderId");

		private static DatabaseColumnBinder Name = new DatabaseColumnBinder("ClaimName");

		private static DatabaseColumnBinder DisplayName = new DatabaseColumnBinder("DisplayName");

		private static DatabaseColumnBinder CaseSensitive = new DatabaseColumnBinder("CaseSensitive");

		public static RegisteredClaimData Bind(DatabaseDataReader reader)
		{
			return new RegisteredClaimData
			{
				Id = Id.GetInt32(reader),
				LoginProviderId = LoginProviderId.GetGuid2(reader),
				Name = Name.GetString(reader),
				DisplayName = DisplayName.GetString(reader),
				CaseSensitive = CaseSensitive.GetBoolean(reader)
			};
		}
	}

	public RegisteredClaimData Create(Guid loginProviderId, string name, string displayName, bool caseSensitive)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_claim_create");
			BindUniqueIdentifier("@loginProviderId", loginProviderId);
			BindString("@name", name, 450, allowNull: false, SqlDbType.NVarChar);
			BindString("@displayName", displayName, 128, allowNull: false, SqlDbType.NVarChar);
			BindBoolean("@caseSensitive", caseSensitive);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				throw new DatabaseConfigMissingRowException();
			}
			RegisteredClaimData result = RegisteredClaimDataColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public int[] QueryIdsByLoginProvider(Guid loginProviderId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_claim_query_ids_by_provider");
			BindUniqueIdentifier("@loginProviderId", loginProviderId);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			DatabaseColumnBinder databaseColumnBinder = new DatabaseColumnBinder("IDClaim");
			List<int> list = new List<int>();
			while (databaseDataReader.Read())
			{
				list.Add(databaseColumnBinder.GetInt32(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public RegisteredClaimData QueryById(int id)
	{
		return QueryByIds(new int[1] { id }).FirstOrDefault();
	}

	public RegisteredClaimData[] QueryByIds(IEnumerable<int> ids)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_claim_query_by_ids");
			BindIdTable("@ids", ids);
			return ExecuteAndReadArray();
		});
	}

	public RegisteredClaimData[] QueryAll()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_claim_query");
			return ExecuteAndReadArray();
		});
	}

	private RegisteredClaimData[] ExecuteAndReadArray()
	{
		DatabaseDataReader databaseDataReader = ExecuteReader();
		List<RegisteredClaimData> list = new List<RegisteredClaimData>();
		while (databaseDataReader.Read())
		{
			list.Add(RegisteredClaimDataColumns.Bind(databaseDataReader));
		}
		if (databaseDataReader.NextResult())
		{
			throw new SecurityDatabaseConfigExtraRowsException();
		}
		return list.ToArray();
	}

	public void Update(int id, string name, string displayName)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_claim_update");
			BindInt("@IDClaim", id);
			BindString("@name", name, 450, allowNull: true, SqlDbType.NVarChar);
			BindString("@displayName", displayName, 128, allowNull: true, SqlDbType.NVarChar);
			ExecuteNonQuery();
		});
	}

	public void Delete(int id, Guid sessionId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_claim_delete");
			BindInt("@IDClaim", id);
			BindGuidType("@sessionId", sessionId.ToString());
			ExecuteNonQuery();
		});
	}
}
