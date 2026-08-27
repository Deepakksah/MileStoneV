using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.DataComponents;

public sealed class IdentityCacheComponent : VmoResourceComponent, IIdentityCacheDataProvider, IDisposable
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

	public Identity ReadIdentity(string sid)
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				Identity identity = null;
				PrepareStoredProcedure("security_read_identity");
				BindSIDType("@SID", sid);
				DatabaseDataReader databaseDataReader = ExecuteReader();
				if (!databaseDataReader.Read())
				{
					return (Identity)null;
				}
				identity = IdentityColumns.Bind(databaseDataReader);
				if (databaseDataReader.NextResult())
				{
					throw new SecurityDatabaseConfigExtraRowsException();
				}
				return identity;
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public void DeleteIdentity(string sid)
	{
		try
		{
			base.Connection.ExecuteRetryable(delegate
			{
				PrepareStoredProcedure("security_delete_identity");
				BindSIDType("@SID", sid);
				ExecuteNonQuery();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}

	public Membership[] GetMemberships()
	{
		try
		{
			return base.Connection.ExecuteRetryable(delegate
			{
				List<Membership> list = new List<Membership>();
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
				return list.ToArray();
			});
		}
		catch (DatabaseException ex)
		{
			MapException(ex);
			throw;
		}
	}
}
