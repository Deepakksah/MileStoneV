using System;

namespace VideoOS.Platform.RuleAction;

public class ActionDefinition
{
	public virtual Guid Id { get; set; }

	public virtual string Name { get; set; }

	public virtual string SelectionText { get; set; }

	public virtual string DescriptionText { get; set; }

	public virtual ActionElement ActionItemKind { get; set; }

	public virtual ActionElement ActionItemKind2 { get; set; }
}
