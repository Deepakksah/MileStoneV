using Microsoft.Win32;
using Microsoft.Win32.SafeHandles;

namespace VideoOS.Common.EnvironmentServices;

public interface IRegistryKeyService
{
	IRegistryKey FromHandle(SafeRegistryHandle handle, RegistryView view);

	IRegistryKey FromHandle(SafeRegistryHandle handle);

	IRegistryKey OpenBaseKey(RegistryHive hKey, RegistryView view);

	IRegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName, RegistryView view);

	IRegistryKey OpenRemoteBaseKey(RegistryHive hKey, string machineName);
}
