using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public class StateGroup
{
	public string Name { get; set; }

	public Guid ID { get; set; }

	public IEnumerable<string> States { get; set; }
}
