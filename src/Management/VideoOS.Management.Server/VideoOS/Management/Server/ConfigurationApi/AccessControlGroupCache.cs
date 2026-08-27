using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.ConfigurationApi.AccessControl;
using VideoOS.Management.Server.ConfigurationApi.AccessControl.Builders;

namespace VideoOS.Management.Server.ConfigurationApi;

public class AccessControlGroupCache
{
	public class AccessControlCache
	{
		public Dictionary<Guid, HashSet<Guid>> GroupMembers = new Dictionary<Guid, HashSet<Guid>>();

		public List<Tuple<Guid, Guid>> GroupGroups = new List<Tuple<Guid, Guid>>();

		public Dictionary<Guid, MIPObject> Members = new Dictionary<Guid, MIPObject>();

		public Dictionary<Guid, ServerGroup> Groups = new Dictionary<Guid, ServerGroup>();

		public bool MembersLoaded;

		public bool GroupsLoaded;

		public AccessControlCache(ServerGroup[] allGroupsForUser)
		{
			GroupMembers = new Dictionary<Guid, HashSet<Guid>>();
			GroupGroups = new List<Tuple<Guid, Guid>>();
			Members = new Dictionary<Guid, MIPObject>();
			Groups = new Dictionary<Guid, ServerGroup>();
			foreach (ServerGroup serverGroup in allGroupsForUser)
			{
				Guid guid = new Guid(serverGroup.Id);
				Guid item = (string.IsNullOrWhiteSpace(serverGroup.ParentId) ? Guid.Empty : new Guid(serverGroup.ParentId));
				Groups.Add(guid, serverGroup);
				GroupGroups.Add(new Tuple<Guid, Guid>(item, guid));
			}
		}

		public Guid LookupGroupIdFromMemberId(Guid memberId)
		{
			foreach (KeyValuePair<Guid, HashSet<Guid>> groupMember in GroupMembers)
			{
				if (groupMember.Value.Contains(memberId))
				{
					return groupMember.Key;
				}
			}
			return Guid.Empty;
		}
	}

	private Dictionary<string, AccessControlCache> _cache = new Dictionary<string, AccessControlCache>();

	private bool _prepareDone;

	public void Prepare(IGeminiRequestContext context, bool includeMembers)
	{
		if (!_prepareDone)
		{
			LoadGroupsAndMembers(context, includeMembers);
			_prepareDone = true;
		}
	}

	private void LoadGroupsAndMembers(IGeminiRequestContext requestContext, bool includeMembers)
	{
		string key = "AccessControlGroup";
		if (_cache.ContainsKey(key))
		{
			return;
		}
		Dictionary<Guid, HashSet<Guid>> dictionary = new Dictionary<Guid, HashSet<Guid>>();
		AccessControlMember[] array = AccessControlMember.QueryAccessControlAllMembers(requestContext);
		foreach (AccessControlMember accessControlMember in array)
		{
			if (!dictionary.TryGetValue(accessControlMember.GroupId, out var value))
			{
				value = new HashSet<Guid>();
				dictionary[accessControlMember.GroupId] = value;
			}
			value.Add(accessControlMember.MemberId);
		}
		bool flag = false;
		ServerGroup[] allGroupsForUser = ServerGroup.QueryServerGroupAllChecked(requestContext, ServerGroup.EnumStructureType.AccessControlGroup);
		flag = true;
		AccessControlCache value2 = new AccessControlCache(allGroupsForUser)
		{
			GroupMembers = dictionary,
			GroupsLoaded = flag
		};
		_cache.Add(key, value2);
	}

	public IEnumerable<Guid> QuerySubGroups(Guid parentGroupId)
	{
		if (_cache.TryGetValue("AccessControlGroup", out var value))
		{
			List<Guid> list = new List<Guid>();
			{
				foreach (Tuple<Guid, Guid> groupGroup in value.GroupGroups)
				{
					if (groupGroup.Item1 == parentGroupId)
					{
						list.Add(groupGroup.Item2);
					}
				}
				return list;
			}
		}
		throw new DeviceGroupNotFoundException();
	}

