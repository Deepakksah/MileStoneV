using System.Runtime.Serialization;

namespace VideoOS.SCH.Services.Agents;

[DataContract(Name = "ServiceEndpoint", Namespace = "http://videoos.net/2/XProtectCSServiceRegistration")]
public class ServiceEndpoint
{
	[DataMember]
	public string authentication { get; set; }

	[DataMember]
	public string description { get; set; }

	[DataMember]
	public string name { get; set; }

	[DataMember]
	public string uri { get; set; }
}
