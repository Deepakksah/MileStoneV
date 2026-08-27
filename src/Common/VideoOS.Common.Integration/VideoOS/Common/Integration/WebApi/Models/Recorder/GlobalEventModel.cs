using System;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public class GlobalEventModel : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid SourceId { get; set; }

	[Required]
	public TriggerEventDto EventCommand { get; set; }
}
