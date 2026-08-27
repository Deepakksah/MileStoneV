using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class ExecuteRuleActionsModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid SourceId { get; set; }

	[Required]
	public IEnumerable<RuleActionExecutionRequestDto> RuleActionExecutionRequest { get; set; }

	public DateTime SenderUtcTime { get; set; }
}
