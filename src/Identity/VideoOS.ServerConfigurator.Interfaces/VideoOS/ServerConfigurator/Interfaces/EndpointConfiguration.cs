using System;

namespace VideoOS.ServerConfigurator.Interfaces;

[Serializable]
public abstract class EndpointConfiguration
{
	public Guid Id { get; }

	public string DisplayName { get; protected set; }

	protected EndpointConfiguration(Guid endpointId, string displayName)
	{
		Id = endpointId;
		DisplayName = displayName;
	}

	public abstract void SetRegistrarFileLocation(string fileLocation);
}
