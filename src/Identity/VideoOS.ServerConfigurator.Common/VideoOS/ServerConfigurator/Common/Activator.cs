using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Serialization;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.ServerConfigurator.RegistrationContainer;

namespace VideoOS.ServerConfigurator.Common;

public class Activator : IActivator, IRegistrationActivator
{
	private class AssemblyQualifiedName
	{
		private string[] _qualifiedNameparts;

		public string AssemblyFileName => _qualifiedNameparts[1] + ".DLL";

		public string FullTypeName => _qualifiedNameparts[0];

		public int PartCount => _qualifiedNameparts.Length;

		public AssemblyQualifiedName(string assemblyQualifiedName)
		{
			_qualifiedNameparts = (from x in assemblyQualifiedName.Split(',')
				select x.Trim()).ToArray();
		}
	}

	private readonly Dictionary<string, object> _instanceCache = new Dictionary<string, object>();

	private static Activator _instance;

	private ISponsor _infiniteSponsor = new InfiniteSponsor();

	public static Activator Instance => _instance ?? (_instance = new Activator());

	public T CreateInstance<T>(string assemblyQualifiedName, string assemblyLocation, ILogger logger)
	{
		bool newInstance;
		return CreateInstance<T>(assemblyQualifiedName, assemblyLocation, logger, useInstanceCache: true, out newInstance, null, createLocal: false, DefaultCompatibilityProvider.Instance);
	}

	public T CreateInstance<T>(string assemblyQualifiedName, string assemblyLocation, ILogger logger, out bool newInstance, ICompatibilityProvider compatibilityProvider)
	{
		return CreateInstance<T>(assemblyQualifiedName, assemblyLocation, logger, useInstanceCache: true, out newInstance, null, createLocal: false, compatibilityProvider);
	}

	public void ClearInstanceCache()
	{
		_instanceCache.Clear();
	}

	public T CreateInstance<T>(string assemblyQualifiedName, string assemblyLocation, ILogger logger, bool useInstanceCache, out bool newInstance, string appDomainBase, bool createLocal, ICompatibilityProvider compatibilityProvider)
	{
		if (assemblyQualifiedName == null)
		{
			throw new ArgumentNullException("assemblyQualifiedName");
		}
		if (compatibilityProvider == null)
		{
			throw new ArgumentNullException("compatibilityProvider");
		}
		if (assemblyLocation == null)
		{
			assemblyLocation = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
		}
		string key = assemblyQualifiedName + assemblyLocation;
		if (useInstanceCache && _instanceCache.ContainsKey(key))
		{
			newInstance = false;
			return (T)_instanceCache[key];
		}
		newInstance = true;
		AssemblyQualifiedName assemblyQualifiedName2 = new AssemblyQualifiedName(assemblyQualifiedName);
		if (assemblyQualifiedName2.PartCount != 5)
		{
			logger?.LogError(FormattableString.Invariant($"AssemblyQualifiedName = '{assemblyQualifiedName}' does not contain 5 commaseparated parts. it contains {assemblyQualifiedName2.PartCount} parts."));
			return default(T);
		}
		try
		{
			string text = Path.Combine(assemblyLocation, assemblyQualifiedName2.AssemblyFileName);
			AssemblyAuthenticator.Authenticate(text);
			object obj = null;
			if (!createLocal)
			{
				try
				{
					AppDomainSetup appDomainSetup = new AppDomainSetup();
					if (appDomainBase != null)
					{
						appDomainSetup.ApplicationBase = appDomainBase;
					}
					else
					{
						appDomainSetup.ApplicationBase = assemblyLocation;
					}
					appDomainSetup.DisallowApplicationBaseProbing = false;
					appDomainSetup.DisallowBindingRedirects = true;
					appDomainSetup.DisallowCodeDownload = false;
					appDomainSetup.ConfigurationFile = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
					appDomainSetup.LoaderOptimization = LoaderOptimization.MultiDomainHost;
					AppDomain appDomain = AppDomain.CreateDomain(assemblyQualifiedName, null, appDomainSetup);
					string assemblyFile = Path.Combine(Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath), "VideoOS.ServerConfigurator.Common.dll");
					appDomain.CreateInstanceFrom(assemblyFile, "VideoOS.ServerConfigurator.Common.BindingRedirector");
					obj = appDomain.CreateInstanceFrom(text, assemblyQualifiedName2.FullTypeName)?.Unwrap();
					logger?.LogInfo(FormattableString.Invariant($"{assemblyQualifiedName} in {assemblyLocation} created succesfully in separate AppDomain: {appDomain?.FriendlyName} (Id = {appDomain?.Id})"));
				}
				catch (SerializationException)
				{
				}
				catch (Exception ex2)
				{
					logger?.LogError(FormattableString.Invariant($"{assemblyQualifiedName} in {assemblyLocation} created succesfully cannot be created in separate AppDomain. An exception was thrown: {ex2}"));
				}
			}
			if (obj == null)
			{
				obj = System.Activator.CreateInstanceFrom(text, assemblyQualifiedName2.FullTypeName)?.Unwrap();
				logger?.LogInfo(FormattableString.Invariant($"{assemblyQualifiedName} in {assemblyLocation} created succesfully in current AppDomain: {AppDomain.CurrentDomain.FriendlyName} (Id = {AppDomain.CurrentDomain.Id}"));
			}
			T compatibleType = compatibilityProvider.GetCompatibleType<T>(obj);
			if (useInstanceCache)
			{
				((obj as MarshalByRefObject)?.GetLifetimeService() as ILease)?.Register(_infiniteSponsor);
				_instanceCache[key] = compatibleType;
			}
			return compatibleType;
		}
		catch (Exception ex3)
		{
			logger?.LogError(ex3.ToString());
			throw;
		}
	}
}
