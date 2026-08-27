using System;
using System.Collections.Generic;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class AccessControlMemberComponent : GeminiResourceComponent, IAccessControlMemberComponent, IDisposable
{
	private static class AccessControlMemberColumns
	{
		private static DatabaseColumnBinder GroupId = new DatabaseColumnBinder("groupId");

		private static DatabaseColumnBinder MemberId = new DatabaseColumnBinder("memberId");

		private static DatabaseColumnBinder LastModified = new DatabaseColumnBinder("LastModified");

		public static AccessControlMember Bind(DatabaseDataReader reader)
		{
			string guid = GroupId.GetGuid(reader);
			Guid guid2 = MemberId.GetGuid2(reader);
			return new AccessControlMember
			{
				GroupId = new Guid(guid),
				MemberId = guid2,
				LastModified = LastModified.GetDateTime(reader, allowNulls: true, DateTime.UtcNow)
			};
		}
	}

	public AccessControlMember[] QueryAccessControlMembers(IGeminiRequestContext requestContext, string groupId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_accesscontrol_members");
			BindGuidType("@groupId", groupId, allowNullOrEmpty: true);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<AccessControlMember> list = new List<AccessControlMember>();
			while (databaseDataReader.Read())
			{
				list.Add(AccessControlMemberColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public AccessControlMember[] QueryAccessControlAllMembers(IGeminiRequestContext requestContext)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_accesscontrol_all_members");
			DatabaseDataReader databaseDataReader = ExecuteReader();
			List<AccessControlMember> list = new List<AccessControlMember>();
			while (databaseDataReader.Read())
			{
				list.Add(AccessControlMemberColumns.Bind(databaseDataReader));
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return list.ToArray();
		});
	}

	public void AddAccessControlMember(IGeminiRequestContext requestContext, string groupId, string memberId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("add_accesscontrol_member");
			BindGuidType("@groupId", groupId, allowNullOrEmpty: true);
			BindGuidType("@memberId", memberId, allowNullOrEmpty: true);
			ExecuteNonQuery();
		});
	}

	public void RemoveAccessControlMember(IGeminiRequestContext requestContext, string groupId, string memberId)
	{
		base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("remove_accesscontrol_member");
			BindGuidType("@groupId", groupId, allowNullOrEmpty: true);
			BindGuidType("@memberId", memberId, allowNullOrEmpty: true);
			ExecuteNonQuery();
		});
	}

	public string QueryOwningGroupId(IGeminiRequestContext requestContext, string memberId)
	{
		return base.Connection.ExecuteRetryable(delegate
		{
			PrepareStoredProcedure("query_accesscontrol_member_groupid");
			BindGuidType("@memberId", memberId, allowNullOrEmpty: true);
			DatabaseDataReader databaseDataReader = ExecuteReader();
			if (databaseDataReader.Read())
			{
				return databaseDataReader.GetString(0);
			}
			if (databaseDataReader.NextResult())
			{
				throw new DatabaseConfigExtraRowsException();
			}
			return (string)null;
		});
	}
}
