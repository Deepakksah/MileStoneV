using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

public sealed class TokenSqlData : ITokenSqlData
{
	private static class SecurityTokenDataColumns
	{
		private static DatabaseColumnBinder TokenId = new DatabaseColumnBinder("TokenId");

		private static DatabaseColumnBinder SID = new DatabaseColumnBinder("SID");

		private static DatabaseColumnBinder Expires = new DatabaseColumnBinder("Expires");

		private static DatabaseColumnBinder RegistrationTime = new DatabaseColumnBinder("RegistrationTime");

		private static DatabaseColumnBinder ManagementClientToken = new DatabaseColumnBinder("ManagementClientToken");

		public static SecurityToken Bind(DatabaseDataReader reader, Func<string, Identity> identityProvider)
		{
			string token = TokenId.GetString(reader);
			string arg = SID.GetString(reader);
			TimeSpan expires = TimeSpan.FromMilliseconds(Expires.GetInt64(reader));
			DateTime dateTime = RegistrationTime.GetDateTime(reader);
			bool boolean = ManagementClientToken.GetBoolean(reader);
			Identity identity = identityProvider(arg);
			if (identity != null)
			{
				return new SecurityToken(identity, token, dateTime, expires, boolean);
			}
			return null;
		}
	}

	private readonly Func<string, Identity> identityProvider;

	private readonly ILogger logger;

	public TokenSqlData(Func<string, Identity> identityProvider, ILogger logger)
	{
		this.identityProvider = identityProvider;
		this.logger = logger;
	}

	public SecurityToken AddToken(string sid, TimeSpan timeout)
	{
		SecurityToken result = null;
		try
		{
			result = Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection conn)
			{
				using (DatabaseCommand databaseCommand = conn.CreateCommand("add_token"))
				{
					databaseCommand.CommandType = CommandType.StoredProcedure;
					databaseCommand.AddParameter("@SID", SqlDbType.NVarChar, sid);
					databaseCommand.AddParameter("@Expires", SqlDbType.BigInt, Convert.ToInt64(timeout.TotalMilliseconds));
					databaseCommand.AddParameter("@ManagementClientToken", SqlDbType.Bit, false);
					using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
					if (databaseDataReader.Read())
					{
						SecurityToken securityToken = SecurityTokenDataColumns.Bind(databaseDataReader, identityProvider);
						if (securityToken != null && securityToken.Identity != null)
						{
							return securityToken;
						}
					}
				}
				return (SecurityToken)null;
			});
		}
		catch (DatabaseException ex)
		{
			logger.LogWarning("Unable to add token to the database. Error: {ExceptionMessage}", ex.Message);
		}
		return result;
	}

	public bool GetAllTokens(out SecurityToken[] result)
	{
		try
		{
			result = Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection conn)
			{
				List<SecurityToken> list = new List<SecurityToken>();
				using DatabaseCommand databaseCommand = conn.CreateCommand("query_all_tokens");
				databaseCommand.CommandType = CommandType.StoredProcedure;
				using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
				while (databaseDataReader.Read())
				{
					SecurityToken securityToken = SecurityTokenDataColumns.Bind(databaseDataReader, identityProvider);
					if (securityToken != null && securityToken.Identity != null)
					{
						list.Add(securityToken);
					}
				}
				return list.ToArray();
			});
		}
		catch (DatabaseException ex)
		{
			logger.LogWarning("Unable to get all tokens from the database. Error: {ExceptionMessage}", ex.Message);
			result = null;
			return false;
		}
		return true;
	}

	public SecurityToken GetSecurityToken(string token)
	{
		try
		{
			return Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection conn)
			{
				using (DatabaseCommand databaseCommand = conn.CreateCommand("query_token_by_id"))
				{
					databaseCommand.CommandType = CommandType.StoredProcedure;
					databaseCommand.AddParameter("@TokenId", SqlDbType.NVarChar, token);
					using DatabaseDataReader databaseDataReader = databaseCommand.ExecuteReader();
					if (databaseDataReader.Read())
					{
						SecurityToken securityToken = SecurityTokenDataColumns.Bind(databaseDataReader, identityProvider);
						if (securityToken != null && securityToken.Identity != null)
						{
							return securityToken;
						}
					}
				}
				return (SecurityToken)null;
			});
		}
		catch (DatabaseException ex)
		{
			logger.LogWarning("Unable to get token from database. Error: {ExceptionMessage}", ex.Message);
		}
		return null;
	}

	public void DeleteToken(string token)
	{
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection conn)
			{
				using DatabaseCommand databaseCommand = conn.CreateCommand("delete_token");
				databaseCommand.CommandType = CommandType.StoredProcedure;
				databaseCommand.AddParameter("@TokenId", SqlDbType.NVarChar, token);
				databaseCommand.ExecuteNonQuery();
			});
		}
		catch (DatabaseException ex)
		{
			logger.LogWarning("Unable to delete token from the database. Error: {ExceptionMessage}", ex.Message);
		}
	}

	public void ClearExpiredTokens()
	{
		try
		{
			Server.Instance.SqlConnectionManager.ExecuteRetryable(delegate(DatabaseConnection conn)
			{
				using DatabaseCommand databaseCommand = conn.CreateCommand("clean_expired_tokens");
				databaseCommand.CommandType = CommandType.StoredProcedure;
				databaseCommand.ExecuteNonQuery();
			});
		}
		catch (DatabaseException ex)
		{
			logger.LogWarning("Unable clear expired tokens from the database. Error: {ExceptionMessage}", ex.Message);
		}
	}
}
