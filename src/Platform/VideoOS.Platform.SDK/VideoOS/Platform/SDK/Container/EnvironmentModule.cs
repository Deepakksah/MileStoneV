using Autofac;
using Autofac.Builder;
using Autofac.Core.Activators.Reflection;
using VideoOS.Platform.Data;
using VideoOS.Platform.SDK.Config;
using VideoOS.Platform.SDK.Facades;
using VideoOS.Platform.SDK.Platform;
using VideoOS.Platform.SDK.Util;

namespace VideoOS.Platform.SDK.Container;

public class EnvironmentModule : Module
{
	protected override void Load(ContainerBuilder builder)
	{
		RegistrationExtensions.RegisterType<EnvironmentService>(builder).As<IEnvironmentService>().SingleInstance();
		RegistrationExtensions.RegisterType<UserContext>(builder).SingleInstance();
		RegistrationExtensions.RegisterType<SDKConfiguration>(builder).As<VideoOS.Platform.Configuration>().As<ISDKConfiguration>()
			.SingleInstance();
		RegistrationExtensions.RegisterType<SDKEnvironmentManager>(builder).As<EnvironmentManager>().As<ISDKEnvironmentManager>()
			.SingleInstance();
		RegistrationExtensions.RegisterType<SDKBookmarkService>(builder).As<BookmarkService>().SingleInstance();
		RegistrationExtensions.RegisterType<SDKRestrictedMediaService>(builder).As<RestrictedMediaService>().SingleInstance();
		RegistrationExtensions.RegisterType<QueryConfigurationChanges>(builder).As<IQueryConfigurationChanges>().InstancePerDependency();
		RegistrationExtensions.FindConstructorsWith<EnvironmentProperties, ConcreteReflectionActivatorData, SingleRegistrationStyle>(RegistrationExtensions.RegisterType<EnvironmentProperties>(builder).As<IEnvironmentProperties>(), (IConstructorFinder)(object)new NonPublicConstructorFinder()).InstancePerDependency();
		RegistrationExtensions.RegisterType<SiteUtil>(builder).As<ISiteUtil>();
		RegistrationExtensions.RegisterType<CredentialCacheService>(builder).As<ICredentialCacheService>();
		RegistrationExtensions.RegisterType<ClientControlFacade>(builder).As<IClientControlFacade>();
		RegistrationExtensions.RegisterType<PluginManagerFacade>(builder).As<IPluginManagerFacade>();
		RegistrationExtensions.RegisterType<ServerTypeCacheFacade>(builder).As<IServerTypeCacheFacade>();
		RegistrationExtensions.RegisterType<LoginSettingsCacheFacade>(builder).As<ILoginSettingsCacheFacade>();
		RegistrationExtensions.RegisterType<ServerOfflineCacheFacade>(builder).As<IServerOfflineCacheFacade>();
	}
}
