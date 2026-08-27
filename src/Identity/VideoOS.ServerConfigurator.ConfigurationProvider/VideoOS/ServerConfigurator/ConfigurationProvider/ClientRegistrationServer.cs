using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class ClientRegistrationServer : ExtensibleDataObject
{
	[DataMember(Order = 0)]
	public string UnsecureIdpAddress { get; set; }

	[DataMember(Order = 1)]
	public string SecureIdpAddress { get; set; }
}
