using System;

namespace VideoOS.Common.Integration.Security;

public class SecurityGroupAccessEntry
{
	private SecurityDataSet.SecurityGroupAccessEntryRow row;

	public bool Deny => row.access_deny;

	public string ActionName => row.action_name;

	public Guid NamespaceId => row.namespace_id;

	public string ObjectId => row.object_id;

	public string UserSid => row.user_sid;

	public SecurityGroupAccessEntry(SecurityDataSet.SecurityGroupAccessEntryRow row)
	{
		this.row = row;
	}
}
