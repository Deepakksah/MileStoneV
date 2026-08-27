using System;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class LogEntryRuleModel : LogEntryModel
{
	public string ServiceName { get; set; }

	public string RuleName { get; set; }

	public Guid EventTypeId { get; set; }

	public string EventType { get; set; }

	public Guid GeneratorId { get; set; }

	public string GeneratorTypeValue { get; set; }

	public string GeneratorName { get; set; }
}
