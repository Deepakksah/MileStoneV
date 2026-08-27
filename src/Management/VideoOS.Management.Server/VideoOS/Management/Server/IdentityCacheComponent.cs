using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public sealed class IdentityCacheComponent : GeminiResourceComponent, IIdentityCacheComponent, IIdentityCacheDataProvider, IDisposable
{
	private static class MembershipColumns
	{
		private static DatabaseColumnBinder Container = new DatabaseColumnBinder("Container");

		private static DatabaseColumnBinder MemberSID = new DatabaseColumnBinder("MemberSID");

		public static Membership Bind(DatabaseDataReader reader)
		{
			return new Membership(Container.GetString(reader, allowNulls: false), MemberSID.GetString(reader, allowNulls: false));
		}
	}

	private static class IdentityColumns
	{
		private static DatabaseColumnBinder Sid = new DatabaseColumnBinder("SID");

		private static DatabaseColumnBinder Type = new DatabaseColumnBinder("Type");

		private static DatabaseColumnBinder DisplayName = new DatabaseColumnBinder("DisplayName");

		private static DatabaseColumnBinder Description = new DatabaseColumnBinder("Description");

		private static DatabaseColumnBinder Domain = new DatabaseColumnBinder("Domain");

		private static DatabaseColumnBinder AccountName = new DatabaseColumnBinder("AccountName");

		private static DatabaseColumnBinder DistinguishedName = new DatabaseColumnBinder("DistinguishedName");

		private static DatabaseColumnBinder Deleted = new DatabaseColumnBinder("Deleted");

		public static Identity Bind(DatabaseDataReader reader)
		{
			Identity identity = new Identity();
			identity.Sid = Sid.GetString(reader, allowNulls: false);
			identity.Type = (IdentityType)Type.GetByte(reader);
			identity.DisplayName = DisplayName.GetString(reader, allowNulls: false);
			identity.Description = Description.GetString(reader, allowNulls: true);
			if (identity.Description == null)
			{
				identity.Description = string.Empty;
			}
			identity.Domain = Domain.GetString(reader, allowNulls: true);
			if (identity.Domain == null)
			{
				identity.Domain = string.Empty;
			}
			identity.AccountName = AccountName.GetString(reader, allowNulls: false);
			identity.DistinguishedName = DistinguishedName.GetString(reader, allowNulls: false);
			identity.Deleted = Deleted.GetBoolean(reader);
			return identity;
		}
	}

	private static class UpdateIdentityColumns
	{
		private static DatabaseColumnBinder MembersChanged = new DatabaseColumnBinder("MembersChanged");

		public static bool Bind(DatabaseDataReader reader)
		{
			return MembersChanged.GetBoolean(reader);
		}
	}

	public IdentityCacheComponent()
	{
	}

	public IdentityCacheComponent(ISqlConnectionManager sqlConnectinManager)
		: base(sqlConnectinManager)
	{
	}

	public Identity ReadIdentity(string sid)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_read_identity");
			BindSIDType("@SID", sid);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (!databaseDataReader.Read())
			{
				return (Identity)null;
			}
			Identity result = IdentityColumns.Bind(databaseDataReader);
			if (databaseDataReader.NextResult())
			{
				throw new SecurityDatabaseConfigExtraRowsException();
			}
			return result;
		});
	}

	private static ICollection<Identity> FilterUsersFromInvalidDomains(ICollection<Identity> membersList, ICollection<string> failingDomains)
	{
		List<Identity> list = new List<Identity>();
		if (membersList != null)
		{
			foreach (Identity members in membersList)
			{
				if (!failingDomains.Contains(members.Domain))
				{
					list.Add(members);
				}
			}
		}
		return list;
	}

	public bool UpdateIdentity(Identity identity, bool updateMembers, ICollection<Identity> membersList, DateTime currentUtcTime, bool finalCall, ref bool initialCall, ICollection<string> failingDomains)
	{
		bool localInitialCall = initialCall;
		bool result = base.Connection.ExecuteRetryable(delegate
		{
			membersList = FilterUsersFromInvalidDomains(membersList, failingDomains);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("DELETE [TempSecurityIdentities] WHERE [SPID] = @@SPID");
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder2.Append("INSERT INTO [TempSecurityIdentities] ([SPID], [SID], [Type], [DisplayName], [Description], [Domain], [AccountName], [DistinguishedName])");
			stringBuilder2.Append("VALUES(@@SPID, @SID{0}, @Type{0}, @DisplayName{0}, @Description{0}, @Domain{0}, @AccountName{0}, @DistinguishedName{0})");
			StringBuilder stringBuilder3 = new StringBuilder();
			stringBuilder3.Append("EXEC security_update_identity @MemberSid, @Type, @DisplayName, @Description, @Domain, @AccountName, @DistinguishedName, @CurrentUtcTime, @UpdateMembers, @failingDomains");
			PrepareSqlBatch();
			if (localInitialCall)
			{
				AddStatement(stringBuilder.ToString());
				localInitialCall = false;
			}
			if (updateMembers && membersList.Count > 0)
			{
				foreach (Identity members in membersList)
				{
					int num = AddStatement(stringBuilder2.ToString(), 7);
					BindSIDType("@SID" + num.ToString(CultureInfo.InvariantCulture), members.Sid);
					BindByte("@Type" + num.ToString(CultureInfo.InvariantCulture), (byte)members.Type);
					BindString("@DisplayName" + num.ToString(CultureInfo.InvariantCulture), members.DisplayName, 512, allowNull: false, SqlDbType.NVarChar);
					BindString("@Description" + num.ToString(CultureInfo.InvariantCulture), Identity.GetFilteredDescription(members), 2048, allowNull: true, SqlDbType.NVarChar);
					BindString("@Domain" + num.ToString(CultureInfo.InvariantCulture), members.Domain, 512, allowNull: false, SqlDbType.NVarChar);
					BindString("@AccountName" + num.ToString(CultureInfo.InvariantCulture), members.AccountName, 512, allowNull: false, SqlDbType.NVarChar);
					BindString("@DistinguishedName" + num.ToString(CultureInfo.InvariantCulture), members.DistinguishedName, 2048, allowNull: false, SqlDbType.NVarChar);
					StringBuilder stringBuilder4 = new StringBuilder();
					stringBuilder4.AppendLine("INSERT INTO [TempSecurityIdentities] ([SPID], [SID], [Type], [DisplayName], [Description], [Domain], [AccountName], [DistinguishedName])");
					stringBuilder4.AppendFormat(CultureInfo.InvariantCulture, "VALUES(@@SPID, '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}')", members.Sid, (int)members.Type, members.DisplayName, Identity.GetFilteredDescription(members), members.Domain, members.AccountName, members.DistinguishedName);
					GeminiTrace.Verbose(TraceType.Database, stringBuilder4.ToString());
				}
			}
			if (finalCall)
			{
				AddStatement(stringBuilder3.ToString());
				BindSIDType("@MemberSID", identity.Sid);
				BindByte("@Type", (byte)identity.Type);
				BindString("@DisplayName", identity.DisplayName, 512, allowNull: false, SqlDbType.NVarChar);
				BindString("@Description", Identity.GetFilteredDescription(identity), 2048, allowNull: true, SqlDbType.NVarChar);
				BindString("@Domain", identity.Domain, 512, allowNull: false, SqlDbType.NVarChar);
				BindString("@AccountName", identity.AccountName, 512, allowNull: false, SqlDbType.NVarChar);
				BindString("@DistinguishedName", identity.DistinguishedName, 2048, allowNull: false, SqlDbType.NVarChar);
				BindDateTime("@CurrentUtcTime", currentUtcTime);
				BindBoolean("@UpdateMembers", updateMembers);
				BindResultTable("@failingDomains", failingDomains);
				StringBuilder stringBuilder5 = new StringBuilder();
				stringBuilder5.AppendFormat(CultureInfo.InvariantCulture, "EXEC security_update_identity '{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', {7}, {8}, {9}", identity.Sid, (int)identity.Type, identity.DisplayName, Identity.GetFilteredDescription(identity), identity.Domain, identity.AccountName, identity.DistinguishedName, currentUtcTime, updateMembers, string.Join(",", failingDomains));
				GeminiTrace.Verbose(TraceType.Database, stringBuilder5.ToString());
				AddStatement(stringBuilder.ToString());
				DatabaseDataReader databaseDataReader = ExecuteReader();
				if (!databaseDataReader.Read())
				{
					throw new SecurityDatabaseConfigMissingRowException();
				}
				bool result2 = UpdateIdentityColumns.Bind(databaseDataReader);
				if (databaseDataReader.Read() || databaseDataReader.NextResult())
				{
					throw new SecurityDatabaseConfigExtraRowsException();
				}
				return result2;
			}
			ExecuteNonQuery();
			return false;
		});
		initialCall = localInitialCall;
		return result;
	}

	public void DeleteIdentity(string sid, bool isAdSync)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_delete_identity");
			BindSIDType("@SID", sid);
			BindBoolean("@AdSync", isAdSync);
			ExecuteNonQuery();
		});
	}

	public void CleanupDeletedIdentities(int expiryDay)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("security_purge_deleted_users");
			BindInt("@expiryDay", expiryDay);
			ExecuteNonQuery();
		});
	}

	public Membership[] GetMemberships()
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			List<Membership> list = new List<Membership>();
			try
			{
				PrepareStoredProcedure("security_query_membership");
				DatabaseDataReader databaseDataReader = ExecuteReader();
				while (databaseDataReader.Read())
				{
					list.Add(MembershipColumns.Bind(databaseDataReader));
				}
				if (databaseDataReader.NextResult())
				{
					throw new SecurityDatabaseConfigExtraRowsException();
				}
			}
			catch (DatabaseException ex)
			{
				MapException(ex);
				throw;
			}
			return list.ToArray();
		});
	}
}
