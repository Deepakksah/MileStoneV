using System;

namespace VideoOS.Common.EnvironmentServices;

public abstract class OSPlatformAttribute : Attribute
{
	public string PlatformName { get; }

	private protected OSPlatformAttribute(string platformName)
	{
	}
}
