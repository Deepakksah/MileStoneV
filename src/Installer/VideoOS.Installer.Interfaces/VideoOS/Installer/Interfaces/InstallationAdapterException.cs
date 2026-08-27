using System;

namespace VideoOS.Installer.Interfaces;

public class InstallationAdapterException : Exception
{
	public InstallationAdapterException(string message)
		: base(message)
	{
	}

	public InstallationAdapterException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
