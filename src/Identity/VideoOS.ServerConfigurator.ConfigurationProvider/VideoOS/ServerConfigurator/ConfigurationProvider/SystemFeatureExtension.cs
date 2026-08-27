using System;
using System.Runtime.CompilerServices;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.ConfigurationProvider;

public static class SystemFeatureExtension
{
	public static VideoOS.ServerConfigurator.Interfaces.SystemFeature ToDataModel(this SystemFeature source)
	{
		return source switch
		{
			SystemFeature.None => VideoOS.ServerConfigurator.Interfaces.SystemFeature.None, 
			SystemFeature.IsManagementServer => VideoOS.ServerConfigurator.Interfaces.SystemFeature.IsManagementServer, 
			_ => throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Unexpected value of {0}: The value was {1}", "source", source))), 
		};
	}

	public static SystemFeature FromDataModel(this VideoOS.ServerConfigurator.Interfaces.SystemFeature source)
	{
		return source switch
		{
			VideoOS.ServerConfigurator.Interfaces.SystemFeature.None => SystemFeature.None, 
			VideoOS.ServerConfigurator.Interfaces.SystemFeature.IsManagementServer => SystemFeature.IsManagementServer, 
			_ => throw new ArgumentException(FormattableString.Invariant(FormattableStringFactory.Create("Unexpected value of {0}: The value was {1}", "source", source))), 
		};
	}
}
