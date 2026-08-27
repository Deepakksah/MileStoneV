using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
public class RegistreyKeyService : IRegistryKeyService
{
	public IRegistryKey FromHandle(SafeRegistryHandle handle, RegistryView view)
	{
		return RegistryKeyWrapper.WrapppedKey(RegistryKey.FromHandle(handle, view));
	}

	public IRegistryKey FromHandle(SafeRegistryHandle handle)
	{
		return RegistryKeyWrapper.WrapppedKey(RegistryKey.FromHandle(handle));
	}

	public IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view)
	{
		return RegistryKeyWrapper.WrapppedKey(RegistryKey.OpenBaseKey(hKey, view));
	}

	public IRegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName, RegistryView view)
	{
		return RegistryKeyWrapper.WrapppedKey(RegistryKey.OpenRemoteBaseKey(hKey, machineName, view));
	}

	public IRegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName)
	{
		return RegistryKeyWrapper.WrapppedKey(RegistryKey.OpenRemoteBaseKey(hKey, machineName));
	}
}
