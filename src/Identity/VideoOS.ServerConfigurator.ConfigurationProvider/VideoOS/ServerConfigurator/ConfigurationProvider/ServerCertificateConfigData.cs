using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class ServerCertificateConfigData : ExtensibleDataObject
{
	[DataMember(Order = 0)]
	public Guid ServerId { get; set; }

	[DataMember(Order = 1)]
	public string DisplayName { get; set; }

	[DataMember(Order = 2)]
	public Guid CertificateGroupId { get; set; }

	[DataMember(Order = 3)]
	public string RegisterImplementationType { get; set; }

	[DataMember(Order = 4)]
	public string RegisterImplementationAssemblyFile { get; set; }
}
