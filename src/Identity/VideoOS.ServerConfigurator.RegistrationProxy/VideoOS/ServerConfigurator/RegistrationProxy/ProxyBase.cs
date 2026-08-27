using System;
using System.IO;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using VideoOS.ServerConfigurator.RegistrationContainer;

namespace VideoOS.ServerConfigurator.RegistrationProxy;

public abstract class ProxyBase<TContainerInterface, TContainerImplementation> where TContainerInterface : IRegistrationContainer where TContainerImplementation : TContainerInterface
{
	private readonly string _assemblyQualifiedName;

	private readonly string _assemblyLocation;

	private readonly ISponsor _sponsor;

	protected TContainerInterface Container { get; private set; }

	protected ProxyBase(string assemblyQualifiedName, string assemblyLocation, ISponsor sponsor)
	{
		_assemblyQualifiedName = assemblyQualifiedName;
		_assemblyLocation = assemblyLocation;
		_sponsor = sponsor;
		Container = CreateInstance<TContainerInterface>(typeof(TContainerImplementation).Assembly.FullName, typeof(TContainerImplementation).FullName, assemblyLocation);
		Container.CreateInstance(_assemblyLocation, _assemblyQualifiedName);
	}

	private T CreateInstance<T>(string assemblyName, string typeName, string appDomainBase)
	{
		AppDomainSetup appDomainSetup = new AppDomainSetup();
		appDomainSetup.ApplicationBase = appDomainBase;
		appDomainSetup.DisallowApplicationBaseProbing = false;
		appDomainSetup.DisallowCodeDownload = false;
		appDomainSetup.DisallowBindingRedirects = true;
		appDomainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
		appDomainSetup.LoaderOptimization = LoaderOptimization.MultiDomainHost;
		AppDomain appDomain = AppDomain.CreateDomain(typeName, null, appDomainSetup);
		string assemblyFile = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "VideoOS.ServerConfigurator.RegistrationProxy.dll");
		appDomain.CreateInstanceFrom(assemblyFile, "VideoOS.ServerConfigurator.RegistrationProxy.AssemblyResolver");
		object obj = appDomain.CreateInstance(assemblyName, typeName)?.Unwrap();
		((obj as MarshalByRefObject)?.GetLifetimeService() as ILease)?.Register(_sponsor);
		return (T)obj;
	}
}
