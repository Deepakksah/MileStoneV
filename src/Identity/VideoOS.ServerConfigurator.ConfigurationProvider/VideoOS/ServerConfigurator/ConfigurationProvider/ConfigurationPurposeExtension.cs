using System;
using System.Runtime.CompilerServices;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

public static class ConfigurationPurposeExtension
{
	public static VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose ToDataModel(this ConfigurationPurpose source)
	{
		return source switch
		{
			ConfigurationPurpose.None => VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose.None, 
			ConfigurationPurpose.ChangeHostName => VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose.ChangeHostName, 
			_ => throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Unexpected value of {0}: The value was {1}", "source", source))), 
		};
	}

	public static ConfigurationPurpose FromDataModel(this VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose source)
	{
		return source switch
		{
			VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose.None => ConfigurationPurpose.None, 
			VideoOS.ServerConfigurator.Interfaces.ConfigurationPurpose.ChangeHostName => ConfigurationPurpose.ChangeHostName, 
			_ => throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Unexpected value of {0}: The value was {1}", "source", source))), 
		};
	}
}
