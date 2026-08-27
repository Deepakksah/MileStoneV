using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Communication.Internal;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class ActionTargetDto
{
	public ActionTargetTypeDto TargetType;

	public List<Guid> TargetItems = new List<Guid>();

	public List<Guid> TargetGroups = new List<Guid>();

	public static ActionTargetDto Create(ActionTarget actionTarget)
	{
		if (actionTarget == null)
		{
			throw new ArgumentNullException("actionTarget");
		}
		ActionTargetDto actionTargetDto = new ActionTargetDto();
		actionTargetDto.TargetType = DtoConvert.ToActionTargetTypeDto(actionTarget.TargetType);
		actionTargetDto.TargetGroups.AddRange(actionTarget.TargetGroups);
		actionTargetDto.TargetItems.AddRange(actionTarget.TargetItems);
		return actionTargetDto;
	}

	public static ActionTarget CreateActionTarget(ActionTargetDto actionTargetDto)
	{
		if (actionTargetDto == null)
		{
			throw new ArgumentNullException("actionTargetDto");
		}
		ActionTarget actionTarget = new ActionTarget();
		actionTarget.TargetType = DtoConvert.ToActionTargetType(actionTargetDto.TargetType);
		actionTarget.TargetGroups.AddRange(actionTargetDto.TargetGroups);
		actionTarget.TargetItems.AddRange(actionTargetDto.TargetItems);
		return actionTarget;
	}
}
