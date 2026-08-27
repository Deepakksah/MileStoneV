using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class ClientEndpointConfiguration : ExtensibleDataObject
{
	[DataMember(Order = 0)]
	public Guid ClientId { get; set; }

	[DataMember(Order = 1)]
	public string DisplayName { get; set; }

	[DataMember(Order = 2)]
	public Guid CertificateGroupDependencyId { get; set; }

	[DataMember(Order = 3, EmitDefaultValue = false)]
	public string RegisterClientRegistrarType { get; set; }

	[DataMember(Order = 4, EmitDefaultValue = false)]
	public string RegisterSetServerAddressImplementationType { get; set; }

	[DataMember(Order = 5)]
	public string RegistrarImplementationLocation { get; set; }

	public ClientEndpointConfiguration()
	{
	}

	public ClientEndpointConfiguration(ClientRegistrationConfigData sourceV1)
	{
		if (sourceV1 == null)
		{
			throw new ArgumentNullException("sourceV1");
		}
		CertificateGroupDependencyId = sourceV1.CertificateGroupDependencyId;
		ClientId = sourceV1.ClientId;
		DisplayName = sourceV1.DisplayName;
		RegisterClientRegistrarType = sourceV1.RegisterImplementationType;
		RegistrarImplementationLocation = sourceV1.RegisterImplementationAssemblyFile;
		RegisterSetServerAddressImplementationType = null;
	}
}