	public IEnumerable<Guid> QueryMembersInGroup(Guid groupId)
	{
		if (_cache.TryGetValue("AccessControlGroup", out var value) && value.GroupMembers.TryGetValue(groupId, out var value2))
		{
			return value2;
		}
		return new HashSet<Guid>();
	}

	public void LoadGroupsForHierarchy(IGeminiRequestContext requestContext)
	{
		if (!_cache.TryGetValue("AccessControlGroup", out var value) || value.GroupsLoaded)
		{
			return;
		}
		HashSet<Guid> hashSet = new HashSet<Guid>();
		foreach (Tuple<Guid, Guid> groupGroup in value.GroupGroups)
		{
			if (groupGroup.Item1 != Guid.Empty)
			{
				hashSet.Add(groupGroup.Item1);
			}
			hashSet.Add(groupGroup.Item2);
		}
		ServerGroup[] array = ServerGroup.QueryServerGroupsByIds(requestContext, hashSet.Select((Guid g) => g.ToString()).ToArray());
		foreach (ServerGroup serverGroup in array)
		{
			value.Groups[new Guid(serverGroup.Id)] = serverGroup;
		}
		value.GroupsLoaded = true;
	}

	public IEnumerable<ServerGroup> LoadGroupsFromCache(IEnumerable<Guid> groupIds)
	{
		List<ServerGroup> list = new List<ServerGroup>();
		if (_cache.TryGetValue("AccessControlGroup", out var value))
		{
			foreach (Guid groupId in groupIds)
			{
				if (value.Groups.TryGetValue(groupId, out var value2))
				{
					list.Add(value2);
				}
			}
		}
		return list;
	}

	public IEnumerable<ConfigurationItem> LoadMembersFromCache(IEnumerable<Guid> memberIds)
	{
		List<ConfigurationItem> list = new List<ConfigurationItem>();
		if (_cache.TryGetValue("AccessControlGroup", out var value))
		{
			ConfigurationItemBuilder configurationItemBuilder = new ConfigurationItemBuilder();
			foreach (Guid memberId in memberIds)
			{
				Guid guid = value.LookupGroupIdFromMemberId(memberId);
				if (value.Members.TryGetValue(memberId, out var value2))
				{
					ConfigurationItemPath path = ConfigurationItemPath.CreateItemPath("AccessControlUnit", memberId.ToString());
					ConfigurationItemPath parentPath = ConfigurationItemPath.CreateFolderPath(ConfigurationItemPath.CreateItemPath("AccessControlGroup", guid.ToString()), "AccessControlUnitFolder");
					ConfigurationItem item = configurationItemBuilder.BuildAccessControlItem(path, parentPath, value2.Name, value2, Array.Empty<string>());
					list.Add(item);
				}
			}
		}
		return list;
	}

	public void LoadMembersForGroup(IGeminiRequestContext requestContext, IEnumerable<Guid> memberIds)
	{
		string key = "AccessControlGroup";
		if (!_cache.TryGetValue(key, out var value))
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (Guid memberId in memberIds)
		{
			if (GroupMemberUtility.HasPermission(requestContext, memberId.ToString()) && !value.Members.ContainsKey(memberId))
			{
				list.Add(memberId.ToString());
			}
		}
		if (list.Count != 0)
		{
			MIPObject[] array = MIPObject.QueryMIPObjectsByIds(requestContext, list.ToArray(), needAdminSecurity: false);
			foreach (MIPObject mIPObject in array)
			{
				value.Members[new Guid(mIPObject.Id)] = mIPObject;
			}
		}
	}

	public void LoadMembersForHierarchy(IGeminiRequestContext requestContext)
	{
		if (!_cache.TryGetValue("AccessControlGroup", out var value) || value.MembersLoaded)
		{
			return;
		}
		MIPObject[] array = MIPObject.QueryMIPObjectsByKind(requestContext, "6d3bdb33-d913-470f-8784-3dd4ee94db8c", null);
		foreach (MIPObject mIPObject in array)
		{
			if (GroupMemberUtility.HasPermission(requestContext, mIPObject.Id))
			{
				value.Members[new Guid(mIPObject.Id)] = mIPObject;
			}
		}
		value.MembersLoaded = true;
	}
}
