using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public sealed class ConfigurationVersionDto
{
	public long ConfigVersion;

	public long SecurityVersion;

	public long TokenVersion;

	public ConfigurationVersionDto(long configVersion, long securityVersion, long tokenVersion)
	{
		ConfigVersion = configVersion;
		SecurityVersion = securityVersion;
		TokenVersion = tokenVersion;
	}

	public ConfigurationVersionDto Clone()
	{
		return new ConfigurationVersionDto(ConfigVersion, SecurityVersion, TokenVersion);
	}
}
