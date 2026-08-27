using System;

namespace VideoOS.Rules.Interfaces;

[Serializable]
public sealed class TargetChangeEvent
{
	private Guid targetId;

	private TargetChangeTargetTypes targetType;

	private TargetChangeActionTypes actionType;

	public Guid TargetId
	{
		get
		{
			return targetId;
		}
		set
		{
			targetId = value;
		}
	}

	public TargetChangeTargetTypes TargetType
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

	public TargetChangeActionTypes ActionType
	{
		get
		{
			return actionType;
		}
		set
		{
			actionType = value;
		}
	}

	public TargetChangeEvent(Guid targetId, TargetChangeTargetTypes targetType, TargetChangeActionTypes targetChangeActionTypes)
	{
		this.targetId = targetId;
		this.targetType = targetType;
		actionType = targetChangeActionTypes;
	}

	public TargetChangeEvent Clone()
	{
		return new TargetChangeEvent(targetId, targetType, actionType);
	}
}
