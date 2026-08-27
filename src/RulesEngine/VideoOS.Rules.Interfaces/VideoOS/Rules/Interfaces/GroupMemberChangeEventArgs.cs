using System;
using System.Diagnostics;

namespace VideoOS.Rules.Interfaces;

[DebuggerDisplay("GroupMemberChangeEventArgs: Group: {GroupId}; Member: {MemberId}")]
public sealed class GroupMemberChangeEventArgs : EventArgs
{
	public Guid GroupId { get; private set; }

	public Guid MemberId { get; private set; }

	public GroupMemberChangeEventArgs(Guid groupId, Guid memberId)
	{
		GroupId = groupId;
		MemberId = memberId;
	}
}
