using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class MipAction2Model : CommandModelBase
{
	[RequiredNonemptyGuid]
	public Guid ActionId { get; set; }

	public string ActionName { get; set; }

	[Required]
	public IEnumerable<Guid> Targets { get; set; }

	[Required]
	public IEnumerable<Guid> Targets2 { get; set; }

	[Required]
	public TriggerEventDto TriggerEvent { get; set; }
}
