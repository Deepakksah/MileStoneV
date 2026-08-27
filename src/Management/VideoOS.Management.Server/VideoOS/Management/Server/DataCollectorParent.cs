using System.Collections.Generic;

namespace VideoOS.Management.Server;

public class DataCollectorParent
{
	public int DataCollectorId { get; set; }

	public string ParentName { get; set; }

	public string Id { get; set; }

	public string ServerType { get; set; }

	public List<DataCollectorChild> Children { get; set; }
}
