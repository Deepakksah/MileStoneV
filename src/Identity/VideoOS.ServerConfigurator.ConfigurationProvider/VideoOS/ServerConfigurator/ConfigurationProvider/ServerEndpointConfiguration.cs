using System;
using System.Runtime.Serialization;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

[Serializable]
[DataContract(Namespace = "")]
public class ServerEndpointConfiguration : ExtensibleDataObject
{
	[DataMember(Order = 0)]
	public Guid ServerId { get; set; }

	[DataMember(Order = 1)]
	public string DisplayName { get; set; }

	[DataMember(Order = 2)]
	public Guid CertificateGroupId { get; set; }

	[DataMember(Order = 3, EmitDefaultValue = false)]
	public string ApplyCertificateRegistrarType { get; set; }

	[DataMember(Order = 4, EmitDefaultValue = false)]
	public string RegisterServiceRegistrarType { get; set; }

	[DataMember(Order = 5)]
	public string RegistrarImplementationLocation { get; set; }

	[DataMember(Order = 6, EmitDefaultValue = false)]
	public string SqlServerRegistrarType { get; set; }

	public ServerEndpointConfiguration()
	{
	}

	public ServerEndpointConfiguration(ServerCertificateConfigData sourceV1)
	{
		if (sourceV1 == null)
		{
			throw new ArgumentNullException("sourceV1");
		}
		ApplyCertificateRegistrarType = sourceV1.RegisterImplementationType;
		CertificateGroupId = sourceV1.CertificateGroupId;
		DisplayName = sourceV1.DisplayName;
		RegisterServiceRegistrarType = null;
		RegistrarImplementationLocation = sourceV1.RegisterImplementationAssemblyFile;
		ServerId = sourceV1.ServerId;
		SqlServerRegistrarType = null;
	}
}
