using System;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public class RuleModel
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public bool Enabled { get; set; }

	public RulePartModel RuleStart { get; set; }

	public RulePartModel RuleStop { get; set; }
}
