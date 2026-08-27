using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace VideoOS.Installer.Bootstrapper;

public class BindingRedirector : MarshalByRefObject
{
	private string _baseDirectory;

	private ILogger _logger;

	public BindingRedirector()
	{
		AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;
		_baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
	}

	public void SetLogger(ILogger logger)
	{
		_logger = logger;
	}

	protected virtual Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
	{
		AssemblyName assemblyName = new AssemblyName(args?.Name);
		Assembly assembly = CreateAssembly(Path.Combine(_baseDirectory, assemblyName.Name + ".dll"), assemblyName.GetPublicKeyToken());
		if (assembly != null)
		{
			Version version = assemblyName.Version;
			Version version2 = assembly.GetName().Version;
			if (version2 < version)
			{
				throw new FileLoadException(FormattableString.Invariant($"Assembly version error: {assemblyName.Name} Requested version: {assemblyName.Version} Available version (lower): {assembly.GetName().Version}"));
			}
			if (_logger != null)
			{
				LoggerExtensions.LogInformation(_logger, FormattableString.Invariant($"Assembly redirected: {assemblyName.Name} Version {version} => {version2}"), Array.Empty<object>());
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
