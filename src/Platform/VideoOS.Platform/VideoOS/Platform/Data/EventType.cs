using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Data;

public class EventType
{
	public string Message { get; set; }

	public Guid ID { get; set; }

	public Guid GroupID { get; set; }

	public Guid? StateGroupID { get; set; }

	public string State { get; set; }

	public List<Guid> SourceKinds { get; set; }

	public Guid DefaultSourceKind { get; set; }
}
