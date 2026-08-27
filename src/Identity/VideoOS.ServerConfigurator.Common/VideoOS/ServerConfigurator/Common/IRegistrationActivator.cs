using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.Common;

public interface IRegistrationActivator
{
	T CreateInstance<T>(string assemblyQualifiedName, string assemblyLocation, ILogger logger, out bool newInstance, ICompatibilityProvider compatibilityProvider);
}
