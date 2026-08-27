using System;
using System.Collections.Generic;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class SendMultiNotificationModel : CommandModelBase
{
	public Guid IDNotificationProfile { get; set; }

	public Guid EventTypeId { get; set; }

	public IEnumerable<Guid> SourceIds { get; set; }

	public IEnumerable<Guid> SourceGroupIds { get; set; }

	public Guid TriggerId { get; set; }

	public Guid RuleId { get; set; }

	public DateTime Time { get; set; }

	public Guid RecorderId { get; set; }
}
