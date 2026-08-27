using System;

namespace VideoOS.Installer.Interfaces.Content;

public class SystemConfigurationElement
{
	public Guid Id { get; }

	public string DisplayNameKey { get; }

	public string ConfigurationImplementationType { get; }

	public SystemFeature SystemFeature { get; }

	public ConfigurationPurpose ConfigurationPurpose { get; }

	public SystemConfigurationElement(Guid id, string displayNameKey, ConfigurationPurpose configPurpose, SystemFeature systemFeature, string configurationImplementationType)
	{
		Id = id;
		DisplayNameKey = displayNameKey;
		ConfigurationPurpose = configPurpose;
		SystemFeature = systemFeature;
		ConfigurationImplementationType = configurationImplementationType;
	}
}
