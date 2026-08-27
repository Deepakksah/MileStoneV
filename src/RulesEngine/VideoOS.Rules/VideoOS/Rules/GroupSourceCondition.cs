using System;
using System.Collections.Generic;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class GroupSourceCondition : SourceCondition
{
	private List<Guid> groupList;

	private IGroupConfiguration groupConfiguration;

	public IEnumerable<Guid> GroupIds => groupList;

	public GroupSourceCondition(List<Guid> groupList, IGroupConfiguration groupConfiguration)
	{
		this.groupList = groupList;
		this.groupConfiguration = groupConfiguration;
	}

	public override List<Guid> GetSources()
	{
		List<Guid> list = new List<Guid>();
		foreach (Guid group in groupList)
		{
			list.AddRange(groupConfiguration.FindAllDescendantMembersByGroup(group));
		}
		return list;
	}
}
