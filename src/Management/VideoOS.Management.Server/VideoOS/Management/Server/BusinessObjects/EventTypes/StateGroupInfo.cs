using System;
using System.Collections.ObjectModel;

namespace VideoOS.Management.Server.BusinessObjects.EventTypes;

public class StateGroupInfo
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public Collection<string> States { get; set; }

	public DateTime LastModified { get; set; }
}
