using System;
using System.Collections.Generic;

namespace VideoOS.Rules.Interfaces;

public interface IGroupConfigurationHelper
{
	bool HasChanges { get; }

	void GroupAdd(Guid groupId, Guid parentGroupId);

	void GroupDelete(Guid groupId);

	void GroupMemberAdd(Guid groupId, Guid memberId);

	void GroupMemberDelete(Guid groupId, Guid memberId);

	IEnumerable<GroupChange> GetChangesOfType(GroupChangeType changeType);
}
