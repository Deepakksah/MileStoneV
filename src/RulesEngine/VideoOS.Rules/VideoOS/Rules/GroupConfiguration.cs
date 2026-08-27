using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public sealed class GroupConfiguration : IGroupConfiguration
{
	private class Group
	{
		public Guid GroupId { get; private set; }

		public Guid ParentGroupId { get; private set; }

		public HashSet<Guid> ChildGroups { get; private set; }

		public HashSet<Guid> Members { get; private set; }

		public bool IsEmpty
		{
			get
			{
				if (ChildGroups.Count == 0)
				{
					return Members.Count == 0;
				}
				return false;
			}
		}

		public Group(Guid groupId, Guid parentGroupId)
		{
			GroupId = groupId;
			ParentGroupId = parentGroupId;
			ChildGroups = new HashSet<Guid>();
			Members = new HashSet<Guid>();
		}
	}

	private Dictionary<Guid, Group> groups;

	private readonly object lockObject = new object();

	public int GroupCount => groups.Count;

	public event EventHandler<GroupMemberChangeEventArgs> GroupMemberAdded;

	public event EventHandler<GroupMemberChangeEventArgs> GroupMemberDeleted;

	public GroupConfiguration()
	{
		groups = new Dictionary<Guid, Group>();
	}

	private void OnGroupMemberAdded(GroupMemberChangeEventArgs e)
	{
		if (this.GroupMemberAdded != null)
		{
			this.GroupMemberAdded(this, e);
		}
	}

	private void OnGroupMemberDeleted(GroupMemberChangeEventArgs e)
	{
		if (this.GroupMemberDeleted != null)
		{
			this.GroupMemberDeleted(this, e);
		}
	}

	public void SetOrUpdateConfiguration(IGroupConfigurationHelper groupHelper, bool firstUpdate)
	{
		if (groupHelper == null)
		{
			throw new ArgumentNullException("groupHelper");
		}
		lock (lockObject)
		{
			List<GroupChange> list = new List<GroupChange>();
			List<GroupChange> list2 = new List<GroupChange>();
			IEnumerable<GroupChange> changesOfType = groupHelper.GetChangesOfType(GroupChangeType.GroupAdd);
			list.AddRange(changesOfType);
			while (list.Count > 0)
			{
				int count = list.Count;
				foreach (GroupChange item in list)
				{
					Group value = null;
					if (item.ParentGroupOrMemberId == Guid.Empty || groups.TryGetValue(item.ParentGroupOrMemberId, out value))
					{
						Group obj = new Group(item.GroupId, item.ParentGroupOrMemberId);
						groups.Add(obj.GroupId, obj);
						value?.ChildGroups.Add(obj.GroupId);
					}
					else
					{
						list2.Add(item);
					}
				}
				list.Clear();
				list.AddRange(list2);
				list2.Clear();
				if (list.Count == count)
				{
					throw new InvalidOperationException("Rule Group Configuration, add group: Unknown parent group");
				}
			}
			changesOfType = groupHelper.GetChangesOfType(GroupChangeType.GroupMemberAdd);
			foreach (GroupChange item2 in changesOfType)
			{
				Group obj2 = groups[item2.GroupId];
				obj2.Members.Add(item2.ParentGroupOrMemberId);
				if (!firstUpdate)
				{
					OnGroupMemberAdded(new GroupMemberChangeEventArgs(obj2.GroupId, item2.ParentGroupOrMemberId));
				}
			}
			changesOfType = groupHelper.GetChangesOfType(GroupChangeType.GroupMemberDelete);
			foreach (GroupChange item3 in changesOfType)
			{
				Group obj3 = groups[item3.GroupId];
				obj3.Members.Remove(item3.ParentGroupOrMemberId);
				if (!firstUpdate)
				{
					OnGroupMemberDeleted(new GroupMemberChangeEventArgs(obj3.GroupId, item3.ParentGroupOrMemberId));
				}
			}
			changesOfType = groupHelper.GetChangesOfType(GroupChangeType.GroupDelete);
			list.Clear();
			list.AddRange(changesOfType);
			list2.Clear();
			while (list.Count > 0)
			{
				int count2 = list.Count;
				foreach (GroupChange item4 in list)
				{
					Group obj4 = groups[item4.GroupId];
					if (obj4.IsEmpty)
					{
						groups.Remove(obj4.GroupId);
						if (obj4.ParentGroupId != Guid.Empty)
						{
							groups[obj4.ParentGroupId].ChildGroups.Remove(obj4.GroupId);
						}
					}
					else
					{
						list2.Add(item4);
					}
				}
				list.Clear();
				list.AddRange(list2);
				list2.Clear();
				if (list.Count == count2)
				{
					throw new InvalidOperationException("Rule Group Configuration, delete group: No empty groups to delete");
				}
			}
		}
	}

	public bool GroupExists(Guid groupId)
	{
		lock (lockObject)
		{
			return groups.ContainsKey(groupId);
		}
	}

	public int GroupMembersCount(Guid groupId)
	{
		lock (lockObject)
		{
			return groups[groupId].Members.Count;
		}
	}

	public Guid[] FindAllDescendantMembersByGroup(Guid groupId)
	{
		lock (lockObject)
		{
			HashSet<Guid> hashSet = new HashSet<Guid>();
			FindDescendantMembersByGroup(groupId, hashSet);
			return hashSet.ToArray();
		}
	}

	private void FindDescendantMembersByGroup(Guid groupId, HashSet<Guid> result)
	{
		Group value = null;
		if (!groups.TryGetValue(groupId, out value))
		{
			return;
		}
		foreach (Guid member in value.Members)
		{
			result.Add(member);
		}
		foreach (Guid childGroup in value.ChildGroups)
		{
			FindDescendantMembersByGroup(childGroup, result);
		}
	}

	public Guid[] FindAllDescendantGroupsByGroup(Guid groupId)
	{
		lock (lockObject)
		{
			HashSet<Guid> hashSet = new HashSet<Guid>();
			FindDescendantGroupsByGroup(groupId, hashSet);
			return hashSet.ToArray();
		}
	}

	private void FindDescendantGroupsByGroup(Guid groupId, HashSet<Guid> result)
	{
		Group value = null;
		if (!groups.TryGetValue(groupId, out value))
		{
			return;
		}
		foreach (Guid childGroup in value.ChildGroups)
		{
			result.Add(childGroup);
			FindDescendantGroupsByGroup(childGroup, result);
		}
	}

	public Guid[] FindAncestorGroupsByMember(Guid memberId)
	{
		lock (lockObject)
		{
			HashSet<Guid> hashSet = new HashSet<Guid>();
			foreach (Group item2 in groups.Values.Where((Group g) => g.Members.Contains(memberId)))
			{
				hashSet.Add(item2.GroupId);
				Guid[] array = FindAncestorGroupsByGroup(item2.GroupId);
				foreach (Guid item in array)
				{
					hashSet.Add(item);
				}
			}
			return hashSet.ToArray();
		}
	}

	public Guid[] FindAncestorGroupsByGroup(Guid groupId)
	{
		lock (lockObject)
		{
			HashSet<Guid> hashSet = new HashSet<Guid>();
			Group value = null;
			if (groups.TryGetValue(groupId, out value))
			{
				while (groups.TryGetValue(value.ParentGroupId, out value))
				{
					hashSet.Add(value.GroupId);
				}
			}
			return hashSet.ToArray();
		}
	}

	public Guid[] FindChildMembersByGroup(Guid groupId)
	{
		lock (lockObject)
		{
			Group value = null;
			if (groups.TryGetValue(groupId, out value))
			{
				return value.Members.ToArray();
			}
			return Array.Empty<Guid>();
		}
	}
}
