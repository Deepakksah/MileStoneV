using System;

namespace VideoOS.ServerConfigurator.Interfaces;

[Serializable]
public class ClientEndpointConfiguration : EndpointConfiguration
{
	public Guid ServerGroupDependency { get; }

	public ImplementationDefinition ClientRegistrar { get; protected set; }

	public ImplementationDefinition ServerAddressRegistrar { get; protected set; }

	public ClientEndpointConfiguration(Guid clientEndpointId, string displayName, Guid serverGroupDependency, string clientRegistrarImplementationType, string serverAddressRegistrarImplementationType)
		: base(clientEndpointId, displayName)
	{
		ServerGroupDependency = serverGroupDependency;
		if (!string.IsNullOrEmpty(clientRegistrarImplementationType))
		{
			ClientRegistrar = new ImplementationDefinition(clientRegistrarImplementationType);
		}
		if (!string.IsNullOrEmpty(serverAddressRegistrarImplementationType))
		{
			ServerAddressRegistrar = new ImplementationDefinition(serverAddressRegistrarImplementationType);
		}
	}

	public ClientEndpointConfiguration(ClientEndpointConfiguration source)
		: this(source?.Id ?? default(Guid), source?.DisplayName, source?.ServerGroupDependency ?? default(Guid), source?.ClientRegistrar?.AssemblyQualifiedName, source?.ServerAddressRegistrar?.AssemblyQualifiedName)
	{
		if (source != null && source.ClientRegistrar?.FileLocation != null)
		{
			SetRegistrarFileLocation(source?.ClientRegistrar.FileLocation);
		}
	}

	public override void SetRegistrarFileLocation(string fileLocation)
	{
		ClientRegistrar?.SetFileLocation(fileLocation);
		ServerAddressRegistrar?.SetFileLocation(fileLocation);
	}
}
