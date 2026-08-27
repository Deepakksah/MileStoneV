using System;
using System.Collections.Generic;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public class ActionTarget
{
	private ActionTargetType targetType;

	private List<Guid> targetItems = new List<Guid>();

	private List<Guid> targetGroups = new List<Guid>();

	public ActionTargetType TargetType
	{
		get
		{
			return targetType;
		}
		set
		{
			targetType = value;
		}
	}

	public List<Guid> TargetItems => targetItems;

	public List<Guid> TargetGroups => targetGroups;

	public ActionTarget()
	{
		targetType = ActionTargetType.Unspecified;
	}

	public ActionTarget(ActionTargetType targetType)
	{
		this.targetType = targetType;
	}
}
