using System;

namespace VideoOS.Management.Server.BusinessObjects.EventTypes;

public class EventTypeInfo
{
	public DateTime LastModified { get; set; }

	public Guid IDEventType { get; set; }

	public string Definition { get; set; }

	public string GeneratorType { get; set; }

	public Guid IDGenerator { get; set; }

	public bool BuiltIn { get; set; }

	public string Name { get; set; }

	public string GeneratorSubType { get; set; }
}
