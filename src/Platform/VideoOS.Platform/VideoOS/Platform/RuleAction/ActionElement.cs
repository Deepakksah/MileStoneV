using System;
using System.Collections.ObjectModel;

namespace VideoOS.Platform.RuleAction;

public class ActionElement
{
	public virtual Collection<Guid> ItemKinds { get; set; }

	public virtual string DefaultText { get; set; }
}
