using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class ServerAddressBase : ExtensibleDataObject
{
	[DataMember(Order = 0)]
	public string FullyQualifiedDomainName { get; set; }

	[DataMember(Order = 1)]
	public int UnsecurePort { get; set; }

	[DataMember(Order = 2)]
	public int SecurePort { get; set; }
}
