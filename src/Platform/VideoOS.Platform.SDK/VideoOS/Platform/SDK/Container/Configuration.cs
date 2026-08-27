using Autofac;
using Autofac.Builder;
using Autofac.Core;

namespace VideoOS.Platform.SDK.Container;

public static class Configuration
{
	public static IContainer BuildContainer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected O, but got Unknown
		ContainerBuilder val = new ContainerBuilder();
		ModuleRegistrationExtensions.RegisterModule(val, (IModule)(object)new EnvironmentModule());
		return val.Build((ContainerBuildOptions)0);
	}
}
