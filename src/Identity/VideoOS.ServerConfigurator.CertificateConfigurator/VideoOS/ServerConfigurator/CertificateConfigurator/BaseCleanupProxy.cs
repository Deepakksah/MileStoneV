using System;
using System.Collections.Generic;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public abstract class BaseCleanupProxy<TUnwrap, TInterface> : MarshalByRefObject where TUnwrap : IUnwrap, TInterface, new()
{
	public static Dictionary<Guid, TInterface> GetEndpointInterfacesBase<TEndpointConfiguration>(IEnumerable<TEndpointConfiguration> endpoints, Func<TEndpointConfiguration, ImplementationDefinition> registrar, VideoOS.ServerConfigurator.Interfaces.ILogger logger, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, bool continueOnError, out StatusCode result) where TEndpointConfiguration : EndpointConfiguration
	{
		if (endpoints == null)
		{
			throw new ArgumentNullException("endpoints");
		}
		if (registrar == null)
		{
			throw new ArgumentNullException("registrar");
		}
		IActivator instance = VideoOS.ServerConfigurator.Common.Activator.Instance;
		Dictionary<Guid, TInterface> dictionary = new Dictionary<Guid, TInterface>();
		result = StatusCode.Success;
		foreach (TEndpointConfiguration endpoint in endpoints)
		{
			ImplementationDefinition implementationDefinition = registrar(endpoint);
			if (implementationDefinition == null)
			{
				continue;
			}
			string remMappedPath = Remapper.Instance.GetRemMappedPath(implementationDefinition.FileLocation);
			if (remMappedPath != null)
			{
				bool newInstance;
				CleanUpWrapper cleanUpWrapper = instance.CreateInstance<CleanUpWrapper>(typeof(CleanUpWrapper).AssemblyQualifiedName, null, logger, useInstanceCache: false, out newInstance, remMappedPath, createLocal: false, DefaultCompatibilityProvider.Instance);
				cleanUpWrapper.CreateInnerImplementation(implementationDefinition.AssemblyQualifiedName, remMappedPath);
				TUnwrap val = new TUnwrap();
				val.SetInnerItem(cleanUpWrapper);
				result = val.Initialize(registrationLogger).StatusCode;
				if (result.IsSuccess())
				{
					dictionary[endpoint.Id] = (TInterface)(object)val;
				}
				else if (!continueOnError)
				{
					break;
				}
			}
			else
			{
				logger?.LogError(FormattableString.Invariant($"Unable to Remap {implementationDefinition.FileLocation} load {implementationDefinition.AssemblyQualifiedName}. This type is ignored at uninstall"));
				if (!continueOnError)
				{
					break;
				}
			}
		}
		return dictionary;
	}
}
