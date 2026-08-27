using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace VideoOS.SqlUpdate.Core.Common;

public class AssemblyWrapper : IAssemblyWrapper
{
	private readonly ILogger _logger;

	private readonly Assembly _assembly;

	public AssemblyWrapper(ILogger logger, string assemblyName)
	{
		_logger = logger;
		_assembly = AppDomain.CurrentDomain.GetAssemblies().First((Assembly a) => a.GetName().Name == assemblyName);
	}

	public Stream GetManifestResourceStream(string resourceName)
	{
		try
		{
			return _assembly.GetManifestResourceStream(resourceName);
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_logger, "{Exception}", new object[1] { ex.Message });
		}
		return null;
	}

	public string[] GetManifestResourceNames()
	{
		return _assembly.GetManifestResourceNames();
	}
}
