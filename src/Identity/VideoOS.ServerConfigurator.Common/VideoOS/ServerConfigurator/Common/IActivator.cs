using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.Common;

public interface IActivator : IRegistrationActivator
{
	T CreateInstance<T>(string assemblyQualifiedName, string assemblyLocation, ILogger logger, bool useInstanceCache, out bool newInstance, string appDomainBase, bool createLocal, ICompatibilityProvider compatibilityProvider);

	T CreateInstance<T>(string assemblyQualifiedName, string assemblyLocation, ILogger logger);
}
