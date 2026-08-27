using System;

namespace VideoOS.Management.Server;

public class ServiceInfoHandler
{
	public Guid type { get; set; }

	public Guid instance { get; set; }

	public string[] uri { get; set; }

	public string name { get; set; }

	public string description { get; set; }

	public bool trusted { get; set; }

	public bool enabled { get; set; }

	public ServiceEndpointHandler[] endpoints { get; set; }

	public string endpointdefinition { get; set; }

	public string customData { get; set; }

	public int dataCollectorId { get; set; }
}
