using System;
using System.IO;
using System.Reflection;
using VideoOS.ServerConfigurator.Common;

namespace VideoOS.ServerConfigurator.RegistrationProxy;

public class AssemblyResolver : BindingRedirector
{
	private string _resolverPath;

	private string _legacyResourcePath;

	public AssemblyResolver()
	{
		_resolverPath = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath);
		_legacyResourcePath = Path.Combine(_resolverPath, "LegacyResources");
	}

	protected override Assembly CurrentDomainAssemblyResolve(object sender, ResolveEventArgs args)
	{
		AssemblyName assemblyName = new AssemblyName(args?.Name);
		string text = null;
		string name = assemblyName.Name;
		if (name != null)
		{
			if (name.Equals("VideoOS.ServerConfigurator.CertificateRegistration", StringComparison.OrdinalIgnoreCase))
			{
				text = Path.Combine(_legacyResourcePath, "VideoOS.ServerConfigurator.CertificateRegistration_2020R2", assemblyName.Name + ".dll");
			}
			else if (name.Equals("VideoOS.CertificateRegistration.Common", StringComparison.OrdinalIgnoreCase))
			{
				text = Path.Combine(_legacyResourcePath, "VideoOS.CertificateRegistration.Common_2020R2", assemblyName.Name + ".dll");
			}
			else
			{
				if (!name.Equals("VideoOS.ServerConfigurator.RegistrationContainer", StringComparison.OrdinalIgnoreCase))
				{
					goto IL_00af;
				}
				text = Path.Combine(_resolverPath, assemblyName.Name + ".dll");
			}
			return BindingRedirector.CreateAssembly(text);
		}
		goto IL_00af;
		IL_00af:
		return base.CurrentDomainAssemblyResolve(sender, args);
	}
}
