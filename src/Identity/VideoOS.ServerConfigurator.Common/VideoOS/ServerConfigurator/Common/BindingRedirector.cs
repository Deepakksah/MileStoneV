using System;
using System.IO;
using System.Reflection;
using VideoOS.ServerConfigurator.RegistrationContainer;

namespace VideoOS.ServerConfigurator.Common;

public class BindingRedirector : MarshalByRefObject
{
	private string _baseDirectory;

	public BindingRedirector()
	{
		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
		_baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
	}

	protected virtual Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
	{
		AssemblyName assemblyName = new AssemblyName(args?.Name);
		Assembly assembly = CreateAssembly(Path.Combine(_baseDirectory, assemblyName.Name + ".dll"), assemblyName.GetPublicKeyToken());
		if (assembly != null)
		{
			Version version = assemblyName.Version;
			if (assembly.GetName().Version < version)
			{
				throw new FileLoadException(FormattableString.Invariant($"Assembly version error: {assemblyName.Name} Requested version: {assemblyName.Version} Available version (lower): {assembly.GetName().Version}"));
			}
		}
		return assembly;
	}

	protected static Assembly CreateAssembly(string filePath)
	{
		return CreateAssembly(filePath, null);
	}

	protected static Assembly CreateAssembly(string filePath, byte[] publicKeyToken)
	{
		if (!File.Exists(filePath))
		{
			return null;
		}
		try
		{
			if (publicKeyToken != null)
			{
				AssemblyAuthenticator.Authenticate(filePath, publicKeyToken);
			}
			else
			{
				AssemblyAuthenticator.Authenticate(filePath);
			}
			return Assembly.LoadFrom(filePath);
		}
		catch
		{
			return null;
		}
	}
}
