using System;
using System.Collections.Generic;

namespace VideoOS.Platform.Proxy.RestApi;

public class ServiceInformation
{
	public Guid type { get; set; }

	public Guid instance { get; set; }

	public List<string> uris { get; set; }

	public string name { get; set; }

	public string description { get; set; }

	public bool trusted { get; set; }

	public bool enabled { get; set; }

	public List<ServiceInformationEndpoint> endpoints { get; set; }

	public string customData { get; set; }

	public int dataCollectorId { get; set; }
}
