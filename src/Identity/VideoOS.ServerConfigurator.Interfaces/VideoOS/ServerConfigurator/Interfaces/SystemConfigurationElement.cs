using System;

namespace VideoOS.ServerConfigurator.Interfaces;

public class SystemConfigurationElement : EndpointConfiguration
{
	public ImplementationDefinition ConfigurationImplementation { get; private set; }

	public SystemFeature SystemFeature { get; }

	public ConfigurationPurpose Purpose { get; }

	public SystemConfigurationElement(Guid elementId, string displayName, SystemFeature systemFeature, ConfigurationPurpose purpose, string configurationImplementationType)
		: base(elementId, displayName)
	{
		SystemFeature = systemFeature;
		Purpose = purpose;
		if (!string.IsNullOrEmpty(configurationImplementationType))
		{
			ConfigurationImplementation = new ImplementationDefinition(configurationImplementationType);
		}
	}

	public override void SetRegistrarFileLocation(string fileLocation)
	{
		ConfigurationImplementation?.SetFileLocation(fileLocation);
	}
}
