using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServiceRegistrationService;

[DebuggerDisplay("ServiceInfo: {name}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ServiceInfo
{
	[DataMember]
	public Guid type { get; set; }

	[DataMember]
	public Guid instance { get; set; }

	[DataMember]
	public string[] uri { get; set; }

	[DataMember]
	public string name { get; set; }

	[DataMember]
	public string description { get; set; }

	[DataMember]
	public bool trusted { get; set; }

	[DataMember]
	public bool enabled { get; set; }

	[DataMember]
	public ServiceEndpoint[] endpoints { get; set; }

	[DataMember]
	public string endpointdefinition { get; set; }
}
