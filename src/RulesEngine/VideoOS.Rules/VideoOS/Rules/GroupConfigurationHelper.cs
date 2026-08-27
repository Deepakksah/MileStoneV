using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class GroupConfigurationHelper : IGroupConfigurationHelper
{
	private List<GroupChange> changes = new List<GroupChange>();

	public bool HasChanges => changes.Count > 0;

	public void GroupAdd(Guid groupId, Guid parentGroupId)
	{
		changes.Add(new GroupChange(groupId, parentGroupId, GroupChangeType.GroupAdd));
	}

	public void GroupDelete(Guid groupId)
	{
		changes.Add(new GroupChange(groupId, Guid.Empty, GroupChangeType.GroupDelete));
	}

	public void GroupMemberAdd(Guid groupId, Guid memberId)
	{
		changes.Add(new GroupChange(groupId, memberId, GroupChangeType.GroupMemberAdd));
	}

	public void GroupMemberDelete(Guid groupId, Guid memberId)
	{
		changes.Add(new GroupChange(groupId, memberId, GroupChangeType.GroupMemberDelete));
	}

	public IEnumerable<GroupChange> GetChangesOfType(GroupChangeType changeType)
	{
		return changes.Where((GroupChange c) => c.ChangeType == changeType);
	}
}
