using System;

namespace VideoOS.ServerConfigurator.Interfaces;

[Serializable]
public class ServerEndpointConfiguration : EndpointConfiguration
{
	public Guid CertificateGroupId { get; private set; }

	public ImplementationDefinition CertificateRegistrar { get; private set; }

	public ImplementationDefinition ThisServerRegistrar { get; private set; }

	public ImplementationDefinition SqlServerRegistrar { get; protected set; }

	public ServerEndpointConfiguration(Guid serverEndpointId, string displayName, Guid certificateGroupId, string registerImplementationType, string registerServiceImplementationType, string setSqlServerImplementationType)
		: base(serverEndpointId, displayName)
	{
		CertificateGroupId = certificateGroupId;
		if (!string.IsNullOrEmpty(registerImplementationType))
		{
			CertificateRegistrar = new ImplementationDefinition(registerImplementationType);
		}
		if (!string.IsNullOrEmpty(registerServiceImplementationType))
		{
			ThisServerRegistrar = new ImplementationDefinition(registerServiceImplementationType);
		}
		if (!string.IsNullOrEmpty(setSqlServerImplementationType))
		{
			SqlServerRegistrar = new ImplementationDefinition(setSqlServerImplementationType);
		}
	}

	public override void SetRegistrarFileLocation(string fileLocation)
	{
		CertificateRegistrar?.SetFileLocation(fileLocation);
		ThisServerRegistrar?.SetFileLocation(fileLocation);
		SqlServerRegistrar?.SetFileLocation(fileLocation);
	}
}
