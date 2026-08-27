using System;

namespace VideoOS.Rules.Interfaces;

public class GroupChange
{
	public Guid GroupId { get; private set; }

	public Guid ParentGroupOrMemberId { get; private set; }

	public GroupChangeType ChangeType { get; private set; }

	public GroupChange(Guid groupId, Guid parentGroupOrMemberId, GroupChangeType changeType)
	{
		GroupId = groupId;
		ParentGroupOrMemberId = parentGroupOrMemberId;
		ChangeType = changeType;
	}
}
