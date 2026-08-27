using System;
using System.ComponentModel.DataAnnotations;

namespace VideoOS.Common.Integration.WebApi.Models.Recorder;

public abstract class CommandModelBase
{
	[Required]
	public CommunicationCallContextDto CallContext { get; set; }

	[RequiredNonemptyGuid]
	public Guid Token { get; set; }
}
