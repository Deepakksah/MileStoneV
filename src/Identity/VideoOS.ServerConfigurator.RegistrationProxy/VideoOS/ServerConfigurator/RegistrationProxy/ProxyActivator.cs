using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.RegistrationProxy;

public class ProxyActivator : IRegistrationActivator
{
	private readonly ISponsor _infiniteSponsor = new InfiniteSponsor();

	private readonly Dictionary<string, object> _instanceCache = new Dictionary<string, object>();

	private static readonly Lazy<ProxyActivator> _lazyInstance = new Lazy<ProxyActivator>(() => new ProxyActivator());

	public static ProxyActivator Instance => _lazyInstance.Value;

	public T CreateInstance<T>(string assemblyQualifiedName, string assemblyLocation, VideoOS.ServerConfigurator.Interfaces.ILogger logger, out bool newInstance, ICompatibilityProvider compatibilityProvider)
	{
		string key = assemblyQualifiedName + assemblyLocation + typeof(T).Name;
		if (_instanceCache.ContainsKey(key))
		{
			newInstance = false;
			return (T)_instanceCache[key];
		}
		Type typeFromHandle = typeof(T);
		object obj = null;
		Type type = typeFromHandle;
		if ((object)type != null)
		{
			if (type == typeof(IApplyCertificate))
			{
				obj = new ApplyCertificateProxy(assemblyQualifiedName, assemblyLocation, _infiniteSponsor);
			}
			else
			{
				if (!(type == typeof(IClientRegistrationV2)))
				{
					goto IL_0091;
				}
				obj = new RegisterClientV2Proxy(assemblyQualifiedName, assemblyLocation, _infiniteSponsor);
			}
			newInstance = true;
			_instanceCache[key] = obj;
			return (T)obj;
		}
		goto IL_0091;
		IL_0091:
		throw new NotImplementedException(FormattableString.Invariant($"There is no proxy implementation for the interface of type {typeFromHandle}"));
	}

	public void ClearInstanceCache()
	{
		_instanceCache.Clear();
	}
}
