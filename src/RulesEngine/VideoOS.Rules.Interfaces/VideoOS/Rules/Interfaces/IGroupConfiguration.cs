using System;

namespace VideoOS.Rules.Interfaces;

public interface IGroupConfiguration
{
	event EventHandler<GroupMemberChangeEventArgs> GroupMemberAdded;

	event EventHandler<GroupMemberChangeEventArgs> GroupMemberDeleted;

	void SetOrUpdateConfiguration(IGroupConfigurationHelper groupHelper, bool firstUpdate);

	bool GroupExists(Guid groupId);

	int GroupMembersCount(Guid groupId);

	Guid[] FindAllDescendantMembersByGroup(Guid groupId);

	Guid[] FindAllDescendantGroupsByGroup(Guid groupId);

	Guid[] FindAncestorGroupsByMember(Guid memberId);

	Guid[] FindAncestorGroupsByGroup(Guid groupId);

	Guid[] FindChildMembersByGroup(Guid groupId);
}
