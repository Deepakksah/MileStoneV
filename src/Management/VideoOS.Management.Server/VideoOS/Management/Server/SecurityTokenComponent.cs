using System;
using System.Data;
using VideoOS.Common.Integration.Security;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class SecurityTokenComponent : GeminiResourceComponent, ISecurityTokenComponent, IDisposable
{
	private static class SecurityTokenColumns
	{
		private static DatabaseColumnBinder TokenId = new DatabaseColumnBinder("TokenId");

		private static DatabaseColumnBinder Sid = new DatabaseColumnBinder("SID");

		private static DatabaseColumnBinder Expires = new DatabaseColumnBinder("Expires");

		private static DatabaseColumnBinder RegistrationTime = new DatabaseColumnBinder("RegistrationTime");

		private static DatabaseColumnBinder ManagementClientToken = new DatabaseColumnBinder("ManagementClientToken");

		public static SecurityToken Bind(DatabaseDataReader reader, IGeminiRequestContext requestContext)
		{
			string token = TokenId.GetString(reader);
			string sid = Sid.GetString(reader);
			long @int = Expires.GetInt64(reader);
			DateTime dateTime = RegistrationTime.GetDateTime(reader);
			bool boolean = ManagementClientToken.GetBoolean(reader);
			return new SecurityToken(IdentityCache.ReadIdentityFromCache(requestContext, sid, QueryMembership.Expanded), token, dateTime, TimeSpan.FromMilliseconds(@int), boolean);
		}
	}

	public SecurityToken AddToken(string sid, TimeSpan timeout, bool managementClientToken)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("add_token");
			BindSIDType("@SID", sid);
			BindLong("@Expires", Convert.ToInt64(timeout.TotalMilliseconds));
			BindBoolean("@ManagementClientToken", managementClientToken);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (SecurityToken)null;
			}
			SecurityToken result = SecurityTokenColumns.Bind(databaseDataReader, RequestContext);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	public SecurityToken QueryToken(string tokenId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_token_by_id");
			BindString("@TokenId", tokenId, 255, allowNull: false, SqlDbType.NVarChar);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			return (!databaseDataReader.Read()) ? null : SecurityTokenColumns.Bind(databaseDataReader, RequestContext);
		});
	}
}
