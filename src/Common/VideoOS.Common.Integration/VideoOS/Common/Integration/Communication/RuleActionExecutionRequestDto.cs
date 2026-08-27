using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class RuleActionExecutionRequestDto
{
	public Guid RequestId;

	public Guid RuleId;

	public Guid RuleActionInstanceId;

	[Required]
	public TriggerEventDto TriggerEvent;

	[Required]
	public ActionTargetCollectionDto ExpandedTargets;

	public static RuleActionExecutionRequestDto Create(RuleActionExecutionRequest ruleActionExecutionRequest)
	{
		if (ruleActionExecutionRequest == null)
		{
			throw new ArgumentNullException("ruleActionExecutionRequest");
		}
		RuleActionExecutionRequestDto ruleActionExecutionRequestDto = new RuleActionExecutionRequestDto();
		ruleActionExecutionRequestDto.RequestId = ruleActionExecutionRequest.ID;
		ruleActionExecutionRequestDto.RuleId = ruleActionExecutionRequest.RuleId;
		ruleActionExecutionRequestDto.RuleActionInstanceId = ruleActionExecutionRequest.RuleActionInstanceId;
		ruleActionExecutionRequestDto.TriggerEvent = TriggerEventDto.Create(ruleActionExecutionRequest.TriggerEvent);
		ruleActionExecutionRequestDto.ExpandedTargets = new ActionTargetCollectionDto();
		foreach (ActionTarget expandedTarget in ruleActionExecutionRequest.ExpandedTargets)
		{
			ActionTargetDto item = ActionTargetDto.Create(expandedTarget);
			ruleActionExecutionRequestDto.ExpandedTargets.Add(item);
		}
		return ruleActionExecutionRequestDto;
	}

	public static RuleActionExecutionRequest CreateRuleActionExecutionRequest(RuleActionExecutionRequestDto dto)
	{
		if (dto == null)
		{
			throw new ArgumentNullException("dto");
		}
		TriggerEvent triggerEvent = TriggerEventDto.CreateTriggerEvent(dto.TriggerEvent);
		ActionTargetCollection actionTargetCollection = new ActionTargetCollection();
		foreach (ActionTargetDto expandedTarget in dto.ExpandedTargets)
		{
			ActionTarget item = ActionTargetDto.CreateActionTarget(expandedTarget);
			actionTargetCollection.Add(item);
		}
		return new RuleActionExecutionRequest(dto.RuleId, dto.RuleActionInstanceId, triggerEvent, actionTargetCollection)
		{
			ID = dto.RequestId
		};
	}
}
